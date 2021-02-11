using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using PrachiIndia.Portal.Helpers;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Microsoft.Ajax.Utilities;
using PrachiIndia.Portal.Framework;
using PrachiIndia.Portal.Models;

namespace PrachiIndia.Portal.Controllers
{
    //[Authorize(Roles = "User,School")]

    public class CatalogueController : Controller
    {
        private IEnumerable<tblCataLog> results;

        // GET: Catalogue
        public ActionResult Index(string isFutureTrack="")
        {
            ViewBag.isFutureTrack = isFutureTrack;
            filedropdwon();
            return View();
        }
        #region All filter and serach shorting from cart page onely this methode is use for resulte start here 
        public ContentResult GetItems(Filters filter)
        {
            var itemRepository = new CatalogRepository();
            var classRepository = new MasterClassRepository();
            var boardRepository = new MasterBoardRepository();
            var categoryies = new MasterSubjectRepository();
            var Series = new MasterSeriesRepositories();
            var ImageUrl = ConfigurationManager.AppSettings["ImageUrl"].ToString(CultureInfo.InvariantCulture);
            var count = 0;
            var countryId = GetCountryId();
            var price = 1;
            if (countryId != -1 && countryId != 86)
            {
                price = 0;
                results = Helpers.Utility.objtblCataLog.Where(t => t.Ebook == 1).ToList<tblCataLog>();
            }
            else
            {
                results = Helpers.Utility.objtblCataLog.ToList<tblCataLog>();
            }
            if (filter.parentId != 0)
            {
                results = results.Where(s => s.SubjectId == filter.parentId).ToList<tblCataLog>();
            }
            if (!string.IsNullOrEmpty(filter.MinPrice) && !string.IsNullOrEmpty(filter.MaxPrice))
            {
                var min = decimal.Parse(filter.MinPrice);
                var max = decimal.Parse(filter.MaxPrice);
                results = results.Where(s => s.PrintPrice >= min && s.PrintPrice <= max).ToList<tblCataLog>();
            }
            if (filter.Board != null)
            {
                //Modifed by Rahul on 30/08/2017 
                results = results.Where(s => filter.Board.Contains(s.BoardId)).ToList<tblCataLog>();

            }
            if (filter.Class != null)
            {
                results = results.Where(s => filter.Class.Contains(s.ClassId)).ToList<tblCataLog>();
            }

            if (filter.subject != null)
            {
                //if (filter.subject.Contains("29"))
                //    results = results.Where(s => s.SubjectId.ToString() == "29").OrderBy(t => t.OrderNo).ToList<tblCataLog>();
                //else
                    results = results.Where(s => filter.subject.Contains(s.SubjectId.ToString())).ToList<tblCataLog>();
            }
            if (filter.Series != null)
            {
                results = results.Where(s => filter.Series.Contains(s.SeriesId.ToString())).ToList<tblCataLog>();
            }
            // results = results.DistinctBy(isbn => new { isbn.ISBN, isbn.ClassId }).ToList<tblCataLog>();
            count = results.Count();
            switch (filter.Short)
            {
                case "high":
                    results = results.OrderBy(s => s.Price).Skip(filter.pageIndex * filter.pageSize).Take(filter.pageSize).ToList<tblCataLog>();
                    break;
                case "Low":
                    results = results.OrderBy(s => s.Price).Skip(filter.pageIndex * filter.pageSize).Take(filter.pageSize).ToList<tblCataLog>();
                    break;
                default:
                    if (filter.subject != null)
                    {
                        if (filter.subject.Contains("29"))
                            results = results.OrderBy(x => x.OrderNo).ToList();
                       
                    }
                    results = results.Skip(filter.pageIndex * filter.pageSize).Take(filter.pageSize).Distinct().ToList<tblCataLog>();
                    break;
            }

            var items = (from item in results
                         select new ItemVM
                         {
                             Id = item.Id,
                             itemprice = String.Format("{0:0.00}", item.PrintPrice),          // "123.0"//item.Price,
                             ebookitemprice = String.Format("{0:0.00}", item.EbookPrice),
                             Title = item.Title,
                             Image = ImageUrl + ((item.Image != null) ? item.Image : ("no-image.png")),//item.Image?? "no_image.jpg",
                             ClassId = Helpers.Utility.classesByClassID(item.ClassId),
                             Class_ID = Convert.ToInt32(item.ClassId),
                             BoardId = Helpers.Utility.BoardsByClassID(item.BoardId),
                             Author = string.IsNullOrEmpty(item.Author) ? "In House Author" : item.Author,
                             ISBN = string.IsNullOrEmpty(item.ISBN) ? "xxx-xx-xxxx-xxx-x" : item.ISBN,
                             count = count,
                             Ebook = item.Ebook,
                             Discount = item.Discount,
                             Price = price,
                             EbookDiscount = item.EbookDiscount,
                             OrderNo = item.OrderNo
                         }).ToList();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };

