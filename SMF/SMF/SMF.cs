using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMFramework
{
    public static class SMF
    {
        private static Dictionary<string, BaseStateMachineDefine> statemachines = new Dictionary<string, BaseStateMachineDefine>();

        public static StateMachine Get<T>(string relateId)
            where T : BaseStateMachineDefine,new()
        {   
            BaseStateMachineDefine machine = new T();

            machine.SetRelateID(relateId);

            machine.LoadLastSavedStateName();

            StateMachine sm = new StateMachine(machine);

            return sm;
        }
    }
}
