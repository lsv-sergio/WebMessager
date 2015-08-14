using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebMessenger.Models;
using System.Web.Mvc;
using System.IO;
using System.Web.Routing;
using WebMessenger.Controllers;

namespace WebMessenger.Utils
{
    public class HtmlResult:IHttpActionResult
    {
        private ViewDataDictionary _viewData;
        private string _viewName;
        private string _controller;

        public HtmlResult(string ViewName, ViewDataDictionary ViewData)
        {
            _viewData = ViewData;
            _viewName = ViewName;
            _controller = "Home";
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {

            var currentContext = HttpContext.Current;
            var baseContext = new HttpContextWrapper(currentContext);
            var routeData = new RouteData();
            routeData.Values.Add("controller", _controller);

            var controllerContext = new ControllerContext(baseContext, routeData, new EmptyController());
            
            //var viewData = new ViewDataDictionary(_viewData);

            var tempData = new TempDataDictionary();

            var partialViewResult = ViewEngines.Engines.FindPartialView(controllerContext,_viewName);
            string result = "";
            if(partialViewResult != null)
            {
                using(StringWriter sw = new StringWriter())
                {
                    ViewContext viewContext = new ViewContext(controllerContext, partialViewResult.View, _viewData, tempData, sw);
                    partialViewResult.View.Render(viewContext, sw);
                    partialViewResult.ViewEngine.ReleaseView(controllerContext, partialViewResult.View);
                    result = sw.GetStringBuilder().ToString();
                }
            }
            HttpResponseMessage response = new HttpResponseMessage();
            if (result.Trim().Length == 0)
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
            else
            {
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Content = new StringContent(result);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
            }

            return Task.FromResult(response);
        }
    }
}