using System;
using System.Threading;
using System.Web.Mvc;
using APP.Web.Common.Models;
using APP.Web.Common.Utilities;

namespace APP.Web.Common.Mvc
{
    public static class StatusExtensions
    {
        private const string SuccessTagFormat = "<span class=\"status-s\">{0}</span>";
        private const string FailTagFormat = "<span class=\"status-f\">{0}</span>";
        private const string PendingTagFormat = "<a id=\"Cancelled\" class=\"icon-remove\" href=\"#\" transactionAmount=\"{1}\" transactionNo=\"{2}\"></a><span class=\"status-o\">{0}</span>";
        private const string CancelledTagFormat = "<span class=\"status-f\">{0}</span>";
        private const string RunningTagFormat = "<span class=\"status-f\">{0}</span>";
        private const string OtherTagFormat = "<span class=\"status-o\">{0}</span>";

        private static MvcHtmlString Print(string format, DropdownListType type, int value, string culutreName)
        {
            string text = DropdownListUtil.GetText(culutreName, type, value);
            return MvcHtmlString.Create(string.Format(format, text));
        }

        private static MvcHtmlString Print(string format, DropdownListType type, int value)
        {
            string culutreName = Thread.CurrentThread.CurrentCulture.Name.ToLower();
            string text = DropdownListUtil.GetText(culutreName, type, value);
            return MvcHtmlString.Create(string.Format(format, text));
        }

        private static MvcHtmlString Print(DropdownListType type, int value)
        {
            string culutreName = Thread.CurrentThread.CurrentCulture.Name.ToLower();
            string text = DropdownListUtil.GetText(culutreName, type, value);
            return MvcHtmlString.Create(text);
        }
    }
}
