using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "This field can't have specail case")]
        [Length(5, 200, ErrorMessage = "This field just container 5-200 characters")]
        public string ProductName { get; set; }

        [Required]
        [Range(0,double.MaxValue, ErrorMessage = "This field can't be negative")]
        public double Weight { get; set; }

        [Required]
        [Range(0,double.MaxValue, ErrorMessage = "This field can't be negative")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "This field can't be negative")]
        public int UnitsInStock { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }
        
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
