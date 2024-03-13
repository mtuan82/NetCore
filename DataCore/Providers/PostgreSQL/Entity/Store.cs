using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataCore.Providers.PostgreSQL.Entity
{
    [Table("Stores")]
    public class Store
    {
        [Key]
        public int Id { get; set; }
    }
}
