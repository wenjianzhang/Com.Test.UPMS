#region License

/**
 * 作者：张文健
 */

#endregion License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Com.Test.UPMS.Web.Filter
{
    public class AuthAttribute : System.Web.Http.Filters.FilterAttribute, System.Web.Http.Filters.IAuthorizationFilter
    {
        private readonly bool _isLogin;

        public AuthAttribute(bool isLogin)
        {
            _isLogin = isLogin;
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            throw new NotImplementedException();
        }
    }
}