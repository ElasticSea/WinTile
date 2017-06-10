using System;
using App.Model;

namespace App.Utils
{
    public class SelectedHolder
    {
        // TODO could fody solve this ?
        public Tile Selected
        {
            get => _selected;
            set
            {
                if (value == _selected) return;

                _selected = value;
                OnSelected(value);
            }
        }

        private Tile _selected;

        public event Action<Tile> OnSelected = tile => { };
    }
}