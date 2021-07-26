using NimaProj.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using Services.Services;
using ReflectionIT.Mvc.Paging;

namespace NimaProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker("admin")]
    public class SpecialOfferController : Controller
    {
        Core _core = new Core();
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                IEnumerable<TblSpecialOffer> SpecialOffers = PagingList.Create(_core.SpecialOffer.Get().OrderByDescending(so => so.SpecialOfferId), 40, page);
                return await Task.FromResult(View(SpecialOffers));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        public async Task<IActionResult> CreateSpecialOffer(TblSpecialOffer specialOffer, int Till)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TblSpecialOffer NewspecialOffer = new TblSpecialOffer();
                    NewspecialOffer.ProductId = specialOffer.ProductId;
                    NewspecialOffer.ValidTill = DateTime.Now.AddDays(Till);
                    _core.SpecialOffer.Add(NewspecialOffer);
                    _core.Save();
                    return Redirect("/Admin/Product");
                }
                return await Task.FromResult(View(specialOffer));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        [HttpPost]
        public void Delete(int id)
        {
            _core.SpecialOffer.DeleteById(id);
            _core.Save();
        }

        public void Remove(int id)
        {
            if (_core.SpecialOffer.Get(s => s.ProductId == id).Count() > 0)
            {
                IEnumerable<TblSpecialOffer> specialOffers = _core.SpecialOffer.Get(s => s.ProductId == id);
                foreach (var item in specialOffers)
                {
                    _core.SpecialOffer.Delete(item);
                }
                _core.Save();
            }
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
