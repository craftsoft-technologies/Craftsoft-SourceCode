using System;

namespace APP.Common.Utilities.DataTypeUtilities
{
    public static class DecimalUtil
    {
        public static string FormatCurrency(string currencyCode, decimal currency)
        {
            return string.Format("{0} {1:#,0.00}", currencyCode, currency);
        }

        public static string FormatCurrency(decimal currency)
        {
            return string.Format("{0:#,0.00}", currency);
        }

        /// <summary>
        /// format decimal to currency: 12,345.00, keep the 2 decimal
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="rounded">
        /// true: round up/down decimal value, for example: 12,345.678 => 12,345.68, 1.234 => 1.23
        /// false: only truncate decimal value, for example: 12,345.678 => 12,345.67
        /// </param>
        /// <returns></returns>
        public static string ToCurrency(this decimal currency, bool rounded = true)
        {
            if (rounded)
            {
                return string.Format("{0:#,0.00}", currency);
                // the same format: currency.ToString("N2");
            }
            else
            {
                return string.Format("{0:#,0.00}", Decimal.Truncate(currency * 100) / 100);
            }
        }

        public static decimal RoundTwoDecimal(this decimal currency)
        {
            return Convert.ToDecimal(ToCurrency(currency));
        }
    }
}
