using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TEPLQMS.Common
{
    public class DatatableCall
    {
        public static T GetRequestValue<T>(string key, HttpRequestBase request)
        {
            var value = request.Form.GetValues(key)?.FirstOrDefault();
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static IEnumerable<T> ApplyFilters<T>(IEnumerable<T> data, Dictionary<string, string> filters)
        {
            foreach (var filter in filters)
            {
                var filterValue = filter.Value;
                if (!string.IsNullOrEmpty(filterValue) && filterValue != "All")
                {
                    data = data.Where(d => GetPropertyValue(d, filter.Key)?.ToString() == filterValue);
                }
            }
            return data;
        }
        public static IEnumerable<T> ApplySorting<T>(IEnumerable<T> data, string sortColumn, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortDirection))
            {
                return data;
            }

            var property = typeof(T).GetProperty(sortColumn);
            if (property == null) return data;

            if (sortDirection == "asc")
            {
                return data.OrderBy(d => property.GetValue(d, null));
            }
            else
            {
                return data.OrderByDescending(d => property.GetValue(d, null));
            }
        }
        public static IEnumerable<T> ApplyPaging<T>(IEnumerable<T> data, int skip, int take)
        {
            //return data.Skip(skip).Take(take);
            return data.Take(take);
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null) return null;

            // Handle nested properties, if any
            foreach (string part in propertyName.Split('.'))
            {
                if (obj == null) return null;

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (info == null) return null;

                obj = info.GetValue(obj, null);
            }
            return obj;
        }
    }
}