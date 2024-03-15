using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Providers.MSSQL.Entity
{
    [Table("Orders")]
    public class Store
    {
        [Key]
        public int Id { get; set; }
    }
}
