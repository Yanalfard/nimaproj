﻿using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;
using Services.Repositories;

namespace Services.Services
{
    public class Core : IDisposable
    {
        private readonly NimaProjContext _context = new NimaProjContext();

        private MainRepo<TblBannerAndSlide> _bannerAndSlide;
        private MainRepo<TblBlog> _blog;
        private MainRepo<TblBlogCommentRel> _blogCommentRel;
        private MainRepo<TblCatagory> _catagory;
        private MainRepo<TblClient> _client;
        private MainRepo<TblComment> _comment;
        private MainRepo<TblConfig> _config;
        private MainRepo<TblContactU> _contactUs;
        private MainRepo<TblDiscount> _discount;
        private MainRepo<TblImage> _image;
        private MainRepo<TblOrder> _order;
        private MainRepo<TblOfflineOrder> _offlineOrder;
        private MainRepo<TblOrderDetail> _orderDetail;
        private MainRepo<TblProduct> _product;
        private MainRepo<TblProductCommentRel> _productCommentRel;
        private MainRepo<TblProductImageRel> _productImageRel;
        private MainRepo<TblProductPropertyRel> _productPropertyRel;
        private MainRepo<TblProperty> _property;
        private MainRepo<TblRegularQuestion> _regularQuestion;
        private MainRepo<TblRole> _role;
        private MainRepo<TblSpecialOffer> _specialOffer;
        private MainRepo<TblVisit> _visit;


        public MainRepo<TblBannerAndSlide> BannerAndSlide => _bannerAndSlide ??= new MainRepo<TblBannerAndSlide>(_context);
        public MainRepo<TblBlog> Blog => _blog ??= new MainRepo<TblBlog>(_context);
        public MainRepo<TblBlogCommentRel> BlogCommentRel => _blogCommentRel ??= new MainRepo<TblBlogCommentRel>(_context);
        public MainRepo<TblCatagory> Catagory => _catagory ??= new MainRepo<TblCatagory>(_context);
        public MainRepo<TblClient> Client => _client ??= new MainRepo<TblClient>(_context);
        public MainRepo<TblComment> Comment => _comment ??= new MainRepo<TblComment>(_context);
        public MainRepo<TblConfig> Config => _config ??= new MainRepo<TblConfig>(_context);
        public MainRepo<TblContactU> ContactU => _contactUs ??= new MainRepo<TblContactU>(_context);
        public MainRepo<TblDiscount> Discount => _discount ??= new MainRepo<TblDiscount>(_context);
        public MainRepo<TblImage> Image => _image ??= new MainRepo<TblImage>(_context);
        public MainRepo<TblOrder> Order => _order ??= new MainRepo<TblOrder>(_context);
        public MainRepo<TblOfflineOrder> OfflineOrder => _offlineOrder ??= new MainRepo<TblOfflineOrder>(_context);
        public MainRepo<TblOrderDetail> OrderDetail => _orderDetail ??= new MainRepo<TblOrderDetail>(_context);
        public MainRepo<TblProduct> Product => _product ??= new MainRepo<TblProduct>(_context);
        public MainRepo<TblProductCommentRel> ProductCommentRel => _productCommentRel ??= new MainRepo<TblProductCommentRel>(_context);
        public MainRepo<TblProductImageRel> ProductImageRel => _productImageRel ??= new MainRepo<TblProductImageRel>(_context);
        public MainRepo<TblProductPropertyRel> ProductPropertyRel => _productPropertyRel ??= new MainRepo<TblProductPropertyRel>(_context);
        public MainRepo<TblProperty> Property => _property ??= new MainRepo<TblProperty>(_context);
        public MainRepo<TblRegularQuestion> RegularQuestion => _regularQuestion ??= new MainRepo<TblRegularQuestion>(_context);
        public MainRepo<TblRole> Role => _role ??= new MainRepo<TblRole>(_context);
        public MainRepo<TblSpecialOffer> SpecialOffer => _specialOffer ??= new MainRepo<TblSpecialOffer>(_context);
        public MainRepo<TblVisit> Visit => _visit ??= new MainRepo<TblVisit>(_context);

        public void Save() => _context.SaveChanges();

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
