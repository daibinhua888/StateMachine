using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMFramework.Repositorys
{
    class InMemoryStateRepositoryImpl : IStateRepository
    {
        private static Dictionary<string, string> table = new Dictionary<string, string>();

        public string Find(string stateMachineType, string relatedId)
        {
            if (!table.ContainsKey(relatedId))
                return null;

            return table[relatedId];
        }

        public void Save(string stateMachineType, string relatedId, string stateName)
        {
            table[relatedId] = stateName;
        }
    }
}
