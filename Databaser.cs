using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YuGiDough {
    public class Databaser {
        public static string imgURL = "https://static-3.studiobebop.net/ygo_data/card_images/";
        public static string ydataloc = "C:\\Users\\lucask\\ygodough\\ydata";
        public static void CardFixer() {
            DirectoryInfo DI = new DirectoryInfo("C:\\Users\\lucask\\ygodough\\Problem Cards");
            string cardname;
            StreamReader sr;
            foreach (var efile in DI.GetFiles()) {
                sr = efile.OpenText();
                sr.ReadLine();
                cardname = sr.ReadLine();
                cardname = cardname.Split('(')[0];
                cardname = cardname.Replace("\'", "%27");
                cardname = cardname.Replace(" ", "_");
                cardname = cardname.Replace('!', '_');
                cardname = cardname.Replace("?", "%3F");
                cardname = cardname.Replace('-', '_');
                cardname = cardname.Replace("&uacute;", "ú");

                cardname = cardname.TrimEnd('_');
                bool imagefound = DownloadRemoteImageFile(imgURL + cardname + ".jpg", ydataloc + "\\" + cardname + ".jpg");
                Console.WriteLine(cardname + ": " + imagefound);
            }
        }

        public static bool DownloadRemoteImageFile(string uri, string fileName) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response;
            try {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception) {
                return false;
            }

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase)) {

                // if the remote file was found, download it
                using (Stream inputStream = response.GetResponseStream())
                using (Stream outputStream = File.OpenWrite(fileName)) {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
                return true;
            }
            else
                return false;
        }
    }
}
