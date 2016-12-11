using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace mClient
{
    public static class Extensions
    {
        /// <summary>
        /// Get display name value for enum value
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        /// <summary>
        /// Adds an item to a list in dictionary if the list already exists, otherwise creates a new list and adds the item.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dict"></param>
        public static void AddOrUpdateDictionaryList<K, V>(this Dictionary<K, IList<V>> dict, K key, V value)
        {
            if (dict.ContainsKey(key))
                dict[key].Add(value);
            else
                dict.Add(key, new List<V>() { value });
        }
    }
}
