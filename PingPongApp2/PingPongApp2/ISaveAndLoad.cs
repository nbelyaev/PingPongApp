using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPongApp2 {
    public interface ISaveAndLoad {
        void SaveText(string filename, string text);
        string LoadText(string filename);
    }
}
