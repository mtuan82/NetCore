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

        public void CreateStore(Store data)
        {
            _dbcontext.Add<Store>(data);
            _dbcontext.SaveChanges();
        }

        public bool UpdateStore(Store data)
        {
            var customer = _dbcontext.Store.FirstOrDefault(x => x.Id == data.Id);
            if (customer != null)
            {
                _dbcontext.Update<Store>(_mapper.Map(data, customer));
                _dbcontext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Store GetStoreById(int id)
        {
            return _dbcontext.Store.FirstOrDefault(x => x.Id == id);
        }

        public List<Customer> GetStoresByName(string name)
        {
            return _dbcontext.Store.Where(x => x.Name.Contains(name)).ToList();
        }

        public bool DeleteStore(int id)
        {
            var customer = _dbcontext.Store.FirstOrDefault(x => x.Id == id);
            if (customer != null)
            {
                _dbcontext.Store.Remove(customer);
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
