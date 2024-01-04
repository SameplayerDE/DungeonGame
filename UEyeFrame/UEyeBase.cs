using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEyeFrame
{
    public class UEyeBase
    {
        public List<UEyeBase> Children = new();
        public bool IsSelected;

        public void Add(UEyeBase child)
        {
            Children.Add(child);
        }
    }
}
