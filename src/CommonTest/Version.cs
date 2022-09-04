using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


    public class Version
    {
        int _count;
        int _completed;
        public Version(int completed, int count)
        {
            _count = count;
            _completed = completed;
        }
        public override string ToString()
        {
            return $"{_completed}/{_count}";
        }
    }

