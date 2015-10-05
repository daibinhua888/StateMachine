using SMFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.状态机
{
    class 交易修改申请 : StateMachineConfig
    {
        protected override void States()
        {
            this.AddCanbeBeginState("已提交申请");
            this.AddAutoTransitToCompleteState("审核通过");
            this.AddAutoTransitToCompleteState("审核拒绝");
            this.AddAutoTransitToCompleteState("取消");
        }

        protected override void Links()
        {
            this.LinkStates("已提交申请", "审核通过");
            this.LinkStates("已提交申请", "审核拒绝");
            this.LinkStates("已提交申请", "取消");
        }

        protected override string MachineType()
        {
            return "交易修改申请";
        }
    }
}
