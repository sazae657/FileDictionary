using Com.Unkor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileDictionaryTest
{
    public class StringableString : IStringable {
        string str;
        public string Raw => str;

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

    public class Class1
    {
        [Fact]
        public void Unpo() {
            const int KOUNT = 10000;
            var baseDir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../work");
            var h = new FileDictionary<StringableString, StringableString>(Path.Combine(baseDir, "dic"), 2);
            h.Prepare();
            h.Clear();
            for (int i = 0; i < KOUNT; ++i) {
                h.Add(new StringableString($"K{i:000}"), new StringableString($"V{i:000}"));
            }
            Assert.Equal(KOUNT, h.Count);
            for (int i = 0; i < KOUNT; ++i) {
                var v = h[new StringableString($"K{i:000}")];
                Assert.Equal($"V{i:000}", v.Raw);

                Assert.Throws<KeyNotFoundException>(() => h[new StringableString($"X{i:000}")]);
            }

            for (int i = 0; i < KOUNT; ++i) {
                Assert.True(h.ContainsKey(new StringableString($"K{i:000}")));
                Assert.False(h.ContainsKey(new StringableString($"V{i:000}")));
            }

            for (int i = 0; i < KOUNT; ++i) {
                StringableString s;
                Assert.True(h.TryGetValue(new StringableString($"K{i:000}"), out s));
                Assert.False(h.TryGetValue(new StringableString($"うんぽ{i:000}"), out s));
            }

            int c = 0;
            foreach(var k in h.Keys) {
                var v = h[k];
                var ks = "V" + k.Raw.Substring(1);
                Assert.Equal(ks, v.Raw);
                c++;
            }
            Assert.Equal(KOUNT, c);

            c = 0;
            foreach(var k in h.Values) {
                c++;
            }
            Assert.Equal(KOUNT, c);

            c = 0;
            foreach (var k in h) {
                c++;
                var ks = k.Key.Raw.Substring(1);
                var vs = k.Key.Raw.Substring(1);
                Assert.Equal(ks, vs);
            }
            Assert.Equal(KOUNT, c);
        }
    }
}
