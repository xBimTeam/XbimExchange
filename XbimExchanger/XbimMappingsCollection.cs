using System;
using System.Collections.Concurrent;

namespace XbimExchanger
{
    public class XbimMappingsCollection<TRepository>
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, IXbimMappings<TRepository>>> _mappings = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, IXbimMappings<TRepository>>>();

       
        public XbimMappingsCollection(TRepository repository)
        {
            Repository = repository;
        }

        public TRepository Repository { get; set; }

        public TMapping GetOrCreateMappings<TMapping>() where TMapping : IXbimMappings<TRepository>, new()
        {
            var mappings = new TMapping {MappingsCollection = this};
            ConcurrentDictionary<Type, IXbimMappings<TRepository>> toMappings;
            if (_mappings.TryGetValue(mappings.MapFromType, out toMappings))
            {
                IXbimMappings<TRepository> imappings;
                if (toMappings.TryGetValue(mappings.MapToType, out imappings))
                    return (TMapping)imappings;
                toMappings.TryAdd(mappings.MapToType, mappings);
                return mappings;
            }
            toMappings = new ConcurrentDictionary<Type, IXbimMappings<TRepository>>();
            toMappings.TryAdd(mappings.MapToType, mappings);
            _mappings.TryAdd(mappings.MapFromType, toMappings);
            return mappings;
        }
    }
}
