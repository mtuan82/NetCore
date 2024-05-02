using Core.Providers.PostgreSQL.Entity;

namespace NetCoreWebAPI.Services.PostgreSQL
{
    public interface IPostgreSQLService
    {
        void CreateStore(Store data);
        bool UpdateStore(Store data);
        Store GetStoreById(int id);
        List<Store> GetStoresByName(string name);
        bool DeleteStore(int id);
    }
}
