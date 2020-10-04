using System;
using System.Collections.Generic;
using System.ComponentModel;
using SQLiteBrowser.Models;
using SQLiteBrowser.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.GridRowsColumns;
using Cell = SQLiteBrowser.Models.Cell;

namespace SQLiteBrowser.Pages
{
    public class CSMarkupPage : ContentPage
    {
        const int cellPadding = 5;
        const int cellMargin = 0;
        const int layoutPadding = 0;
        const int layoutMargin = 0;
        const int columnSpacing = 2;
        const int rowSpacing = 2;
        const int layoutSpacing = 2;
        const int cellHeight = 30;

        static Color cellColor = Color.White;
        static Color cellBorderColor = Color.DarkGray;


        AltBrowserViewModel ViewModel = new AltBrowserViewModel();
        Grid Headers = new Grid { BackgroundColor = cellBorderColor, ColumnSpacing = columnSpacing, Margin = layoutMargin};
        View MainGrid;

        public CSMarkupPage()
        {
            BindingContext = ViewModel;
            Content = GetContent();
        }

        enum BodyRow
        {
            Picker,
            Headers,
            Collection,
            BlankSpace
        }
        CollectionView CollectionView;
        private View GetContent()
        {
            Padding = new Thickness(0,40,0,0);
            CollectionView = new CollectionView
            {
                Margin = layoutMargin,
                VerticalOptions = LayoutOptions.Start,
                
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    Span = 5,
                    HorizontalItemSpacing = columnSpacing,
                    VerticalItemSpacing = rowSpacing,
                },
                BackgroundColor = cellBorderColor,
                ItemTemplate = new DataTemplate(() =>
                {

                    var grid = new Label { MaxLines = 1, BackgroundColor = cellColor, Margin = cellMargin, Padding = cellPadding, HeightRequest = cellHeight }.Bind(Label.TextProperty);

                    return grid;
                })
            }
                .Row(BodyRow.Collection)
                .Bind(ItemsView.ItemsSourceProperty, nameof(ViewModel.AllCells));

          


            MainGrid = new Grid
            {
                RowSpacing = layoutSpacing,
                BackgroundColor = cellBorderColor,
                Padding = layoutPadding,
                Margin = layoutMargin,
                RowDefinitions = Rows.Define(
                    (BodyRow.Picker, 50),
                    (BodyRow.Headers, 50),
                    (BodyRow.Collection, Star)
                    ),
                Children =
                {
                    new Picker { Title = "Table", BackgroundColor = cellColor, ItemDisplayBinding = new Binding(nameof(Table.name)) }
                        .Bind(Picker.ItemsSourceProperty, nameof(ViewModel.Tables))
                        .Bind(Picker.SelectedItemProperty, nameof(ViewModel.SelectedTable))
                    

                    ,
                    Headers.Row(BodyRow.Headers),
                    new StackLayout
                    {

                        Children =
                        {
                            CollectionView
                        }
                    }.Row(BodyRow.Collection)
                }
            };
            bool horizontalScrollingEnabled = true;
            if(horizontalScrollingEnabled)
            {
                var scroller = new ScrollView
                {
                    Orientation = ScrollOrientation.Horizontal,
                    Content = MainGrid
                };
                return scroller;
            }


            return MainGrid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.AltRows.CollectionChanged += AltRows_CollectionChanged;
            ViewModel.PropertyChanged += HeadersChanged;
            ViewModel.OnAppearing();
        }

        private void HeadersChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ViewModel.ColumnHeaders))
                return;

            Headers.Children.Clear();
            Headers.ColumnDefinitions.Clear();
            
            int columnNumber = 0;
            foreach (var column in ViewModel.SelectedTable.ColumnInfos)
            {
                Headers.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Headers.Children.Add(new Label { Text = column.Name, BackgroundColor = cellColor, Margin = cellMargin, Padding = cellPadding }.Column(columnNumber));
                columnNumber++;
            }

        }

        private void AltRows_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            MainGrid.WidthRequest = ViewModel.ColumnHeaders.Cells.Count * 300;
            (CollectionView.ItemsLayout as GridItemsLayout).Span = ViewModel.ColumnHeaders.Cells.Count;

        }
    }
}
