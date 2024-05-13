using AutoMapper;
using Core.Providers.MSSQL;
using Core.Providers.MySQL.Entity;
using Core.Providers.MSSQL.Entity;

namespace NetCoreWebAPI.Services.MSSQL
{
    public class MSSQLService: IMSSQLService
    {
        private readonly MSSQLContext _dbcontext;
        private readonly IMapper _mapper;

        public MSSQLService(MSSQLContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public void CreateStore(Order data)
        {
            _dbcontext.Add<Order>(data);
            _dbcontext.SaveChanges();
        }

        public bool UpdateStore(Order data)
        {
            var customer = _dbcontext.Order.FirstOrDefault(x => x.Id == data.Id);
            if (customer != null)
            {
                _dbcontext.Update<Order>(_mapper.Map(data, customer));
                _dbcontext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Order GetStoreById(int id)
        {
            return _dbcontext.Order.FirstOrDefault(x => x.Id == id);
        }

        public List<Order> GetStoresByName(string name)
        {
            return _dbcontext.Order.Where(x => x.Name.Contains(name)).ToList();
        }

        public bool DeleteStore(int id)
        {
            var customer = _dbcontext.Order.FirstOrDefault(x => x.Id == id);
            if (customer != null)
            {
                _dbcontext.Order.Remove(customer);
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
