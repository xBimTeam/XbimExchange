using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace XbimExchanger
{
    public interface IXbimMappings<TRepository>
    {

        Type MapFromType { get; }
        Type MapToType { get; }

        Type MapKeyType { get; }

        XbimMappingsCollection<TRepository> MappingsCollection { get; set; }
    }

    /// <summary>
    /// Abstract class for mapping between different object models and schemas
    /// </summary>
    /// <typeparam name="TFromKey">Type of the key in the From object to link mappings</typeparam>
    /// <typeparam name="TFromObject">Type of the object to map from</typeparam>
    /// <typeparam name="TToObject">Type of the object to map to</typeparam>
    /// <typeparam name="TRepository">The repository that holds new objects</typeparam>
    public abstract class XbimMappings<TRepository,TFromKey, TFromObject, TToObject> : IXbimMappings<TRepository> where TToObject : new()
    {


        
        protected ConcurrentDictionary<TFromKey, TToObject> Results = new ConcurrentDictionary<TFromKey, TToObject>();

        protected XbimMappings(XbimMappingsCollection<TRepository> mappingsCollection)
        {    
            MappingsCollection = mappingsCollection;     
        }

        protected XbimMappings()
        {
            
        }
        
        /// <summary>
        /// Returns the IDictionary of all objects that have been mapped in this mapping class
        /// </summary>
        public IDictionary<TFromKey, TToObject> Mappings
        {
            get { return Results; }
        }


        /// <summary>
        /// Creates an instance of toObject, override for special creation situations
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual TToObject CreateToObject(TFromKey key)
        {
            return new TToObject();
        }
        /// <summary>
        /// Gets the ToObject with the specified key
        /// </summary>
        /// <param name="key">The key to look the object up with</param>
        /// <param name="to">the object which is mapped to this key</param>
        /// <returns>false if no object has been added to this mapping</returns>
        public bool Get(TFromKey key, out TToObject to)
        {
            return Results.TryGetValue(key, out to);
        }

        /// <summary>
        /// Gets the object with the specified key or creates one if it does not exist 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TToObject GetOrCreate(TFromKey key)
        {
            return Results.GetOrAdd(key, CreateToObject(key));
        }

        /// <summary>
        /// Adds a mapping between the two object all mapped properties are mapped over by the Mapping function
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Returns the object which has been added to the mapping</returns>
        public TToObject AddMapping(TFromObject from, TToObject to)
        {
            BeforeMapping();
            TToObject res = Mapping(from, to); 
            AfterMapping();
            return res;
        }
        /// <summary>
        /// Called before any mapping operation is performed
        /// </summary>
        protected virtual void BeforeMapping() { }
        /// <summary>
        /// Overrident in the concrete class to perform the actual mapping
        /// </summary>
        /// <param name="from"></param>
        /// <returns>the mapped object</returns>
        protected abstract TToObject Mapping(TFromObject from, TToObject  to  );
        /// <summary>
        /// Called after any mapping has been performed
        /// </summary>
        protected virtual void AfterMapping() { }

        public Type MapFromType
        {
            get { return typeof (TFromObject); }
        }

        public Type MapToType
        {
            get { return typeof(TToObject); }
        }

        public Type MapKeyType
        {
            get { return typeof(TFromKey); }
        }

        public XbimMappingsCollection<TRepository> MappingsCollection { get; set; }
    }
}
