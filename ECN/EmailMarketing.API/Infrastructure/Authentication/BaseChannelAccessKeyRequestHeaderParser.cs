using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

using FrameworkModel = ECN_Framework_Entities.Accounts.BaseChannel;
using FrameworkObjectManager = ECN_Framework_BusinessLayer.Accounts.BaseChannel;

namespace EmailMarketing.API.Infrastructure.Authentication
{
    public class BaseChannelAccessKeyRequestHeaderParser : AccessKeyRequestHeaderParserBase<FrameworkModel>
    {
        /// <inheritdoc/>
        protected override string FrameworkObjectFriendlyName
        {
            get { return "base-channel"; }
        }

        /// <inheritdoc/>
        protected override string FrameworkObjectStashKey
        {
            get { return Strings.Properties.APIBaseChannelStashKey; }
        }

        /// <inheritdoc/>
        protected override bool TryGetFrameworkObject(HttpActionContext actionContext, Guid parsedHeaderValue, out FrameworkModel frameworkObject)
        {
            //frameworkObject = KMPlatform.BusinessLogic.User.GetByAccessKey(parsedHeaderValue.ToString(), true);
            frameworkObject = FrameworkObjectManager.GetAll().FirstOrDefault(x => x.AccessKey == parsedHeaderValue);
            if (null != frameworkObject)
            {
                return true;
            }

            return false;
        }
    }
}