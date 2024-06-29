using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        public int CategoryId { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "This field can't have specail case")]
        [Length(5, 200, ErrorMessage = "This field just container 5-200 characters")]
        public string CategoryName { get; set; }

        public virtual ICollection<Product>? Products { get; set; }  
    }
}
