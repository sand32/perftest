using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace aspnetcore
{
    public static class ListOperations
    {
        public static List<T> Paginate<T>(this List<T> list, Filter filter)
        {
            if(filter == null) return list;
            if(filter.Size == 0) filter.Size = 30;
            return list.Skip(filter.Offset).Take(filter.Size).ToList();
        }

        public static List<T> Sort<T>(this List<T> list, Filter filter)
        {
            if(filter == null || filter.Sort == null) return list;
            // Short-circuit
            if(string.IsNullOrWhiteSpace(filter?.Sort)) return list;

            // Prepare sort list
            string[] sortTerms = filter.Sort.Split(",", StringSplitOptions.RemoveEmptyEntries);
            PropertyInfo[] properties = typeof(T).GetProperties();

            // If these sort fields aren't all in the properties list, abort.
            if(!sortTerms.Select(f => f.Replace("-", "").ToLower()).All(f => properties.Any(p => p.Name.ToLower() == f)))
            {
                throw new Exception("Unable to sort list, invalid field name encountered.");
            }

            // Sort by first criteria
            IOrderedEnumerable<T> sortedList;
            if(sortTerms[0][0] == '-')
            {
                PropertyInfo info = properties.Single(x => x.Name.Equals(sortTerms[0].Substring(1), StringComparison.CurrentCultureIgnoreCase));
                sortedList = list.OrderByDescending(f => info.GetValue(f));
            }
            else
            {
                PropertyInfo info = properties.Single(x => x.Name.Equals(sortTerms[0], StringComparison.CurrentCultureIgnoreCase));
                sortedList = list.OrderBy(f => info.GetValue(f));
            }

            // Sort by remaining criteria
            for(int i = 1; i < sortTerms.Length; ++i)
            {
                if(sortTerms[i][0] == '-')
                {
                    PropertyInfo info = properties.Single(x => x.Name.Equals(sortTerms[i].Substring(1), StringComparison.CurrentCultureIgnoreCase));
                    sortedList = sortedList.ThenByDescending(f => info.GetValue(f));
                }
                else
                {
                    PropertyInfo info = properties.Single(x => x.Name.Equals(sortTerms[i], StringComparison.CurrentCultureIgnoreCase));
                    sortedList = sortedList.ThenBy(f => info.GetValue(f));
                }
            }
            return sortedList.ToList();
        }
        
        public static List<T> Search<T>(this List<T> list, Dictionary<string, string> search)
        {
            if(search == null) return list;
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach(var property in properties.Where(p => search.ContainsKey(p.Name)))
            {
                var searchValues = search[property.Name].Split(',');
                if(property.PropertyType == typeof(string))
                {
                    list = list.Where(m => searchValues.Any(value => property.GetValue(m).ToString().StartsWith(value))).ToList();
                }
                else if(property.PropertyType.IsValueType)
                {
                    list = list.Where(m => searchValues.Any(value => property.GetValue(m).ToString() == value)).ToList();
                }
            }
            return list;
        }

        public static List<object> SelectFields<T>(this List<T> list, Filter filter)
        {
            PropertyInfo[] selectedProperties = typeof(T).GetProperties();
            string[] selection;
            if(filter == null || filter.Fields == null)
            {
                selection = selectedProperties.Select(x => x.Name).ToArray();
            }
            else
            {
                selection = filter.Fields.Split(",", StringSplitOptions.RemoveEmptyEntries);
            }

            selectedProperties = selectedProperties.Where(x => selection.Any(y => y.Equals(x.Name, StringComparison.CurrentCultureIgnoreCase))).ToArray();
            var result = new List<object>();
            foreach(T element in list)
            {
                var expando = new ExpandoObject();
                var expandoDict = expando as IDictionary<string, object>;
                foreach(PropertyInfo property in selectedProperties)
                {
                    var propertyName = char.ToLowerInvariant(property.Name[0]) + property.Name.Substring(1);
                    expandoDict.Add(propertyName, property.GetValue(element));
                }
                result.Add(expando);
            }
            return result;
        }

        public static List<object> IdToUri(this List<object> list, string prefix)
        {
            foreach(var item in list)
            {
                var itemAsDictionary = item as IDictionary<string, object>;
                itemAsDictionary.Add("_href", $"{prefix}{itemAsDictionary["id"]}");
                itemAsDictionary.Remove("id");
            }
            return list;
        }
    }
}
