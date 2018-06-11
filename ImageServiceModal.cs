using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Drawing.Imaging;
using System.Configuration;

namespace ImageService
{
    public class ImageServiceModal : IImageServiceModal
    {
        private string outputFolder;            // The Output Folder
        private string thumbnailFolder;         // The Thumbnail Folder
        private int thumbnailSize;              // The Size Of The Thumbnail Size
        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");

        public ImageServiceModal() {
            this.outputFolder = ConfigurationManager.AppSettings["OutputDir"];
            this.thumbnailFolder = outputFolder + @"\thumbnail";
            this.thumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
            createBaseFolder();   
        }

        /**
         * @arg path - format date yyyy\mm.
         * creates a new folder in the output folder and thumbnail by date (format yyyy\mm). 
         **/
        public void createDirectory(string path) {
            Directory.CreateDirectory(outputFolder + @"\"+ path);
            Directory.CreateDirectory(thumbnailFolder + @"\" + path); 
        }

        /**
         * @arg pathToFile - full path to file
         * @arg dstpath - a full path to the destination.
         * move file from path to dst path (not copy!). 
         **/
        public void MoveFile(string pathToFile, string dstpath) {
            File.Move(pathToFile, dstpath);    
        }

       /**
        * creates the base folders - output folder (hidden folder) and thumbnail folder. 
        **/
        public void createBaseFolder() {
            if (!Directory.Exists(outputFolder))
            {
                DirectoryInfo di = Directory.CreateDirectory(outputFolder);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
            Directory.CreateDirectory(thumbnailFolder);
        } 

        /**
         * @arg path - full path to file.
         * retrieves the datetime WITHOUT loading the whole image
         **/
        public DateTime getDateTakenFromImage(string path) {
            try {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                }
            }
            // if the pic dosn't have the taken date, the function returns creation date. 
            catch (Exception e) { 
                DateTime creation = File.GetCreationTime(path);
                return creation;
            }
        }

        /**
         * @arg path - full path to file.
         * @arg result. (Success or failure)
         * given a full path, we get the taken date of the image from getDateTakenFromImage, 
         * create folders by the date, move the image to the folders
         * and than create thumbnail by function AddThumbNail .
         **/
        public string AddFile(string path, out bool result) {
            result = true;
            string dstpath = "";
            string message = "";
            string fileName = "";
            try {
                fileName = Path.GetFileName(path);
                DateTime time = getDateTakenFromImage(path);
                string timeName = time.Year.ToString() + @"\" + time.Month.ToString();
                dstpath = outputFolder + @"\" + timeName + @"\" + fileName;
                createDirectory(timeName);               
                //if there is another file with the same name, I add to the nameFile 1/2/3... 
                int counter = 0;
                while (File.Exists(dstpath)) {
                    counter++;
                    fileName = counter + fileName;
                    dstpath = outputFolder + @"\" + timeName + @"\" + fileName; ;
                }
                MoveFile(path, dstpath);
                AddThumbNail(dstpath, thumbnailFolder + @"\" + timeName + @"\" + fileName);
                message = "Image " + fileName + " move to: " + dstpath;
            } catch (Exception e) {
                result = false;
                message = e.Message;
            }
            return message;
        }

       /**
        * @arg path - full path to file.
        * @arg dstpath.
        * create thumbNail in 'thumbnailSize' size and save the thumbNail in dstpath.
        **/
        public void AddThumbNail(string path, string dstpath) {
             string fileName = Path.GetFileName(path);
            using (Image image = Image.FromFile(path))
            {
                Image thumbNail = image.GetThumbnailImage(this.thumbnailSize, this.thumbnailSize, null, new IntPtr());
                thumbNail.Save(dstpath);
            } 
         }
    }
}


