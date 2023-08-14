using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
    public class ShoppingCart
    {
        public Product product { get; set; }
        [Range(1, 1000, ErrorMessage = "Please Enter a value in ramge of 1 and 1000")]

        public int count { get; set; }

    }
}
