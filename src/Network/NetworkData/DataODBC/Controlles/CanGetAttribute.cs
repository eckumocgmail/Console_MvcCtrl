using Microsoft.AspNetCore.Mvc;

using System;

public class CanGetAttribute : HttpGetAttribute
{
    private string route;

    public CanGetAttribute(string route):base(route)
    {
        this.route = route;
    }
}