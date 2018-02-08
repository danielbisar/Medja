using System;
using System.Collections.Generic;
using System.Linq;

namespace Medja.OpenTk.Eval
{
    public class MenuController
    {
        private Stack<Menu> _previousMenus;
        private List<Menu> _menus;

        public readonly Property<Menu> PropertyCurrentMenu;
        public Menu CurrentMenu
        {
            get { return PropertyCurrentMenu.Get(); }
            set { PropertyCurrentMenu.Set(value); }
        }            

        public MenuController()
        {
            PropertyCurrentMenu = new Property<Menu>();

            _previousMenus = new Stack<Menu>();
            _menus = new List<Menu>();
        }

        public bool CanBack()
        {
            return _previousMenus.Count > 0;
        }

        public void Back()
        {
            CurrentMenu = _previousMenus.Pop();
        }

        public void NavigateTo(string menuId)
        {
            var nextMenu = _menus.FirstOrDefault(p => p.Id == menuId);

            if (nextMenu == null)
                throw new ArgumentOutOfRangeException(nameof(menuId), "Given menu id is unknown");

            _previousMenus.Push(CurrentMenu);
            CurrentMenu = nextMenu;
        }

        internal void Add(Menu menu)
        {
            _menus.Add(menu);

            if (CurrentMenu == null)
                CurrentMenu = menu;
        }
    }
}
