using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorp_barbershops_cl
{
    public class barbershops_cl : BaseScript
    {
        public barbershops_cl()
        {
            API.RegisterCommand("stops", new Action<int, List<object>, string>((source, args, raw) =>
            {
                API.ClearPedTasks(API.PlayerPedId(), 1, 1);
                API.ClearPedSecondaryTask(API.PlayerPedId());
                API.ClearPedTasksImmediately(API.PlayerPedId(), 1, 1);
            }), false);
        }
    }
}
