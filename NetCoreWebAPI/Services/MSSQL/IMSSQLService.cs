using Core.Providers.MSSQL.Entity;

namespace NetCoreWebAPI.Services.MSSQL
{
    public interface IMSSQLService
    {
        void CreateStore(Order data);
        bool UpdateStore(Order data);
        Order GetStoreById(int id);
        List<Order> GetStoresByName(string name);
        bool DeleteStore(int id);
    }
}
