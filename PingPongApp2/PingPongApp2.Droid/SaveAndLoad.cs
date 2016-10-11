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
using Xamarin.Forms;
using PingPongApp2.Droid;
using System.IO;

[assembly: Dependency(typeof(SaveAndLoad))]
namespace PingPongApp2.Droid {
    public class SaveAndLoad : ISaveAndLoad {
        public void SaveText(string filename, string text) {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            System.IO.File.WriteAllText(filePath, text);
        }
        public string LoadText(string filename) {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);

            string output = "";

            if (File.Exists(filePath)) {
                output= File.ReadAllText(filePath);
            }

            return output;
        }
    }
}