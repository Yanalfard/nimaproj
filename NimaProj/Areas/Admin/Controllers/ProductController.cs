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
    [PermissionChecker("admin")]
    public class ProductController : Controller
    {
        Core _core = new Core();
        [HttpGet]
        public async Task<IActionResult> Index(ProductsInAdminVm productsInAdmin)
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
                return await Task.FromResult(View(PagingList.Create(data.OrderByDescending(b => b.ProductId), 20, productsInAdmin.PageId)));
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
                ViewBag.Brands = _core.Brand.Get();
                ViewBag.Parentcatagories = _core.Catagory.Get(c => c.ParentId == null && c.IsBlog == false);
                return await Task.FromResult(View());
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
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
                ViewBag.Brands = _core.Brand.Get();
                ViewBag.Parentcatagories = _core.Catagory.Get(c => c.ParentId == null && c.IsBlog == false);
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
                            NewProduct.BrandId = product.BrandId;
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
                return await Task.FromResult(Redirect("Error"));
            }
        }


        [HttpGet]
        public async Task<IActionResult> PropertyList()
        {
            try
            {
                return await Task.FromResult(ViewComponent("PropertyListAdmin"));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        public async Task<IActionResult> AddProperty(int id)
        {
            try
            {
                ViewBag.id = id;
                List<TblProperty> list = _core.ProductPropertyRel.Get(i => i.ProductId == id).Select(i => i.Property).ToList();
                return await Task.FromResult(View(_core.Product.GetById(id)));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddProperty(int id, List<int?> PropertyId, List<string> Value)
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
                return await Task.FromResult(Redirect("/Admin/Product"));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        public async Task<IActionResult> AddGallery(int id)
        {
            try
            {
                ViewBag.id = id;
                List<TblProductImageRel> list = _core.ProductImageRel.Get(i => i.ProductId == id).ToList();
                return await Task.FromResult(View(list));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
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
                return await Task.FromResult(RedirectToAction("Index"));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
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
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                ViewBag.Brands = _core.Brand.Get();
                ViewBag.Parentcatagories = _core.Catagory.Get(c => c.ParentId == null && c.IsBlog == false);
                return await Task.FromResult(View(_core.Product.GetById(id)));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            try
            {
                TblProductPropertyRel rel = _core.ProductPropertyRel.GetById(id);
                bool isDeleted = _core.ProductPropertyRel.Delete(rel);
                _core.Save();
                return await Task.FromResult(Ok(isDeleted));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(TblProduct product,
                                                IFormFile MainImage
            )
        {
            try
            {
                ViewBag.Brands = _core.Brand.Get();
                ViewBag.Parentcatagories = _core.Catagory.Get(c => c.ParentId == null && c.IsBlog == false);
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
                    EditProduct.BrandId = product.BrandId;
                    _core.Product.Update(EditProduct);
                    _core.Save();
                    return await Task.FromResult(Redirect("/Admin/Product"));
                }
                return await Task.FromResult(View(product));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }


        }

        [HttpGet]
        public async Task<IActionResult> StopSales(bool id)
        {
            try
            {
                List<TblProduct> products = _core.Product.Get().ToList();
                foreach (TblProduct i in products)
                {
                    i.IsDeleted = id;
                    _core.Product.Update(i);
                }
                _core.Save();
                return await Task.FromResult(Redirect("/Admin/Product"));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        public async Task<IActionResult> SpecialOffer(int id)
        {
            try
            {
                return await Task.FromResult(ViewComponent("SpecialOfferAddAdmin", new { id = id }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
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
