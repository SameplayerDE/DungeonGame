using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEyeFrame
{
    public class Menu
    {
        private List<UEyeBase> _items;
        private int _index;

        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _items[_index].IsSelected = false;
                _index = Math.Clamp(value, 0, _items.Count - 1);
                _items[_index].IsSelected = true;
            }
        }

        public List<UEyeBase> Items
        {
            get { return _items; }
        }

        public Menu()
        {
            _items = new();
            _index = 0;
        }

        public void Add(UEyeBase item)
        {
            _items.Add(item);
        }

        //public void Invoke()
        //{
        //    _items[_index].Action?.Invoke();
        //}
    }
}
