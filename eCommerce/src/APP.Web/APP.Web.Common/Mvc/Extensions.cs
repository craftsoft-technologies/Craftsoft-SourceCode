using System;
using System.Linq.Expressions;
using System.Threading;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Globalization;
using APP.Web.Common.Models;
using APP.Web.Common.Utilities;

namespace APP.Web.Common.Mvc
{
    public static class HtmlExtension
    {
        internal static CultureInfo DefaultCultureInfo = new CultureInfo("en");
        internal static CultureInfo CurrentCultureInfo { get; set; }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DropdownListType dropdownListType)
        {
            string culture = Thread.CurrentThread.CurrentCulture.Name.ToLower();
            return htmlHelper.DropDownListFor(expression, DropdownListUtil.GetMvcDropdownList(culture, dropdownListType));
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DropdownListType dropdownListType, string selectedValue)
        {
            string culture = Thread.CurrentThread.CurrentCulture.Name.ToLower();
            return htmlHelper.DropDownListFor(expression, DropdownListUtil.GetMvcDropdownList(culture, dropdownListType, selectedValue));
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DropdownListType dropdownListType, object htmlAttributes)
        {
            string culture = Thread.CurrentThread.CurrentCulture.Name.ToLower();
            return htmlHelper.DropDownListFor(expression, DropdownListUtil.GetMvcDropdownList(culture, dropdownListType), htmlAttributes);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DropdownListType dropdownListType, string selectedValue, object htmlAttributes)
        {
            string culture = Thread.CurrentThread.CurrentCulture.Name.ToLower();
            return htmlHelper.DropDownListFor(expression, DropdownListUtil.GetMvcDropdownList(culture, dropdownListType, selectedValue), htmlAttributes);
        }

        public static MvcHtmlString DropDownListWithDashEmptyOptionFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DropdownListType dropdownListType, object htmlAttributes)
        {
            string culture = Thread.CurrentThread.CurrentCulture.Name.ToLower();
            return htmlHelper.DropDownListFor(expression, DropdownListUtil.GetMvcDropdownListWithTitle(culture, dropdownListType, "-", string.Empty), htmlAttributes);
        }

        public static MvcHtmlString DropDownListWithTitleFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DropdownListType dropdownListType, object htmlAttributes, string title)
        {
            string culture = Thread.CurrentThread.CurrentCulture.Name.ToLower();
            return htmlHelper.DropDownListFor(expression, DropdownListUtil.GetMvcDropdownListWithTitle(culture, dropdownListType, title, string.Empty), htmlAttributes);
        }

        public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string labelText, object htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            TagBuilder tag = new TagBuilder("label");
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.Attributes.Add("for", htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(labelText);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        /// <summary>Converts decimal amounts to currency format and, by default, styles them with red font-color if value is negative.</summary>
        /// <param name="amount">Decimal-typed amount to wrap around.</param>
        /// <param name="htmlAttributes">Additional attributes to attach to the element.</param>
        /// <param name="negate">True to multiply the specified amount by negative one; otherwise, false.</param>
        /// <returns>Span-wrapped money amount</returns>
        public static MvcHtmlString ToHtmlCurrency(this decimal amount, object htmlAttributes = null, bool negate = false)
        {
            TagBuilder tag = new TagBuilder("span");

            if (htmlAttributes != null)
            {
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            if (negate)
                amount = Decimal.Negate(amount);

            if (amount < 0)
            {
                if (tag.Attributes.ContainsKey("class"))
                    tag.Attributes["class"] += " negative";
                else
                    tag.Attributes.Add("class", "negative");
            }

            tag.SetInnerText(amount.ToCurrency());

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static decimal ToDecimalByEnGB(this decimal input)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            CultureInfo defaultCulture = new CultureInfo("en");

            Thread.CurrentThread.CurrentCulture = defaultCulture;
            string currency = input.ToString();
            Thread.CurrentThread.CurrentCulture = cultureInfo;

            return decimal.Parse(currency, defaultCulture);
        }

        public static string ToDecimalDisplay(this decimal input)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            CultureInfo defaultCulture = new CultureInfo("en");

            Thread.CurrentThread.CurrentCulture = defaultCulture;
            string currency = input.ToString();
            Thread.CurrentThread.CurrentCulture = cultureInfo;

            return currency;
        }

        public static void OpenDefaultCultureInfo(CultureInfo currentCultureInfo)
        {
            CurrentCultureInfo = currentCultureInfo;
            Thread.CurrentThread.CurrentCulture = DefaultCultureInfo;
        }

        public static void CloseDefaultCultureInfo()
        {
            Thread.CurrentThread.CurrentCulture = CurrentCultureInfo ?? DefaultCultureInfo;
        }

    }
}