            var result = new List<ItemVM>();
           
          
            return new ContentResult()
            {
                Content = serializer.Serialize(items),
                ContentType = "application/json",
            };
        }
        #endregion

        public class searchlist
        {
            public Int64 value { get; set; }
            public string label { get; set; }
        }
        #region for searching from cart using auto complte start here
        public ContentResult Searchresulte(string id)
        {
            //SearchFilter filter
            var itemRepository = new CatalogRepository();
            var classRepository = new MasterClassRepository();
            var categoryies = new MasterSubjectRepository();
            var Series = new MasterSeriesRepositories();
            IQueryable<tblCataLog> query = itemRepository.GetAll();
            var eligs = new List<searchlist>();
            if (id != null)//filter.Subject != null)
            {
                //id = Request.QueryString["term"];
                var elig = query.Where(t => (t.MasterSubject.Title).Contains(id) || (t.MasterSery.Title).Contains(id))
                           .Select(x => new { x.MasterSery.Id, x.MasterSery.Title }).Distinct();

                eligs = (from item in elig
                         select new searchlist
                         {
                             value = item.Id,
                             label = item.Title
                         }).ToList();

            }
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(eligs),
                ContentType = "application/json",
            };
        }
        public ContentResult SearchId(SearchFilter filter)
        {
            var itemRepository = new CatalogRepository();
            var classRepository = new MasterClassRepository();
            var categoryies = new MasterSubjectRepository();
            var Series = new MasterSeriesRepositories();
            IQueryable<tblCataLog> query = itemRepository.GetAll();

            var eligs = new List<searchlist>();
            if (filter.title != null)
            {
                var elig = query.Where(t => filter.title.ToLower().Contains(t.MasterSubject.Title.ToLower()) || filter.title.ToLower().Contains(t.MasterSery.Title) && t.MasterSery.Id == filter.Id)
                           .Select(x => new { x.MasterSery.Id, x.MasterSery.Title }).Distinct().SingleOrDefault();


                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
                return new ContentResult()
                {
                    Content = serializer.Serialize(elig),
                    ContentType = "application/json",
                };
            }
            else
            {
                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
                return new ContentResult()
                {
                    Content = serializer.Serialize(null),
                    ContentType = "application/json",
                };
            }

        }
        #endregion
        #region AddToCart Work Start Here
        [Authorize]
        public async Task<ContentResult> AddToCart(string BookId, string Ebook = "0", string Pbook = "0",bool solutions = false)
        {
            string results = "failure";
            var id = int.Parse(BookId);
            if (Ebook == Framework.BookType.EBook.ToString() && Pbook == Framework.BookType.PBook.ToString())
            {
                AddBookToCart(id, BookType.EBook, false, solutions);
                results = AddBookToCart(id, BookType.PBook, false);
            }
            else if (Ebook == Framework.BookType.EBook.ToString())
            {

                results = AddBookToCart(id, BookType.EBook, false, solutions);
            }
            else if (Pbook == Framework.BookType.PBook.ToString())
            {
                results = AddBookToCart(id, BookType.PBook, false, solutions);
            }
            else if (solutions == true)
            {
                results = AddBookToCart(id, BookType.EBook, false, solutions);
            }
            else
            {
                results = AddBookToCart(id, BookType.PBook, false, solutions);
            }
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(results),
                ContentType = "application/json",
            };
        }
        public ActionResult Cart()
        {
            var cartRepository = new CartRepository();
            var itemRepository = new CatalogRepository();
            var userId = User.Identity.GetUserId();
            var carts = cartRepository.SearchFor(t => t.UserId == userId && t.IsWishList != true).ToList();
            var results = (from cartItem in carts
                           select new ProductItem
                           {
                               Id = cartItem.Id,
                               Price = String.Format("{0:0.00}", cartItem.tblCataLog.Price),
                               Quantity = cartItem.Quntity ?? 0,
                               Title = cartItem.tblCataLog.Title,
                               ItemId = cartItem.ItemId ?? 0,
                               EbookPrice = String.Format("{0:0.00}", cartItem.tblCataLog.Price),
                               Discount = cartItem.Discount ?? 0,
                               IsSolution = cartItem.IsSolution??false,
                               SolutionPrice = cartItem.tblCataLog.SolutionPrice??0
                           }).ToList();
            return View(results);
        }
        //this methode is use for Get Cart Value for cart
        public ContentResult GetCarts()
        {
            var context = new dbPrachiIndia_PortalEntities();
            var cartRepository = new CartRepository();
            var itemRepository = new CatalogRepository();
            var ImageUrl = ConfigurationManager.AppSettings["ImageUrl"].ToString(CultureInfo.InvariantCulture);
            var userId = User.Identity.GetUserId();
            var carts = cartRepository.GetAll().Where(t => t.UserId == userId && t.IsWishList != true).ToList();//"c667741a-27d1-4438-8662-cc51a784e5d6").ToList();
            var results = new List<ProductItem>();
            foreach (var cartItem in carts)
            {
                //var taxPrice = 0;
                var taxRate = context.GSTTaxLists.FirstOrDefault(t => t.Id == cartItem.tblCataLog.Tax);
                var bookprice = string.Empty;
                //if (cartItem.IsSolution ?? false)
                //{
                //    bookprice = String.Format("{0:0.00}", cartItem.tblCataLog.SolutionPrice);
                //}
                //else {
                //    bookprice = String.Format("{0:0.00}", cartItem.tblCataLog.Price);
                //}
                var item = new ProductItem
                {
                    Id = cartItem.Id,
                    Image = ImageUrl + cartItem.tblCataLog.Image,//cartItem.Item.Image,

                    Price = String.Format("{0:0.00}", cartItem.tblCataLog.Price),//cartItem.Item.Price ?? 0,//String.Format("{0:0.00}", item.Price)

                    Quantity = cartItem.Quntity ?? 0,
                    Title = cartItem.tblCataLog.Title,
                    ItemId = cartItem.ItemId ?? 0,
                    Discount = cartItem.Discount ?? 0,
                    // EbookPrice = String.Format("{0:0.00}", cartItem.tblCataLog.Price),
                    Classname = cartItem.tblCataLog.ClassId,
                    BoardName = cartItem.tblCataLog.BoardId,
                    BookType = cartItem.BookType.ToString(),
                    TaxRate = taxRate != null ? taxRate.Rate : 0,
                    //SolutionPrice = cartItem.tblCataLog.SolutionPrice??0,
                   // IsSolution=cartItem.tblCataLog.IsSolution??false
                };
                if (cartItem.BookType == BookType.PBook)
                {
                    item.Price = String.Format("{0:0.00}", cartItem.tblCataLog.PrintPrice);
                }
                else if(cartItem.BookType == BookType.EBook && cartItem.IsSolution== true)
                {
                    item.IsSolution = true;
                    item.SolutionPrice = cartItem.tblCataLog.SolutionPrice??0;
                    item.Title = "Solutions of " + item.Title  + " (Online version)";
                    item.Price = String.Format("{0:0.00}", cartItem.tblCataLog.SolutionPrice);
                }
                else if (cartItem.BookType == BookType.EBook)
                {
                    item.Price = String.Format("{0:0.00}", cartItem.tblCataLog.EbookPrice);
                }
                else if (cartItem.BookType == BookType.Both)
                {
                    var price = cartItem.tblCataLog.PrintPrice + cartItem.tblCataLog.EbookPrice;
                    item.Price = String.Format("{0:0.00}", price);
                }
                results.Add(item);

            }

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(results),
                ContentType = "application/json",
            };
        }
        #endregion
        #region wishlist Work Start Here
        public async Task<ContentResult> AddToWishList(string BookId, string Ebook = "0", string Pbook = "0")
        {
            string results = "failure";
            var id = int.Parse(BookId);
            if (Ebook == Framework.BookType.EBook.ToString() && Pbook == Framework.BookType.PBook.ToString())
            {
                AddBookToCart(id, BookType.EBook, true);
                results = AddBookToCart(id, BookType.PBook, true);
            }
            else if (Ebook == Framework.BookType.EBook.ToString())
            {

                results = AddBookToCart(id, BookType.EBook, true);
            }
            else if (Pbook == Framework.BookType.PBook.ToString())
            {
                results = AddBookToCart(id, BookType.PBook, true);
            }
            else
            {

                results = AddBookToCart(id, BookType.PBook, true);
            }

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(results),
                ContentType = "application/json",
            };
        }

        string AddBookToCart(int BookId, int ebooks, bool IsWishList,bool solution=false)
        {
            var cartRepository = new CartRepository();
            var itemRepository = new CatalogRepository();
            var result = itemRepository.FindByIdAsync(BookId);
            var issolution = false;
            //if (solution)
            //{
            //    issolution= solution
            //}
            dynamic discount;
            if (ebooks == 2)
            {
                discount = result.EbookDiscount  * Convert.ToInt32(result.EbookPrice) /100;
            }
            else
            {
                discount = result.Discount * Convert.ToInt32(result.PrintPrice) / 100;
            }
            string results = "failure";
            if (result != null)
            {
                var cart = new Cart
                {
                    ItemId = BookId,
                    UserId = User.Identity.GetUserId(),
                    CreatedOn = DateTime.UtcNow,
                    Quntity = 1,
                    UpdatedOn = DateTime.UtcNow,
                    Status = true,
                    Discount = discount,
                    BookType = ebooks,
                    IsWishList = IsWishList,
                    IsSolution=solution
                };
                var item = cartRepository.FindByItemAndUserId(cart.ItemId, cart.UserId, cart.BookType ?? 0, cart.IsWishList ?? false);
                if (item == null)
                {
                    cartRepository.CreateAsync(cart);
                    results = "success";
                }
                else if(item.IsSolution??false)
                {
                    results = "Solution already in cart";
                }
                else
                {
                    item.Quntity = item.Quntity + 1;
                    item.UpdatedOn = DateTime.UtcNow;
                    cartRepository.UpdateAsync(item);
                    results = "success";
                }
            }
            return results;
        }
        public ContentResult GetWishList()
        {
            var cartRepository = new CartRepository();
            var itemRepository = new CatalogRepository();
            var ImageUrl = ConfigurationManager.AppSettings["ImageUrl"].ToString(CultureInfo.InvariantCulture);
            var carts = cartRepository.FindByCarts(User.Identity.GetUserId()).Where(t => t.IsWishList == true).ToList();//"c667741a-27d1-4438-8662-cc51a784e5d6").ToList();
            var results = (from cartItem in carts
                           select new ProductItem
                           {
                               Id = cartItem.Id,
                               Image = ImageUrl + cartItem.tblCataLog.Image,//cartItem.Item.Image,
                               Price = String.Format("{0:0.00}", cartItem.tblCataLog.Price),//cartItem.Item.Price ?? 0,//String.Format("{0:0.00}", item.Price)
                               Quantity = cartItem.Quntity ?? 0,
                               Title = cartItem.tblCataLog.Title,
                               ItemId = cartItem.ItemId ?? 0,
                               Discount = cartItem.Discount ?? 0,
                               EbookPrice = String.Format("{0:0.00}", cartItem.tblCataLog.Price),
                               Classname = cartItem.tblCataLog.ClassId,
                               BoardName = cartItem.tblCataLog.BoardId,
                               BookType = cartItem.BookType.ToString()
                           }).ToList();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(results),
                ContentType = "application/json",
            };
        }
        public async Task<ContentResult> RemoveWishList(int Id)
        {
            var cartRepository = new CartRepository();
            try
            {
                var CartItem = cartRepository.FindByCart(Id);
                cartRepository.RemoveAsync(CartItem);
            }
            catch (Exception ex)
            {
            }
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(""),
                ContentType = "application/json",
            };
        }
        public async Task<ContentResult> AddwishlistToCart()
        {
            var cartRepository = new CartRepository();
            Cart objcart = new Sql.Cart();
            try
            {
                var UserId = User.Identity.GetUserId();
                var CartItem = cartRepository.GetAllCart(UserId);
                foreach (var item in CartItem)
                {
                    objcart.Id = item.Id;
                    objcart.IsWishList = false;
                    var result = cartRepository.UpdateWishListToCart(objcart);
                }
            }
            catch (Exception ex)
            {
                string exc = ex.Message;
            }
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(""),
                ContentType = "application/json",
            };

        }
        #endregion
        //this method use for get series descripion for detail page
        public ActionResult GetSeriesDesc(int ID)
        {

            var CategoryRepository = new CatalogRepository();
            var SeriesRepository = new MasterSeriesRepositories();
            List<SeriesVm> LIST = new List<SeriesVm>();

            var result = SeriesRepository.GetAll().Where(t => t.Id == ID).OrderBy(t => t.OredrNo).ToList();
            foreach (var item in result)
            {
                var Seriess = new SeriesVm
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    SubjectId = item.SubjectId,
                    Image = item.Image
                };
                LIST.Add(Seriess);
            }
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(LIST),
                ContentType = "application/json",
            };
        }



        #region Added For Remove Add Cart element from here start here by deepak:06_09_2016 , 26-12-2019
        public async Task<ContentResult> RemoveCart(int Id)
        {
            var cartRepository = new CartRepository();
            var CartItem = cartRepository.FindByCartId(Id);
            cartRepository.RemoveAsync(CartItem);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(""),
                ContentType = "application/json",
            };
        }
        #endregion

        #region Update Quntity from add cart start here start by deepak:06_09_2016
        public async Task<ContentResult> UpdateItemQuntity(int Quantity, int Id)
        {
            var cartRepository = new CartRepository();

            var CartItem = cartRepository.FindByCartId(Id);
            CartItem.Id = Id;
            CartItem.Quntity = Quantity;
            cartRepository.UpdateAsync(CartItem);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(""),
                ContentType = "application/json",
            };
        }
        #endregion
        #region Added For Get Subject At Catelog Page Created By Deepak start Here :17_08_2016
        public ContentResult GetSubJect(string ID)
        {
            var CategoryRepository = new MasterSubjectRepository();

            var results = CategoryRepository.GetAll().Where(t => t.Status == 1).ToList().OrderBy(t => t.OredrNo);
            var Item = (from item in results
                        select new Sql.MasterSubject
                        {
                            Id = item.Id,
                            Title = item.Title,
                        }).ToList();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(Item),
                ContentType = "application/json",
            };
        }
        #endregion
        //heere getseries calliung at catalouge page for fillter
        public ContentResult GetSeries(List<string> Id)
        {
            var CategoryRepository = new CatalogRepository();
            var SeriesRepository = new MasterSeriesRepositories();
            var subjectRepository = new MasterSubjectRepository();
            List<SeriesVm> LIST = new List<SeriesVm>();
            List<Sql.MasterSery> result = new List<MasterSery>();
            //if (Id > 0) { }

            //Added by Rahul on 30/08/2017
            //Resolved null reference excepction. 
            if (Id != null)
            {

                result = SeriesRepository.GetAll().Where(t => Id.Contains(t.SubjectId.ToString()) && t.Status == 1).OrderBy(t => t.OredrNo).ToList();
            }
            else
            {
                result = SeriesRepository.GetAll().Where(x => 1 == 2 && x.Status == 1).OrderBy(t => t.OredrNo).ToList();
            }

            foreach (var item in result)
            {
                var subId = item.SubjectId ?? 0;
                var sub = subjectRepository.GetByIdAsync(subId);
                var Seriess = new SeriesVm
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    SubjectId = item.SubjectId,
                    SubjectName = sub != null ? sub.Title : string.Empty,
                    Image = item.Image
                };
                LIST.Add(Seriess);
            }
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(LIST.OrderBy(t => t.SubjectId)),
                ContentType = "application/json",
            };
        }
        //#region Added For Bind Class At Catelog Page 17_08_2016
        public ContentResult GetClass()
        {
            var ClassRepository = new MasterClassRepository();
            var results = ClassRepository.GetAll().Where(x => x.Status == 1).ToList().OrderBy(s => s.OredrNo);
            var Item = (from item in results
                        select new Sql.MasterClass
                        {
                            Id = item.Id,
                            Title = item.Title,
                            OredrNo = item.OredrNo
                        }).ToList();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(Item),
                ContentType = "application/json",
            };
        }
        //#endregion
        //#region Added For Bind Board At Catelog Page 17_08_2016
        public ContentResult GetBoard()
        {
            var BoardRepository = new MasterBoardRepository();
            var results = BoardRepository.GetAll().Where(x => x.Status == 1).ToList();
            var Item = (from item in results
                        select new Sql.MasterBoard
                        {
                            Id = item.Id,
                            Title = item.Title
                        }).ToList();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(Item),
                ContentType = "application/json",
            };
        }

        public ContentResult GetSubMenu()
        {
            var SubjectRepository = new MasterSubjectRepository();
            var results = SubjectRepository.GetAll().Where(t => t.Status == 1).OrderBy(t => t.OredrNo).ToList();
            var Item = (from item in results
                        select new Sql.MasterSubject
                        {
                            Id = item.Id,
                            Title = item.Title
                        }).ToList();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(Item),
                ContentType = "application/json",
            };
        }
        //here we use detail for indivisule book detail
        [ValidateInput(false)]
        public ActionResult detail(string Id)
        {
            var itemRepository = new CatalogRepository();
            var list = GetAllCountry();
            var userId = User.Identity.GetUserId();
            var otherThenIndia = false;
            var user = new dbPrachiIndia_PortalEntities().AspNetUsers.FirstOrDefault(t => t.Id == userId);
            if (user != null && user.CountryId != 0 && user.CountryId != null && user.CountryId != 86)
            {
                otherThenIndia = true;
            }
            ViewData["list"] = list;
            TempData["id"] = Id;
            int itemId = Utility.ToSafeInt(TempData["id"]);//Convert.ToInt32(Id);
            var ImageUrl = ConfigurationManager.AppSettings["ImageUrl"].ToString(CultureInfo.InvariantCulture);
            var result = itemRepository.FindByIdAsync(itemId);

            if (result == null)
                return RedirectToAction("Index");


            var ClassRepository = new MasterClassRepository();
            var BoardRepository = new MasterBoardRepository();
            int ClassID = Utility.ToSafeInt(result.ClassId);
            int BoardID = Utility.ToSafeInt(result.BoardId);
            var Class = ClassRepository.GetAll().Where(i => i.Id == ClassID).Select(c => new { class_value = c.Title }).First().class_value.ToString();
            var Board = BoardRepository.GetAll().Where(i => i.Id == BoardID).Select(c => new { Board_value = c.Title }).First().Board_value.ToString();

            var Item =
                         new ItemVM
                         {
                             Id = result.Id,
                             Title = result.Title,
                             Image = ImageUrl + ((result.Image != null) ? result.Image : ("no_image.jpg")),
                             Description = result.Description,
                             ISBN = result.ISBN,
                             Author = result.Author,
                             Discount = result.Discount,
                             Price = result.Price,
                             Status = result.Status,
                             Class = Class,
                             BoardId = Board,
                             PrintPrice = otherThenIndia == true ? 0 : result.PrintPrice,
                             EbookPrice = result.EbookPrice,
                             EbookDiscount=result.EbookDiscount,
                             SeriesDescription = result.MasterSery.Description,
                             Ebook = result.Ebook,
                             Youtubelink=result.Youtubelink
                         };

            ViewData["item"] = Item;

            return View();

        }
        //this methode use for login popup when end user not login and want to add book at cart
        [ChildActionOnly]
        public PartialViewResult _login()
        {
            return PartialView();
        }
        #region Here we use country state and city master
        private object GetAllCountry()
        {
            var CountryRepo = new CountryRepositories();
            //var CountryList = CountryRepo.GetAll().Where(t => t.Status == true).ToList();
            var CountryList = CountryRepo.GetAll().ToList();
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            foreach (var item in CountryList)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = item.Name, Value = item.CountryId.ToString() });
            }
            return list;
        }
        [AllowAnonymous]
        public object GetSates()
        {
            var StateRepo = new StateRepositories();
            var StateList = StateRepo.GetAll().ToList();
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            foreach (var item in StateList)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = item.StateName, Value = item.StaeteId.ToString() });
            }
            return list;
        }

        [AllowAnonymous]
        public object GetCities()
        {
            var CityRepo = new CityRepositories();
            var CityList = CityRepo.GetAll().ToList();
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            foreach (var item in CityList)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = item.CityName, Value = item.CityId.ToString() });
            }
            return list;
        }
        [AllowAnonymous]
        public JsonResult GetCity(string StateId)
        {
            var CityRepo = new CityRepositories();
            var CityList = CityRepo.GetAll().ToList();
            var district = from s in CityList.ToList()
                           where s.StateId == Convert.ToInt32(StateId)
                           select s;
            return Json(new SelectList(district.ToArray(), "CityId", "CityName"), JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult GetSate(string id)
        {
            var StateRepo = new StateRepositories();
            var StateList = StateRepo.GetAll().ToList();
            var district = from s in StateList.ToList()
                           where s.CountryId == Convert.ToInt32(id)
                           select s;
            return Json(new SelectList(district.ToArray(), "StaeteId", "StateName"), JsonRequestBehavior.AllowGet);
        }
        private void filedropdwon()
        {
            var list = GetAllCountry();
            ViewData["list"] = list;
        }
        #endregion
        // from this method we get user detail at checkout page 
        //here we get cart user information
        public ContentResult GetCartUserInfo()
        {
            VmShipingAddress model = new VmShipingAddress();
            var CountryRepo = new CountryRepositories();
            var StateRepo = new StateRepositories();
            var CityRepo = new CityRepositories();
            var user = new AspNetUserRepository().FindById(User.Identity.GetUserId());

            if (user != null)
            {
                foreach (var item in user)
                {
                    var Cid = Convert.ToInt32(item.Country != null ? item.Country : "0");
                    var Sid = Convert.ToInt32(item.State != null ? item.State : "0");
                    var CiId = Convert.ToInt32(item.City != null ? item.City : "0");
                    var countryid = CountryRepo.FindByItemId(Cid) != null ? CountryRepo.FindByItemId(Cid).Name : "India";
                    var StateId = StateRepo.FindByItemId(Sid) != null ? StateRepo.FindByItemId(Sid).StateName : "";
                    var CityId = CityRepo.FindByItemId(CiId) != null ? CityRepo.FindByItemId(CiId).CityName : "";
                    model.Name = item.FirstName;
                    model.Email = item.Email;
                    model.Country = countryid;
                    model.City = CityId;
                    model.State = StateId;
                    model.Pincode = item.PinCode;
                    model.PhoneNumber = item.PhoneNumber;
                    model.Address = item.Address;
                }

            }
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(model),
                ContentType = "application/json",
            };
        }
        //here we add address book for end user but now it not use from this method we affect AddressBook Table .
        public ContentResult AddUserAddress(UserAddressBook model)
        {
            var UserAddressBook = new UserAddressBookRepository();
            var results = false;
            var user = new AspNetUserRepository().FindById(User.Identity.GetUserId());
            foreach (var item in user)
            {
                model.UserId = item.Id;
                model.IsActive = true;
                model.CreatedBy = item.Id;
                model.CreatedOn = DateTime.Now;
            }
            var userdetail = UserAddressBook.FindById(model.UserId);
            if (userdetail != null)
            {
                var resulte = UserAddressBook.CreateAsync(model);
                var abc = resulte.Status;
                results = resulte.IsCanceled;
            }
            else
            {
                var resulte = UserAddressBook.UpdateAsync(model);
                var abc = resulte.Status;
                results = resulte.IsCanceled;
            }
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(results),
                ContentType = "application/json",
            };
        }
        //here ue get user billing address 
        public ContentResult GetUserBillingAddress()
        {
            AspNetUser model = new AspNetUser();
            var UserAddressBook = new UserAddressBookRepository();
            var AspNetUser = new AspNetUserRepository();
            var userId = User.Identity.GetUserId();
            var user = AspNetUser.FindByIdAsync(userId);
            var CountryRepo = new CountryRepositories();
            var StateRepo = new StateRepositories();
            var CityRepo = new CityRepositories();
            //foreach (var item in user)
            //{
            //    var Cid = Convert.ToInt32(item.Country != null ? item.Country : "0");
            //    var Sid = Convert.ToInt32(item.State != null ? item.State : "0");
            //    var CiId = Convert.ToInt32(item.City != null ? item.City : "0");
            //    var countryid = CountryRepo.FindByItemId(Cid) != null ? CountryRepo.FindByItemId(Cid).Name : "India";
            //    var StateId = StateRepo.FindByItemId(Sid) != null ? StateRepo.FindByItemId(Sid).StateName : "";
            //    var CityId = CityRepo.FindByItemId(CiId) != null ? CityRepo.FindByItemId(CiId).CityName : "";
            //    model.FirstName = item.FirstName;
            //    model.PhoneNumber = item.PhoneNumber;
            //    model.Email = item.Email;
            //    model.City = CityId;
            //    model.Address = item.Address;
            //    model.PinCode = item.PinCode;
            //    model.State = StateId;
            //    model.Country = countryid;
            //}
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(user),
                ContentType = "application/json",
            };
        }
        //this method is for invoice detail 
        public ContentResult GetInvoiceDetails()
        {
            var cartRepository = new CartRepository();
            var itemRepository = new CatalogRepository();
            var ImageUrl = ConfigurationManager.AppSettings["ImageUrl"].ToString(CultureInfo.InvariantCulture);
            var carts = cartRepository.FindByCarts(User.Identity.GetUserId()).Where(t => t.IsWishList != true).ToList();//"c667741a-27d1-4438-8662-cc51a784e5d6").ToList();
            var results = (from cartItem in carts
                           select new ProductItem
                           {
                               Id = cartItem.Id,
                               Image = ImageUrl + cartItem.tblCataLog.Image,
                               Price = String.Format("{0:0.00}", cartItem.tblCataLog.Price),//cartItem.Item.Price ?? 0,//String.Format("{0:0.00}", item.Price)
                               Quantity = cartItem.Quntity ?? 0,
                               Title = cartItem.tblCataLog.Title,
                               ItemId = cartItem.ItemId ?? 0,
                               Discount = cartItem.Discount ?? 0,

                           }).ToList();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(results),
                ContentType = "application/json",
            };
        }
        //this class use for book type 

        long GetCountryId()
        {
            var userId = User.Identity.GetUserId();
            var context = new dbPrachiIndia_PortalEntities();
            var item = context.AspNetUsers.FirstOrDefault(t => t.Id == userId);
            if (item != null)
                return item.CountryId ?? -1;

            return -1;
        }

        [ValidateInput(false)]
        public ActionResult Solutions(string Id)
        {
            var itemRepository = new CatalogRepository();
            var list = GetAllCountry();
            var userId = User.Identity.GetUserId();
            var otherThenIndia = false;
            var user = new dbPrachiIndia_PortalEntities().AspNetUsers.FirstOrDefault(t => t.Id == userId);
            if (user != null && user.CountryId != 0 && user.CountryId != 86)
            {
                otherThenIndia = true;
            }
            ViewData["list"] = list;
            TempData["id"] = Id;
            int itemId = Utility.ToSafeInt(TempData["id"]);//Convert.ToInt32(Id);
            var ImageUrl = ConfigurationManager.AppSettings["ImageUrl"].ToString(CultureInfo.InvariantCulture);
            var result = itemRepository.FindByIdAsync(itemId);

            if (result == null)
                return RedirectToAction("Index");


            var ClassRepository = new MasterClassRepository();
            var BoardRepository = new MasterBoardRepository();
            int ClassID = Utility.ToSafeInt(result.ClassId);
            int BoardID = Utility.ToSafeInt(result.BoardId);
            var Class = ClassRepository.GetAll().Where(i => i.Id == ClassID).Select(c => new { class_value = c.Title }).First().class_value.ToString();
            var Board = BoardRepository.GetAll().Where(i => i.Id == BoardID).Select(c => new { Board_value = c.Title }).First().Board_value.ToString();

            var Item =
                         new ItemVM
                         {
                             Id = result.Id,
                             Title = result.Title,
                             Image = ImageUrl + ((result.Image != null) ? result.Image : ("no_image.jpg")),
                             Description = result.Description,
                             ISBN = result.ISBN,
                             Author = result.Author,
                             Discount = result.Discount,
                             Price = result.SolutionPrice,
                             Status = result.Status,
                             Class = Class,
                             BoardId = Board,
                             PrintPrice = otherThenIndia == true ? 0 : result.SolutionPrice,
                             EbookPrice = result.EbookPrice,
                             SeriesDescription = result.MasterSery.Description,
                             Ebook = result.Ebook,

                         };

            ViewData["item"] = Item;

            return View();

        }
    }
}