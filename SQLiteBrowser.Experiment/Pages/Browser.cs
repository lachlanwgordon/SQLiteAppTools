using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SQLiteBrowser.ViewModels;
using Xamarin.Forms;
using SQLite;
using System.Threading.Tasks;

namespace SQLiteBrowser
{
    public class Browser : ContentPage
    {
        private static bool Initialized { get; set; }
        private static SQLite.SQLiteConnection Connection;
        private static SQLite.SQLiteAsyncConnection AsyncConnection;

        public static async Task Init(string databasePath, params Type[] types)
        {
            Connection = new SQLiteConnection(databasePath);
            Connection.CreateTables(CreateFlags.None, types);
            AsyncConnection = new SQLiteAsyncConnection(databasePath);
            await AsyncConnection.CreateTablesAsync(CreateFlags.None, types);
        }

        public static async Task Init(SQLite.SQLiteConnection connection)
        {
            Connection = connection;
            var asyncConnection = new SQLiteAsyncConnection(connection.DatabasePath);
            var types = connection.TableMappings.Select(x => x.MappedType).ToArray();
            await asyncConnection.CreateTablesAsync(CreateFlags.None, types);
            AsyncConnection = asyncConnection;
            Connection = connection;
        }

        public static void Init(SQLite.SQLiteAsyncConnection asyncConnection)
        {
            var connection = new SQLiteConnection(asyncConnection.DatabasePath);
            var types = asyncConnection.TableMappings.Select(x => x.MappedType).ToArray();
            connection.CreateTables(CreateFlags.None, types);
            AsyncConnection = asyncConnection;
            Connection = connection;

        }


        private const int CharacterWidth = 11;
        BrowserViewModel VM = new BrowserViewModel();

        CollectionView CollectionView;

        CollectionView CollectionViewTemplate => new CollectionView
        {
            Margin = 0,
            BackgroundColor = Color.Olive,
            ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
            {
                VerticalItemSpacing = 1,
                Span = 1
            }
        };

        Picker Picker = new Picker
        {
            BackgroundColor = Color.DarkGoldenrod
        };

        Grid Headers = new Grid
        {
            ColumnSpacing = 1,
            BackgroundColor = Color.Gray,
            Padding = 0,
            Margin = 1,
        };

        ScrollView ScrollView = new ScrollView
        {
            Orientation = ScrollOrientation.Horizontal,
            Padding = 0,
            Margin = 0,

        };

        StackLayout InnerStack = new StackLayout
        {
            
        };

        public Browser()
        {
            BindingContext = VM;
            CollectionView = CollectionViewTemplate;
            Content = new StackLayout
            {
                Padding = new Thickness(0, 30, 0, 0),
                Spacing = 0,
                Children =
                {
                    Picker,
                    ScrollView
                    //{
                        
                    //    Content = new StackLayout
                    //    {
                    //        Spacing = 0,
                    //        Padding = 0,
                    //        Margin = 0,
                    //        Children =
                    //        {
                    //            Headers,
                    //            CollectionView
                    //        }
                    //    }
                    //}

                }
            };
            ScrollView.Content = InnerStack;
            InnerStack.Children.Add(Headers);
            InnerStack.Children.Add(CollectionView);


        }

        private async void Picker_Unfocused(object sender, FocusEventArgs e)
        {
            await VM.LoadRecords();

            //Make a header row

            Headers.Children.Clear();
            Headers.ColumnDefinitions.Clear();

            InnerStack.Children.Remove(CollectionView);
            CollectionView = CollectionViewTemplate;
            InnerStack.Children.Add(CollectionView);

            //CollectionView.WidthRequest = 150 * VM.Columns.Count;

            //Headers.WidthRequest = 150 * VM.Columns.Count;
            //ForceLayout();
            foreach (var column in VM.Columns)
            {
                Headers.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(column.MaxLength * CharacterWidth, GridUnitType.Absolute) });
            }


            //Make Header Labels
            foreach (var column in VM.Columns)
            {
                var label = new Label
                {
                    Text = column.Name,
                    BackgroundColor = Color.White,
                    Padding = 0,
                    Margin = 0,

                };
                Headers.Children.Add(label, column.ColumnNumber, 0);
            }

            CollectionView.ItemsSource = VM.Rows;

            CollectionView.ItemTemplate = new DataTemplate(() =>
            {
                Debug.WriteLine($"BC: {BindingContext}");

                var rowLayout = new Grid
                {
                    HorizontalOptions = LayoutOptions.Start,
                    Padding = 0,
                    Margin = 0,
                    ColumnSpacing = 1,
                    BackgroundColor = Color.Gray,
                    //ColumnDefinitions = Headers.ColumnDefinitions,
                };
                //rowLayout.BindingContext = new Binding("Properties");
                //BindableLayout.SetItemsSource(rowLayout, "Properties");
                //var props = BindingContext as Row;

                foreach (var column in VM.Columns)
                {
                    rowLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(column.MaxLength * CharacterWidth, GridUnitType.Absolute) });
                }

                foreach (var column in VM.Columns)
                {

                    var label = new Label
                    {
                        Padding = 0,
                        Margin = 0,
                        BackgroundColor = Color.White,
                        Text = column.Name,

                        //BindingContext = props.Properties.ElementAt(column.ColumnNumber)
                    };
                    label.BindingContextChanged += LabelBindingContextChanged;
                    //rowLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150, GridUnitType.Absolute) });

                    //label.SetBinding(Label.TextProperty, "Text");
                    rowLayout.Children.Add(label, column.ColumnNumber, 0);

                    if(column.ColumnNumber > rowLayout.ColumnDefinitions.Count - 1)
                    {
                        Debug.WriteLine($"HeaderColCount = {Headers.ColumnDefinitions.Count} RowColDefCount: {rowLayout.ColumnDefinitions.Count} colCount = {VM.Columns.Count}");
                    }
                }
                return rowLayout;
            });

            //ForceLayout();


        }

        private void LabelBindingContextChanged(object sender, EventArgs e)
        {
            var label = sender as Label;
            var row = (label.BindingContext as Row);
            if (row == null)
            {
                return;
            }

            var index = (label.Parent as Grid).Children.IndexOf(label);
            label.Text = row.Properties.ElementAtOrDefault(index).Text;
            Debug.WriteLine($"labelBD: {label.BindingContext}  text: {label.Text}");
        }

        protected override async void OnAppearing()
        {
            if (Connection == null || AsyncConnection == null)
                throw new InvalidOperationException("Please call SQLiteBrowser.Browser.Initialize before attempting to use the page");


            base.OnAppearing();

            //Events. Don't Forget To unsubscribe
            Picker.Unfocused += Picker_Unfocused;

            //Data Init
            await VM.InitializeAsync(Connection, AsyncConnection);

            //Set Data Properties
            Picker.ItemsSource = VM.Mappings;
            Picker.ItemDisplayBinding = new Binding("TableName");
            Picker.SetBinding(Picker.SelectedItemProperty, "SelectedMapping");
            CollectionView.ItemsSource = VM.Rows;

        }
    }
}

