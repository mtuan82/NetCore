using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataCore.Providers.MSSQL.Entity
{
    [Table("Orders")]
    public class Store
    {
        [Key]
        public int Id { get; set; }
    }
}
