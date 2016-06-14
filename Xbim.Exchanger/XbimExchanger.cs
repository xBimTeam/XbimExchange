using System;
using System.Collections.Concurrent;
using System.Globalization;
using Xbim.CobieLiteUk;

namespace XbimExchanger
{
    public abstract class XbimExchanger<TSourceRepository, TTargetRepository> 
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, IXbimMappings<TSourceRepository, TTargetRepository>>> _mappings = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, IXbimMappings<TSourceRepository, TTargetRepository>>>();

        /// <summary>
        /// This property can be used by Exchanger to set up a context for all mappings (like a specific stage of project for example).
        /// </summary>
        public object Context { get; protected set; }

        protected XbimExchanger(TSourceRepository source, TTargetRepository target)
        {
            _target = target;
            _source = source;
            ReportProgress = new ProgressReporter(); //no ReportProgressDelegate set so will not report
        }

        private readonly TSourceRepository _source;
        private readonly TTargetRepository _target;
        public TTargetRepository TargetRepository { get { return _target; } }
        public TSourceRepository SourceRepository { get { return _source; } }

        /// <summary>
        /// Object to use to report progress on Exchangers
        /// </summary>
        public ProgressReporter ReportProgress
        { get; set; }


        public TMapping GetOrCreateMappings<TMapping>() where TMapping : IXbimMappings<TSourceRepository, TTargetRepository>, new()
        {
            var mappings = new TMapping {Exchanger = this};
            ConcurrentDictionary<Type, IXbimMappings<TSourceRepository, TTargetRepository>> toMappings;
            if (_mappings.TryGetValue(mappings.MapFromType, out toMappings))
            {
                IXbimMappings<TSourceRepository, TTargetRepository> imappings;
                if (toMappings.TryGetValue(mappings.MapToType, out imappings))
                    return (TMapping)imappings;
                toMappings.TryAdd(mappings.MapToType, mappings);
                return mappings;
            }
            toMappings = new ConcurrentDictionary<Type, IXbimMappings<TSourceRepository, TTargetRepository>>();
            toMappings.TryAdd(mappings.MapToType, mappings);
            _mappings.TryAdd(mappings.MapFromType, toMappings);
            return mappings;
        }

        public abstract TTargetRepository Convert();

        private int _strId;
        public virtual string GetStringIdentifier() {
            return (_strId++).ToString(CultureInfo.InvariantCulture);
        }

        private int _intId;
        public virtual int GetIntIdentifier()
        {
            return _intId++;
        }
        public virtual Guid GetGuidIdentifier()
        {
            return Guid.NewGuid();
        }       
    }
}
