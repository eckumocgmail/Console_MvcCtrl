 
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Text;

namespace DataODBC
{
    class OdbcResourceProvider
    {
        /**
        * загрузка бинарных данных в хранилище
        */
        public static void UploadBinaryString(string name, string type, string data)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values["resource_name"] = name;
            values["mime_type"] = type;
            values["resource_data"] = data;

            //TableManagerStatefull fasade = (TableManagerStatefull)(((DatabaseManager)(GetRestfullModel()["database"])).fasade["resources"]);
            //fasade.Create(values);
        }

        /**
         * загрузка бинарных данных в хранилище
         */
        public static void UploadResource(string name, string type, string data)
        {
            byte[] bytes = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                bytes[i] = (byte)data[i];
            }
            Upload(name, type, bytes);
        }

        /**
         * получение списка ресурсов
         */
        public static JArray ResourceList()
        {
            return GetDataSource().Execute("select resource_id, resource_name, mime_type from resources");
        }

        /**
         * выгрузка бинарных данных из хранилище
         */
        public static byte[] Download(string id)
        {
            return GetDataSource().ReadBlob("select resource_data from resources where resource_id=" + id);
        }

        private static OdbcDataSource GetDataSource()
        {
            return new OdbcDataSource();
        }

        /**
         * загрузка бинарных данных в хранилище
         */
        public static void Upload(string name, string mime, byte[] data)
        {
            GetDataSource().InsertBlob("insert into resources (resource_name,mime_type,resource_data) values (\"" + name + "\",\"" + mime + "\",?)", "@bin_data", data);
        }
    }
}
