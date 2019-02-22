using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibrarySystem.Models;
using LibrarySystem.Service;

namespace LibrarySystem.Controllers
{
    public class HomeController : Controller
    {
        private HomeService homeService;
        public HomeController()
        {
           homeService = new HomeService();
        }
        // GET: Home
        //回傳 HomeBookData
        public ActionResult Index()
        {
            HomeBookData homeBookData = new HomeBookData();
            ViewBag.BookClassSelectList = homeService.GetBookClassSelectLists();
            ViewBag.BookKeeperSelectList = homeService.GetBookKeeperSelectLists();
            ViewBag.BookStatusSelectList = homeService.GetBookStatusSelectLists();
            ViewBag.ResultMessage = TempData["ResultMessage"];
            return View(homeBookData);
        }

        [HttpPost]
        //回傳 HomeBookData
        public ActionResult Index(HomeBookData homeBookData)
        {            
            //1.service 找出資料
            List<HomeBookData> QueryhomeBookDatas = homeService.QuerySearchBookData(homeBookData);
            //2.取得下拉選單資料
            ViewBag.BookClassSelectList = homeService.GetBookClassSelectLists();
            ViewBag.BookKeeperSelectList = homeService.GetBookKeeperSelectLists();
            ViewBag.BookStatusSelectList = homeService.GetBookStatusSelectLists();
            //3. 用ViewBag傳入 Query後資料
            ViewBag.QueryBookDatas = QueryhomeBookDatas;            
            return View(homeBookData);
        }

        //回傳 BookData
        
        public ActionResult InsertBookData()
        {
            ViewBag.BookClassSelectList = homeService.GetBookClassSelectLists();
            ViewBag.BookKeeperSelectList = homeService.GetBookKeeperSelectLists();
            ViewBag.BookStatusSelectList = homeService.GetBookStatusSelectLists();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //回傳 BookData
        public ActionResult InsertBookData(BookData bookData)
        {
            //確認ModelState
            if (ModelState.IsValid)
            {
                //1.service 新增
                homeService.InsertBookData(bookData);
                TempData["ResultMessage"] = string.Format("書本 [{0}] 建立成功", bookData.BOOK_NAME);
                //2.Redirect To index 
                return RedirectToAction("Index");
            }
            //如果錯誤導回 Get InsertBookData
            return View("InsertBookData");
        }
        public ActionResult EditBookData(int id)
        {
            //1.從service 拿出 該Bookid的資料
            EditBookDetails result = homeService.GetBookDetails(id);
            //2.三個下拉選單
            ViewBag.BookClassSelectList = homeService.GetBookClassSelectLists();
            ViewBag.BookKeeperSelectList = homeService.GetBookKeeperSelectLists();
            ViewBag.BookStatusSelectList = homeService.GetBookStatusSelectLists();
            //3.Return 回Edit畫面
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBookData(EditBookDetails editBookDetails)
        {
            //確認ModelState
            if (ModelState.IsValid)
            {
                //1.從service 修改 該筆bookId 的資料
                homeService.EditBookDeatails(editBookDetails);
                //2.Redirect To index 
                return RedirectToAction("Index");
            }
            //如果錯誤導回 Get EditBookData
            return View();
        }

        [HttpPost]        
        public JsonResult DeleteBookData(int BookId)
        {
            try
            {
                
                //Models.EmployeeService EmployeeService = new Models.EmployeeService();
                homeService.DeleteBookData(BookId);
                return this.Json(true);
            }

            catch (Exception ex)
            {
                return this.Json(false);
            }
        }
    }
}