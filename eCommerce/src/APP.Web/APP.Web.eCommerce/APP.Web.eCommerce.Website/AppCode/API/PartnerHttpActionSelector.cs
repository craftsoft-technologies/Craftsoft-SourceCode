using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;

namespace APP.Web.eCommerce.Website.AppCode.API
{
    public class PartnerHttpActionSelector : ApiControllerActionSelector
    {
        #region V1
        //public override ILookup<string, HttpActionDescriptor> GetActionMapping(HttpControllerDescriptor controllerDescriptor)
        //{
        //    var allMethods = controllerDescriptor.ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
        //    var validMethods = Array.FindAll(allMethods, IsValidActionMethod);
        //    Dictionary<string, ReflectedHttpActionDescriptor> actionDescriptors = new Dictionary<string, ReflectedHttpActionDescriptor>();

        //    foreach (var actionDescriptor in validMethods.Select(m => new ReflectedHttpActionDescriptor(controllerDescriptor, m)))
        //    {
        //        string key = string.Empty;
        //        System.Web.Http.RouteAttribute route = actionDescriptor.MethodInfo.GetCustomAttribute<System.Web.Http.RouteAttribute>();
        //        key = route != null ? route.Template : actionDescriptor.ActionName;

        //        if (actionDescriptors.Keys.Contains(key))
        //        {
        //            if (actionDescriptor.MethodInfo.DeclaringType == controllerDescriptor.ControllerType && actionDescriptor.MethodInfo.ReflectedType == controllerDescriptor.ControllerType)
        //            {
        //                actionDescriptors[key] = actionDescriptor;
        //            }
        //        }
        //        else
        //        {
        //            actionDescriptors.Add(key, actionDescriptor);
        //        }
        //    }

        //    return actionDescriptors.ToLookup(pair => pair.Key, pair => (HttpActionDescriptor)pair.Value);
        //}

        //public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        //{
        //    Type controllerType = controllerContext.Controller.GetType();

        //    object actionName;
        //    var hasActionName = controllerContext.RouteData.Values.TryGetValue("action", out actionName);

        //    var method = controllerContext.Request.Method;
        //    var allMethods = controllerContext.ControllerDescriptor.ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
        //    var validMethods = Array.FindAll(allMethods, IsValidActionMethod);

        //    string actionNM = actionName.ToString().ToLowerInvariant();

        //    if (hasActionName)
        //    {
        //        validMethods = validMethods.Where(s => s.Name.ToLowerInvariant() == actionNM || (s.GetCustomAttribute<System.Web.Http.RouteAttribute>() == null ? false : s.GetCustomAttribute<System.Web.Http.RouteAttribute>().Template.ToLowerInvariant() == actionNM)).ToArray();
        //        if (validMethods.Length > 1)
        //        {
        //            validMethods = validMethods.Where(s => s.DeclaringType == controllerType && s.ReflectedType == controllerType).ToArray();
        //        }
        //    }

        //    var actionDescriptors = new HashSet<ReflectedHttpActionDescriptor>();
        //    IDictionary<ReflectedHttpActionDescriptor, string[]> _actionParams = new Dictionary<ReflectedHttpActionDescriptor, string[]>();

        //    foreach (var actionDescriptor in validMethods.Select(m => new ReflectedHttpActionDescriptor(controllerContext.ControllerDescriptor, m)))
        //    {
        //        actionDescriptors.Add(actionDescriptor);

        //        var parameters = actionDescriptor.ActionBinding.ParameterBindings
        //                         .Where(b => !b.Descriptor.IsOptional && b.Descriptor.ParameterType.UnderlyingSystemType.IsPrimitive)
        //                         .Select(b => b.Descriptor.Prefix ?? b.Descriptor.ParameterName).ToArray();

        //        _actionParams.Add(actionDescriptor, parameters);
        //    }

        //    IEnumerable<ReflectedHttpActionDescriptor> actionsFoundSoFar;

        //    if (hasActionName)
        //    {
        //        actionsFoundSoFar = actionDescriptors.Where(i => (i.ActionName.ToLowerInvariant() == actionNM && i.SupportedHttpMethods.Contains(method)) || (i.MethodInfo.GetCustomAttribute<System.Web.Http.RouteAttribute>() == null ? false : i.MethodInfo.GetCustomAttribute<System.Web.Http.RouteAttribute>().Template.ToLowerInvariant() == actionNM)).ToArray();
        //    }
        //    else
        //    {
        //        actionsFoundSoFar = actionDescriptors.Where(i => i.ActionName.ToLowerInvariant().Contains(method.ToString().ToLowerInvariant()) && i.SupportedHttpMethods.Contains(method)).ToArray();
        //    }

