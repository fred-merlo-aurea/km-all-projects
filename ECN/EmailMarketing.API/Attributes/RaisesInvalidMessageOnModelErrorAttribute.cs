using EmailMarketing.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace EmailMarketing.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class RaisesInvalidMessageOnModelErrorAttribute: OrderedActionFilterAttribute
    {
        public RaisesInvalidMessageOnModelErrorAttribute() : base(order: 99) { }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse( 
                    System.Net.HttpStatusCode.BadRequest, actionContext.ModelState);
                if(ExceptionsLoggedAttribute.LogNonServerErrors)
                {
                    AuthenticatedUserControllerBase c = actionContext.ControllerContext.Controller as AuthenticatedUserControllerBase;
                    c.LogError(ExceptionsLoggedAttribute.WebApiNonServerErrorLevel,
                        new HttpException( 
                            (int)actionContext.Response.StatusCode ,
                            actionContext.Response.ReasonPhrase, 
                            new HttpRequestValidationException( Newtonsoft.Json.JsonConvert.SerializeObject( actionContext.ModelState) )
                        ), 
                        actionContext.Response.ReasonPhrase);
                }
            }
            base.OnActionExecuting(actionContext);
        }
    }
}