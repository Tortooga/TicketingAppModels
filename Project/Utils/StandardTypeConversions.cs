using System.Runtime.CompilerServices;

namespace Utilities
{
    class TypeConversion
    {
        public static Object? ConvertTo(string val, string type)
        {
            switch (type)
            {
                case "String":
                    return val;
                case "Int32":
                    if (!int.TryParse(val, out int intVal))
                    {
                        return null;
                    }
                    return intVal;
                case "Boolean":
                    if (!Boolean.TryParse(val, out bool boolVal))
                    {
                        return null;
                    }
                    return boolVal;
                case "Single":
                    if (!Single.TryParse(val, out Single singleVal))
                    {
                        return null;
                    }
                    return singleVal;
                default:
                    return null;
            }
        }
    }
}