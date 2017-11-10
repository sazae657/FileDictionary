using Com.Unkor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner {
    public class StringableString : IStringable, IEquatable<StringableString> {
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

        public bool Equals(StringableString other) {
            return false;
        }
        public override int GetHashCode() {
            return str.GetHashCode();
        }
    }
    class Program {

        static void InsertTest(IDictionary<StringableString, StringableString> dic) {
            var sb = new StringBuilder();
            for (int j = 0; j < 98; j++) {
                sb.Append($"ん");
            }
            var bk = sb.ToString();
            for (int i = 0; i < 1000000; ++i) {
                dic.Add(new StringableString($"K{i}{bk}"), new StringableString($"V{i}{bk}"));
            }
        }

        static void RunStdndard() {
            GC.Collect();
            long baseSize, curSize;
            var sw = new System.Diagnostics.Stopwatch();

            var h2 = new Dictionary<StringableString, StringableString>();

            baseSize = Environment.WorkingSet;

            sw.Reset();
            sw.Start();
            InsertTest(h2);
            sw.Stop();
            curSize = Environment.WorkingSet;
            Console.WriteLine($"Dictionary: Add {sw.ElapsedMilliseconds} msec");
            Console.WriteLine($"Dictionary: heap {curSize - baseSize} byte ({(((double)curSize - (double)baseSize) / 1024 / 1024):0.0}) MB");
        }

        static void RunFile() {
            GC.Collect();
            long baseSize, curSize;
            var sw = new System.Diagnostics.Stopwatch();

            var h = new FileDictionary<StringableString, StringableString>(@"C:\TMP\hash", 3);
            h.Prepare();

            baseSize = Environment.WorkingSet;

            sw.Start();
            InsertTest(h);
            sw.Stop();
            curSize = Environment.WorkingSet;
            Console.WriteLine($"FileDictionary: Add {sw.ElapsedMilliseconds} msec");
            Console.WriteLine($"FileDictionary: heap {curSize - baseSize} byte ({(((double)curSize - (double)baseSize) / 1024 / 1024):0.0}) MB");

        }


        static void Main(string[] args) {
            RunStdndard();
            RunFile();
        }
    }
}
