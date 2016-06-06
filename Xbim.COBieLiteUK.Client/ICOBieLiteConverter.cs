using System.ComponentModel;

namespace Xbim.COBieLiteUK.Client
{
    public interface ICobieLiteConverter
    {
        void Run(CobieConversionParams args);

        BackgroundWorker Worker
        { get; }
    }
}