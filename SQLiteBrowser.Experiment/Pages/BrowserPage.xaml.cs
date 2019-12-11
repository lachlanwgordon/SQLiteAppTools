using System;
using Xamarin.Forms;


namespace SQLiteBrowser.Pages
{
    public partial class BrowserPage : ContentPage
    {
        public BrowserPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await VM.InitializeAsync();
            //VM.PropertyChanged += VM_PropertyChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //VM.PropertyChanged -= VM_PropertyChanged;
        }

        void Picker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
        }
        //private void VM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if(e.PropertyName == "TotalCharacterLength")
        //    {
        //        var factor = (double) Resources["CharacterWidth"];
        //        TheCollectionView.WidthRequest = VM.TotalCharacterLength * factor;
        //    }
        //}

    }
}