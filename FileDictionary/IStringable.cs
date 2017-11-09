using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Unkor {
    public interface IStringable {
        string ToString(); 
        object FromString(string str);
    }

    public interface IStringable<T> {
        string ToString();
        T FromString(string str);
    }
}
