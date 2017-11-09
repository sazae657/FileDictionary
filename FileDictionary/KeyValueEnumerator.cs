using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Unkor {

    public delegate T ReadKeyValueDelegaty<T>(string key, string value);
    public delegate T ReadValueDelegaty<T>(string path);

    public class KeyValueEnumerator<K, V> : IEnumerator<KeyValuePair<K,V>>
        where K : IStringable, new()
        where V : IStringable, new() {

        KeyValuePair<K, V> current;
        public KeyValuePair<K, V> Current => current;

        object IEnumerator.Current => throw new NotImplementedException();
        List<string> dirMap;
        List<string> files;

        void GenerateDirMap(string dir, int d) {
            if (d >= depth) {
                dirMap.Add(dir);
                return;
            }
            for (int i = 0; i < 16; ++i) {
                var xd = Path.Combine(dir, i.ToString("X2"));
                GenerateDirMap(xd, d + 1);
            }
        }

        int index;
        int fileIndex;

        string keyRoot;
        string valueRoot;
        int depth;
        ReadKeyValueDelegaty<KeyValuePair<K, V>> ReadKey;
        //ReadValueDelegaty<V> ReadValue;

        public KeyValueEnumerator(
            string root, string vr, int depth, ReadKeyValueDelegaty<KeyValuePair<K, V>> readKey) {
            this.keyRoot = root;
            this.valueRoot = vr;
            this.depth = depth;
            dirMap = new List<string>();
            files = new List<string>();
            index = 0;
            fileIndex = 0;
            ReadKey = readKey;

            GenerateDirMap("", 0);
        }

        string AsKeyRoot(int index) => Path.Combine(keyRoot, dirMap[index]);

        bool SpoolNext() {
            while (files.Count == 0) {
                if (index >= dirMap.Count) {
                    return false;
                }
                files.AddRange(Directory.GetFiles(AsKeyRoot(index)));
                index++;
            }
            if (files.Count == 0) {
                return false;
            }
            return true;
        }

        public bool MoveNext() {
            if (index > dirMap.Count) {
                return false;
            }
            if (fileIndex >= files.Count || files.Count == 0) {
                fileIndex = 0;
                files.Clear();
                if (true != SpoolNext()) {
                    return false;
                }
            }

            var sp = files[fileIndex];
            sp = sp.Substring(keyRoot.Length+1);
            sp = Path.Combine(valueRoot, sp);

            current = ReadKey(files[fileIndex], sp);

            fileIndex++;
            return true;
        }

        public void Reset() {
            index = 0;
            fileIndex = 0;
            files.Clear();
        }

        public void Dispose() {
            dirMap.Clear();
            dirMap = null;

            files.Clear();
            files = null;
        }
    }
}
