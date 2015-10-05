using SMFramework.Exceptions;
using SMFramework.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMFramework
{
    public abstract class StateMachineConfig
    {
        private IStateRepository stateRepository = null;
        private Dictionary<string, State> states = new Dictionary<string, State>();
        private string initialState = string.Empty;
        private string completeState = string.Empty;
        private string relatedId = string.Empty;
        private string stateMachineType = string.Empty;

        private string currentState = string.Empty;


        public string InitialState { get { return this.initialState; } }
        public string CompleteState { get { return this.completeState; } }

        public StateMachineConfig()
        {
            this.initialState = "Init";
            this.completeState = "Done";

            this.stateMachineType = MachineType();

            this.AddState("Init", false, false);
            States();
            this.AddState("Done", false, false);

            AutoLinkToBeginStates();

            Links();

            AutoLinkAutoCompleteStates();

            this.currentState = this.initialState;

            //this.stateRepository = new StateRepositoryImpl();
            this.stateRepository = new InMemoryStateRepositoryImpl();
        }

        private void AutoLinkAutoCompleteStates()
        {
            foreach (var ele in this.states)
                if (ele.Value.AutoTransitToCompleteState)
                    this.LinkStates(ele.Value.StateName, "Done");
        }

        private void AutoLinkToBeginStates()
        {
            foreach (var ele in this.states)
                if (ele.Value.CanbeBeginState)
                    this.LinkStates("Init", ele.Value.StateName);
        }

        public void LoadLastSavedStateName()
        {
            string lastState = stateRepository.Find(this.stateMachineType, this.relatedId);

            if (string.IsNullOrEmpty(lastState))
            {
                currentState = this.initialState;
                return;
            }

            this.currentState = lastState;
        }

        protected abstract string MachineType();

        protected abstract void States();

        protected abstract void Links();

        protected void AddCanbeBeginState(string stateName)
        {
            AddState(stateName, false, true);
        }

        protected void AddAutoTransitToCompleteState(string stateName)
        {
            AddState(stateName, true, false);
        }

        protected void AddState(string stateName, bool autoTransitToCompleteState=false, bool canbeBeginState=false)
        {
            var state = new State(stateName, autoTransitToCompleteState, canbeBeginState);

            this.AddState(state);
        }

        protected void LinkStates(string from, string to)
        { 
            var fromState=this.states[from];
            var toState = this.states[to];

            LinkStates(fromState, toState);
        }


        private void AddState(State state)
        {
            states.Add(state.StateName, state);
        }

        private void LinkStates(State from, State to)
        {
            Util.MapOneWayTransition(from, to);
        }

        private void SetCurrentState(string stateName)
        {
            this.currentState = stateName;

            //db save
            stateRepository.Save(this.stateMachineType, this.relatedId, this.currentState);
        }

        internal void TransitToNewState(string newStateName)
        {
            if (!CanTransitable(newStateName))
                throw new CannotTransitStateException();

            SetCurrentState(newStateName);

            ProcessAutoCompleteStates(newStateName);
        }

        private void ProcessAutoCompleteStates(string stateName)
        {
            var state=this.states[stateName];

            if (state.AutoTransitToCompleteState)
                SetCurrentState(this.CompleteState);
        }

        internal bool CanTransitable(string newStateName)
        {
            //根据当前状态名，得到所有第一级后续状态名
            //如存在列表中，则能转换过去
            //如不存在，则不能转换过去

            var currentState=this.states[this.currentState];

            bool exists = false;
            foreach (var st in currentState.NextStates)
            {
                if (st.StateName == newStateName)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        internal void SetRelateID(string relateId)
        {
            this.relatedId = relateId;
        }
    }
}
