using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;
using Nustache.Core;

namespace Jonesie.Web.Utilities
{
    public class NustacheView : IView
    {
        private readonly RazorViewEngine _engine;
        private readonly ControllerContext _controllerContext;
        private readonly string _viewPath;
        private readonly string _masterPath;

        public NustacheView(
            RazorViewEngine engine,
            ControllerContext controllerContext,
            string viewPath,
            string masterPath)
        {
            _engine = engine;
            _controllerContext = controllerContext;
            _viewPath = viewPath;
            _masterPath = masterPath;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            var viewTemplate = GetTemplate();

            var data = viewContext.ViewData.Model; //_engine.RootContext == NustacheViewEngineRootContext.ViewData
                          // ? viewContext.ViewData
                          // : viewContext.ViewData.Model;

            if (!string.IsNullOrEmpty(_masterPath))
            {
                var masterTemplate = LoadTemplate(_masterPath);

                masterTemplate.Render(
                    data,
                    writer,
                    name =>
                        {
                            if (name == "Body")
                            {
                                return GetTemplate();
                            }

                            var template = viewTemplate.GetTemplateDefinition(name);

                            if (template != null)
                            {
                                return template;
                            }

                            return FindPartial(name);
                        });
            }
            else
            {
                GetTemplate().Render(data, writer, FindPartial);
            }
        }

        public string GetViewPath()
        {
            return _viewPath;
        }

        private Template GetTemplate()
        {
            return LoadTemplate(_viewPath);
        }

        private Template LoadTemplate(string path)
        {
            var key = "Nustache:" + path;

            if (_controllerContext.HttpContext.Cache[key] != null)
            {
                return (Template)_controllerContext.HttpContext.Cache[key];
            }

            var templatePath = _controllerContext.HttpContext.Server.MapPath(path);
            var templateSource = File.ReadAllText(templatePath);
            var template = new Template();
            template.Load(new StringReader(templateSource));

            _controllerContext.HttpContext.Cache.Insert(key, template, new CacheDependency(templatePath));

            return template;
        }

        private Template FindPartial(string name)
        {
            var viewResult = _engine.FindPartialView(_controllerContext, name, false);

            if (viewResult != null)
            {
                if (viewResult.View == null)
                {
                    var stringBuilder = new StringBuilder();
                    
                    foreach (var str in viewResult.SearchedLocations)
                    {
                        stringBuilder.AppendLine();
                        stringBuilder.Append(str);
                    }

                    var msg = string.Format(
                        "The partial view '{0}' was not found or no view engine supports the searched locations. The following locations were searched:{1}",
                        name,
                        stringBuilder);

                    throw new InvalidOperationException(msg);
                }

                var nustacheView = viewResult.View as NustacheView;

                if (nustacheView != null)
                {
                    return nustacheView.GetTemplate();
                }
            }

            return null;
        }
    }
}