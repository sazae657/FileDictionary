using Com.Unkor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner {
    public class StringableString : IStringable {
        string str;
        public StringableString() {
            str = "";
        }

        public StringableString(string str) {
            this.str = str;
        }

        public override string ToString() {
            return str;
        }

        public object FromString(string str) {
            this.str = str;
            return this;
        }
    }
    class Program {


        static void Main(string[] args) {
    
            var h = new FileDictionary<StringableString, StringableString>(@"C:\TMP\hash", 2);
            h.Prepare();
            h.Clear();
            for (int i = 0; i < 100; ++i) {
                h.Add(new StringableString($"K{i:000}うんぽ"), new StringableString($"V{i:000}うんぽっぽ"));
            }
            int c = 0;
            foreach (var k in h.Keys) {
                Console.WriteLine(k);
                c++;
            }
            Console.WriteLine("Keys={c}");

            foreach (var k in h.Values) {
                Console.WriteLine(k);
                c++;
            }
            Console.WriteLine("Values={c}");

            var keys = new StringableString[h.Count];
            h.Keys.CopyTo(keys, 0);

            Console.WriteLine(h.Count);
        }
    }
}
