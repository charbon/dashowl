using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace DashOwl.WebAPI
{
    public class MediaAssetDtoModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(MediaAssetDto))
            {
                return false;
            }

            //var request = actionContext.ControllerContext.Request.Content.;

            //request.
            //IDictionary<string, ModelMetadata> dictModel = bindingContext.PropertyMetadata;

            //if (dictModel == null)
            //{
            //    return false;
            //}

            //string key = val.RawValue as string;
            //if (key == null)
            //{
            //    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Wrong value type");
            //    return false;
            //}

            MediaAssetDto result;
            //if (_locations.TryGetValue(key, out result) || GeoPoint.TryParse(key, out result))
            //{
            //    bindingContext.Model = result;
            //    return true;
            //}

            bindingContext.ModelState.AddModelError(
                bindingContext.ModelName, "Cannot convert value to MediaAssetDto");
            return false;
        }
    }
}