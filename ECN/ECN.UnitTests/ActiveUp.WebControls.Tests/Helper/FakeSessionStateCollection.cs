using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveUp.WebControls.Tests.Helper
{
    public class FakeSessionStateCollection : IDictionary<string, object>
    {
        private IList<string> _keys = new List<string>();
        private IDictionary<string, object> _items = new Dictionary<string, object>();

        public object this[int index]
        {
            get => this.GetItem(index);
            set => this.SetItem(index, value);
        }

        public object this[string key]
        {
            get => this.GetItem(key);
            set => this.SetItem(key, value);
        }

        public ICollection<string> Keys => _items.Keys;

        public ICollection<object> Values => _items.Values;

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(string key, object value)
        {
            this.SetItem(key, value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            this.SetItem(item);
        }

        public void Clear()
        {
            _keys.Clear();
            _items.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _items.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _items.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return this.RemoveItem(key);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return this.RemoveItem(item.Key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _items.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void RemoveAt(int index)
        {
            this.RemoveItem(index);
        }

        private object GetItem(int index)
        {
            object value = null;

            if (_keys.Count > index)
            {
                string key = _keys[index];
                value = _items[key];
            }

            return value;
        }

        private object GetItem(string key)
        {
            return _items.ContainsKey(key) ? _items[key] : null;
        }

        private void SetItem(KeyValuePair<string, object> item)
        {
            SetItem(item.Key, item.Value);
        }

        private void SetItem(string key, object value)
        {
            if (!_items.ContainsKey(key))
                _keys.Add(key);

            _items[key] = value;
        }

        private void SetItem(int index, object value)
        {
            if (_keys.Count > index)
            {
                string key = _keys[index];
                _items[key] = value;
            }
        }

        private bool RemoveItem(int index)
        {
            bool result = false;

            if (_keys.Count > index)
            {
                string key = _keys[index];
                _items.Remove(key);

                _keys.RemoveAt(index);

                result = true;
            }

            return result;
        }

        private bool RemoveItem(string key)
        {
            bool result = false;

            if (_items.ContainsKey(key))
            {
                int index = _keys.IndexOf(key);
                _keys.RemoveAt(index);

                _items.Remove(key);

                result = true;
            }

            return result;
        }
    }
}
