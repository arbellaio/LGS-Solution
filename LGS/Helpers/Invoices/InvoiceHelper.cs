using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace LGS.Helpers.Invoices
{
    public static class InvoiceHelper
    {
        public static string GetInvoiceNumber(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                userName = "Unknown";
            }
            var year = DateTime.UtcNow.Year.ToString();
            year = year.Remove(0, 2);
            var dayofYear = DateTime.UtcNow.DayOfYear.ToString("000");
            var time = DateTime.UtcNow.ToString("HHmmss");

            return userName + year + dayofYear + time;
        }

        public static string GetStringItemsSum(List<Item> items)
        {
            decimal sum = 0;
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    sum += Convert.ToDecimal(item.price);
                }

                return sum.ToString();
            }

            return null;
        }

        public static string GetUserNameFromEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var userName = (email).Split('@')[0];
                return userName;
            }

            return null;
        }
    }
}