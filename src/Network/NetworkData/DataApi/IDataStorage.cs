using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public interface IDataStorage
    {
 
        public void RestoreDatabaseFrom(
                string ConnectionString,
                string FilePath,
                bool FullMode);
        public void BackupDatabaseToFile(
                string ConnectionString,
                string FilePath,
                bool FullMode);
    }
}