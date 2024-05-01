using AutoMapper;
using Core.Providers.MySQL;
using Core.Providers.MySQL.Entity;

namespace NetCoreWebAPI.Services.MySQL
{
    public class MySQLService: IMySQLService
    {
        private readonly MySQLContext _dbcontext;
        private readonly IMapper _mapper;
        public MySQLService(MySQLContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public void CreateCustomer(Customer data)
        {
            _dbcontext.Add<Customer>(data);
            _dbcontext.SaveChanges();
        }

        public bool UpdateCustomer(Customer data)
        {
            var customer = _dbcontext.Customer.FirstOrDefault(x => x.Id == data.Id);
            if (customer != null)
            {
                _dbcontext.Update<Customer>(_mapper.Map(data, customer));
                _dbcontext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Customer GetCustomerById(int id)
        {
            return _dbcontext.Customer.FirstOrDefault(x => x.Id == id);
        }

        public List<Customer> GetCustomersByName(string name)
        {
            return _dbcontext.Customer.Where(x => x.Name.Contains(name)).ToList();
        }

        public bool DeleteCustomer(int id)
        {
            var customer = _dbcontext.Customer.FirstOrDefault(x => x.Id == id);
            if (customer != null)
            {
                _dbcontext.Customer.Remove(customer);
                _dbcontext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
