using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using MonoTouch.Foundation;
//using MonoTouch.UIKit;

namespace PingPongApp2.iOS {
    class FileConnection {
        public string GetFilePath() {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filename = System.IO.Path.Combine(path, "myfile.txt");
            return filename;
        }
    }
}