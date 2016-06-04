using System.ComponentModel;

namespace Xbim.COBieLiteUK.Client
{
    public interface ICOBieLiteWorker
    {
        void Run(Params args);

        BackgroundWorker Worker
        { get; }
    }
}