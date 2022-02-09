using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WPFAppTestImageEditor.Model;

namespace WPFAppTestImageEditor.ViewModel
{
    public class EditableImageViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<EditableImage> images;
        private EditableImage _selectedImage;

        private BitmapImage _imageForEditor;

        public BitmapImage ImageForEditor
        {
            get { return SelectedImage != null ? SetImageForEditor() : null; }
            set
            {
                _imageForEditor = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage SetImageForEditor()
        {

            using Stream stream = new FileStream(SelectedImage.OriginalFileLocation, FileMode.Open);
            _imageForEditor = new BitmapImage();
            _imageForEditor.BeginInit();
            _imageForEditor.CacheOption = BitmapCacheOption.OnLoad;
            _imageForEditor.StreamSource = stream;
            _imageForEditor.EndInit();
            _imageForEditor.Freeze();
            return _imageForEditor;
        }

        public EditableImage SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }
        }


        public EditableImageViewModel()
        {
            images = new ObservableCollection<EditableImage>();
            _imageForEditor = new BitmapImage();
        }

        public ObservableCollection<EditableImage> Images
        {
            get => images; set
            {
                images = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        internal void LoadImages(string selectedPath)
        {
            Images.Clear();

            var files = Directory.EnumerateFiles(selectedPath).Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("jpeg"));
            foreach (var file in files.Where(f => !f.Contains("_edited")))
            {
                Images.Add(new EditableImage() { OriginalFileLocation = file });
            }
            foreach (var file in files.Where(f => f.Contains("_edited")))
            {
                var filename = Path.GetFileNameWithoutExtension(file).Replace("_edited", "");

                var originalimage = Images.Where(i => Path.GetFileNameWithoutExtension(i.OriginalFileLocation).Contains(filename)).FirstOrDefault();
                originalimage.EditedFileLocation = file;
            }

        }
    }
}
