using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Нехватает tool-manifest, nunit-test
/// </summary>
public enum DotnetTemplates
{
    nunitTest,
    toolManifest,

    wpf, 
    wpflib, 
    wpfcustomcontrollib, 
    wpfusercontrollib, 
    winforms, 
    winformscontrollib, 
    winformslib, 
    worker, 
    mstest, 
    nunit, 
    xunit, 
    razorcomponent, 
    page, 
    viewimports, 
    viewstart, 
    blazorserver, 
    blazorwasm, 
    web, 
    webapp, 
    react,
    reactredux, 
    razorclasslib, 
    webapi, 
    grpc, 
    gitignore, 
    mvc, 
    angular, 
    razor, 
    console, 
    classlib, 
    proto, 
    sln, 
    webconfig, 
    nugetconfig, 
    globaljson


    
}