using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class IntRandomExtensions
{
    private static Random R = new Random();
 
  
    public static int _floor(this decimal v)
    {
        //int xmax = int.MaxValue;
        //int xmin = int.MinValue;
        return (int)Math.Floor(v);
    }
}