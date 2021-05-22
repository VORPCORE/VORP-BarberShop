using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorp_barbershops_cl
{
    public class barbershops_cl : BaseScript
    {
        public static uint lastBeard = 0;
        public static uint lastHair = 0;

        public barbershops_cl()
        {
        }

        public static async Task ExitBarber()
        {
            API.ClearPedTasks(API.PlayerPedId(), 1, 1);
            API.ClearPedSecondaryTask(API.PlayerPedId());
            API.ClearPedTasksImmediately(API.PlayerPedId(), 1, 1);
            await Delay(50);
            TriggerEvent("vorpcharacter:refreshPlayerSkin");
        }

        public static void SetComponentToPed(string compName, int index)
        {
            if (compName.Contains("Beard"))
            {
                if (index == 0)
                {
                    lastBeard = 0;
                    Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xF8016BCA, 0);
                    Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
                }
                else
                {
                    lastBeard = SkinUtils.BEARD_MALE[index - 1];
                    Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), lastBeard, true, true, true);
                    Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, false);
                }
            }
            if (compName.Contains("Hair"))
            {
                if (index == 0)
                {
                    lastHair = 0;
                    Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x864B03AE, 0);
                    Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
                }
                else
                {
                    if (API.IsPedMale(API.PlayerPedId()))
                    {
                        lastHair = SkinUtils.HAIR_MALE[index - 1];
                        Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), lastHair, true, true, true);
                        Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, false);
                    }
                    else
                    {
                        lastHair = SkinUtils.HAIR_FEMALE[index - 1];
                        Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), lastHair, true, true, true);
                        Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, false);
                    }
                    
                }
            }

        }

        internal static void LoadComps()
        {
            TriggerEvent("vorpcharacter:getPlayerComps", new Action<dynamic, dynamic>((skin, cloths) => 
            {
                if (API.IsPedMale(API.PlayerPedId()))
                {
                    uint beard = ConvertValue(skin.Beard);
                    int index = SkinUtils.BEARD_MALE.IndexOf(beard);
                    if (index != -1)
                    {
                        BarberMenu.setIndex(0, index + 1);
                        lastBeard = beard;
                    }
                    uint hair = ConvertValue(skin.Hair);
                    int index2 = SkinUtils.HAIR_MALE.IndexOf(hair);
                    if (index2 != -1)
                    {
                        BarberMenu.setIndex(1, index2 + 1);
                        lastHair = hair;
                    }
                }
                else
                {
                    uint hair = ConvertValue(skin.Hair);
                    int index2 = SkinUtils.HAIR_FEMALE.IndexOf(hair);
                    if (index2 != -1)
                    {
                        BarberMenu.setIndex(1, index2 + 1);
                        lastHair = hair;
                    }
                }
            
            }));
        }

        internal static async Task BuyService()
        {
            TriggerServerEvent("vorpbarbershop:BuyService", lastBeard, lastHair);
            API.ClearPedTasks(API.PlayerPedId(), 1, 1);
            await Delay(2000);
        }

        public static uint ConvertValue(string s)
        {
            uint result;

            if (uint.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                int interesante = int.Parse(s);
                result = (uint)interesante;
                return result;
            }
        }
    }
}
