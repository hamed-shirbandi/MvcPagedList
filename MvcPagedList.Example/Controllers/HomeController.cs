using MvcPagedList.Example.Enums;
using MvcPagedList.Example.Service.Users;
using MvcPagedList.Example.Service.Users.Dto;
using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcPagedList.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private int pageSize;
        private int recordsPerPage;
        private int TotalItemCount;



        public HomeController()
        {
            _userService = new UserService();
        }

            


        /// <summary>
        /// 
        /// </summary>
        public ActionResult Index()
        {
            #region Pagging

            var page = 1;
            pageSize = 0;
            recordsPerPage = 5;
            TotalItemCount = 0;


            #endregion

            AddFakeUsers();

            var users = _userService.Search(page: page, recordsPerPage:recordsPerPage, term:"", sortBy:SortBy.AddDate, sortOrder:SortOrder.Desc, pageSize: out pageSize, TotalItemCount:out TotalItemCount);
             
            #region ViewBags

            ViewBag.SortOrder = EnumHelper.GetSelectList(typeof(SortOrder));
            ViewBag.SortBy = EnumHelper.GetSelectList(typeof(SortBy));
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemCount = TotalItemCount;


            #endregion

            return View(users);
        }






        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Search(int page = 1, string term = "",SortBy sortBy = SortBy.AddDate, SortOrder sortOrder = SortOrder.Desc)
        {
            System.Threading.Thread.Sleep(700);
            #region Pagging

            pageSize = 0;
            recordsPerPage = 5;
            TotalItemCount = 0;


            #endregion

            AddFakeUsers();

            var users = _userService.Search(page: page, recordsPerPage: recordsPerPage, term: term,sortBy: sortBy, sortOrder: sortOrder, pageSize: out pageSize, TotalItemCount: out TotalItemCount);

            #region ViewBags


            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemCount = TotalItemCount;


            #endregion

            return PartialView("_UsersList", users);
        }






        /// <summary>
        /// 
        /// </summary>
        void AddFakeUsers()
        {
            for (int i = 0; i < 50; i++)
            {
                _userService.Create(new UserInput
                {
                    Id=i,
                    Name = "Name " + i,
                    Family = "Family " + i,
                    AddDate=DateTime.Now.AddDays(-i),
                });

            }
        }




    }
}