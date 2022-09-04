using eckumoc_common_api.CommonCollections;

using System;

/// <summary>
/// 
/// </summary>
public class CommonCollectionsTest : TestingUnit
{
    public CommonCollectionsTest()
    {
        Push(new CommonDictionaryTest());
        Push(new CommonDimensionTest());
     
    }
}


public class CommonDictionaryTest : TestingElement
{
    protected override void OnTest()
    {
        canRaiseEvents();
        canUseDictionary();
    }

    private void canUseDictionary()
    {
        string code = "";
        var dictionary = new CommonDictionary<string>();
        dictionary.OnGet += (sender, evt) =>
        {
            Console.WriteLine(sender, evt);
        };
        dictionary.OnSet += (sender, before, after) =>
        {
            Console.WriteLine(sender, before, after);
        };
        dictionary.Set("button", @"<div>{{label}}</div>");
        dictionary.Set("button", @"<div>{{label}}</div>");
        dictionary.Get("button");
        dictionary.Get("button");
        code = dictionary.ToString();


        var decoded = new CommonDictionary<string>();

        dictionary.ToJsonOnScreen();
        decoded.ToJsonOnScreen();
    }

    private void canRaiseEvents()
    {
        var dictionary = new CommonDictionary<string>();
        dictionary.OnSet += (key, before, after) =>
        {
            Messages.Add($"[CommonDictionary] генерирует события при исп. метода Set");
        };
        dictionary.OnGet += (key, value) =>
        {
            Messages.Add($"[CommonDictionary] генерирует события при исп. метода Get");
        };
        dictionary.Set("test", "test");
        dictionary.Get("test");
    }
}

/// <summary>
/// 
/// </summary>
public class CommonDimensionTest : TestingElement
{
    protected override void OnTest()
    {
        var dimension = new CommonDimension<Type>();
    }
}
