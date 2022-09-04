using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Domain.Odbc.Controllers
{
    public class DatabaseManagerHelper
    {
        public string enableChangeDetectionScript()
        {
            return "exec sys.sp_cdc_enable_db";
        }
        public string disableChangeDetectionScript()
        {
            return "exec sys.sp_cdc_disable_db";
        }
        public string enableChangeDetectionForTableScript()
        {
            return "exec sys.sp_cdc_enable_table";
        }
        public string disableChangeDetectionForTableScript()
        {
            return "exec sys.sp_cdc_disable_table";
        }

      


        public string createUniqConstraintScript(string tableFullName, string columnName)
        {
            return $"ALTER TABLE {tableFullName} ADD CONSTRAINT AK_Password UNIQUE ({columnName});  ";
        }
    }
}
