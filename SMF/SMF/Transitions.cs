using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMFramework
{
    internal static class Transitions
    {
        internal static void MapOneWayTo(State from, State to)
        {
            if (from.NextStates.Contains(to))
                return;

            from.NextStates.Add(to);
        }
    }
}
