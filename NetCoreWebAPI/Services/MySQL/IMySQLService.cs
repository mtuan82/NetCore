using Core.Providers.MySQL.Entity;

namespace NetCoreWebAPI.Services.MySQL
{
    public interface IMySQLService
    {
        void CreateCustomer(Customer data);
        bool UpdateCustomer(Customer data);
        Customer GetCustomerById(int id);
        List<Customer> GetCustomersByName(string name);
        bool DeleteCustomer(int id);

    }
}
