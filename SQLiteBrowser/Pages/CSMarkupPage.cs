using System;
using SQLiteBrowser.Models;
using SQLiteBrowser.ViewModels;
using Xamarin.Forms;
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
        View MainGrid;
        public Grid BodyGrid;
        Grid Headers = new Grid { BackgroundColor = cellBorderColor, ColumnSpacing = columnSpacing, Margin = layoutMargin };
        Picker Picker = new Picker
        {
            Title = "Table",
            BackgroundColor = cellColor,
            ItemDisplayBinding = new Binding(nameof(Table.Name)),
        };

        public CSMarkupPage()
        {
            BindingContext = ViewModel;
            Content = GetContent();
        }

        enum MainRow
        {
            Picker,
            Body,
        }

        enum BodyRow
        {
            Headers,
            Collection,
        }
        CollectionView CollectionView;


        private View GetContent()
        {
            Padding = new Thickness(0, 40, 0, 0);
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
                    var cell = new Label
                    {
                        MaxLines = 1,
                        BackgroundColor = cellColor,
                        Margin = cellMargin,
                        Padding = cellPadding,
                        HeightRequest = cellHeight
                    };
                    cell.SetBinding(Label.TextProperty, nameof(Cell.DisplayText));
                    cell.SetBinding(Label.HorizontalTextAlignmentProperty, nameof(Cell.Alignment));
                    return cell;
                })
            };
            Grid.SetRow(CollectionView, (int)BodyRow.Collection);

            View body;

            Grid.SetRow(Headers, (int)BodyRow.Headers);
            BodyGrid = new Grid
            {
                BackgroundColor = cellBorderColor,
                RowSpacing = layoutSpacing,

                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute) } ,
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    Headers,
                    CollectionView
                }
            };
            
            var scroller = new ScrollView
            {
                BackgroundColor = cellBorderColor,
                Orientation = ScrollOrientation.Horizontal,
                Content = BodyGrid
            };
            Grid.SetRow(scroller, (int)MainRow.Body);
            body = scroller;
            

            MainGrid = new Grid
            {
                RowSpacing = layoutSpacing,
                BackgroundColor = cellBorderColor,
                Padding = layoutPadding,
                Margin = layoutMargin,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute) } ,
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                },

                Children =
                {
                    Picker,
                    body
                }
            };

            return MainGrid;
        }

        private async void TableSelected(object sender, FocusEventArgs e)
        {
            var table = (sender as Picker).SelectedItem as Table;
            ViewModel.SelectedTable = table;
            await ViewModel.LoadTableData(table);
            CollectionView.ItemsSource = ViewModel.AllCells;
            UpdateView();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ViewModel.LoadTables();
            Picker.ItemsSource = ViewModel.Tables;
            Picker.Unfocused += TableSelected;
        }

        protected override void OnDisappearing()
        {
            Picker.Unfocused -= TableSelected;

            base.OnDisappearing();
        }

        private void UpdateView()
        {
            BodyGrid.WidthRequest = ViewModel.SelectedTable.Columns.Count * 200;

            Headers.Children.Clear();

            int columnNumber = 0;
            foreach (var column in ViewModel.SelectedTable.Columns)
            {
                var label = new Label
                {
                    Text = column.Name,
                    BackgroundColor = cellColor,
                    Margin = cellMargin,
                    Padding = cellPadding
                };
                Grid.SetColumn(label, columnNumber);
                Headers.Children.Add(label);

                columnNumber++;
            }
            CollectionView.ItemsSource = ViewModel.AllCells;
            (CollectionView.ItemsLayout as GridItemsLayout).Span = ViewModel.SelectedTable.Columns.Count;
        }
    }
}
