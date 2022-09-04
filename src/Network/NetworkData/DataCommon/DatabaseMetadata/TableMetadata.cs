using ApplicationCore.Domain.Odbc.Controllers;

using COM;

using Microsoft.Extensions.DependencyInjection;


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ValidationAnnotationsNS;

namespace ApplicationCore.Domain.Odbc.Metadata
{



    /// <summary>
    /// Класс определяет свойства сущности
    /// </summary>
    [Display(Name = "Модель данных")]
    public class TableMetaData: BaseEntity
    {
        
        



        [Display(Name = "Схема")]
        public string schema { get; set;  } = "dbo";


        [NotNullNotEmpty]
        [Display(Name = "Единственное число")]
        public string name { get; set; } = "";


        [Display(Name = "Множественное число")]
        public string multicount_name { get; set; } = "";

        [Display(Name = "Заголовок")]
        public string caption { get; set; }

        [Display(Name = "Единственное число")]
        public string singlecount_name { get; set; } = "";

        [Display(Name = "Описание")]
        public string description { get; set; }

        [NotNullNotEmpty]
        [Required(ErrorMessage="Необходимо задать первичный ключ")]
        public string pk = "ID";


        // таблицы в которых возможны множественные ссылки на уникальный обьект тек. таблицы
        [Display(Name = "Ссылки")]
        public List<string> references { get; set; } = new List<string>();


        // ключ- наименование колонки внешнего ключа,  значение - наименование таблицы на которую ссылается ( на первичный ключ которой ссылается внешний)
        [Display(Name = "Внешние ключи")]
        public Dictionary<string, string> fk { get; set; } = new Dictionary<string, string>();     
        public Dictionary<string, long> keywords { get; set; } = new Dictionary<string, long>();     


        [Display(Name = "Элементы")]
        public Dictionary<string, ColumnMetaData> columns { get; set; } = new Dictionary<string, ColumnMetaData>(); 



        public TableMetaData() { }



        public string toSql()
        {
            throw new Exception();
            //return new SqlFactory().CreateTable(this);
        }

        public bool IsValidated()
        {
            return this.pk != null && this.name != null;
        }

        public string getTableNameCamelized()
        {
            return Naming.ToCapitalStyle(this.name);
        }


        public string getTableNameCapitalized()
        {             
            return Naming.ToCapitalStyle( this.name );
        }


      
        public static TableMetaData operator !(TableMetaData x)
        {
            return null;
        }
      
        public static TableMetaData operator ~(TableMetaData x)
        {
            return null;
        }
 
        public static TableMetaData operator &(TableMetaData x, TableMetaData y)
        {
            return null;
        }

        public static TableMetaData operator |(TableMetaData x, TableMetaData y)
        {
            return null;
        }

        public static TableMetaData operator ^(TableMetaData x, TableMetaData y)
        {
            return null;
        }

       /* public static TableMetaData operator ==(TableMetaData x, TableMetaData y)
        {
            return null;
        }*/

       /* public static TableMetaData operator !=(TableMetaData x, TableMetaData y)
        {
            return null;
        }*/

        public static TableMetaData operator <(TableMetaData x, TableMetaData y)
        {
            return null;
        }

        public static TableMetaData operator >(TableMetaData x, TableMetaData y)
        {
            return null;
        }

        public static TableMetaData operator <=(TableMetaData x, TableMetaData y)
        {
            return null;
        }

        public static TableMetaData operator >=(TableMetaData x, TableMetaData y)
        {
            return null;
        }


        public List<string> GetAnnotations(string column)
        {
            List<string> annotations = new List<string>();
            List<string> _ids = new List<string>(Naming.SplitName(this.name));
            List<string> ids = new List<string>();
            foreach(string id in _ids)
            {
                ids.Add(id.ToLower());
            }
            annotations.Add($"[System.ComponentModel.DataAnnotations.Schema.Column(\"{column}\")]");
            if (ids.Contains("url"))
            {
                annotations.Add("[System.ComponentModel.DataAnnotations.Url()]");
            }
            if (ids.Contains("email"))
            {
                annotations.Add("[System.ComponentModel.DataAnnotations.EmailAddress()]");
            }
            if (ids.Contains("phone")|| ids.Contains("tel"))
            {
                annotations.Add("[System.ComponentModel.DataAnnotations.Phone()]");
            }
            if (ids.Contains("password"))
            {
                annotations.Add("[DataType(DataType.Password)]");
            }
            if (ids.Contains("image")|| ids.Contains("imageurl"))
            {
                annotations.Add("[DataType(DataType.ImageUrl)]");
            } 
            if ( this.columns[column].type.ToLower()=="date")
            {
                annotations.Add("[DisplayFormat(DataFormatString = \"" + "{"+"0:dd.MM.yyyy"+"}"+"\", ApplyFormatInEditMode = true)]");
            }
            if (this.columns[column].name==this.getPrimaryKey() || this.columns[column].primary)
            {
                annotations.Add("[System.ComponentModel.DataAnnotations.Key()]");
            }
            if (this.columns[column].nullable == false)
            {
                annotations.Add("[System.ComponentModel.DataAnnotations.Required()]");
            }
            return annotations;
        }

        public string getPrimaryKey()
        {
            string primaryKey = this.name.ToUpper() + "ID";
            string singleIdName = this.singlecount_name.ToUpper() + "ID";
            string multiIdName = this.multicount_name.ToUpper() + "ID";
            if ( this.pk == null)
            {

                foreach ( var columnEntry in this.columns )
                {
                    if (columnEntry.Value.primary == true)
                    {
                        return this.pk = columnEntry.Value.name;
                        
                    }
                    else if (columnEntry.Key.ToUpper() == "ID")
                    {
                        return this.pk = columnEntry.Value.name;                        
                    }
                    else if (columnEntry.Key.ToUpper() == (singleIdName) || columnEntry.Key.ToUpper() == (multiIdName))
                    {
                        return this.pk = columnEntry.Key;                        
                    }

                }

            }
            else
            {
                return this.pk;
            }
            return "ID";
            //throw new Exception($"Метаданные талицы: {this.name} не содержат определние первичного ключа");        
        }


        public bool ContainsBlob()
        {
             
            foreach (var columnEntry in this.columns)
            {
                if (columnEntry.Value.type.ToLower() == "blob")
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> GetTextColumns()
        {
            List<string> textColumns = new List<string>();
            foreach (var columnEntry in this.columns)
            {
                List<string> types = new List<string>(new String[] { "nvarchar", "varchar", "ntext", "text", "char" });

                if (types.Contains(columnEntry.Value.type.ToLower()))
                {
                    textColumns.Add(columnEntry.Value.name);
                }
            }
            return textColumns;
        }

        public string getTableNameKebabed()
        {
            string kebab = "";
            for(int i=0; i< this.name.Length; i++)
            {
                if( i!=0 && this.name[i].ToString() == this.name[i].ToString().ToUpper())
                {
                    kebab += "-" + this.name[i].ToString().ToLower();
                }
                else
                {
                    kebab += this.name[i].ToString().ToLower();
                }
            }
            return kebab;
        }
    }
}
