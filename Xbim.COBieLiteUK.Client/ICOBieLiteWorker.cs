using System.ComponentModel;

namespace Xbim.COBieLiteUK.Client
{
    public interface ICOBieLiteWorker
    {
        void Run(CobieConversionParams args);

        BackgroundWorker Worker
        { get; }
    }
}