using System;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using APP.Web.Common.Mvc;

namespace APP.Web.Common.Utilities
{
    public static class FormatUtility
    {
        public static string FormatCurrency(string currencyCode, decimal currency)
        {
            HtmlExtension.OpenDefaultCultureInfo(Thread.CurrentThread.CurrentCulture);
            string currencyString = string.Format("{0} {1:#,0.00}", currencyCode, currency);
            HtmlExtension.CloseDefaultCultureInfo();
            return currencyString;
        }

        public static string FormatCurrency(decimal currency)
        {
            HtmlExtension.OpenDefaultCultureInfo(Thread.CurrentThread.CurrentCulture);
            string currencyString = string.Format("{0:#,0.00}", currency);
            HtmlExtension.CloseDefaultCultureInfo();
            return currencyString;
        }

        /// <summary>
        /// format decimal to currency: 12,345.00, keep the 2 decimal
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="rounded">
        /// true: round up/down decimal value, for example: 12,345.678 => 12,345.68, 1.234 => 1.23
        /// false: only truncate decimal value, for example: 12,345.678 => 12,345.67
        /// </param>
        /// <returns></returns>
        public static string ToCurrency(this decimal amount, bool rounded = true)
        {
            HtmlExtension.OpenDefaultCultureInfo(Thread.CurrentThread.CurrentCulture);
            string currencyString = string.Empty;
            if (rounded)
            {
                currencyString = string.Format("{0:#,0.00}", amount);
                // the same format: currency.ToString("N2");
            }
            else
            {
                currencyString = string.Format("{0:#,0.00}", (decimal.Truncate(amount * 100) / 100));
            }
            HtmlExtension.CloseDefaultCultureInfo();
            return currencyString;
        }

        /// <summary>
        /// format decimal to currency: 12,345, keep the 0 decimal
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="rounded">
        /// true: round up/down decimal value, for example: 12,345.678 => 12,346, 1.234 => 1
        /// false: only truncate decimal value, for example: 12,345.678 => 12,345
        /// </param>
        /// <returns></returns>
        public static string ToZeroCurrency(this decimal amount, bool rounded = true)
        {
            HtmlExtension.OpenDefaultCultureInfo(Thread.CurrentThread.CurrentCulture);
            string currencyString = string.Empty;
            if (rounded)
            {
                currencyString = string.Format("{0:#,0}", amount);
                // the same format: currency.ToString("N2");
            }
            else
            {
                currencyString = string.Format("{0:#,0}", (decimal.Truncate(amount * 100) / 100));
            }
            HtmlExtension.CloseDefaultCultureInfo();
            return currencyString;
        }

        public static decimal ParseCurrency(this decimal amount, bool rounded = true)
        {
            return decimal.Parse(amount.ToCurrency(), NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint);
        }

        public static string ToNegativeCurrency(this decimal amount)
        {
            StringBuilder sb = new StringBuilder();
            if (amount < 0)
            {
                return sb.AppendFormat("<span style=\"color: Red;\">{0}</span>", amount.ToCurrency()).ToString();
            }
            else
            {
                return amount.ToCurrency();
            }
        }

        public static decimal ToDecimal(this decimal input, int decimalPlaces)
        {
            return Math.Round(input, decimalPlaces, MidpointRounding.AwayFromZero);
        }
    }
}
