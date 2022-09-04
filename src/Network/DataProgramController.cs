using CommonHttp;

using eckumoc_common_api.Views.Navigation;

using Microsoft.AspNetCore.Mvc;

using MVC.Controllers;

using System.Collections.Generic;

public class DataProgramController: ServiceMethodController<NameSpaceModule.LocalServiceProvider>
{

    public override object Index()
    {
        var odbc = new OdbcDriverManager();
        return odbc.GetOdbcDrivers();
    }


}