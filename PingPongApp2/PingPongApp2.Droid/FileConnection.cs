using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PingPongApp2.Droid {
    class FileConnection {
        public string GetFilePath() {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filename = System.IO.Path.Combine(path, "myfile.txt");
            return filename;
        }
        
    }
}