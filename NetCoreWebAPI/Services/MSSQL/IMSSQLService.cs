using Core.Providers.MSSQL.Entity;

namespace NetCoreWebAPI.Services.MSSQL
{
    public interface IMSSQLService
    {
        void CreateStore(Store data);
        bool UpdateStore(Store data);
        Store GetStoreById(int id);
        List<Store> GetStoresByName(string name);
        bool DeleteStore(int id);
    }
}
