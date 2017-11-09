using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Unkor {
    public class DirectoryEnumerator<T> : IEnumerator<T>
        where T : IStringable, new()
    {

        T current;
        public T Current => current;

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

        ReadValueDelegaty<T> ReadValue;

        int index;
        int fileIndex;

        string root;
        int depth;
        public DirectoryEnumerator(string root, int depth, ReadValueDelegaty<T> readValue) {
            this.root = root;
            this.depth = depth;
            dirMap = new List<string>();
            files = new List<string>();
            ReadValue = readValue;
            index = 0;
            fileIndex = 0;

            GenerateDirMap(root, 0);
        }

        public DirectoryEnumerator<T> Clone() {
            return (new DirectoryEnumerator<T>(this.root, this.depth, ReadValue));
        }

        bool SpoolNext() {
            while(files.Count ==0) {
                if (index >= dirMap.Count) {
                    return false;
                }
                files.AddRange(Directory.GetFiles(dirMap[index]));
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
            current = ReadValue(files[fileIndex]);
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
