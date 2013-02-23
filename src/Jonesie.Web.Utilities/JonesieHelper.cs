using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Jonesie.Web.Common;
using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Contracts.Data;
using Jonesie.Web.Entities.Data;
using System.Linq.Expressions;
using System.Web.Routing;

namespace Jonesie.Web.Utilities
{
    /// <summary>
    /// Some useful HTML Helper extensions
    /// </summary>
    public static class JonesieHelper
    {
        static INavigationRepository _navRepos;
        static ICache _cache;
        static ISettings _settings;

        /// <summary>
        /// Initializes the <see cref="JonesieHelper" /> class.
        /// </summary>
        static JonesieHelper()
        {
            _navRepos = DependencyResolver.Current.GetService<INavigationRepository>();
            _cache = DependencyResolver.Current.GetService<ICache>();
            _settings = DependencyResolver.Current.GetService<ISettings>();
        }

        /// <summary>
        /// Labels the with tooltip.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MvcHtmlString LabelWithTooltip<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
          var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

          string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
          string labelText = metaData.DisplayName ?? metaData.PropertyName ?? htmlFieldName.Split('.').Last();

          if (String.IsNullOrEmpty(labelText))
            return MvcHtmlString.Empty;

          var label = new TagBuilder("label");
          label.Attributes.Add("for", helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

          if (!string.IsNullOrEmpty(metaData.Description))
          {
            label.Attributes.Add("rel", "tooltip");
            //label.Attributes.Add("data-placement", "left");
            label.Attributes.Add("data-original-title", metaData.Description);
          }

          label.SetInnerText(labelText);
          return MvcHtmlString.Create(label.ToString());
        }

        /// <summary>
        /// Get a descriptions for an annotated item
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MvcHtmlString DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
          var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

          if (!string.IsNullOrEmpty(metaData.Description))
          {
            return MvcHtmlString.Create(metaData.Description);
          }

          return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Expose the settings via the helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns></returns>
        public static ISettings Settings(this HtmlHelper helper)
        {
            return _settings;
        }

        /// <summary>
        /// A Dynamic data driven menu.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="elementId">The element id.</param>
        /// <param name="menuName">Name of the menu.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public static MvcHtmlString DynamicMenu(this HtmlHelper helper, string topMenuName = "Main", bool level2 = false, string elementClass = null)
        {
          var menuToDisplay = topMenuName;

          if (_navRepos != null)
          {
            var allItems = _navRepos.GetOptions().Items;

            // get the current route and action so we can highlight the selected menu option 
            RouteBase route = helper.ViewContext.RouteData.Route;
            string action = helper.ViewContext.RouteData.Values["action"].ToString().ToLower();
            string controller = helper.ViewContext.RouteData.Values["controller"].ToString().ToLower();

            // use the values from the auth attribute if we have them
            if (helper.ViewContext.RouteData.DataTokens.ContainsKey("menu_controller"))
            {
              controller = helper.ViewContext.RouteData.DataTokens["menu_controller"].ToString().ToLower();
            }
            if (helper.ViewContext.RouteData.DataTokens.ContainsKey("menu_action"))
            {
              action = helper.ViewContext.RouteData.DataTokens["menu_action"].ToString().ToLower();
            }


            // find all the options that point to the current page - there may be one for the top level menu and another for the level 2 menu
            var matchingOptions = allItems.Where(o => (!string.IsNullOrWhiteSpace(o.Controller)) && (!string.IsNullOrWhiteSpace(o.Action)) && (o.Controller.ToLower() == controller) && (o.Action.ToLower() == action));
            var topLevelOption = matchingOptions.Where(o => o.MenuName == topMenuName).FirstOrDefault();
            var level2option = matchingOptions.Where(o => o.MenuName != topMenuName).FirstOrDefault();
            SiteNavigation optionToHighlight = null;

            // if we are doing level 2 menu then we need to figure out the current top level menu using 
            // the controller and action 
            if (level2)
            {
              optionToHighlight = level2option;

              if (level2option == null)
              {
                return new MvcHtmlString(string.Empty);
              }

              if (!string.IsNullOrWhiteSpace(level2option.ChildMenuName))
              {
                menuToDisplay = level2option.ChildMenuName;
              }
              else if (topMenuName != level2option.MenuName)
              {
                menuToDisplay = level2option.MenuName;
              }
              else
              {
                return new MvcHtmlString(string.Empty);
              }

            }
            else
            {
              // use the current page to figure out the top level menu option to highlight
              //if the current page is in the main menu then we can just use that
              if (topLevelOption == null)
              {
                // see if we have a 2nd level option
                if (level2option != null)
                {
                  // find the top level menu that refers to the menu of the level 2 option
                  topLevelOption = allItems.Where(o => o.ChildMenuName == level2option.MenuName && o.MenuName == topMenuName).FirstOrDefault();
                }
              }

              optionToHighlight = topLevelOption;

            }

            // we are now ready to generate the html
            var menus = allItems.Where(o => o.MenuName == menuToDisplay);

            if (menus != null && menus.Count() > 0)
            {
              StringBuilder sb = new StringBuilder();

              foreach (var option in menus)
              {
                // see if this option is active
                var buggerOff = !option.Active;

                // check the role first - a high level security trim
                if (!buggerOff && !string.IsNullOrWhiteSpace(option.Roles))
                {
                  buggerOff = true;

                  var roles = option.Roles.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                  foreach (var role in roles)
                  {
                    if (HttpContext.Current.User.IsInRole(role))
                    {
                      buggerOff = false;
                      break;
                    }
                  }
                }

                if (!buggerOff)
                {
                  var css = string.Empty;
                  //if (option.Controller.ToLower() == controller && option.Action.ToLower() == action)
                  if (elementClass != null)
                  {
                    css += elementClass;
                  }
                  if (option == optionToHighlight)
                  {
                    css += " active";
                  }
                  if (!string.IsNullOrWhiteSpace(css))
                  {
                    css = " class = '{0}'".FormatWith(css);
                  }

                  if (!string.IsNullOrWhiteSpace(option.Controller))
                  {
                    // make sure the user has permission for the specified action before adding it to the menu
                    var roles = option.Roles.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var ok = roles.Length == 0;

                    var i = 0;
                    while (!ok && i < roles.Length)
                    {
                      var role = roles[i];
                      ok = HttpContext.Current.User.IsInRole(role);
                      i++;
                    }


                    // if (HttpContext.Current.User.HasPermission(option.Controller + "." + option.Action))
                    if(ok)
                    {
                      sb.Append("<li{0}>".FormatWith(css));
                      sb.Append(helper.ActionLink(option.DisplayLabel, option.Action, option.Controller));
                      sb.Append("</li>");
                    }
                  }
                  else
                  {
                    sb.Append("<li{0}>".FormatWith(css));
                    sb.Append("<a href='");
                    sb.Append(option.Url);
                    sb.Append("' target='_blank'");
                    sb.Append(">");
                    sb.Append(option.DisplayLabel);
                    sb.Append("</a>");
                    sb.Append("</li>");
                  }
                }
              }

              //sb.Append("</ul>");

              return new MvcHtmlString(sb.ToString());
            }
          }

          return new MvcHtmlString(string.Empty);
        }

