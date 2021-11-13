using System;
using System.Linq;

namespace Common.Extentions
{
    public static class GenenricExtentions
    {
        public static string GetGenericTypeName(this Type type)
        {
            if (type == null) return string.Empty;

            if (!type.IsGenericType) return type.Name;

            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());

            return $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }

        public static string GetGenericTypeName(this object @object)
        {
            return @object?.GetType().GetGenericTypeName();
        }
    }
}