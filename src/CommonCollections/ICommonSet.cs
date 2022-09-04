 
using API;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISuperDbSet<TEntity> :
    ISet<TEntity>,
    System.Linq.IQueryable<TEntity> where TEntity : class
{
    void Init(string state);
    public Task Update(TEntity targetData);
    IKV<string,string> Find(int accountId);
}