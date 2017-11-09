using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Unkor {
    public class DirectoryCollection<T> : ICollection<T>
        where T : IStringable, new()
    {
        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => true;

        DirectoryEnumerator<T> directory;

        public DirectoryCollection(string root, int depth, ReadValueDelegaty<T> readValue) {
            directory = new DirectoryEnumerator<T>(root, depth, readValue);
        }

        public void Add(T item) {
            throw new NotImplementedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(T item) {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex) {
            var dn = directory.Clone();
            int i = 0;
            for (i = 0; i < arrayIndex; ++i) dn.MoveNext();

            for (i = 0; i < array.Length; ++i) {
                if (!dn.MoveNext()) {
                    break;
                }
                array[i] = dn.Current;
            }
        }

        public bool Remove(T item) {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator() => directory;

        IEnumerator IEnumerable.GetEnumerator() => directory;

    }
}
