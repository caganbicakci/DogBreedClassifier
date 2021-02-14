using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;
using Xam.Plugins.OnDeviceCustomVision;
using System.Linq;


namespace ImageClassifierApp
{
    class MainViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            TakePhotoCommand = new Command(async () => await TakePhoto());
            ClassifyCommand = new Command(async () => await Classify());
            PickPhotoCommand = new Command(async () => await PickPhoto());

        }

        private async Task TakePhoto()
        {
            Image = null;
            ImageTag = "";
            ImageProbability = "";

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));

            photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { PhotoSize = PhotoSize.Medium });
            Image = ImageSource.FromStream(() => photo.GetStream());

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));

        }

        private async Task PickPhoto()
        {
            Image = null;
            ImageTag = "";
            ImageProbability = "";

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));

            photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { PhotoSize = PhotoSize.Medium });
            Image = ImageSource.FromStream(() => photo.GetStream());

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
        }

        private async Task Classify()
        {
            if (Image != null && photo != null)
            {
                var classifications = await CrossImageClassifier.Current.ClassifyImage(photo.GetStream());

                ImageTag = classifications.OrderByDescending(c => c.Probability).First().Tag.ToString();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageTag)));

                ImageProbability = classifications.OrderByDescending(c => c.Probability).First().Probability.ToString("%0.");
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageProbability)));

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("", "Please take a picture!", "OK");
            }


        }
        
        private MediaFile photo;
        public ImageSource Image { get; private set; }
        public string ImageTag { get; private set; }
        public string ImageProbability { get; private set; }
        public ICommand TakePhotoCommand { get; }
        public ICommand PickPhotoCommand { get; }
        public ICommand ClassifyCommand { get; }
        

    }
}
