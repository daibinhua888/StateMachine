using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SMFramework
{
    public class StateMachine
    {
        private BaseStateMachineDefine stateMachineDefine = null;

        public StateMachine(BaseStateMachineDefine define)
        {
            this.stateMachineDefine = define;
        }

        public void TransitToState(string newStateName, Action action=null, bool transactionSupport=false)
        { 
            //先判断是否能把当前状态转换过去
            //yes
            //      -->执行action
            //      -->更改状态名
            //no
            //      -->抛错

            if (!stateMachineDefine.CanTransitable(newStateName))
                throw new CannotTransitStateException();

            Action innerAction = () =>
            {
                if (action != null)
                    action();

                stateMachineDefine.TransitToNewState(newStateName);
            };

            if (!transactionSupport)
            {
                innerAction();
                return;
            }

            using (var ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel =  IsolationLevel.ReadCommitted}))
            {
                innerAction();

                ts.Complete();
            }
        }

        public bool CanTransitToState(string newStateName)
        {
            return stateMachineDefine.CanTransitable(newStateName);
        }
    }
}
