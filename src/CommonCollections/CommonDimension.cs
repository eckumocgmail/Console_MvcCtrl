using API;

using eckumoc_common_api.CommonCollections;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;


public interface ICommonDimension<T>: ICommonDictionary<ICommonDictionary<T>>
{

}
public class CommonDimension<T> : 
    CommonDictionary<ICommonDictionary<T>>,
    ICommonDimension<T> where T : class
{
      
    public CommonDimension(): base()
    {
    }
}
