using CitizenFX.Core;
using System;
using static CitizenFX.Core.Native.API;


namespace vorp_barbershops_cl
{
    class ClearCaches : BaseScript
    {
        public ClearCaches()
        {
            EventHandlers["onResourceStop"] += new Action<string>(OnResourceStop);
        }

        private void OnResourceStop(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            Debug.WriteLine($"{resourceName} cleared blips and NPC's.");

            foreach (int blip in InitBarbers.ShopsBlips)
            {
                int _blip = blip;
                RemoveBlip(ref _blip);
            }

            foreach (int npc in InitBarbers.ShopsPeds)
            {
                int _ped = npc;
                DeletePed(ref _ped);
            }
        }

    }
}
