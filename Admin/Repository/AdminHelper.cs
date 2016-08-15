using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace Admin.Repository
{
    public static class AdminHelper
    {
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static string ToJson(this object obj, int recursionDepth)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return serializer.Serialize(obj);
        }

        public static IHtmlString GetMessage(this HtmlHelper htmlHelper, bool messageType, string itemName)
        {
            string successMessage = "<strong> Success! <strong>  " + itemName + " is created!";
            string failureMessage = "<strong> Failure! <strong>  " + itemName + " is NOT created. Contact administrator!";
            var paragraph = new TagBuilder("p");
                if (!String.IsNullOrEmpty(itemName))
                { 
                    if (messageType)
                    {
                        paragraph.Attributes["class"] = "text-success";
                        paragraph.InnerHtml = successMessage;
                    }
                    else
                    {
                        paragraph.Attributes["class"] = "text-danger";
                        paragraph.InnerHtml = failureMessage;
                    }
                }
            
            return MvcHtmlString.Create(paragraph.ToString());
        }
        public static IHtmlString SortIdentifier(this HtmlHelper htmlHelper, string sortOrder, string field)
        {
            if (string.IsNullOrEmpty(sortOrder) || (sortOrder.Trim() != field && sortOrder.Replace("_desc", "").Trim() != field)) return null;

            string glyph = "glyphicon glyphicon-circle-arrow-up";

            if (sortOrder.ToLower().Contains("desc"))
            {
                glyph = "glyphicon glyphicon-circle-arrow-down";
            }
            var span = new TagBuilder("span");
            span.Attributes["class"] = glyph;
            span.Attributes["style"] = "color:green";

            return MvcHtmlString.Create(span.ToString());
        }

        public static RouteValueDictionary ToRouteValueDictionary(this NameValueCollection collection, string newKey, string newValue)
        {

            var routeValueDictionary = new RouteValueDictionary();
            foreach (var key in collection.AllKeys)
            {
                if (key == null) continue;
                if (routeValueDictionary.ContainsKey(key))
                    routeValueDictionary.Remove(key);
                routeValueDictionary.Add(key, collection[key]);
            }
            if (string.IsNullOrEmpty(newValue))
            {
                routeValueDictionary.Remove(newKey);
            }
            else
            {
                if (routeValueDictionary.ContainsKey(newKey))
                    routeValueDictionary.Remove(newKey);
                routeValueDictionary.Add(newKey, newValue);
            }
            return routeValueDictionary;
        }

    }
}