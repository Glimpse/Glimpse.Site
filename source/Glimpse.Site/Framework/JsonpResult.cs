using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Glimpse.Site.Framework
{

    public class JsonpResult : JsonResult
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), Converters = new List<JsonConverter> { new DictionaryKeysAreNotPropertyNamesJsonConverter() } };

        private string _callbackQueryParameter;

        public string CallbackQueryParameter
        {
            get { return _callbackQueryParameter ?? "callback"; }
            set { _callbackQueryParameter = value; }
        }

        public string Callback { get; set; }

        public JsonpResult(object data)
        {
            Data = data;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a
        /// custom type that inherits from <see cref="T:System.Web.Mvc.ActionResult"/>.
        /// </summary>
        /// <param name="context">The context within which the
        /// result is executed.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;
            response.ContentType = !String.IsNullOrEmpty(this.ContentType) ? this.ContentType : "text/javascript";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (string.IsNullOrEmpty(Callback))
            {
                Callback = context.HttpContext.Request.QueryString[CallbackQueryParameter];
            }

            if (Data != null)
            {
                var serializedObject = JsonConvert.SerializeObject(Data, settings);
                response.Write(Callback + "(" + serializedObject + ");");
            }
        }
    }
}