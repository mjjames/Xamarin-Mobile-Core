using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MKS.Mobile.Core.Interfaces;

namespace MKS.Mobile.Core.iOS
{
    public class iOSHttpUtility : IHttpUtility
    {

        public string HtmlEncode(string value)
        {
            return System.Web.HttpUtility.HtmlEncode(value);
        }
    }
}