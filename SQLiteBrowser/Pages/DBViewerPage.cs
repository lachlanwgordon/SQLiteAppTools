using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SQLiteBrowser.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.GridRowsColumns;

namespace SQLiteBrowser.Pages
{
    public class DBViewerPage : ContentPage
    {
        AltBrowserViewModel VM => BindingContext as AltBrowserViewModel;
        public DBViewerPage()
        {
            BindingContext = new AltBrowserViewModel();
            Content = BuildContent();
        }



        enum Row { Header, Body }
        private View BuildContent() => new Grid
        {
            RowDefinitions = Rows.Define(
                (Row.Header, 80),
                (Row.Body, GridLength.Star)),
            Children =
            {
                new Picker
                {
                    Title = "Table", ItemDisplayBinding = new Binding(nameof(VM.SelectedTable.name)),

                }
                    .Invoke (p => p.Unfocused += (s,e) => {VM.TableSelected.Execute(s); })
                    .Assign(out TablePicker)
                    .Bind(Picker.ItemsSourceProperty, nameof(VM.Tables))
                    .Bind(Picker.SelectedItemProperty, nameof(VM.SelectedTable) ),
                new ScrollView
                {
                    Orientation = ScrollOrientation.Horizontal,
                    Content = new Grid
                    {
                        RowDefinitions = Rows.Define(
                            (Row.Header, 80),
                            (Row.Body, GridLength.Star)),
                        Children =
                        {
                            new AbsoluteLayout
                            {
                                Children =
                                {
                                    new Label{ Text = "A row of table headers should be here"}
                                }
                            }
                            .Assign(out HeaderRow),

                            new CollectionView
                            {
                                ItemTemplate = new DataTemplate(() => new Label{Text = "Cell" }),
                                EmptyView = new Label { Text = "this is an empty collection"}
                            }
                            .Row(Row.Body)
                            .Assign(out MainCollection)

                        }
                    }
                }.Row(Row.Body)
            }
        };



        AbsoluteLayout HeaderRow = new AbsoluteLayout();
        private Picker TablePicker;

        CollectionView MainCollection;
        private AbsoluteLayout CollectionTemplateLayout;


        //DataTemplate CollectionTemplate => new DataTemplate(() =>
        //{
        //    return CollectionTemplateLayout;
        //});

        //AbsoluteLayout CollectionTemplateLayout = new AbsoluteLayout
        //{
        //    Children =
        //    {
        //        new Label { Text = "cell", BackgroundColor = Color.Plum,}
        //    }
        //};


        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Setup events
            VM.PropertyChanged += VM_PropertyChanged;

            //Init VM
            VM.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            VM.PropertyChanged -= VM_PropertyChanged;
        }


        private async void VM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"Property changed{e.PropertyName}");
            //if (e.PropertyName == nameof(VM.SelectedTable))
            //{
            //    MainCollection.ItemsSource = null;
            //}

            if (e.PropertyName == nameof(VM.ColumnHeaders))
            {
                Debug.WriteLine($"Property changed column headers");

                MainCollection.ItemsSource = null;
                //MainCollection.ItemTemplate = null;
                await Task.Delay(500);

                UpdatedHeaders();
                UpdateRows();
                await Task.Delay(500);
                Debug.WriteLine($"Set source to {VM.AltRows}");
                MainCollection.ItemsSource = VM.AltRows;
            }


        }

        private void UpdateRows()
        {
        }

        const double cellWidth = 100;
        private void UpdatedHeaders()
        {
            HeaderRow.Children.Clear();

            int columnNumber = 0;

            foreach (var header in VM.ColumnHeaders.Cells)
            {
                var headerLabel = new Label
                {
                    Text = header.DisplayText,
                    BackgroundColor = Color.Beige,
                };

                AbsoluteLayout.SetLayoutBounds(headerLabel, new Rectangle(columnNumber * cellWidth, 0, cellWidth, 1));
                AbsoluteLayout.SetLayoutFlags(headerLabel, AbsoluteLayoutFlags.HeightProportional);
               
                HeaderRow.Children.Add(headerLabel);
                columnNumber++;
            }
            columnNumber = 0;
            MainCollection.ItemTemplate = new DataTemplate(() =>
            {
                var colNumber = 0;
                var collectionTemplateLayout = new AbsoluteLayout { HeightRequest = 40, };

                foreach (var header in VM.ColumnHeaders.Cells)
                {
                    var cellTemplateLabel = new Label
                    {
                        BackgroundColor = Color.Blue,
                        Text = "Celllllll"
                    }.Bind(Label.TextProperty, $"Cell{colNumber}.DisplayText");

                    AbsoluteLayout.SetLayoutBounds(cellTemplateLabel, new Rectangle(colNumber * cellWidth, 0, cellWidth, 1));
                    AbsoluteLayout.SetLayoutFlags(cellTemplateLabel, AbsoluteLayoutFlags.HeightProportional);
                    collectionTemplateLayout.Children.Add(cellTemplateLabel);
                    colNumber++;

                }
                Debug.WriteLine(collectionTemplateLayout.Width);
                return collectionTemplateLayout;
            });

        }
    }
}
