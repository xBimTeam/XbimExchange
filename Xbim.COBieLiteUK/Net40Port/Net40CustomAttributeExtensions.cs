using System;
using System.Collections.Generic;
using System.Reflection;
using msSys = System;

namespace Xbim.CobieLiteUk.Net40PortHelpers
{
     
    public static class CustomAttributeExtensions
    {
        public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo element, bool var = true) where T : msSys.Attribute
        {
            return (IEnumerable<T>)element.GetCustomAttributes(typeof(T), var);
        }

        public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element, Type attributeType, bool var)
        {

            foreach (var att in element.GetCustomAttributes(var))
            {
                if (att.GetType() == attributeType)
                    yield return (Attribute)att;
            }    
        }

        public static void SetValue(this PropertyInfo pi, object obj, object value)
        {
            pi.SetValue(obj, value, null);
        }

        public static object GetValue(this PropertyInfo pi, object obj)
        {
            return pi.GetValue(obj, null);
        }

    }
}

