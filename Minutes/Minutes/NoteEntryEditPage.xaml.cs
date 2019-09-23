using Minutes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;

namespace Minutes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteEntryEditPage : ContentPage
    {
        private NoteEntry entry;
        public NoteEntryEditPage(NoteEntry entry)
        {
            InitializeComponent();
            BindingContext = this.entry = entry;
            CameraButton.Clicked += CameraButton_Clicked;
        }
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            if (entry != null)
            {
                await App.Entries.UpdateAsync(entry);
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Device.Idiom == TargetIdiom.Desktop
                || Device.Idiom == TargetIdiom.Tablet)
            {
                
                textEditor.Focus();
            }
        }
        private async void OnDeleteEntry(object sender, EventArgs e)
        {
            TextToSpeech.SpeakAsync("Are you sure you want to delete "+entry.Title, new SpeechOptions() { Volume=1.0f, Pitch = 1.0f});
            if (await DisplayAlert("Delete Entry", $"Are you sure you want to delete the entry {Title}?", "Yes", "No"))
            {
                await App.Entries.DeleteAsync(entry);
                entry = null; // deleted!
                await Navigation.PopAsync();
            }
        }
        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
        }
    }
}