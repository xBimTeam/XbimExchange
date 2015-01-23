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
    /// <typeparam name="TSourceKey">Type of the key in the From object to link mappings</typeparam>
    /// <typeparam name="TSourceObject">Type of the object to map from</typeparam>
    /// <typeparam name="TTargetObject">Type of the object to map to</typeparam>
    /// <typeparam name="TRepository">The repository that holds new objects</typeparam>
    public abstract class XbimMappings<TRepository,TSourceKey, TSourceObject, TTargetObject> : IXbimMappings<TRepository> where TTargetObject : new()
    {


        
        protected ConcurrentDictionary<TSourceKey, TTargetObject> Results = new ConcurrentDictionary<TSourceKey, TTargetObject>();

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
        public IDictionary<TSourceKey, TTargetObject> Mappings
        {
            get { return Results; }
        }

       
        /// <summary>
        /// Creates an instance of toObject, override for special creation situations
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TTargetObject CreateTargetObject()
        {
            return new TTargetObject();
        }
        /// <summary>
        /// Gets the ToObject with the specified key
        /// </summary>
        /// <param name="key">The key to look the object up with</param>
        /// <param name="to">the object which is mapped to this key</param>
        /// <returns>false if no object has been added to this mapping</returns>
        public bool GetTargetObject(TSourceKey key, out TTargetObject to)
        {
            return Results.TryGetValue(key, out to);
        }

        /// <summary>
        /// Gets the object with the specified key or creates one if it does not exist 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TTargetObject GetOrCreateTargetObject(TSourceKey key)
        {
            return Results.GetOrAdd(key, CreateTargetObject());
        }

       
        /// <summary>
        /// Adds a mapping between the two object all mapped properties are mapped over by the Mapping function
        /// </summary>
        /// <param name="source">The object to map data from</param>
        /// <param name="target">The object to map data to</param>
        /// <returns>Returns the object which has been added to the mapping</returns>
        public TTargetObject AddMapping(TSourceObject source, TTargetObject target)
        {
           // BeforeMapping();
            TTargetObject res = Mapping(source, target); 
           // AfterMapping();
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
        protected abstract TTargetObject Mapping(TSourceObject from, TTargetObject  to  );
        /// <summary>
        /// Called after any mapping has been performed
        /// </summary>
        protected virtual void AfterMapping() { }

        public Type MapFromType
        {
            get { return typeof (TSourceObject); }
        }

        public Type MapToType
        {
            get { return typeof(TTargetObject); }
        }

        public Type MapKeyType
        {
            get { return typeof(TSourceKey); }
        }

        public XbimMappingsCollection<TRepository> MappingsCollection { get; set; }
    }
}
