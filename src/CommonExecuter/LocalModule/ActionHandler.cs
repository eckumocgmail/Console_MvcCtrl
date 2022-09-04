using System;

public class ActionHandler
{
    public ActionHandler()
    {
    }

    public Action<object> OnSuccess { get; set; }
    public Action<string> OnError { get; set; }
}