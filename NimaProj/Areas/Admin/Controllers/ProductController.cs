using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using DataLayer.ViewModels;
using ReflectionIT.Mvc.Paging;
using Services.Services;
using Microsoft.AspNetCore.Http;
using NimaProj.Utilities;
using System.IO;

namespace NimaProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[PermissionChecker("admin,employee")]
    public class ProductController : Controller
    {
        Core _core = new Core();
        [HttpGet]
        public IActionResult Index(ProductsInAdminVm productsInAdmin)
        {
            try
            {
                IEnumerable<TblProduct> data = _core.Product.Get();

                if (!string.IsNullOrEmpty(productsInAdmin.Search))
                {
                    data = data.Where(c => c.Name.Contains(productsInAdmin.Search) || c.SearchText.Contains(productsInAdmin.Search));
                }
                ViewBag.pageid = productsInAdmin.PageId;
                ViewBag.Search = productsInAdmin.Search;
                return View(PagingList.Create(data.OrderByDescending(b => b.ProductId), 20, productsInAdmin.PageId));

            }
            catch
            {
                return Redirect("/");
            }

        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                ViewBag.Parentcatagories = _core.Catagory.Get(c => c.ParentId == null);
                return View();
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblProduct product,
                                                IFormFile MainImage
            )
        {

            try
            {
                ViewBag.Parentcatagories = _core.Catagory.Get(c => c.ParentId == null);
                if (ModelState.IsValid)
                {
                    if (MainImage == null)
                    {
                        ModelState.AddModelError("MainImage", "تصویر الزامی میباشد");
                        return await Task.FromResult(View(product));
                    }
                    else if (MainImage != null && !MainImage.IsImages())
                    {
                        ModelState.AddModelError("MainImage", " نوع فایل مشکل دارد");
                        return await Task.FromResult(View(product));
                    }
                    else
                    {
                        if (MainImage.Length > 3000000)
                        {
                            ModelState.AddModelError("MainImage", "حجم فایل عکس اصلی بیش از اندازه میباشد");
                            return await Task.FromResult(View(product));
                        }
                        else
                        {
                            //New Prodcut
                            TblProduct NewProduct = new TblProduct();
                            NewProduct.Name = product.Name;
                            if (MainImage != null)
                            {
                                NewProduct.MainImage = Guid.NewGuid().ToString() + Path.GetExtension(MainImage.FileName);
                                string saveDirectory = Path.Combine(
                                                        Directory.GetCurrentDirectory(), "wwwroot/Images/ProductMain");
                                string savePath = Path.Combine(saveDirectory, NewProduct.MainImage);

                                if (!Directory.Exists(saveDirectory))
                                {
                                    Directory.CreateDirectory(saveDirectory);
                                }

                                using (var stream = new FileStream(savePath, FileMode.Create))
                                {
                                    await MainImage.CopyToAsync(stream);
                                }
                                /// #region resize Image
                                ImageConvertor imgResizer = new ImageConvertor();
                                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ProductMain/thumb", NewProduct.MainImage);
                                imgResizer.Image_resize(savePath, thumbPath, 300);
                                /// #endregion

                            }
                            NewProduct.Price = product.Price;
                            NewProduct.DescriptionShortHtml = product.DescriptionShortHtml;
                            NewProduct.DescriptionLongHtml = product.DescriptionLongHtml;
                            NewProduct.CatagoryId = product.CatagoryId;
                            NewProduct.DateCreated = DateTime.Now;
                            NewProduct.SearchText = product.SearchText;
                            NewProduct.IsOfflineOrder = product.IsOfflineOrder;
                            _core.Product.Add(NewProduct);
                            _core.Save();
                            //New Prodcut



                            return await Task.FromResult(Redirect("/Admin/Product"));
                        }
                    }


                }

                return await Task.FromResult(View(product));
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }


        [HttpGet]
        public IActionResult PropertyList()
        {
            return ViewComponent("PropertyListAdmin");
        }
        public IActionResult AddProperty(int id)
        {
            try
            {
                ViewBag.id = id;
                List<TblProperty> list = _core.ProductPropertyRel.Get(i => i.ProductId == id).Select(i => i.Property).ToList();
                return View(_core.Product.GetById(id));
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public IActionResult AddProperty(int id, List<int?> PropertyId, List<string> Value)
        {
            try
            {
                List<TblProductPropertyRel> pros = new List<TblProductPropertyRel>();
                for (int i = 0; i < PropertyId.Count; i++)
                {
                    TblProductPropertyRel propertyRel = new TblProductPropertyRel();
                    propertyRel.ProductId = id;
                    if (Value[i] != null)
                    {
                        //propertyRel.PropertyId = PropertyId[i].Value;
                        //propertyRel.Value = "";
                        propertyRel.PropertyId = PropertyId[i].Value;
                        propertyRel.Value = Value[i];
                        pros.Add(propertyRel);
                    }
                }
                _core.ProductPropertyRel.Get(i => i.ProductId == id).ToList().ForEach(j => _core.ProductPropertyRel.Delete(j));
                _core.Save();
                foreach (var item in pros)
                {
                    if (!_core.ProductPropertyRel.Get().Any(i => i.ProductId == item.ProductId && i.PropertyId == item.PropertyId))
                    {
                        _core.ProductPropertyRel.Add(item);
                        _core.Save();
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }

        public IActionResult AddGallery(int id)
        {
            try
            {
                ViewBag.id = id;
                List<TblProductImageRel> list = _core.ProductImageRel.Get(i => i.ProductId == id).ToList();
                return View(list);
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddGallery(int id, List<IFormFile> GalleryFile)
        {
            try
            {

                if (GalleryFile.Count > 0)
                {
                    TblProduct product = _core.Product.GetById(id);
                    foreach (var galleryimage in GalleryFile)
                    {
                        if (galleryimage.IsImages() && galleryimage.Length < 3000000)
                        {
                            TblImage NewImage = new TblImage();
                            NewImage.ImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(galleryimage.FileName);
                            string savePathAlbum = Path.Combine(
                                                Directory.GetCurrentDirectory(), "wwwroot/Images/ProductAlbum/", NewImage.ImageUrl
                                            );
                            using (var stream = new FileStream(savePathAlbum, FileMode.Create))
                            {
                                await galleryimage.CopyToAsync(stream);
                            }
                            /// #region resize Image
                            ImageConvertor imgResizer = new ImageConvertor();
                            string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ProductAlbum/thumb/", NewImage.ImageUrl);
                            imgResizer.Image_resize(savePathAlbum, thumbPath, 300);
                            /// #endregion
                            _core.Image.Add(NewImage);
                            _core.Save();
                            TblProductImageRel imageRel = new TblProductImageRel();
                            imageRel.ProductId = product.ProductId;
                            imageRel.ImageId = NewImage.ImageId;

                            _core.ProductImageRel.Add(imageRel);
                            _core.Save();
                        }

                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }

        public string RemoveAlbumImage(int id)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ProductAlbum", _core.Image.GetById(id).ImageUrl);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            TblProductImageRel tblProductImageRel = _core.ProductImageRel.Get().Where(i => i.ImageId == id).SingleOrDefault();
            _core.ProductImageRel.Delete(tblProductImageRel);
            _core.Image.DeleteById(id);
            _core.Save();
            return "true";
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                ViewBag.Parentcatagories = _core.Catagory.Get(c => c.ParentId == null);
                return View(_core.Product.GetById(id));
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public IActionResult DeleteProperty(int id)
        {
            TblProductPropertyRel rel = _core.ProductPropertyRel.GetById(id);
            bool isDeleted = _core.ProductPropertyRel.Delete(rel);
            _core.Save();
            return Ok(isDeleted);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(TblProduct product,
                                                IFormFile MainImage
            )
        {
            try
            {
                ViewBag.CatagoryName = _core.Catagory.GetById(product.CatagoryId).Name;
                ViewBag.Parentcatagories = _core.Catagory.Get(c => c.ParentId == null);
                //ViewBag.Brands = _core.Brand.Get();
                if (ModelState.IsValid)
                {
                    TblProduct EditProduct = _core.Product.GetById(product.ProductId);

                    if (MainImage != null && MainImage.IsImages() && MainImage.Length < 3000000)
                    {
                        try
                        {
                            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ProductMain/", EditProduct.MainImage);
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                            var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ProductMain/thumb/", EditProduct.MainImage);
                            if (System.IO.File.Exists(imagePath2))
                            {
                                System.IO.File.Delete(imagePath2);
                            }
                        }
                        catch
                        {

                        }


                        EditProduct.MainImage = Guid.NewGuid().ToString() + Path.GetExtension(MainImage.FileName);
                        string saveDirectory = Path.Combine(
                                                Directory.GetCurrentDirectory(), "wwwroot/Images/ProductMain/");
                        string savePath = Path.Combine(saveDirectory, EditProduct.MainImage);

                        if (!Directory.Exists(saveDirectory))
                        {
                            Directory.CreateDirectory(saveDirectory);
                        }

                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await MainImage.CopyToAsync(stream);
                        }
                        /// #region resize Image
                        ImageConvertor imgResizer = new ImageConvertor();
                        string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ProductMain/thumb", EditProduct.MainImage);
                        imgResizer.Image_resize(savePath, thumbPath, 300);
                        /// #endregion
                    }

                    EditProduct.Name = product.Name;
                    EditProduct.Price = product.Price;
                    EditProduct.SearchText = product.SearchText;
                    EditProduct.IsOfflineOrder = product.IsOfflineOrder;
                    //EditProduct.BrandId = product.BrandId;
                    EditProduct.DescriptionShortHtml = product.DescriptionShortHtml;
                    EditProduct.DescriptionLongHtml = product.DescriptionLongHtml;
                    EditProduct.CatagoryId = product.CatagoryId;
                    _core.Product.Update(EditProduct);
                    _core.Save();
                    return await Task.FromResult(Redirect("/Admin/Product"));
                }

                return View(product);
            }
            catch
            {
                return RedirectToAction("Index");
            }


        }

        [HttpGet]
        public IActionResult StopSales(bool id)
        {
            List<TblProduct> products = _core.Product.Get().ToList();
            foreach (TblProduct i in products)
            {
                i.IsDeleted = id;
                _core.Product.Update(i);
            }

            _core.Save();
            return Redirect("/Admin/Product");
        }

        public IActionResult SpecialOffer(int id)
        {
            return ViewComponent("SpecialOfferAddAdmin", new { id = id });
        }

        public string Delete(int id)
        {
            try
            {
                TblProduct product = _core.Product.GetById(id);
                if (product.TblOrderDetails.Count() > 0)
                {
                    return "سفارشی برای این کالا وجود دارد";
                }
                else
                {
                    IEnumerable<TblProductImageRel> imageRels = _core.ProductImageRel.Get(pi => pi.ProductId == product.ProductId);
                    if (imageRels.Count() > 0)
                    {
                        foreach (var item in imageRels)
                        {
                            var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ProductAlbum", item.Image.ImageUrl);
                            if (System.IO.File.Exists(deleteImagePath))
                            {
                                System.IO.File.Delete(deleteImagePath);
                            }
                            var deleteImagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ProductAlbum/thumb/", item.Image.ImageUrl);
                            if (System.IO.File.Exists(deleteImagePath2))
                            {
                                System.IO.File.Delete(deleteImagePath2);
                            }
                            _core.Image.Delete(item.Image);
                            _core.Save();
                        }
                        _core.Save();
                    }
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ProductMain", product.MainImage);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ProductMain/thumb/", product.MainImage);
                    if (System.IO.File.Exists(imagePath2))
                    {
                        System.IO.File.Delete(imagePath2);
                    }
                    _core.Product.Delete(product);
                    _core.Save();
                    return "true";
                }
            }
            catch
            {
                return "خطا در حذف";
            }

        }

        public void Selling(int id)
        {
            TblProduct product = _core.Product.GetById(id);
            product.IsDeleted = !product.IsDeleted;
            _core.Product.Update(product);
            _core.Save();
        }
        public void IsOfflineOrder(int id)
        {
            TblProduct product = _core.Product.GetById(id);
            product.IsOfflineOrder = !product.IsOfflineOrder;
            _core.Product.Update(product);
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
