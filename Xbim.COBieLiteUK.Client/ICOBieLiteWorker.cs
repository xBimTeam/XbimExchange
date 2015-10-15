using System.ComponentModel;

namespace Xbim.Client
{
    public interface ICOBieLiteWorker
    {
        void Run(Params args);

        BackgroundWorker Worker
        { get; }
    }
}