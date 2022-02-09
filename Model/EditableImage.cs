using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WPFAppTestImageEditor.Model
{
    public class EditableImage : INotifyPropertyChanged
    {
        private string originalFileLocation;
        private string editedFileLocation;

        public string OriginalFileLocation
        {
            get => originalFileLocation; set
            {
                originalFileLocation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BitmapImageOriginalThumbnail));
            }
        }

        public string EditedFileLocation
        {
            get => editedFileLocation; set
            {
                editedFileLocation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BitmapImageEditedThumbnail));
            }
        }

        public BitmapImage BitmapImageOriginalThumbnail => GenerateBitmap(originalFileLocation, 100);
        public BitmapImage BitmapImageEditedThumbnail => !string.IsNullOrEmpty(editedFileLocation) ? GenerateBitmap(editedFileLocation, 100) : null;
        public BitmapImage BitmapImageOriginalFullSize => GenerateBitmap(originalFileLocation, 1000);
        public BitmapImage BitmapImageEditedFullSize => !string.IsNullOrEmpty(editedFileLocation) ? GenerateBitmap(editedFileLocation, 1000) : null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public static BitmapImage GenerateBitmap(string path, int DecodePixelWidth = 200)
        {
            BitmapImage bitmap = new();
            using Stream stream = new FileStream(path, FileMode.Open);
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.DecodePixelWidth = DecodePixelWidth;
            bitmap.EndInit();
            return bitmap;
        }

    }
}
