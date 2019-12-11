using System;

using Xamarin.Forms;

namespace SQLiteBrowser.Experiment.ViewModels
{
    public class TestModel : ContentPage
    {
        public TestModel()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

