using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace MvcPagedList
{
    public static class PagedList
    {
        static bool hasNextPage;
        static bool hasPreviousPage;
        static bool isFirstPage;
        static bool isLastPage;
        static TagBuilder prevBtn;
        static TagBuilder nextBtn;
        static TagBuilder wrapper;
        static TagBuilder ul;


        /// <summary>
        /// 
        /// </summary>
        public static MvcHtmlString Pager(string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, PagerOptions pagerOptions)
        {

            if (pagerOptions.DisplayMode == PagedListDisplayMode.Never || (pagerOptions.DisplayMode == PagedListDisplayMode.IfNeeded && pagerOptions.PageCount <= 1))
                return null;


            InitialPager(pagerOptions);

            InitialTags(pagerOptions);

            GeneratePrevBtn(actionName, controllerName, routeValues, ajaxOptions, pagerOptions);

            GeneratePageNumbers(actionName, controllerName, routeValues, ajaxOptions, pagerOptions);

            GenerateNextBtn(actionName, controllerName, routeValues, ajaxOptions, pagerOptions);

            wrapper.InnerHtml = ul.ToString(TagRenderMode.Normal);

            GenerateInfoArea(actionName, controllerName, routeValues, ajaxOptions, pagerOptions);

            return MvcHtmlString.Create(wrapper.ToString(TagRenderMode.Normal));

        }





        /// <summary>
        /// 
        /// </summary>
        private static void InitialTags(PagerOptions pagerOptions)
        {
            prevBtn = new TagBuilder("a");
            prevBtn.AddCssClass("btn btn-default");
            prevBtn.AddCssClass("ajax-paging");

            nextBtn = new TagBuilder("a");
            nextBtn.AddCssClass("btn btn-default");
            nextBtn.AddCssClass("ajax-paging");

            wrapper = new TagBuilder("nav");
            wrapper.MergeAttribute("aria-label", "Page navigation");
            wrapper.AddCssClass(pagerOptions.WrapperClasses);



            ul = new TagBuilder("ul");
            ul.AddCssClass(pagerOptions.UlElementClasses);
        }




        /// <summary>
        /// 
        /// </summary>
        private static void InitialPager(PagerOptions pagerOptions)
        {
            hasNextPage = pagerOptions.currentPage < pagerOptions.PageCount;
            hasPreviousPage = pagerOptions.currentPage > 1;
            isFirstPage = pagerOptions.currentPage == 1;
            isLastPage = pagerOptions.currentPage == pagerOptions.PageCount;

        }




        /// <summary>
        /// 
        /// </summary>
        public static void GenerateNextBtn(string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, PagerOptions pagerOptions)
        {

            if (pagerOptions.DisplayLinkToNextPage == PagedListDisplayMode.Always || (pagerOptions.DisplayLinkToNextPage == PagedListDisplayMode.IfNeeded && !isLastPage))
            {

                var span = new TagBuilder("span");
                span.InnerHtml = pagerOptions.LinkToNextPageFormat;
                var page = pagerOptions.currentPage >= pagerOptions.PageCount ? pagerOptions.PageCount : pagerOptions.currentPage + 1;


                nextBtn.MergeAjaxAttribute(ajaxOptions);

                nextBtn.MergeUrlAttribute(actionName, controllerName, routeValues, page);

                nextBtn.InnerHtml = span.ToString(TagRenderMode.Normal);
            }
        }





        /// <summary>
        /// 
        /// </summary>
        public static void GeneratePageNumbers(string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, PagerOptions pagerOptions)
        {

            for (int page = 1; page <= pagerOptions.PageCount; page++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass(pagerOptions.LiElementClasses);

                if (page == 1 && pagerOptions.currentPage > pagerOptions.PageCount)
                {
                    li.AddCssClass("active");
                }
                else if (page == pagerOptions.currentPage)
                {
                    li.AddCssClass("active");
                }



                var span = new TagBuilder("span");
                span.InnerHtml = page.ToString();

                var a = new TagBuilder("a");
                a.AddCssClass("ajax-paging");

                a.MergeAjaxAttribute(ajaxOptions);
                a.MergeUrlAttribute(actionName, controllerName, routeValues, page);

                a.InnerHtml = span.ToString(TagRenderMode.Normal);
                li.InnerHtml = a.ToString(TagRenderMode.Normal);
                ul.InnerHtml = ul.InnerHtml + li.ToString(TagRenderMode.Normal);


            }
        }



        /// <summary>
        /// 
        /// </summary>
        public static void GeneratePrevBtn(string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, PagerOptions pagerOptions)
        {
            if (pagerOptions.DisplayLinkToPreviousPage == PagedListDisplayMode.Always || (pagerOptions.DisplayLinkToPreviousPage == PagedListDisplayMode.IfNeeded && !isFirstPage))
            {

                var span = new TagBuilder("span");
                span.InnerHtml = pagerOptions.LinkToPreviousPageFormat;

                var page = pagerOptions.currentPage <= 1 ? 1 : pagerOptions.currentPage - 1;


                prevBtn.MergeAjaxAttribute(ajaxOptions);

                prevBtn.MergeUrlAttribute(actionName, controllerName, routeValues, page);

                prevBtn.InnerHtml = span.ToString(TagRenderMode.Normal);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static void GenerateInfoArea(string actionName, string controllerName, object routeValues, object ajaxAttributes, PagerOptions pagerOptions)
        {

            if (pagerOptions.DisplayInfoArea == true)
            {
                var infoDiv = new TagBuilder("div");
                infoDiv.AddCssClass("well well-sm text-primary clearfix text-center");


                if (pagerOptions.DisplayPageCountAndCurrentLocation == true)
                {

                    var infoSpan = new TagBuilder("span");
                    infoSpan.AddCssClass("pull-right");
                    infoSpan.InnerHtml = pagerOptions.CurrentLocationFormat + " " + pagerOptions.currentPage + " " + pagerOptions.PageCountFormat + " " + pagerOptions.PageCount;
                    infoDiv.InnerHtml = infoSpan.ToString(TagRenderMode.Normal);

                }


                if (pagerOptions.DisplayTotalItemCount == true)
                {
                    var infoSpan = new TagBuilder("span");
                    infoSpan.AddCssClass("pull-left");
                    infoSpan.SetInnerText(pagerOptions.TotalItemCountFormat + " " + pagerOptions.TotalItemCount);
                    infoDiv.InnerHtml = infoDiv.InnerHtml + infoSpan.ToString(TagRenderMode.Normal);
                }




                if (hasPreviousPage)
                {
                    infoDiv.InnerHtml += prevBtn.ToString(TagRenderMode.Normal);
                }

                if (hasNextPage)
                {
                    infoDiv.InnerHtml += nextBtn.ToString(TagRenderMode.Normal);
                }

                wrapper.InnerHtml = wrapper.InnerHtml + infoDiv.ToString(TagRenderMode.Normal);

            }


        }




        /// <summary>
        /// 
        /// </summary>
        private static void MergeUrlAttribute(this TagBuilder tagBuilder, string actionName, string controllerName, object routeValues, int page)
        {
            string values = string.Empty;
            if (routeValues != null)
                values = String.Join("&", routeValues.GetType().GetProperties().Select(p => p.Name + "=" + p.GetValue(routeValues, null)));
            tagBuilder.MergeAttribute("href", "/" + controllerName + "/" + actionName + "?page=" + page + "&" + values);
        }




        /// <summary>
        /// 
        /// </summary>
        public static void MergeAjaxAttribute(this TagBuilder tagBuilder, AjaxOptions ajaxOptions)
        {
            foreach (var ajaxOption in ajaxOptions.ToUnobtrusiveHtmlAttributes())
                tagBuilder.Attributes.Add(ajaxOption.Key, ajaxOption.Value.ToString());
        }
    }




}

