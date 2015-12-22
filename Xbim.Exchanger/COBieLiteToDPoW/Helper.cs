using System;

namespace XbimExchanger.COBieLiteToDPoW
{
    static class Helper
    {
        public static TOut TryConvertEnum<TIn, TOut>(TIn input, TOut defaultValue) where TIn : struct where TOut: struct
        {
            TOut output;
            if (Enum.TryParse<TOut>(input.ToString(), out output))
                return output;
            return defaultValue;
        }
    }
}
