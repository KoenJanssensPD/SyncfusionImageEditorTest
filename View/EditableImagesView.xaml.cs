using Ookii.Dialogs.Wpf;
using Syncfusion.UI.Xaml.ImageEditor;
using Syncfusion.UI.Xaml.ImageEditor.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFAppTestImageEditor.ViewModel;

namespace WPFAppTestImageEditor.View
{
    /// <summary>
    /// Interaction logic for EditableImagesView.xaml
    /// </summary>
    public partial class EditableImagesView : UserControl
    {

        private readonly EditableImageViewModel viewModel;

        public EditableImagesView()
        {
            this.viewModel = new EditableImageViewModel();
            InitializeComponent();
            this.DataContext = viewModel;
            EventManager.RegisterClassHandler(typeof(EditableImagesView), Keyboard.KeyUpEvent, new KeyEventHandler(WindowKeyUpHandler));

            viewModel.LoadImages(@"Assets\");
        }

        void WindowKeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {

            }
            if (e.Key == Key.P)
            {
                PenSettings penSettings = new PenSettings()
                {
                    StrokeWidth = 20,
                    Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 255)),
                    Fill = new SolidColorBrush(Color.FromRgb(0, 0, 255))
                };
                ImageEditor.AddShape(ShapeType.Arrow, penSettings);
            }
            if (e.Key == Key.S)
            {
                string newfilename = System.IO.Path.GetDirectoryName(viewModel.SelectedImage.OriginalFileLocation) + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileNameWithoutExtension(viewModel.SelectedImage.OriginalFileLocation) + "_edited" + System.IO.Path.GetExtension(viewModel.SelectedImage.OriginalFileLocation);

                if (!File.Exists(newfilename) || MessageBox.Show("is already edited! Overwrite?", "overschrijven?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ImageEditor.Save(filePath: newfilename);
                }
            }
        }


        public EditableImageViewModel ViewModel => viewModel;

        private void cmdChooseFolder_Click(object sender, RoutedEventArgs e)
        {

            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true)
            {
                viewModel.LoadImages(dialog.SelectedPath);
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ImageEditor.ImageSource = viewModel.ImageForEditor;
        }

        private void ImageEditor_ImageSaved(object sender, Syncfusion.UI.Xaml.ImageEditor.ImageSavedEventArgs e)
        {
            viewModel.SelectedImage.EditedFileLocation = e.Location;
        }
    }
}
