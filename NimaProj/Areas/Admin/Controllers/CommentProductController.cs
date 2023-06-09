﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using Services.Services;
using ReflectionIT.Mvc.Paging;
using NimaProj.Utilities;
namespace NimaProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker("admin")]
    public class CommentProductController : Controller
    {
        Core _core = new Core();
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                IEnumerable<TblComment> comments = PagingList.Create(_core.Comment.Get(i => i.IsBlog == false).OrderByDescending(c => c.DateCreated), 30, page);

                return await Task.FromResult(View(comments));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        public async Task<IActionResult> Info(int id)
        {
            try
            {

                return await Task.FromResult(ViewComponent("CommentInfoAdmin", new { id = id }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        public string Delete(int id)
        {
            if (_core.Comment.Get(c => c.ParentId == id).Count() > 0)
            {
                return "شما نمیتوانید این نظر را حذف کنید. پاسخی برای کامنت ثبت شده است";
            }
            else
            {
                _core.Comment.DeleteById(id);
                _core.Save();
                return "true";
            }
        }

        [HttpPost]
        public void ChangeStatus(int id)
        {
            TblComment comment = _core.Comment.GetById(id);
            comment.IsValid = !comment.IsValid;
            _core.Comment.Update(comment);
            _core.Save();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _core.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
