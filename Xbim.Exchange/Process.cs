using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.Exchange
{
    public class Process<ModelIn, ModelOut>
    {
        private List<Operation<ModelIn, ModelOut, ModelOut>> _operations;

        public void Add(Operation<ModelIn, ModelOut, ModelOut> operation)
        {
            _operations.Add(operation);
        }

        public bool Execute()
        {
            foreach (var o in _operations)
            {
                o.Execute();
            }
        }
    }
}
