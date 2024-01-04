using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEyeFrame
{
    public class Counter : UEyeBase
    {
        public int Min;
        public int Max;
        public int Step;
        public int Value;

        public Counter(int min = 0, int max = 100, int step = 1, int value = 50)
        {
            Min = min;
            Max = max;
            Step = step;
            Value = value;
        }
    }
}
