# What is this ?

Easily paging in Asp.net Mvc that get data as chunks from database

# Install via NuGet

To install MvcPagedList, run the following command in the Package Manager Console
```code
pm> Install-Package MvcPagedList
```
You can also view the [package page](https://www.nuget.org/packages/MvcPagedList/) on NuGet.

# How to use ?

First install package from nuget.
now we need to implement method that take data from database like bellow
``` c#
 public IEnumerable<UserOutput> Search(int page, int recordsPerPage, string term, SortBy sortBy, SortOrder sortOrder, out int pageSize, out int TotalItemCount)
        {
            var queryable = users.AsQueryable();

            // by term
            if (!string.IsNullOrEmpty(term))
            {
                queryable = queryable.Where(c => c.Family.Contains(term) || c.Name.Contains(term));

            }

            //sorting
            switch (sortBy)
            {
                case SortBy.AddDate:
                    queryable = sortOrder == SortOrder.Asc ? queryable.OrderBy(u => u.AddDate) : queryable.OrderByDescending(u => u.AddDate);
                    break;
                case SortBy.DisplayName:
                    queryable = sortOrder == SortOrder.Asc ? queryable.OrderBy(u => u.Name).ThenBy(u => u.Family) : queryable.OrderByDescending(u => u.Name).ThenByDescending(u => u.Family);
                    break;
                default:
                    break;
            }

            // get total and pageSize
            TotalItemCount = queryable.Count();
            pageSize = (int)Math.Ceiling((double)TotalItemCount / recordsPerPage);

            page = page > pageSize || page < 1 ? 1 : page;

            //take recordes
            var skiped = (page - 1) * recordsPerPage;
            queryable = queryable.Skip(skiped).Take(recordsPerPage);


            return queryable.Select(u => new UserOutput
            {
                Id = u.Id,
                AddDate = u.AddDate.ToShortDateString(),
                Name = u.Name,
                Family = u.Family,

            }).ToList();
        }
```
Then we need to call this method in controller and set some ViewBags for index and search action

```c#
        public ActionResult Index()
        {
  
            var page = 1;
            pageSize = 0;
            recordsPerPage = 5;
            TotalItemCount = 0;

 
            var users = _userService.Search(page: page, recordsPerPage:recordsPerPage, term:"", sortBy:SortBy.AddDate, sortOrder:SortOrder.Desc, pageSize: out pageSize, TotalItemCount:out TotalItemCount);

          
            ViewBag.SortOrder = EnumHelper.GetSelectList(typeof(SortOrder));
            ViewBag.SortBy = EnumHelper.GetSelectList(typeof(SortBy));
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemCount = TotalItemCount;

            return View(users);
        }



 public ActionResult Search(int page = 1, string term = "",SortBy sortBy = SortBy.AddDate, SortOrder sortOrder = SortOrder.Desc)
        {

            pageSize = 0;
            recordsPerPage = 5;
            TotalItemCount = 0;

            var users = _userService.Search(page: page, recordsPerPage: recordsPerPage, term: term,sortBy: sortBy, sortOrder: sortOrder, pageSize: out pageSize, TotalItemCount: out TotalItemCount);

         
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemCount = TotalItemCount;

            return PartialView("_UsersList", users);
        }


```
And now we need to add index.cshtml like this :

```code
@model IEnumerable<MvcPagedList.Example.Service.Users.Dto.UserOutput>
@{
    ViewBag.Title = "Home Page";
}

<div class="panel panel-default">
    <div class="panel-heading">
        <div id="search-form" class="form-inline">
            @using (Ajax.BeginForm("search", "home", new AjaxOptions()
            {
                HttpMethod = "GET",
                LoadingElementId = "global-ajax-loading",
                UpdateTargetId = "ajax-show-list",
                InsertionMode = InsertionMode.Replace
            }, new { @class = "form", role = "search" }))
            {
                <div class="form-group">
                    @Html.TextBox("term", null, new { @class = "form-control", id = "", placeholder = "search ..." })
                </div>
                <div class="form-group">
                    @Html.DropDownList("SortBy", null, new { @class = "form-control" })
                </div>
                <div class="form-group">
                    @Html.DropDownList("SortOrder", null, new {  @class = "form-control" })
                </div>
                <div class="form-group">
                    <input type="submit" class="btn btn-primary" value="Search" />
                </div>

            }
        </div>

    </div>
    <div id="ajax-show-list" class="panel-body">
        @Html.Partial("_UsersList", Model)
    </div>

</div>


```
Add partial view with _UsersList.cshtml name
```code
@model IEnumerable<MvcPagedList.Example.Service.Users.Dto.UserOutput>

@if (Model.Count() == 0)
{
    <div class="alert alert-info">
        No Users found
    </div>
}
else
{
    <table class="table table-striped table-hover">
        <thead>
            <tr>
                <td> Name</td>
                <td> Family</td>
                <td>Add Date</td>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Model)
                {
                <tr>
                    <td>@item.Name</td>
                    <td>@item.Family</td>
                        <td>@item.AddDate</td>
                    </tr>
            }
        </tbody>
    </table>
}


@Html.Partial("_UsersPagedList")


```
Add partial view with _UsersPagedList name

```code
@using MvcPagedList;

    @PagedList.Pager(actionName: "search", controllerName: "home",
    routeValues: new
    {
        term = Request.QueryString["term"],
        sortOrder = Request.QueryString["sortOrder"],
        sortBy = Request.QueryString["sortBy"],

    },
    ajaxOptions: new AjaxOptions
    {
        HttpMethod = "Get",
        UpdateTargetId = "ajax-show-list",
        LoadingElementId = "global-ajax-loading",


    },
    pagerOptions: new PagerOptions
    {
        currentPage = (int)ViewBag.CurrentPage,
        PageCount = (int)ViewBag.PageSize,
        TotalItemCount = (int)ViewBag.TotalItemCount,
        DisplayMode = PagedListDisplayMode.IfNeeded,
        DisplayInfoArea = true,
        LinkToNextPageFormat = "next",
        LinkToPreviousPageFormat = "prev",
        CurrentLocationFormat = "page",
        PageCountFormat = "of",
        TotalItemCountFormat = "total count",
        WrapperClasses= "text-center",

    }
        )


```

# Screenshots

![alt text](https://github.com/hamed-shirbandi/MvcPagedList/blob/master/MvcPagedList.Example/Content/img/screenShots/Screenshot-1.png)
