using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMFramework
{
    public class State
    {
        public string StateName { get; set; }

        public bool AutoTransitToCompleteState { get; set; }

        public bool CanbeBeginState { get; set; }

        public List<State> NextStates { get; set; }

        public State(string state, bool autoTransitToCompleteState = false, bool canbeBeginState=false)
        {
            this.StateName = state;
            this.AutoTransitToCompleteState = autoTransitToCompleteState;
            this.CanbeBeginState = canbeBeginState;
            this.NextStates = new List<State>();
        }
    }
}
