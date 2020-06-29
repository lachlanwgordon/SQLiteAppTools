using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

namespace SQLiteBrowser.Pages
{
    public partial class FakeScrolling : ContentPage
    {
        public FakeScrolling()
        {
            Stopwatch.Start();
            Debug.WriteLine($"Contructed  {Stopwatch.Elapsed}");
            MakeData();
            InitializeComponent();
            FindLabels();
            Debug.WriteLine($"initialized  {Stopwatch.Elapsed}");
        }

        private void MakeData()
        {
            for (int x = 0; x < maxColumn; x++)
            {
                for (int y = 0; y < maxRow; y++)
                {
                    var text = $"text for {x}, {y}";
                    Data.Add((x, y), text);
                }
            }
        }

        public void FindLabels()
        {
            foreach (var view in XAMLContentGrid.Children)
            {
                if (view is Label label)
                {

                    var row = Grid.GetRow(label);
                    var col = Grid.GetColumn(label);
                    Labels.Add((row, col), label as Label);

                    label.Text = $"Label Found {col},{row}";
                }
            }
        }

        Stopwatch Stopwatch = new Stopwatch();

        Dictionary<(int x, int y), Label> Labels = new Dictionary<(int x, int y), Label>();



        //Dummy Data
        public int maxColumn = 15;
        public int maxRow = 50;
        Dictionary<(int x, int y), string> Data = new Dictionary<(int x, int y), string>();


        public int VisibleRowCount { get; set; } = 15;
        public int VisibleColumnCount { get; set; } = 8;
        public int RendererdRowCount { get; set; } = 19;
        public int RenderedColumnCount { get; set; } = 12;

        public double ColumnWidth => ContentScroller.Width / VisibleColumnCount;
        public double RowHeight => ContentScroller.Height / VisibleRowCount;

