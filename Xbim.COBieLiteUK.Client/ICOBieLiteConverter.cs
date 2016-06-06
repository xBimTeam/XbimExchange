using System.ComponentModel;

namespace Xbim.COBieLiteUK.Client
{
    public interface ICOBieLiteConverter
    {
        void Run(CobieConversionParams args);

        BackgroundWorker Worker
        { get; }
    }
}