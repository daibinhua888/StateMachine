using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMFramework
{
    internal interface IStateRepository
    {
        string Find(string stateMachineType, string relatedId);

        void Save(string stateMachineType, string relatedId, string stateName);
    }
}
