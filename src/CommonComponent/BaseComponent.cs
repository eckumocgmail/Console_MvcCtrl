

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;


public abstract class BaseComponent: ChangeSupport, OnInit, OnChange, OnDestroy, OnUpdate
{
    public bool Initialized { get; set; } = false;
    public bool Destroyed { get; set; } = false;
  
    public int Updated { get; set; } = 0;


    public abstract void Update();
    public abstract void Destroy();
    public abstract void Change();
    public abstract void Init();
}



