using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMFramework
{
    public static class SMF
    {
        private static Dictionary<string, StateMachineConfig> statemachines = new Dictionary<string, StateMachineConfig>();

        public static StateMachine Get<T>(string relateId)
            where T : StateMachineConfig,new()
        {   
            StateMachineConfig machine = new T();

            machine.SetRelateID(relateId);

            machine.LoadLastSavedStateName();

            StateMachine sm = new StateMachine(machine);

            return sm;
        }
    }
}
