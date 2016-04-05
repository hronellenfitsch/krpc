using System;
using System.Reflection;
using System.Collections.Generic;

namespace KRPC.Utils
{
    static class Reflection
    {
        static IEnumerable<Type> AllTypes ()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                // Get all types that can be loaded from the assembly
                Type[] types;
                try {
                    types = assembly.GetTypes ();
                } catch (ReflectionTypeLoadException) {
                    // FIXME: should include loadable types from partially loadable assembly, but causes MonoDevelop crash:
                    // Assertion at class.c:5594, condition `!mono_loader_get_last_error ()
                    //types = e.Types.Where (x => x != null).ToArray ();
                    types = new Type[] { };
                }
                foreach (var type in types) {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Returns the type with the specified full name, from all assemblies, or null if no such type exists.
        /// </summary>
        public static Type GetType (string name)
        {
            name = name.Replace ('+', '.');
            foreach (var type in AllTypes()) {
                if (type.FullName.Replace ('+', '.') == name)
                    return type;
            }
            return null;
        }

        /// <summary>
        /// Returns all types with the specified attribute, from all assemblies.
        /// </summary>
        public static IEnumerable<Type> GetTypesWith<TAttribute> (bool inherit = false)
            where TAttribute : Attribute
        {
            foreach (var type in AllTypes()) {
                if (type.IsDefined (typeof(TAttribute), inherit))
                    yield return type;
            }
        }

        /// <summary>
        /// Returns all methods within a given type that have the specified attribute.
        /// </summary>
        public static IEnumerable<MethodInfo> GetMethodsWith<TAttribute> (Type cls, bool inherit = false)
            where TAttribute : Attribute
        {
            foreach (var method in cls.GetMethods ()) {
                if (method.IsDefined (typeof(TAttribute), inherit)) {
                    yield return method;
                }
            }
        }

        /// <summary>
        /// Returns all properties within a given type that have the specified attribute.
        /// </summary>
        public static IEnumerable<PropertyInfo> GetPropertiesWith<TAttribute> (Type cls, bool inherit = false)
            where TAttribute : Attribute
        {
            foreach (var property in cls.GetProperties ()) {
                if (property.IsDefined (typeof(TAttribute), inherit)) {
                    yield return property;
                }
            }
        }

        /// <summary>
        /// Return attribute of type T for the given member. Does not follow inheritance.
        /// Throws ArgumentException if there is no attribute, or more than one attribute.
        /// </summary>
        public static T GetAttribute<T> (MemberInfo member)
        {
            object[] attributes = member.GetCustomAttributes (typeof(T), false);
            if (attributes.Length != 1)
                throw new ArgumentException ();
            return (T)attributes [0];
        }

        /// <summary>
        /// Return true if member has the attribute of type T. Does not follow inheritance.
        /// </summary>
        public static bool HasAttribute<T> (MemberInfo member)
        {
            return member.GetCustomAttributes (typeof(T), false).Length == 1;
        }

        /// <summary>
        /// Extension method to check if a type is static.
        /// </summary>
        public static bool IsStatic (this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        /// <summary>
        /// Extension method to check if a property is static.
        /// </summary>
        public static bool IsStatic (this PropertyInfo property)
        {
            return (property.GetGetMethod () == null || property.GetGetMethod ().IsStatic) && (property.GetSetMethod () == null || property.GetSetMethod ().IsStatic);
        }

        /// <summary>
        /// Extension method to check if a property is public.
        /// </summary>
        public static bool IsPublic (this PropertyInfo property)
        {
            return (property.GetGetMethod () == null || property.GetGetMethod ().IsPublic) && (property.GetSetMethod () == null || property.GetSetMethod ().IsPublic);
        }

        /// <summary>
        /// Returns true if the given type is an instance of the given generic type.
        /// </summary>
        public static bool IsGenericType (Type type, Type genericType)
        {
            while (type != null) {
                if (type.IsGenericType && type.GetGenericTypeDefinition () == genericType)
                    return true;
                foreach (var intType in type.GetInterfaces())
                    if (IsGenericType (intType, genericType))
                        return true;
                type = type.BaseType;
            }
            return false;
        }
    }
}
