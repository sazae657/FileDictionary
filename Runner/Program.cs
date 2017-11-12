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
            return str.Equals(other.str);
        }
        public override int GetHashCode() {
            return str.GetHashCode();
        }
    }
    class Program {
        const int KOUNT = 1000000;

        static void InsertTest(IDictionary<StringableString, StringableString> dic) {
            var sb = new StringBuilder();
            for (int j = 0; j < 98; j++) {
                sb.Append($"ん");
            }
            var bk = sb.ToString();
            for (int i = 0; i < KOUNT; ++i) {
                dic.Add(new StringableString($"K{i}{bk}"), new StringableString($"V{i}{bk}"));
            }
        }

        static void FindTest(IDictionary<StringableString, StringableString> dic) {
            var sb = new StringBuilder();
            for (int j = 0; j < 98; j++) {
                sb.Append($"ん");
            }
            var bk = sb.ToString();
            for (int i = 0; i < KOUNT; ++i) {
                var k = dic[(new StringableString($"K{i}{bk}"))];
            }
        }

        static void RemoveTest(IDictionary<StringableString, StringableString> dic) {
            var sb = new StringBuilder();
            for (int j = 0; j < 98; j++) {
                sb.Append($"ん");
            }
            var bk = sb.ToString();
            for (int i = 0; i < KOUNT; ++i) {
                dic.Remove(new StringableString($"K{i}{bk}"));
            }
        }

        static void IterationTest(IDictionary<StringableString, StringableString> dic) {
            foreach (var k in dic.Keys) {
                if (k != null) continue;
            }
        }

        static void IterationTest2(IDictionary<StringableString, StringableString> dic) {
            foreach (var k in dic.Values) {
                if (k != null) continue;
            }
        }

        static void IterationTest3(IDictionary<StringableString, StringableString> dic) {
            foreach (var k in dic) {
                if (k.Value != null) continue;
            }
        }

        static void RunStdndard() {
            GC.Collect();
            long baseSize, curSize;
            var sw = new System.Diagnostics.Stopwatch();

            var h = new Dictionary<StringableString, StringableString>();

            baseSize = Environment.WorkingSet;

            sw.Reset();
            sw.Start();
            InsertTest(h);
            sw.Stop();
            curSize = Environment.WorkingSet;
            Console.WriteLine($"Dictionary: Add {sw.ElapsedMilliseconds} msec");
            Console.WriteLine($"Dictionary: heap {curSize - baseSize} byte ({(((double)curSize - (double)baseSize) / 1024 / 1024):0.0}) MB");

            sw.Reset();
            sw.Start();
            FindTest(h);
            sw.Stop();
            Console.WriteLine($"Dictionary: Find {sw.ElapsedMilliseconds} msec");

            sw.Reset();
            sw.Start();
            IterationTest(h);
            sw.Stop();
            Console.WriteLine($"Dictionary: Iteration {sw.ElapsedMilliseconds} msec");

            sw.Reset();
            sw.Start();
            IterationTest2(h);
            sw.Stop();
            Console.WriteLine($"Dictionary: Iteration2 {sw.ElapsedMilliseconds} msec");

            sw.Reset();
            sw.Start();
            IterationTest3(h);
            sw.Stop();
            Console.WriteLine($"Dictionary: Iteration3 {sw.ElapsedMilliseconds} msec");

            sw.Reset();
            sw.Start();
            RemoveTest(h);
            sw.Stop();
            Console.WriteLine($"Dictionary: Remove {sw.ElapsedMilliseconds} msec");
        }

        static void RunFile() {
            GC.Collect();
            long baseSize, curSize;
            var sw = new System.Diagnostics.Stopwatch();

            var h = new FileDictionary<StringableString, StringableString>(@"C:\TMP\hash", 2);
            h.Prepare();

            baseSize = Environment.WorkingSet;

            sw.Start();
            InsertTest(h);
            sw.Stop();
            curSize = Environment.WorkingSet;
            Console.WriteLine($"FileDictionary: Add {sw.ElapsedMilliseconds} msec");
            Console.WriteLine($"FileDictionary: heap {curSize - baseSize} byte ({(((double)curSize - (double)baseSize) / 1024 / 1024):0.0}) MB");


            sw.Reset();
            sw.Start();
            FindTest(h);
            sw.Stop();
            Console.WriteLine($"Dictionary: Find {sw.ElapsedMilliseconds} msec");

            sw.Reset();
            sw.Start();
            IterationTest(h);
            sw.Stop();
            Console.WriteLine($"Dictionary: Iteration {sw.ElapsedMilliseconds} msec");

            sw.Reset();
            sw.Start();
            IterationTest2(h);
            sw.Stop();
            Console.WriteLine($"Dictionary: Iteration2 {sw.ElapsedMilliseconds} msec");

            sw.Reset();
            sw.Start();
            IterationTest3(h);
            sw.Stop();
            Console.WriteLine($"Dictionary: Iteration3 {sw.ElapsedMilliseconds} msec");

            sw.Reset();
            sw.Start();
            RemoveTest(h);
            sw.Stop();
            Console.WriteLine($"Dictionary: Remove {sw.ElapsedMilliseconds} msec");
        }


        static void Main(string[] args) {
            RunStdndard();
            //RunFile();
        }
    }
}
