using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IEntityRepository<TEntity>
{
    public Task CreateRecord(TEntity target);
    public Task DeleteRecord(TEntity p);
    public Task<TEntity> FindRecord(int id);
    public Task<IEnumerable<TEntity>> SelectAll();
    public Task UpdateRecord(TEntity targetData);
    public int PostRecord(TEntity data);
    public int PutRecord(TEntity data);
    public int PatchRecord(params TEntity[] dataset);
    public int DeleteRecordById(int id);
    public IEnumerable<TEntity> GetRecordById(int? id);
}