using System;
using System.Diagnostics;
using System.Linq;
using SQLiteAppTools.Converters;
using SQLiteAppTools.Models;
using SQLiteAppTools.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;  
using Cell = SQLiteAppTools.Models.Cell;
using Picker = Xamarin.Forms.Picker;
using ScrollView = Xamarin.Forms.ScrollView;
using SearchBar = Xamarin.Forms.SearchBar;

namespace SQLiteAppTools.Pages
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
        const int cellWidth = 250;

        static readonly Color cellColor = Color.White;
        static readonly Color cellBorderColor = Color.DarkGray;

        readonly AltBrowserViewModel ViewModel = new AltBrowserViewModel();

        View MainGrid;
        CollectionView CollectionView;
        public Grid BodyGrid;

        readonly Grid Headers = new Grid
        {
            BackgroundColor = cellBorderColor,
            ColumnSpacing = columnSpacing,
            Margin = layoutMargin
        };

        readonly Picker Picker = new Picker
        {
            Title = "Table",
            BackgroundColor = cellColor,
            ItemDisplayBinding = new Binding(nameof(Table.Name)),
        };

        SearchBar SearchBar = new SearchBar
        {
            Placeholder = "Search",
            BackgroundColor = cellColor,
            PlaceholderColor = cellBorderColor,
        };

        public CSMarkupPage()
        {
            BindingContext = ViewModel;
            Content = GetContent();
            On<iOS>().SetUseSafeArea(true);
        }

        enum MainRow
        {
            Picker,
            Search,
            Body,
        }

        enum BodyRow
        {
            Headers,
            Collection,
        }

        private View GetContent()
        {
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
                }
            };
            Grid.SetRow(Headers, (int)BodyRow.Headers);

            var scroller = new ScrollView
            {
                BackgroundColor = cellBorderColor,
                Orientation = ScrollOrientation.Horizontal,
                Content = BodyGrid
            };


            MainGrid = new Grid
            {
                RowSpacing = layoutSpacing,
                BackgroundColor = cellBorderColor,
                Padding = layoutPadding,
                Margin = layoutMargin,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute) } ,
                    new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute) } ,
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                },

                Children =
                {
                    Picker,
                    SearchBar,
                    scroller
                }
            };
            Grid.SetRow(SearchBar, (int)MainRow.Search);
            Grid.SetRow(scroller, (int)MainRow.Body);

            return MainGrid;
        }

        private void UpdateCollectionView()
        {
            if (CollectionView != null && BodyGrid.Children.Contains(CollectionView)) 
                BodyGrid.Children.Remove(CollectionView);

            CollectionView = new CollectionView
            {
                Margin = layoutMargin,
                VerticalOptions = LayoutOptions.Start,
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    Span = ViewModel.SelectedTable.Columns.Count,
                    HorizontalItemSpacing = columnSpacing,
                    VerticalItemSpacing = rowSpacing,
                },
                BackgroundColor = cellBorderColor,
                ItemsSource = ViewModel.AllCells,

                ItemTemplate = new DataTemplate(() =>
                {
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
                    cell.SetBinding(Label.HorizontalTextAlignmentProperty, $"{nameof(Cell.Column)}.{nameof(Column.CLRType)}", BindingMode.OneTime, TypeToAlignmentConverter.Instance);

                    var tap = new TapGestureRecognizer();
                    tap.Tapped += CellTapped;
                    cell.GestureRecognizers.Add(tap);

                    return cell;
                })
            };

            Grid.SetRow(CollectionView, (int)BodyRow.Collection);
            BodyGrid.Children.Add(CollectionView);
            BodyGrid.WidthRequest = ViewModel.SelectedTable.Columns.Count * cellWidth;

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
                    HorizontalTextAlignment = (TextAlignment)TypeToAlignmentConverter.Instance.Convert(column.CLRType, typeof(TextAlignment), null, null),
                };
                Grid.SetColumn(label, columnNumber++);
                Headers.Children.Add(label);
            }
        }

        private async void CellTapped(object sender, EventArgs e)
        {
            var label = sender as Label;
            var cell = label.BindingContext as Cell;

            var alertDisplayed = false;

            if(cell.Column.Name.ToLower().EndsWith("id"))
            {
                var tableName = cell.Column.Name.Substring(0, cell.Column.Name.Length - 2);
                var table = ViewModel.Tables.FirstOrDefault(x => x.Name == tableName);

                if(table != null)
                {
                    alertDisplayed = true;
                    var shouldSearch = await DisplayAlert("Foreign Key", $"Would you like to search for {cell.DisplayText} in {table.Name}?", "Yes", "No");

                    if(shouldSearch)
                    {
                        Picker.SelectedItem = table;
                        ViewModel.SelectedTable = table;
                        await ViewModel.LoadTableData(table);
                        UpdateCollectionView();
                        SearchBar.Text = cell.DisplayText;
                    }
                }
            }

            if(!alertDisplayed)
            {
                if(cell.DisplayText.StartsWith("http"))
                {
                    alertDisplayed = true;
                    var open = await DisplayAlert("Open", $"Would you like to open {cell.DisplayText}", "yes", "no");
                    if(open)
                        Device.OpenUri(new Uri(cell.DisplayText));
                }
            }

            if(!alertDisplayed)
            {
                await DisplayAlert("Clicked", cell.DisplayText, "Okay");
            }
        }

        private async void TableSelected(object sender, FocusEventArgs e)
        {
            if (!((sender as Picker).SelectedItem is Table table))
                return;

            ViewModel.SelectedTable = table;
            await ViewModel.LoadTableData(table);
            UpdateCollectionView();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ViewModel.LoadTables();
            Picker.ItemsSource = ViewModel.Tables;
            Picker.Unfocused += TableSelected;
            SearchBar.TextChanged += Search;
        }

        bool isSearching;
        private void Search(object sender, TextChangedEventArgs e)
        {
            if (isSearching)
                return;
            if( e.NewTextValue.Length < 3 && CollectionView.ItemsSource != ViewModel.AllCells)
            {
                CollectionView.ItemsSource = ViewModel.AllCells;
            }

            isSearching = true;
            var table = Picker.SelectedItem as Table;
            var filtered = ViewModel.Search(e.NewTextValue, table);
            CollectionView.ItemsSource = filtered;
            isSearching = false;
        }

        protected override void OnDisappearing()
        {
            Picker.Unfocused -= TableSelected;
            SearchBar.TextChanged -= Search;

            base.OnDisappearing();
        }
    }
}
