using BusinessObject.Utils.CustomerAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "This field can't be blank")]
        public string MemberId { get; set; }

        [Required(ErrorMessage = "This field can't be blank")]
        [CheckDateValid(false)]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "This field can't be blank")]
        [CheckDateValid(false)]
        public DateTime RequiredDate {  get; set; }

        [Required(ErrorMessage = "This field can't be blank")]
        [CheckDateValid(false)]
        public DateTime ShippedDate { get; set; }

        [Required(ErrorMessage = "This field can't be blank")]
        [Range(0, double.MaxValue, ErrorMessage = "This field can't be negative")]
        public decimal Freight { get; set; }

        [ForeignKey(nameof(MemberId))]
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
