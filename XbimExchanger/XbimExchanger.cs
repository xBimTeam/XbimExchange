using System;
using System.Collections.Concurrent;

namespace XbimExchanger
{
    public abstract class XbimExchanger<TSourceRepository, TTargetRepository>
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, IXbimMappings<TSourceRepository, TTargetRepository>>> _mappings = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, IXbimMappings<TSourceRepository, TTargetRepository>>>();

       
        public XbimExchanger(TSourceRepository source, TTargetRepository target)
        {
            _target = target;
            _source = source;
        }

        private TSourceRepository _source;
        private TTargetRepository _target;
        public TTargetRepository TargetRepository { get { return _target; } }
        public TSourceRepository SourceRepository { get { return _source; } }

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

        private int _strId = 0;
        public virtual string GetStringIdentifier() {
            return (_strId++).ToString();
        }

        private int _intId = 0;
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
