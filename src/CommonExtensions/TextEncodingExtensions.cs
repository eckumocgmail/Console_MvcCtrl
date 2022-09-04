using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TextEncodingExtensions
{
    public static string ToUrlEncode(this string url)
    {
        return System.Web.HttpUtility.UrlEncode(url);
    }
    public static byte[] GetBytes(this string text)
    {
        return Encoding.ASCII.GetBytes(text);
    }
}