        /// <summary>
        /// See if the user has permission to execute an action on a controller
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="actionPath">The action path.</param>
        /// <returns>
        ///   <c>true</c> if the specified helper has permission; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPermission(this IPrincipal user, string actionPath)
        {
            return AuthorizeActionAttribute.UserHasServicePoint(user, actionPath);
        }

        /// <summary>
        /// Images the link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="imgSrc">The img SRC.</param>
        /// <param name="alt">The alt.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="imgHtmlAttributes">The img HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString ImageLink(this HtmlHelper htmlHelper, string imgSrc, string alt, string actionName, string controllerName, object routeValues, object htmlAttributes, object imgHtmlAttributes)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            TagBuilder imgTag = new TagBuilder("img");
            imgTag.MergeAttribute("src", urlHelper.Content(imgSrc));
            imgTag.MergeAttributes((IDictionary<string, string>)imgHtmlAttributes, true);
            string url = urlHelper.Action(actionName, controllerName, routeValues);

            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgTag.ToString();
            imglink.MergeAttributes((IDictionary<string, string>)htmlAttributes, true);

            return new MvcHtmlString(imglink.ToString());

        }

        /// <summary>
        /// Renders the module script tag.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns></returns>
        public static MvcHtmlString RenderModuleScript(this HtmlHelper helper)
        {
            JonesieViewEngine engine = (JonesieViewEngine)ViewEngines.Engines[0];
            IViewModule module = engine.GetModule(helper.ViewContext.Controller);

            var relfile = "";
            var path = "";

            if (module != null)
            {
                relfile = "~/Modules/" + module.GetType().Namespace + "/Scripts/module.js";
                path = HttpContext.Current.Server.MapPath(relfile);
            }

            if (File.Exists(path))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript' src='");
                sb.Append(VirtualPathUtility.ToAbsolute(relfile));
                sb.Append("'></script>");
                return new MvcHtmlString(sb.ToString());
            }

            return new MvcHtmlString(string.Empty);

        }

        /// <summary>
        /// Renders the content of the raw.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="serverPath">The server path.</param>
        /// <returns></returns>
        public static MvcHtmlString RenderRawContent(this HtmlHelper helper, string serverPath)
        {
            string filePath = HttpContext.Current.Server.MapPath(serverPath);

            //load from file
            StreamReader streamReader = File.OpenText(filePath);
            string markup = streamReader.ReadToEnd();
            streamReader.Close();

            return new MvcHtmlString(markup);

        }

        /// <summary>
        /// Renders the content of the template.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static IHtmlString RenderTemplateContent(this HtmlHelper helper, string serverPath, object model)
        {
            string filePath = HttpContext.Current.Server.MapPath(serverPath);

            //load from file
            var streamReader = File.OpenText(filePath);

            var template = new Nustache.Core.Template();
            var writer = new StringWriter();
            template.Load(streamReader);
            template.Render(model, writer, null);

            streamReader.Close();

            return new HtmlString(writer.ToString());

        }


    }
}
