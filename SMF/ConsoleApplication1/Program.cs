using ConsoleApplication1.状态机;
using SMFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string requestId = Guid.NewGuid().ToString();

            var test=SMF.Get<交易修改申请>(requestId).CanTransitToState("已提交申请");

            Console.WriteLine(string.Format("能否切换到 已提交申请 状态？{0}", test));

            SMF.Get<交易修改申请>(requestId).TransitToState("已提交申请");

            SMF.Get<交易修改申请>(requestId).TransitToState("审核拒绝", () =>
            {
                Console.WriteLine("...");
            });

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}