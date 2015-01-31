using System;
using System.Linq;
using System.Web.Mvc;

namespace PharmacySolution.Web.Helpers
{
    public static class MenuHelper
    {
        public static string IsSelected(this HtmlHelper html, string controller = null)
        {
            const string cssClass = "active";
            var currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            return controller.Split(';').Contains(currentController) ? cssClass : "";
        }
    }
}