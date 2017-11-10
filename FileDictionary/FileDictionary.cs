using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Unkor {

    public class FileDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDisposable
                where TKey :IStringable, new()
                where TValue : IStringable,new()
        {

        SHA256 sha512;
        int depth;
        public int Depth => depth;

        public string RootDirectory { get; private set; }

        string KeyRoot;
        int count;

        public void Prepare() {
            if (! Directory.Exists(RootDirectory)) {
                throw new DirectoryNotFoundException(
                    $"RootDirectory<{RootDirectory}>が無いから無理");
            }
            KeyRoot = Path.Combine(RootDirectory, "Values");
            PrepareSubDirectory(KeyRoot);
        }

        void PrepareSubDirectory(string dir) {
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            PrepareHashDirectory(dir, 0);
        }

        void PrepareHashDirectory(string dir, int d) {
            if (d >= depth) {
                return;
            }
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            for (int i = 0; i < 16; ++i) {
                var xd = Path.Combine(dir, i.ToString("X2"));
                if (!Directory.Exists(xd)) {
                    Directory.CreateDirectory(xd);
                }
                PrepareHashDirectory(xd, d + 1);
            }
        }

        void SwipeAll() {
            Swipe(KeyRoot);
        }

        void Swipe(string dir) {
            foreach(var path in Directory.GetFiles(dir)) {
                File.Delete(path);
            }
            foreach(var path in Directory.GetDirectories(dir)) {
                Swipe(path);
            }
        }

        string ByteArrayToString(byte[] ba) {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        string GenerateBaseName(byte[] hash) {
            var dir = KeyRoot;
            for (int i = 0; i < depth; ++i) {
                dir = Path.Combine(dir, (hash[i] >> 4).ToString("X2"));
            }
            dir = Path.Combine(dir, ByteArrayToString(hash));
            return dir;
        }

        public bool ContainsPair(TKey key, TValue value) {
            var hkey = Encoding.Default.GetBytes(key.ToString());
            var hval = Encoding.Default.GetBytes(value.ToString());
            var hk = sha512.ComputeHash(hkey);

            var b = GenerateBaseName(hk);
            string dk = b + ".key";
            string dv = b + ".value";

            return (File.Exists(dk) && File.Exists(dv));
        }

        public bool ContainsKeyNational(TKey key) {
            var hkey = Encoding.Default.GetBytes(key.ToString());
            var hk = sha512.ComputeHash(hkey);

            var dk = GenerateBaseName(hk) + ".key";
            return (File.Exists(dk));
        }

        public void WriteKeyPair(TKey key, TValue value) {
            var hkey = Encoding.Default.GetBytes(key.ToString());
            var hval = Encoding.Default.GetBytes(value.ToString());
            var hk = sha512.ComputeHash(hkey);
            //var hv = sha512.ComputeHash(hval);
            var b = GenerateBaseName(hk);
            string dk = b + ".key";
            string dv = b + ".value";
            if (!File.Exists(dk)) {
                count++;
            }

            using (var fs = new System.IO.FileStream(dk, System.IO.FileMode.Create, System.IO.FileAccess.Write)) {
                fs.Write(hkey, 0, hkey.Length);
            }
            using (var fs = new System.IO.FileStream(dv, System.IO.FileMode.Create, System.IO.FileAccess.Write)) {
                fs.Write(hval, 0, hval.Length);
            }
        }

        public TValue ReadValue(TKey key) {
            var hkey = Encoding.Default.GetBytes(key.ToString());
            var hk = sha512.ComputeHash(hkey);
            //var hv = sha512.ComputeHash(hval);
            var b = GenerateBaseName(hk);
            string dk = b + ".key";
            string dv = b + ".value";

            if (! File.Exists(dk)) {
                throw new KeyNotFoundException();
            }

            string val = null; 
            using (var fs = new System.IO.FileStream(dv, System.IO.FileMode.Open)) {
                var v = new byte[fs.Length];
                fs.Read(v, 0, v.Length);
                val = Encoding.Default.GetString(v);
            }
            return (TValue)(new TValue()).FromString(val) ;
        }

        
        public KeyValuePair<TKey, TValue> ReadKeyValueFromFile(string baseName) {
            var key = Path.Combine(KeyRoot, baseName + ".key");
            var  value = Path.Combine(KeyRoot, baseName + ".value");
            if (!File.Exists(key)) {
                throw new FileNotFoundException($"Key無い<{key}>");
            }
            if (!File.Exists(value)) {
                throw new FileNotFoundException($"Value無い<{value}>");
            }

            string xkey = null;
            using (var fs = new System.IO.FileStream(key, System.IO.FileMode.Open)) {
                var v = new byte[fs.Length];
                fs.Read(v, 0, v.Length);
                xkey = Encoding.Default.GetString(v);
            }

            string xvalue = null;
            using (var fs = new System.IO.FileStream(value, System.IO.FileMode.Open)) {
                var v = new byte[fs.Length];
                fs.Read(v, 0, v.Length);
                xvalue = Encoding.Default.GetString(v);
            }
            return 
                (new KeyValuePair<TKey, TValue>((TKey)(new TKey()).FromString(xkey), (TValue)(new TValue()).FromString(xvalue)));
        }


        public T ReadValueFile<T>(string path) where T: IStringable, new() {
            if (!File.Exists(path)) {
                throw new FileNotFoundException($"無い<{path}>");
            }
            string val = null;
            using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Open)) {
                var v = new byte[fs.Length];
                fs.Read(v, 0, v.Length);
                val = Encoding.Default.GetString(v);
            }
            return (T)(new T()).FromString(val);
        }


        bool RemoveKey(TKey key) {
            var hkey = Encoding.Default.GetBytes(key.ToString());
            var hk = sha512.ComputeHash(hkey);

            var b = GenerateBaseName(hk);
            string dk = b + ".key";
            string dv = b + ".value";

            if (!File.Exists(dk)) {
                return false;
            }
            File.Delete(dk);
            File.Delete(dv);
            count--;
            return true;
        }



        public FileDictionary(string root, int depth) {
            this.depth = depth;
            RootDirectory = root;
            count = 0;
            sha512 = new SHA256CryptoServiceProvider();
        }

        public TValue this[TKey key] {
            get => ReadValue(key);
            set => WriteKeyPair(key, value);
        }

        public ICollection<TKey> Keys =>
            new DirectoryCollection<TKey>(KeyRoot, depth, (s) => ReadValueFile<TKey>(s));

        public ICollection<TValue> Values =>
            new DirectoryCollection<TValue>(KeyRoot, depth, (s)=> ReadValueFile<TValue>(s));

        public int Count => count;

        public bool IsReadOnly => false;


        public void Add(TKey key, TValue value) =>
            WriteKeyPair(key, value);
        

        public void Add(KeyValuePair<TKey, TValue> item) =>
            WriteKeyPair(item.Key, item.Value);
        

        public void Clear() => SwipeAll();
        

        public bool Contains(KeyValuePair<TKey, TValue> item) => ContainsPair(item.Key, item.Value);

        public bool ContainsKey(TKey key) => ContainsKeyNational(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
            new KeyValueEnumerator<TKey, TValue>(KeyRoot, depth, (k)=> ReadKeyValueFromFile(k));

        public bool Remove(TKey key) => RemoveKey(key);


        public bool Remove(KeyValuePair<TKey, TValue> item)  => RemoveKey(item.Key);

        public bool TryGetValue(TKey key, out TValue value) {
            value = default(TValue);
            if (ContainsKey(key)) {
                value = ReadValue(key);
                return true;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
        #endregion
    }
}
