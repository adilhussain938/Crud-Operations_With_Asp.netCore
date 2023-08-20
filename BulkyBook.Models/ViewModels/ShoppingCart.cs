using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId{ get; set; }
       
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product product { get; set; }
        
        [Range(1, 1000, ErrorMessage = "Please Enter a value in range of 1 and 1000")]
        public int count { get; set; }

        public string ApplicationUserId { get; set; }
        
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
