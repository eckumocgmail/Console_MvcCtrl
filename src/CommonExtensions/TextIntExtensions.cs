using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public static class TextIntExtensions
{
    public static bool IsHour(this int number)
    {
        return number > 0 && number <= 24;
    }
    public static bool IsMinute(this int number)
    {
        return number > 0 && number < 60;
    }
    public static bool IsSecond(this int number)
    {
        return number > 0 && number < 60;
    }
    public static bool IsMillisecond(this int number)
    {
        return number > 0 && number < 1000;
    }

    public static bool IsYear(this int number)
    {
        return number > 0 && number < 2100;
    }
    public static bool IsLeapYear(this int number)
    {
        return number.IsYear() && ((number % 4) == 0);
    }
    public static bool IsMonth(this int number)
    {
        return number > 0 && number <= 12;
    }

    public static bool IsDayOfMonth(this int number, int year, int month)
    {
        if (year.IsYear() == false || month.IsMonth() == false)
        {
            return false;
        }
        int minDate = 1;
        int maxDate = (month < 8) ? (30 + month % 2) : (30 + (month % 2 == 1 ? 0 : 1));
        if (month == 2)
        {
            maxDate = year.IsLeapYear() ? 29 : 28;
        }
        return number >= minDate && number <= maxDate;
    }
}