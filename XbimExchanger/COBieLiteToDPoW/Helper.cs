using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
