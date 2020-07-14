using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MenuAPI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorp_barbershops_cl
{
    class BarberMenu
    {
        private static Menu mainMenu = new Menu(GetConfig.Langs["TitleBarberMenu"], GetConfig.Langs["SubTitleBarberMenu"]);
        private static bool setupDone = false;
        private static MenuListItem BeardsBtn;
        private static MenuListItem HairBtn;
        private static bool isBuy = false;

        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(mainMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            List<string> beardValues = new List<string>();

            if (IsPedMale(PlayerPedId()))
            {
                beardValues.Add(GetConfig.Langs["NoBeard"]);

                for (int b = 1; b < SkinUtils.BEARD_MALE.Count + 1; b++)
                {
                    beardValues.Add(GetConfig.Langs["Beard"] + b.ToString());
                }
            }
            else
            {
                beardValues.Add(GetConfig.Langs["NoExist"]);
            }

            BeardsBtn = new MenuListItem(GetConfig.Langs["BeardList"], beardValues, 0, GetConfig.Langs["BeardDesc"])
            {

            };

            mainMenu.AddMenuItem(BeardsBtn);

            List<string> hairValues = new List<string>();

            if (IsPedMale(PlayerPedId()))
            {
                hairValues.Add(GetConfig.Langs["NoHair"]);

                for (int b = 1; b < SkinUtils.HAIR_MALE.Count + 1; b++)
                {
                    hairValues.Add(GetConfig.Langs["Hair"] + b.ToString());
                }
            }
            else
            {
                hairValues.Add(GetConfig.Langs["NoHair"]);

                for (int b = 1; b < SkinUtils.HAIR_FEMALE.Count + 1; b++)
                {
                    hairValues.Add(GetConfig.Langs["Hair"] + b.ToString());
                }
            }

            HairBtn = new MenuListItem(GetConfig.Langs["HairList"], hairValues, 0, GetConfig.Langs["HairDesc"])
            {

            };

            mainMenu.AddMenuItem(HairBtn);

            //Finish Button
            MenuItem FinishBtn = new MenuItem(string.Format(GetConfig.Langs["FinishBtn"], GetConfig.Config["BarberCost"].ToString()), GetConfig.Langs["SubFinishBtn"])
            {
                RightIcon = MenuItem.Icon.TICK
            };
            mainMenu.AddMenuItem(FinishBtn);

            //Events
            mainMenu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                if (_itemIndex == 0)
                {
                   barbershops_cl.SetComponentToPed("Beard", _newIndex);
                }else if (_itemIndex == 1)
                {
                    barbershops_cl.SetComponentToPed("Hair", _newIndex);
                }
            };

            mainMenu.OnMenuClose += (_menu) =>
            {
                if (!isBuy)
                {
                    barbershops_cl.ExitBarber();
                }

            };

            mainMenu.OnMenuOpen += (_menu) => 
            {
                isBuy = false;
                barbershops_cl.LoadComps();
            };

            mainMenu.OnItemSelect += (_menu, _item, _index) =>
            {
                // Code in here would get executed whenever an item is pressed.
                if (_index == 2)
                {
                    isBuy = true;
                    barbershops_cl.BuyService();
                    mainMenu.CloseMenu();
                }
            };

        }

        public static void setIndex(int first, int second)
        {
            switch (first)
            {
                case 0:
                    BeardsBtn.ListIndex = second;
                    break;
                case 1:
                    HairBtn.ListIndex = second;
                    break;
            }
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return mainMenu;
        }
    }
}
