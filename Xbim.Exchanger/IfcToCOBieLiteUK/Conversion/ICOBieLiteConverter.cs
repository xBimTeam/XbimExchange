using System.ComponentModel;

namespace XbimExchanger.IfcToCOBieLiteUK.Conversion
{
    public interface ICobieLiteConverter
    {
        void Run(CobieConversionParams args);

        BackgroundWorker Worker
        { get; }
    }
}