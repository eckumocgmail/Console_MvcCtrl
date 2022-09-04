using ApplicationCore.Domain.Odbc.Metadata;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DataContext
{
    public int SaveChanges()
    {
        throw new NotImplementedException();
    }

    public void Remove<TEntity>(TEntity p) where TEntity : BaseEntity
    {
        throw new NotImplementedException();
    }

    public void Add<TEntity>(TEntity p) where TEntity : BaseEntity
    {
        throw new NotImplementedException();
    }

    public void Update<TEntity>(TEntity p) where TEntity : BaseEntity
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}