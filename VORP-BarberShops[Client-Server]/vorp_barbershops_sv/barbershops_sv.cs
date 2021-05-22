﻿using CitizenFX.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorp_barbershops_sv
{
    public class barbershops_sv : BaseScript
    {
        public barbershops_sv()
        {
            EventHandlers["vorpbarbershop:BuyService"] += new Action<Player, uint, uint>(BuyService);
        }

        private async void BuyService([FromSource] Player player, uint beard, uint hair)
        {
            int _source = int.Parse(player.Handle);
            double cost = LoadConfig.Config["BarberCost"].ToObject<double>();
            PlayerList pl = new PlayerList();
            TriggerEvent("vorp:getCharacter", _source, new Action<dynamic>(async (user) =>
            {
                double money = user.money;

                if (money >= cost)
                {
                    TriggerEvent("vorp:removeMoney", _source, 0, cost);
                    await Delay(100);
                    JObject newcomps = new JObject();
                    newcomps.Add("Beard", beard);
                    newcomps.Add("Hair", hair);
                    TriggerEvent("vorpcharacter:setPlayerSkinChange", _source, newcomps.ToString());
                    player.TriggerEvent("vorp:TipRight", string.Format(LoadConfig.Langs["Bought"], cost.ToString()), 2000);
                }
                else
                {
                    player.TriggerEvent("vorp:TipRight", LoadConfig.Langs["NoMoney"], 2000);
                    TriggerClientEvent("vorpcharacter:refreshPlayerSkin");
                }


            }));
        }
    }
}
