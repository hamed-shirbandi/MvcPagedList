using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace MvcPagedList
{
    public static class PagedList
    {


        /// <summary>
        /// 
        /// </summary>
        public static MvcHtmlString Pager(string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, PagerOptions pagerOptions)
        {

            #region Init

            bool hasNextPage = pagerOptions.currentPage < pagerOptions.PageCount;
            bool hasPreviousPage = pagerOptions.currentPage > 1;
            bool isFirstPage = pagerOptions.currentPage == 1;
            bool isLastPage = pagerOptions.currentPage == pagerOptions.PageCount;

            if (pagerOptions.DisplayMode == PagedListDisplayMode.Never || (pagerOptions.DisplayMode == PagedListDisplayMode.IfNeeded && pagerOptions.PageCount <= 1))
                return null;

            string @params = String.Join("&", routeValues.GetType().GetProperties().Select(p => p.Name + "=" + p.GetValue(routeValues, null)));


            var nextBtn = new TagBuilder("a");
            var prevBtn = new TagBuilder("a");
            prevBtn.AddCssClass("btn btn-default");
            nextBtn.AddCssClass("btn btn-default");
            var wrapper = new TagBuilder("nav");
            wrapper.MergeAttribute("aria-label", "Page navigation");
            var ul = new TagBuilder("ul");

            wrapper.AddCssClass(pagerOptions.WrapperClasses);
            ul.AddCssClass(pagerOptions.UlElementClasses);


            #endregion

            #region previous



            if (pagerOptions.DisplayLinkToPreviousPage == PagedListDisplayMode.Always || (pagerOptions.DisplayLinkToPreviousPage == PagedListDisplayMode.IfNeeded && !isFirstPage))
            {
                prevBtn.AddCssClass("ajax-paging");

                var span = new TagBuilder("span");
                span.SetInnerText(pagerOptions.LinkToPreviousPageFormat);

                var page = pagerOptions.currentPage <= 1 ? 1 : pagerOptions.currentPage - 1;


                foreach (var ajaxOption in ajaxOptions.ToUnobtrusiveHtmlAttributes())
                    prevBtn.Attributes.Add(ajaxOption.Key, ajaxOption.Value.ToString());

                prevBtn.MergeAttribute("href", "/" + controllerName + "/" + actionName + "?" + @params + "&page=" + page);

                prevBtn.InnerHtml = span.ToString(TagRenderMode.Normal);
            }



            #endregion

            #region numbers


            for (int i = 1; i <= pagerOptions.PageCount; i++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass(pagerOptions.LiElementClasses);

            
                if (i == 1 && pagerOptions.currentPage > pagerOptions.PageCount)
                {
                    li.AddCssClass("active");
                }
                else if (i == pagerOptions.currentPage)
                {
                    li.AddCssClass("active");
                }

                var a = new TagBuilder("a");
                a.AddCssClass("ajax-paging");

                var span = new TagBuilder("span");
                span.SetInnerText(i.ToString());



                foreach (var ajaxOption in ajaxOptions.ToUnobtrusiveHtmlAttributes())
                    a.Attributes.Add(ajaxOption.Key, ajaxOption.Value.ToString());
                
                a.MergeAttribute("href", "/" + controllerName + "/" + actionName + "?" + @params + "&page=" + i);



                a.InnerHtml = span.ToString(TagRenderMode.Normal);
                li.InnerHtml = a.ToString(TagRenderMode.Normal);
                ul.InnerHtml = ul.InnerHtml + li.ToString(TagRenderMode.Normal);
            }




            #endregion

            #region Next



            if (pagerOptions.DisplayLinkToNextPage == PagedListDisplayMode.Always || (pagerOptions.DisplayLinkToNextPage == PagedListDisplayMode.IfNeeded && !isLastPage))
            {
                nextBtn.AddCssClass("ajax-paging");
                var span = new TagBuilder("span");
                span.SetInnerText(pagerOptions.LinkToNextPageFormat);
                var page = pagerOptions.currentPage >= pagerOptions.PageCount ? pagerOptions.PageCount : pagerOptions.currentPage + 1;

                foreach (var ajaxOption in ajaxOptions.ToUnobtrusiveHtmlAttributes())
                    nextBtn.Attributes.Add(ajaxOption.Key, ajaxOption.Value.ToString());
                nextBtn.MergeAttribute("href", "/" + controllerName + "/" + actionName + "?" + @params + "&page=" + page);
                

                nextBtn.InnerHtml = span.ToString(TagRenderMode.Normal);
            }


            #endregion

            #region Info area

            wrapper.InnerHtml = ul.ToString(TagRenderMode.Normal);

            if (pagerOptions.DisplayInfoArea == true)
            {
                var infoDiv = new TagBuilder("div");
                infoDiv.AddCssClass("well well-sm text-primary clearfix text-center");


                if (pagerOptions.DisplayPageCountAndCurrentLocation == true)
                {

                    var infoSpan = new TagBuilder("span");
                    infoSpan.AddCssClass("pull-right");
                    infoSpan.SetInnerText(pagerOptions.CurrentLocationFormat + " " + pagerOptions.currentPage + " " + pagerOptions.PageCountFormat + " " + pagerOptions.PageCount);
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






            #endregion

            return MvcHtmlString.Create(wrapper.ToString(TagRenderMode.Normal));
        }


    }
    

}
