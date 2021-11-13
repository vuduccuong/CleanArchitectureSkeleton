using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extentions
{
    public static class StringExtentions
    {
        /// <summary>
        /// Format text theo đa ngôn ngữ
        /// </summary>
        /// <param name="str1">first string</param>
        /// <param name="str2">second string</param>
        /// <returns>New string</returns>
        public static string FormatCurentCulture(string str1, string str2)
        {
            return string.Format(CultureInfo.CurrentCulture, str1, str2);
        }

        /// <summary>
        /// So sánh 2 chuỗi chấp nhận chuỗi không thống nhất chữ hoa-thường
        /// </summary>
        /// <param name="str1">first string</param>
        /// <param name="str2">second string</param>
        /// <returns>True: Nếu 2 chuỗi giống nhau</returns>
        public static bool EqualsIgnoreCase(this string str1, string str2)
        {
            return str1.Equals(str2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