        Grid ContentGrid;
        private void MakeContentGrid()
        {
            Debug.WriteLine($"start make grid  {Stopwatch.Elapsed}");
            Grid grid = new Grid
            {
                Padding = 0,
                Margin = 0,
                RowSpacing = 1,
                ColumnSpacing = 1,
                BackgroundColor = Color.FromHex("#220000FF"),

            }.Assign(out ContentGrid);

            //Prepare dummy data
            //Current test set is 15*50 this will be replaced with a view model
            
            Debug.WriteLine($"before Generate grid  {Stopwatch.Elapsed}");

            //GenerateGrid();
            //PopulateGrid();
            Debug.WriteLine($"after Generate grid  {Stopwatch.Elapsed}");


            //Set the row and column definitions to be four more
            for (int x = 0; x < RenderedColumnCount; x++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(ColumnWidth, GridUnitType.Absolute) });
            }
            for (int y = 0; y < RendererdRowCount; y++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(RowHeight, GridUnitType.Absolute) });
            }

            grid.WidthRequest = RenderedColumnCount * ColumnWidth;
            grid.HeightRequest = RendererdRowCount * RowHeight;

            GiantStack.WidthRequest = maxColumn * ColumnWidth;
            GiantStack.HeightRequest = maxRow * RowHeight;


            Debug.WriteLine($"about to set content grid  {Stopwatch.Elapsed}");
            //ContentScroller.Content = grid;
            Debug.WriteLine($"set content grid  {Stopwatch.Elapsed}");
        }

        private void GenerateGrid()
        {
            //Prepare rendered Grid and Dictionary of labels
            for (int x = 0; x < RenderedColumnCount; x++)
            {
                for (int y = 0; y < RendererdRowCount; y++)
                {
                    var cell = new Label { Text = Data[(x, y)], BackgroundColor = Color.White, Margin = 1 }.Row(y).Column(x);
                    Labels.Add((x, y), cell);
                    ContentGrid.Children.Add(cell);
                }
            }
        }

        private void PopulateGrid(int startPositionY = 0)
        {
            Debug.WriteLine($"Resetting grid to {startPositionY} + {RendererdRowCount} ");
            if(startPositionY + RendererdRowCount >= maxRow - 1)
            {
                return;
            }

            foreach (var label in Labels)
            {
                var data = Data[(label.Key.x, label.Key.y + startPositionY)];
                label.Value.Text = data;
            }
        }

        double scrolledDistanceY;
        int previousTopRowPosition = 0;
        private async void GiantScroller_Scrolled(object sender, ScrolledEventArgs e)
        {
            Debug.WriteLine($"scrolled: {e.ScrollY},{e.ScrollX}");

            //Delete this line
            //await ContentScroller.ScrollToAsync(e.ScrollX, e.ScrollY, false);

            //return;

            var topRowPositionShouldBe = (int) (e.ScrollY / RowHeight);
            Debug.WriteLine($"scrolled: {e.ScrollY} top row should be {topRowPositionShouldBe}");


            //Scrolled a whole row, redraw grid and reset
            if(previousTopRowPosition != topRowPositionShouldBe)
            {
                Debug.WriteLine($"redraw grid for {topRowPositionShouldBe}");
                previousTopRowPosition = topRowPositionShouldBe;
                PopulateGrid(topRowPositionShouldBe);
                await ContentScroller.ScrollToAsync(0, 0, false);
            }
            //Small scroll, animate
            else
            {
                Debug.WriteLine($"scroll inner to {e.ScrollY}");

                var scrollDifference = e.ScrollY - (previousTopRowPosition * RowHeight);

                await ContentScroller.ScrollToAsync(0, scrollDifference, false);
            }


            //if(e.ScrollY > RowHeight * 2)
            //{
            //    Debug.WriteLine($" not Scrolling {e.ScrollX},{e.ScrollY}");
            //    await ContentScroller.ScrollToAsync(e.ScrollX, RowHeight * 2, false);
            //}
            //else
            //{
            //    Debug.WriteLine($"Scrolling {e.ScrollX},{e.ScrollY}");


            //    await ContentScroller.ScrollToAsync(e.ScrollX, e.ScrollY, false);
            //}





        }

        int rowPosition = 0;
        int columnPosition = 0;
        int previousRowsPanned = 0;
        int previousColumnsPanned = 0;


        double bubbleTranslation;
        private void UpdateValue(object s, PanUpdatedEventArgs e)
        {
            //Debug.WriteLine($"{e.StatusType}  {e.TotalX}, {e.TotalY}");


            if (e.StatusType == GestureStatus.Running)
            {

                TheBubble.TranslationY = bubbleTranslation -  e.TotalY / 2;



                //var width = (int)InnerGrid.Width;
                //var colWidth = width / ColumnCount;
                //var columnsPanned = (int)-(e.TotalX / colWidth);

                //var height = (int)InnerGrid.Height;
                //var rowHeight = height / RowCount;
                //var rowsPanned = (int)-(e.TotalY / rowHeight);


                //if (rowsPanned != 0 || columnsPanned != 0)
                //{

                //    if(rowsPanned > previousRowsPanned)
                //    {
                //        if(rowPosition + RowCount <  maxRow)
                //            rowPosition++;
                //    }
                //    else if (rowsPanned < previousRowsPanned)
                //    {
                //        if(rowPosition > 0)
                //            rowPosition--;
                //    }
                //    if (columnsPanned > previousColumnsPanned)
                //    {
                //        if(columnPosition + ColumnCount <  maxColumn)
                //            columnPosition++;
                //    }
                //    else if (columnsPanned < previousColumnsPanned)
                //    {
                //        if(columnPosition > 0)
                //            columnPosition--;
                //    }

                //    Debug.WriteLine($"panning new pos {columnPosition}, { rowPosition}");

                //    for (int x = 0; x < ColumnCount; x++)
                //    {
                //        for (int y = 0; y < RowCount; y++)
                //        {
                //            var label = Labels[(x, y)];
                //            label.Text = Data[(x + columnPosition, y + rowPosition)];
                //        }
                //    }

                //    previousRowsPanned = rowsPanned;
                //    previousColumnsPanned = columnsPanned;
                //}
                
            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                previousRowsPanned = 0;
                previousColumnsPanned = 0;
                bubbleTranslation = TheBubble.TranslationY;
            }

        }

        BoxView TheBubble;

        protected override void OnAppearing()
        {
            Debug.WriteLine($"onapering {Stopwatch.Elapsed}");
            base.OnAppearing();
            Debug.WriteLine($"about tomake {Stopwatch.Elapsed}");
            //MakeContentGrid();
            Debug.WriteLine($"made {Stopwatch.Elapsed}");

            //var portionOfHeight = (double) RowCount / maxRow;
            //var bubbleHeight = InnerGrid.Height * portionOfHeight;
            //TheBubble = new BoxView
            //{
            //    CornerRadius = 5,
            //    WidthRequest = 10,
            //    HeightRequest = bubbleHeight,
            //    VerticalOptions = LayoutOptions.Start,
            //    HorizontalOptions = LayoutOptions.End,
            //    Color = Color.BurlyWood
            //}.Column(ColumnCount - 1).RowSpan(RowCount);



            //TheGrid.Children.Add(TheBubble);
        }
    }

   
}
