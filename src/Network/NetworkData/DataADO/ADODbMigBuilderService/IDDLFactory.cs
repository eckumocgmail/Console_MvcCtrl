using ApplicationCore.Domain.Odbc.Metadata;

using System;

namespace DataADO
{
    public interface IDDLFactory
    {
        string CreateForeignkey(string relativeTable, string table, string column, bool? onDeleteCascade = false, bool? onUpdateCascade = null);
        string CreateTable(Type metadata);
        TableMetaData CreateTableMetaData(Type metadata);
        string CreateTable(TableMetaData metadata);
    }
}