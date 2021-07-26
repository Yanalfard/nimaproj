using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Services;
using DataLayer.Models;
using ReflectionIT.Mvc.Paging;
using NimaProj.Utilities;

namespace NimaProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker("admin")]
    public class PropertyController : Controller
    {
        Core _core = new Core();
        public async Task<IActionResult> Index(int page=1)
        {
            try
            {
                IEnumerable<TblProperty> properties = PagingList.Create(_core.Property.Get().OrderByDescending(p => p.PropertyId), 10, page);
                return await Task.FromResult(View(properties));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        public string Create(string Name)
        {
            if (_core.Property.Get().Any(p => p.Name == Name))
            {
                return "ویژگی تکراری میباشد";
            }
            else
            {
                TblProperty property = new TblProperty();
                property.Name = Name;
                _core.Property.Add(property);
                _core.Save();
                return "true";
            }
        }

        public string Delete(int id)
        {
            TblProperty property = _core.Property.GetById(id);
            if (property.TblProductPropertyRels.Count() == 0)
            {
                _core.Property.DeleteById(id);
                _core.Save();
                return "true";
                
            }
            else
            {
                return "ویژگی در محصولی استفاده شده است";
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
