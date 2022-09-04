using Newtonsoft.Json.Linq;
using ApplicationCore.Domain.Odbc.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Odbc.Controllers;
using COM;
using DataADO;

namespace DataODBC
{
    public class TableManagerStatefull : BaseService, ITableManagerStatefull
    {
        public ITableManager tableManager;
        public JArray dataRecords;
        public OdbcDatabaseManager databaseManager;

        public TableManagerStatefull()
        {
            
        }
        public TableManagerStatefull(ITableManager tm)
        {
            this.tableManager = tm;
            this.dataRecords = this.tableManager.SelectAll();
            this.tableManager.SelectMaxId();
        }


        /**
         * выбор обьектов ссылающихся на запись в заданной таблице с заданным идентификатором
         */
        public object SelectReferencesFrom(string table, Int64 record_id)
        {

            string fk = (from p in this.tableManager.GetMetadata().fk where p.Value.ToLower() == table.ToLower() select p.Key).SingleOrDefault<string>();
            return (from r in this.dataRecords where r[fk].Value<Int64>() == record_id select r).ToList();
        }


        /**
         * выбор обьектов ссылающихся на запись в заданной таблице с заданным идентификатором
         */
        public object SelectReferencesTo(string table, Int64 record_id)
        {
            OdbcTableManager tableRef = (OdbcTableManager)this.databaseManager.GetFasade()[table];
            return new List<object>();
        }


        /**
         * выбор обьектов ссылающихся на запись в заданной таблице с заданным идентификатором
         */
        public object SelectNotReferencesTo(string table, Int64 record_id)
        {
            string fk = (from p in this.tableManager.GetMetadata().fk where p.Value.ToLower() == table.ToLower() select p.Key).SingleOrDefault<string>();
            return (from r in this.dataRecords where r[fk].Value<Int64>() == record_id select r).ToList();
        }


        public TableManagerStatefull(OdbcDatabaseManager databaseManager, OdbcTableManager tableManager)
        {
            this.databaseManager = databaseManager;
            this.tableManager = tableManager;
            this.dataRecords = this.tableManager.SelectAll();
          //  this.tableManager.SelectMaxId();
        }


        public Int64 Count()
        {
            ///return this.dataRecords.Count();
            return tableManager.Count();
        }


        public OdbcTableManager Get(string table)
        {
            return this.databaseManager.Get(table);
        }


        public TableMetaData GetMetadata()
        {
            return this.tableManager.GetMetadata();
        }


        public JArray SelectAll()
        {
            return this.dataRecords;
        }


        public object WhereColumnValueIn(string column, JArray values)
        {
            List<Int64> valuesList = new List<Int64>();
            foreach (JValue val in values)
            {
                valuesList.Add(val.Value<Int64>());
            }
            return (from rec in this.dataRecords where valuesList.Contains(rec[column].Value<Int64>()) select rec).ToList();
        }


        public JToken Select(Int64 id)
        {
            TableMetaData metadata = this.GetMetadata();
            string pk = metadata.getPrimaryKey();
            if (pk == null)
            {
                throw new NullReferenceException(pk);
            }
            //if(Logging) System.Console.WriteLine(this.dataRecords);
            return (from r in this.dataRecords where r[pk].Value<Int64>() == id select r).SingleOrDefault<JToken>();
        }

        public JArray Search(List<string> terms, string query)
        {
            JArray arr = new JArray();

            foreach (var record in this.dataRecords)
            {
                foreach (string term in terms)
                {
                    if (record[term].Value<string>().IndexOf(query) != -1)
                    {
                        arr.Add(record);
                        break;
                    }
                }

            }
            return arr;
        }

        public JArray Join(Int64 id, string table)
        {
            throw new Exception("unsupported");
            //return this.datasource.Execute( "select * from " + metadata.name + " where " + metadata.pk + " = " + id );
        }


        public JArray SelectPage(Int64 page, Int64 size)
        {
            JArray arr = new JArray();
            JObject[] records = this.dataRecords.Values<JObject>().ToArray<JObject>();
            for (Int64 i = ((page) * size); i < ((page + 1) * size); i++)
            {
                if (i < records.Length)
                {
                    arr.Add(records[i]);
                }
            }
            return arr;
        }


