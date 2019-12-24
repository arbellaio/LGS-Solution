#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using LGS;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Account/ForgotPassword.cshtml")]
    public partial class _Views_Account_ForgotPassword_cshtml : System.Web.Mvc.WebViewPage<LGS.Models.ForgotPasswordViewModel>
    {
        public _Views_Account_ForgotPassword_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\Account\ForgotPassword.cshtml"
  
    Layout = "../Shared/_LayoutAccount.cshtml";
    ViewBag.Title = "Forgot your password?";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"lockscreen-wrapper\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"lockscreen-logo\"");

WriteLiteral(">\r\n        <a");

WriteLiteral(" href=\"../../index2.html\"");

WriteLiteral("><b>LGS</b>LTE</a>\r\n    </div>\r\n    <!-- User name -->\r\n    <div");

WriteLiteral(" class=\"lockscreen-name\"");

WriteLiteral("></div>\r\n\r\n    <!-- START LOCK SCREEN ITEM -->\r\n    <div");

WriteLiteral(" class=\"lockscreen-item\"");

WriteLiteral(">\r\n        <!-- lockscreen image -->\r\n        <div");

WriteLiteral(" class=\"lockscreen-image\"");

WriteLiteral(">\r\n            <img");

WriteLiteral(" src=\"/favicon.ico\"");

WriteLiteral(" alt=\"Web App Ico\"");

WriteLiteral(">\r\n        </div>\r\n\r\n");

            
            #line 21 "..\..\Views\Account\ForgotPassword.cshtml"
        
            
            #line default
            #line hidden
            
            #line 21 "..\..\Views\Account\ForgotPassword.cshtml"
         using (Html.BeginForm("ForgotPassword", "Account", FormMethod.Post, new {@class = "lockscreen-credentials"}))
        {
            
            
            #line default
            #line hidden
            
            #line 23 "..\..\Views\Account\ForgotPassword.cshtml"
       Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 23 "..\..\Views\Account\ForgotPassword.cshtml"
                                    
            
            
            #line default
            #line hidden
            
            #line 24 "..\..\Views\Account\ForgotPassword.cshtml"
       Write(Html.ValidationSummary("", new {@class = "text-danger"}));

            
            #line default
            #line hidden
            
            #line 24 "..\..\Views\Account\ForgotPassword.cshtml"
                                                                     


            
            #line default
            #line hidden
WriteLiteral("            <div");

WriteLiteral(" class=\"input-group\"");

WriteLiteral(">\r\n\r\n");

WriteLiteral("                ");

            
            #line 28 "..\..\Views\Account\ForgotPassword.cshtml"
           Write(Html.TextBoxFor(m => m.Email, new {@class = "form-control", @placeholder = "Enter Email"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                <div");

WriteLiteral(" class=\"input-group-btn\"");

WriteLiteral(">\r\n                    <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn\"");

WriteLiteral(">\r\n                        <i");

WriteLiteral(" class=\"fa fa-arrow-right text-muted\"");

WriteLiteral("></i>\r\n                    </button>\r\n                </div>\r\n            </div>\r" +
"\n");

            
            #line 35 "..\..\Views\Account\ForgotPassword.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n    <!-- /.lockscreen-item -->\r\n    <div");

WriteLiteral(" class=\"help-block text-center\"");

WriteLiteral(">\r\n        Enter your email to reset your password\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"text-center\"");

WriteLiteral(">\r\n        <a");

WriteLiteral(" href=\"/Account/Login\"");

WriteLiteral(">Or sign in as a different user</a>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"lockscreen-footer text-center\"");

WriteLiteral(">\r\n        Copyright &copy; ");

            
            #line 45 "..\..\Views\Account\ForgotPassword.cshtml"
                    Write(DateTime.Today.Year);

            
            #line default
            #line hidden
WriteLiteral("\r\n        <b>\r\n            <a");

WriteLiteral(" href=\"\"");

WriteLiteral(" class=\"text-black\"");

WriteLiteral(">LGS LTE</a>\r\n        </b><br>\r\n        All rights reserved\r\n    </div>\r\n</div>\r\n" +
"\r\n\r\n");

DefineSection("Scripts", () => {

WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 55 "..\..\Views\Account\ForgotPassword.cshtml"
Write(Scripts.Render("~/bundles/jqueryval"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

});

        }
    }
}
#pragma warning restore 1591