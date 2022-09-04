using System;
using System.Collections.Generic;

/// <summary>
/// Модуль тестирования наследует данный класс.
/// </summary>
public class TestingUnit: TestingElement
{
    public TestingUnit()
    {
    }

    protected override void OnTest()
    {
        foreach(var kv in this.Container)
        {
            ((TestingElement)kv.Value).DoTest();
        }
    }

    public void Append(TestingElement test)
    {
        this.Container[test.GetType().Name] = test;
    }

    public void Push(string name, Action todo)
    {
        this.Push(new TestingOperation(name, todo));
    }
}

