using AutoMapper;
using Core.Providers.PostgreSQL.Entity;
using Core.Providers.PostgreSQL;

namespace NetCoreWebAPI.Services.PostgreSQL
{
    public class PostgreSQLService: IPostgreSQLService
    {
        private readonly PostgreSQLContext _dbcontext;
        private readonly IMapper _mapper;

        public PostgreSQLService(PostgreSQLContext postgreSQLContext, IMapper mapper)
        {
            _dbcontext = postgreSQLContext;
            _mapper = mapper;
        }

        public void CreateStore(Store data)
        {
            _dbcontext.Add<Store>(data);
            _dbcontext.SaveChanges();
        }

        public bool UpdateStore(Store data)
        {
            var customer = _dbcontext.Stores.FirstOrDefault(x => x.Id == data.Id);
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
            return _dbcontext.Stores.FirstOrDefault(x => x.Id == id);
        }

        public List<Store> GetStoresByName(string name)
        {
            return _dbcontext.Stores.Where(x => x.Name.Contains(name)).ToList();
        }

        public bool DeleteStore(int id)
        {
            var Store = _dbcontext.Stores.FirstOrDefault(x => x.Id == id);
            if (Store != null)
            {
                _dbcontext.Stores.Remove(Store);
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