        public Int64 Create(Dictionary<string, object> values)
        {
            try
            {
                this.dataRecords.Add(JObject.FromObject(values));
                this.tableManager.Create(values);
                return 1;
            }
            catch (Exception ex)
            {
                Writing.ToConsole(ex.ToString());
                return -1;
            }
        }


        public Int64 Update(Dictionary<string, object> values)
        {
            if (this.GetMetadata().pk == null)
            {
                throw new Exception("primary key not defined");
            }
            if (!values.ContainsKey(this.GetMetadata().pk))
            {
                throw new Exception("values argument has not primary key identifier");
            }
            Int64 objectId = Int64.Parse(values[this.GetMetadata().pk].ToString());

            JToken record = this.Select(objectId);
            JObject jvalues = JObject.FromObject(values);
            foreach (var p in values)
            {
                record[p.Key] = jvalues[p.Key];
            }
            new Task(() => { this.tableManager.Update(values); }).Start();
            return 1;
        }


        public Int64 Delete(Int64 id)
        {
            this.dataRecords.Remove(this.Select(id));
            new Task(() => { this.tableManager.Delete(id); }).Start();
            return 1;
        }


        public object GetAssociations(string key)
        {
            switch (key)
            {
                case "$list": return this.GetCommands();
                case "$metadata": return this.GetMetadata();
                case "$popular": return this.GetPopular();
                //case "$latest": return this.GetLatest();
                // case "$rating": return this.GetRating();
                //  case "$stats": return this.GetStats();
                case "$keywords": return this.GetKeywords();
                case "$errors": return this.GetErrors();
                default: return null;
            }
        }

        public object GetCommands()
        {
            return new string[] { "$stats", "$keywords", "$errors", "$rating", "$latest", "$popular", "$metadata", "$list" };
        }

        public object GetErrors()
        {
            return null;
        }


        public object GetPopular()
        {
            return null;
        }


        IDictionary<string, int> keywords { get; set; }
        public IDictionary<string, int> GetKeywords()
        {
            this.keywords = new Dictionary<string, int>();
            string name = this.GetMetadata().name;
            TableMetaData metadata = ((OdbcTableManager)this.tableManager).GetMetadata();
            string pk = //this.GetMetaData().Tables.ContainsKey(metadata.singlecount_name) ?
                        //this.GetMetaData().Tables[metadata.singlecount_name].getPrimaryKey() :
                        //this.GetMetaData().Tables[metadata.name].getPrimaryKey();
                        this.tableManager.GetMetadata().getPrimaryKey();

            if (pk == null)
            {
                throw new Exception("Primary key udefined for table " + name);
            }
            var tms = new TableManagerStatefull(((OdbcTableManager)tableManager));
            List<string> textColumns = ((OdbcTableManager)tableManager).GetMetadata().GetTextColumns();
            foreach (JObject record in tms.dataRecords)
            {
                try
                {
                    System.Console.WriteLine(record);
                    int id = record[pk].Value<int>();
                 
                    Dictionary<string, int> statisticsForThisRecord = new Dictionary<string, int>();
                    foreach (string column in textColumns)
                    {
                        if (record[column] != null)
                        {
                            System.Console.WriteLine(record.ToString());
                            string textValue = record[column].Value<string>();
                            if (String.IsNullOrEmpty(textValue)) continue;
                            foreach (string word in textValue.Split(" "))
                            {
                                bool isRus = Validation.IsRus(word);
                                bool isEng = Validation.IsEng(word);
                                if (isRus == false && isEng == false)
                                    continue;
                                if (keywords.ContainsKey(word))
                                {
                                    keywords[word]++;
                                }
                                else
                                {
                                    keywords[word] = 1;
                                }

                                if (statisticsForThisRecord.ContainsKey(word))
                                {
                                    statisticsForThisRecord[word]++;
                                }
                                else
                                {
                                    statisticsForThisRecord[word] = 1;
                                }
                            }
                        }
                    }

                    //statistics[name + "/" + id] = statisticsForThisRecord;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex);
                    continue;
                }
            }
            return keywords;

        }
    }
}
