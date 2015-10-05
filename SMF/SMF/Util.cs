using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMFramework
{
    internal static class Util
    {
        internal static void MapOneWayTransition(State from, State to)
        {
            if (from.NextStates.Contains(to))
                return;

            from.NextStates.Add(to);
        }
    }
}
