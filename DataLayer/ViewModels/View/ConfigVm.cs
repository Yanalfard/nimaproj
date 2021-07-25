using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.ViewModels
{
    public class ConfigVm
    {
        [Required(ErrorMessage = "لطفا لینک ایمیل را وارد کنید")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا لینک تلگرم را وارد کنید")]
        public string Telegram { get; set; }
        [Required(ErrorMessage = "لطفا لینک تلگرم را وارد کنید")]
        public string Inista { get; set; }
        [Required(ErrorMessage = "لطفا موبایل را وارد کنید")]
        public string TellHome { get; set; }
        [Required(ErrorMessage = "لطفا شماره را وارد کنید")]
        public string TellMobile { get; set; } 
        [Required(ErrorMessage = "لطفا آدرس را وارد کنید")]
        public string Address { get; set; } 
        [Required(ErrorMessage = "لطفا آدرس واتساب را وارد کنید")]
        public string Whatsapp { get; set; }
        [Required(ErrorMessage = "لطفا  شرایط ارسال  را وارد کنید")]
        public string SharyeteErsal { get; set; }
        [Required(ErrorMessage = "لطفا  شرایط خرید  را وارد کنید")]
        public string OrderDetails { get; set; }
        [Required(ErrorMessage = "لطفا درباره ما را کامل کنید")]
        public string DarBareyeMa { get; set; }







    }
}
