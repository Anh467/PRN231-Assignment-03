using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Utils.CustomerAttribute
{
    public class CheckDateValidAttribute : ValidationAttribute
    {
        private bool isHigher = false;

        public CheckDateValidAttribute() { }

        public CheckDateValidAttribute(bool isHigher) {
            this.isHigher = isHigher == null ? true : isHigher;
        }

        public override bool IsValid(object? value)
        {
            var dateTemp = (DateTime) value;
            return this.isHigher? dateTemp.CompareTo(DateTime.Now) > 0:
                    dateTemp.CompareTo(DateTime.Now) <= 0;
        }
    }
}
