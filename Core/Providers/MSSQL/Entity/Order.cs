using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Providers.MSSQL.Entity
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
