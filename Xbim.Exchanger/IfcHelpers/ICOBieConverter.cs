using System.ComponentModel;

namespace XbimExchanger.IfcHelpers
{
    public interface ICobieConverter
    {
        void Run(CobieConversionParams args);

        BackgroundWorker Worker
        { get; }
    }
}