using System;
using System.Diagnostics;
using SQLiteBrowser.Converters;
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

        int templateCount;
        private View GetContent()
        {
            Padding = new Thickness(0, 40, 0, 0);
            CollectionView = new CollectionView
            {
                Margin = layoutMargin,
                VerticalOptions = LayoutOptions.Start,
                //ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    Span = 5,
                    HorizontalItemSpacing = columnSpacing,
                    VerticalItemSpacing = rowSpacing,
                },
                BackgroundColor = cellBorderColor,
                ItemTemplate = new DataTemplate(() =>
                {
                    var count = templateCount++;
                    //Debug.WriteLine($"template start templateCount: {count} time: {stopwatch.ElapsedMilliseconds}");

                    var cell = new Label
                    {
                        MaxLines = 1,
                        BackgroundColor = cellColor,
                        Margin = cellMargin,
                        Padding = cellPadding,
                        HeightRequest = cellHeight,
                        LineBreakMode = LineBreakMode.NoWrap
                    };
                    cell.SetBinding(Label.TextProperty, nameof(Cell.DisplayText), BindingMode.OneTime);
                    cell.SetBinding(Label.HorizontalTextAlignmentProperty, "Column.CLRType", BindingMode.OneTime, new TypeToAlignmentConverter());
                    Debug.WriteLine($"template end templateCount: {count} time: {stopwatch.ElapsedMilliseconds}");
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
            stopwatch.Reset();
            stopwatch.Start();
            Debug.WriteLine($"Table selected {stopwatch.ElapsedMilliseconds}");

            var table = (sender as Picker).SelectedItem as Table;
            ViewModel.SelectedTable = table;
            await ViewModel.LoadTableData(table);
            CollectionView.ItemsSource = ViewModel.AllCells;
            Debug.WriteLine($"Table data loaded {stopwatch.ElapsedMilliseconds}");

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

        Stopwatch stopwatch = new Stopwatch();
        private void UpdateView()
        {
            Debug.WriteLine($"update view {stopwatch.ElapsedMilliseconds}");

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
                    Padding = cellPadding,
                    HorizontalTextAlignment = column.CLRType.IsPrimitive ? TextAlignment.End : TextAlignment.Start
                };
                Grid.SetColumn(label, columnNumber);
                Headers.Children.Add(label);

                columnNumber++;
            }
            Debug.WriteLine($"headers added {stopwatch.ElapsedMilliseconds}");

            CollectionView.ItemsSource = ViewModel.AllCells;
            Debug.WriteLine($"source set {stopwatch.ElapsedMilliseconds}");

            (CollectionView.ItemsLayout as GridItemsLayout).Span = ViewModel.SelectedTable.Columns.Count;
            Debug.WriteLine($"Width set {stopwatch.ElapsedMilliseconds}");
        }
    }
}
