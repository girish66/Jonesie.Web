using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Jonesie.Web.Site
{
    public abstract class JonesieWebViewPage : WebViewPage
    {
        public HelperResult RenderSection(string name, Func<dynamic, HelperResult> defaultContents)
        {
            if (this.IsSectionDefined(name))
            {
                return this.RenderSection(name);
            }
            return defaultContents(null);
        }

        //public HtmlString SearchPath
        //{
        //    get { return new HtmlString(Properties.Settings.Default.SearchAction); }
        //}
    }

    public abstract class JonesieWebViewPage<TModel> : WebViewPage<TModel>
    {
        public HelperResult RenderSection(string name, Func<dynamic, HelperResult> defaultContents)
        {
            if (this.IsSectionDefined(name))
            {
                return this.RenderSection(name);
            }
            return defaultContents(null);
        }

        //public HtmlString SearchPath
        //{
        //    get { return new HtmlString(Properties.Settings.Default.SearchAction); }
        //}
    }
}