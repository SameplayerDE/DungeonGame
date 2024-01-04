using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEyeFrame
{
    public class NumberField : UEyeBase
    {
        private int _value;

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnValueChanged?.Invoke(Value);
                //if (value >= Min && value <= Max)
                //{
                //    _value = value;
                //    OnValueChanged?.Invoke(Value);
                //}
            }
        }
        public int Min;
        public int Max;
        public Action<int> OnValueChanged;

        public NumberField(int min = 0, int max = 100, int value = 0, Action<int> onValueChanged = null)
        {
            Min = min;
            Max = max;   
            _value = value;
            OnValueChanged = onValueChanged;
        }
    }
}
