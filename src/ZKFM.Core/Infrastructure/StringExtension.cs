﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ZKFM.Core.Infrastructure
{
    public static class StringExtension
    {
        /// <summary>
        /// 为字符串添加指定样式占位符
        /// </summary>
        /// <param name="s">要修改的字符串</param>
        /// <param name="placeholder">占位符规则</param>
        public static string AddBrackets(this string s, string placeholder = "[{0}]")
        {
            return string.Format(placeholder, s ?? string.Empty);
        }

        public static string ToMD5(this string s)
        {
            byte[] bs = Encoding.UTF8.GetBytes(s);
            using (MD5 md5 = MD5.Create())
            {
                bs = md5.ComputeHash(bs);
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bs.Length; i++)
            {
                sb.Append(bs[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static string UrlEncode(this string s)
        {
            return System.Net.WebUtility.UrlEncode(s);
        }
    }
}
