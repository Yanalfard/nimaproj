using DataLayer.Models;
using DataLayer.ViewModels.View;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using ReflectionIT.Mvc.Paging;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.Controllers
{
    public class ProductController : Controller
    {
        private Core _core = new Core();
        [Route("Product/{id?}/{name?}")]
        public async Task<IActionResult> Index(int page = 1, int? id = 0, string name = "", string nameSearch = "")
        {
            try
            {

                List<TblProduct> list = new List<TblProduct>();
                if (id != 0)
                {
                    ViewBag.name = name;
                    list.AddRange(_core.Product.Get(i => (i.CatagoryId == id || i.Catagory.ParentId == id) && (i.IsDeleted == false),
                   orderBy: i => i.OrderByDescending(i => i.ProductId)));
                }
                else
                {
                    ViewBag.name = "همه محصولات";
                    list.AddRange(_core.Product.Get(i => i.IsDeleted == false,
                        orderBy: i => i.OrderByDescending(i => i.ProductId)));
                }
                if (!string.IsNullOrEmpty(nameSearch))
                {
                    ViewBag.name = nameSearch;
                    list = list.Where(i => i.Name.Contains(nameSearch) || i.SearchText.Contains(nameSearch)).ToList();
                }
                //    return View(PagingList.Create(list, 2, page));
                return View(list);
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }


        }
        [Route("ViewProduct/{id}/{name}")]
        public async Task<IActionResult> ProductView(int id, string name = "")
        {
            try
            {
                TblProduct selectedProduct = _core.Product.GetById(id);
                List<TblProduct> list = _core.Product.Get(i => i.ProductId != id &&
                i.CatagoryId == selectedProduct.CatagoryId ||
                  i.SearchText.Contains(selectedProduct.Name)).ToList();
                list = list.Distinct().ToList();
                ViewBag.listProducts = list;
                list.ShuffleList();
                return await Task.FromResult(View(selectedProduct));

            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        public async Task<IActionResult> SendComment(SendCommentVm comment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TblComment addComment = new TblComment();
                    addComment.Body = comment.Body;
                    //addComment.ClientId = SelectUser().ClientId;
                    addComment.ClientId = 1;
                    addComment.DateCreated = DateTime.Now;
                    addComment.ParentId = comment.ParentId;
                    if (comment.ProductId == null)
                    {
                        addComment.IsBlog = true;
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        if (User.Claims.Last().Value != "admin")
                        {
                            addComment.IsValid = true;
                        }
                    }
                    _core.Comment.Add(addComment);
                    _core.Save();
                    if (comment.ParentId == null && comment.BlogId == null)
                    {
                        TblProductCommentRel addCommentRel = new TblProductCommentRel();
                        addCommentRel.ProductId = (int)comment.ProductId;
                        addCommentRel.CommentId = addComment.CommentId;
                        _core.ProductCommentRel.Add(addCommentRel);
                        _core.Save();
                    }
                    else if (comment.ParentId == null && comment.ProductId == null)
                    {
                        TblBlogCommentRel addCommentRel = new TblBlogCommentRel();
                        addCommentRel.BlogId = Convert.ToInt32(comment.BlogId);
                        addCommentRel.CommentId = addComment.CommentId;
                        _core.BlogCommentRel.Add(addCommentRel);
                        _core.Save();
                    }
                    return await Task.FromResult(PartialView());
                }
                return await Task.FromResult(PartialView(comment));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
    }
}
