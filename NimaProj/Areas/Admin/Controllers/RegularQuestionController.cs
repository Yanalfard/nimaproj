using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NimaProj.Utilities;
using Services.Services;
using DataLayer.Models;
using ReflectionIT.Mvc.Paging;

namespace NimaProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker("admin")]
    public class RegularQuestionController : Controller
    {
        Core _core = new Core();

        public async Task<IActionResult> Index(int page = 1)
        {

            try
            {
                IEnumerable<TblRegularQuestion> regularQuestions = PagingList.Create(_core.RegularQuestion.Get().OrderByDescending(rq => rq.RegularQuestionId), 40, page);
                return await Task.FromResult(View(regularQuestions));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                return await Task.FromResult(ViewComponent("CreateRegularQuestionAdmin"));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create(TblRegularQuestion regularQuestion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _core.RegularQuestion.Add(regularQuestion);
                    _core.Save();
                    return Redirect("/Admin/RegularQuestion");
                }
                return await Task.FromResult(View(regularQuestion));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                return await Task.FromResult(ViewComponent("EditRegularQuestionAdmin", new { id = id }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(TblRegularQuestion regularQuestion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _core.RegularQuestion.Update(regularQuestion);
                    _core.Save();
                    return Redirect("/Admin/RegularQuestion");
                }
                return await Task.FromResult(View(regularQuestion));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }


        public async Task<IActionResult> RegularQuestionInfo(int id)
        {
            try
            {
                return await Task.FromResult(ViewComponent("RegularQuestionInfoAdmin", new { id = id }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        [HttpPost]
        public void Delete(int id)
        {
            _core.RegularQuestion.DeleteById(id);
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
