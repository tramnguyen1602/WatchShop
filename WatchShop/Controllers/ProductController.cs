﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using WatchShop.DAO;
using WatchShop.EntityFramework;
using PagedList;

namespace WatchShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product

        public ActionResult ProductList(string sortOrder, int? page)
        {          
            ProductDAO dao = new ProductDAO();
            IEnumerable<Product> list;
            ViewBag.CurrentSort = sortOrder;
            switch (sortOrder)
            {
                case "Newest":
                    list = dao.Newest();
                    break;
                case "BestSeller":
                    list = dao.BestSeller();
                    break;
                case "AscendingPrice":
                    list = dao.AscendingPrice();
                    break;
                case "DescendingPrice":
                    list = dao.DescendingPrice();
                    break;
                default:
                    list = dao.GetListProducts();
                    break;
            }
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

      

     



        // GET: Product/Details/5
        public ActionResult ProductDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new ProductDAO();


            Product product = dao.GetById(id);

            //Chuyển xml ảnh sang dạng list
            var images = product.MoreImages;
            List<string> listImagesReturn = new List<string>();
            if (images != null)
            {
                XElement xImages = XElement.Parse(images);
                foreach (XElement element in xImages.Elements())
                {
                    listImagesReturn.Add(element.Value);
                }
            }


            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Images = listImagesReturn;
            ViewBag.RelatedProducts = dao.GetRelatedProducts(id);
            return View(product);
        }

    }
}
