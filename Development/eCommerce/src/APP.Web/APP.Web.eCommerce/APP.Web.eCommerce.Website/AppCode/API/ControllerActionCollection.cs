using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;

namespace APP.Web.eCommerce.Website.AppCode.API
{
    public class ControllerActionCollection
    {
        public Dictionary<string, HttpControllerDescriptor> Controllers { get; set; }
        public Dictionary<string, Dictionary<string, ReflectedHttpActionDescriptor>> Actions { get; set; }

        public ControllerActionCollection()
        {
            Controllers = new Dictionary<string, HttpControllerDescriptor>();
            Actions = new Dictionary<string, Dictionary<string, ReflectedHttpActionDescriptor>>();
        }
    }
}