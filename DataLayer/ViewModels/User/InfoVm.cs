using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.ViewModels
{
    public class InfoVm
    {
        [Display(Name = "نام ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(25, ErrorMessage = "تعداد کاراکتر بیشتر است")]
        [StringLength(25)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
