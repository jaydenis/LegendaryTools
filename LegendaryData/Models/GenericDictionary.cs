using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryData.Models
{
    public class GenericDictionary
    {
        public Dictionary<string, object> _dict = new Dictionary<string, object>();

        public void Add<T>(string key, T value) where T : class
        {
            _dict.Add(key, value);
        }

        public T GetValue<T>(string key) where T : class
        {
            return _dict[key] as T;
        }
    }
    public class GlobalStore
    {
        private readonly IDictionary<Type, IEnumerable> _globalStore;

        public GlobalStore()
        {
            _globalStore = new ConcurrentDictionary<Type, IEnumerable>();
        }

        public IEnumerable<T> GetFromCache<T>()
            where T : class
        {
            return (IEnumerable<T>)_globalStore[typeof(T)];
        }

        public void SetCache<T>(IEnumerable<T> cache)
            where T : class
        {
            _globalStore[typeof(T)] = cache;
        }
    }
}