        //    var actionsFound = FindActionUsingRouteAndQueryParameters(controllerContext, actionsFoundSoFar, _actionParams);

        //    if (actionsFound == null || !actionsFound.Any()) throw new HttpResponseException(controllerContext.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cannot find a matching action."));
        //    if (actionsFound.Count() > 1) throw new HttpResponseException(controllerContext.Request.CreateErrorResponse(HttpStatusCode.Ambiguous, "Multiple action were found that match the request."));

        //    return actionsFound.FirstOrDefault();
        //} 
        #endregion

        #region V2
        public override ILookup<string, HttpActionDescriptor> GetActionMapping(HttpControllerDescriptor controllerDescriptor)
        {
            return LazyStorage.ContActions.Value.Actions[controllerDescriptor.ControllerName].ToLookup(pair => pair.Key.Split('-')[0], pair => (HttpActionDescriptor)pair.Value);
        }

        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            object actionName;
            var hasActionName = controllerContext.RouteData.Values.TryGetValue("action", out actionName);

            var method = controllerContext.Request.Method;
            string actionNM = actionName.ToString().ToLowerInvariant();

            var conByActions = LazyStorage.ContActions.Value.Actions[controllerContext.ControllerDescriptor.ControllerName];
            ReflectedHttpActionDescriptor candidate;
            conByActions.TryGetValue(actionNM, out candidate);
            List<ReflectedHttpActionDescriptor> actionsFoundSoFar = conByActions.Where(s => s.Key.Split('-')[0] == actionNM && s.Value.SupportedHttpMethods.Contains(method)).Select(s => s.Value).ToList();

            IDictionary<ReflectedHttpActionDescriptor, string[]> _actionParams = new Dictionary<ReflectedHttpActionDescriptor, string[]>();
            foreach (var actionDescriptor in actionsFoundSoFar)
            {
                var parameters = actionDescriptor.ActionBinding.ParameterBindings
                                        .Where(b => !b.Descriptor.IsOptional && b.Descriptor.ParameterType.UnderlyingSystemType.IsPrimitive)
                                        .Select(b => b.Descriptor.Prefix ?? b.Descriptor.ParameterName).ToArray();
                _actionParams.Add(actionDescriptor, parameters);
            }

            var actionsFound = FindActionUsingRouteAndQueryParameters(controllerContext, actionsFoundSoFar, _actionParams);

            if (actionsFound == null || !actionsFound.Any()) throw new HttpResponseException(controllerContext.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cannot find a matching action."));
            if (actionsFound.Count() > 1) throw new HttpResponseException(controllerContext.Request.CreateErrorResponse(HttpStatusCode.Ambiguous, "Multiple action were found that match the request."));

            return actionsFound.FirstOrDefault();
        }
        #endregion

        private IEnumerable<ReflectedHttpActionDescriptor> FindActionUsingRouteAndQueryParameters(HttpControllerContext controllerContext, IEnumerable<ReflectedHttpActionDescriptor> actionsFound, IDictionary<ReflectedHttpActionDescriptor, string[]> actionParams)
        {
            var routeParameterNames = new HashSet<string>(controllerContext.RouteData.Values.Keys, StringComparer.OrdinalIgnoreCase);

            if (routeParameterNames.Contains("controller")) routeParameterNames.Remove("controller");
            if (routeParameterNames.Contains("action")) routeParameterNames.Remove("action");

            var hasQueryParameters = controllerContext.Request.RequestUri != null && !String.IsNullOrEmpty(controllerContext.Request.RequestUri.Query);
            var hasRouteParameters = routeParameterNames.Count != 0;

            if (hasRouteParameters || hasQueryParameters)
            {
                var combinedParameterNames = new HashSet<string>(routeParameterNames, StringComparer.OrdinalIgnoreCase);
                if (hasQueryParameters)
                {
                    foreach (var queryNameValuePair in controllerContext.Request.GetQueryNameValuePairs())
                    {
                        combinedParameterNames.Add(queryNameValuePair.Key);
                    }
                }

                actionsFound = actionsFound.Where(s => actionParams[s].All(combinedParameterNames.Contains));

                if (actionsFound.Count() > 1)
                {
                    actionsFound = actionsFound
                        .GroupBy(descriptor => actionParams[descriptor].Length)
                        .OrderByDescending(g => g.Key)
                        .First();
                }
            }
            else
            {
                actionsFound = actionsFound.Where(descriptor => actionParams[descriptor].Length == 0);
            }

            return actionsFound;
        }

        private static bool IsValidActionMethod(MethodInfo methodInfo)
        {
            if (methodInfo.IsSpecialName) return false;
            return !methodInfo.GetBaseDefinition().DeclaringType.IsAssignableFrom(typeof(ApiController));
        }
    }
}