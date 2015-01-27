using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.Exchange
{
    public abstract class Operation<In, Out, OutModel> : IOperation
    {
        private OutModel _model;

        public Operation(OutModel model)
        {
            _model = model;
        }

        public abstract Out Execute(In input);


        private List<Func<OutModel, bool>> _postProcesses = new List<Func<OutModel,bool>>();

        protected List<Func<OutModel, bool>> PostProcesses { get { return _postProcesses; } }

        public bool PostProcess()
        {
            var result = true;
            var todo = new List<Func<OutModel,bool>>();
            foreach (var p in _postProcesses)
            {
                var r = p(_model);
                if (!r) todo.Add(p);
                result = result && r;
            }
            return result;
        }

        object IOperation.Execute(object input)
        {
            return Execute((In)input);
        }
    }
}
