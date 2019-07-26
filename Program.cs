using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YuGiDough {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string tempPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            Card.basePath = System.IO.Path.GetDirectoryName(tempPath).Substring(6).Replace("/","\\\\");

            Application.Run(new Main_Menu());
        }
    }
    public static class StringExtensions {
        public static bool Contains(this string source, string toCheck, StringComparison comp) {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}
