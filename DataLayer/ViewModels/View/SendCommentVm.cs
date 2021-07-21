using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.ViewModels.View
{
    public class SendCommentVm
    {
        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(500, ErrorMessage = "کاراکتر بیشتر است")]
        public string Body { get; set; }
        public int? ProductId { get; set; }
        public int? BlogId { get; set; }
        public int CommentId { get; set; }
        public int? ParentId { get; set; }
    }
}
