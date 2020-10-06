﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        View MainGrid;
        public Grid BodyGrid;
        Grid Headers = new Grid { BackgroundColor = cellBorderColor, ColumnSpacing = columnSpacing, Margin = layoutMargin};
        Picker Picker = new Picker
        {
            Title = "Table",
            BackgroundColor = cellColor,
            ItemDisplayBinding = new Binding(nameof(Table.name)),
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

                    var grid = new Label { MaxLines = 1, BackgroundColor = cellColor, Margin = cellMargin, Padding = cellPadding, HeightRequest = cellHeight }
                        .Bind(Label.TextProperty)

                        .Bind(Label.HorizontalTextAlignmentProperty, "Alignment");

                    return grid;
                })
            }
                .Row(BodyRow.Collection);

            View body;
            BodyGrid = new Grid
            {
                BackgroundColor = cellBorderColor,
                RowSpacing = layoutSpacing,
                RowDefinitions = Rows.Define(
                    (BodyRow.Headers, 50),
                    (BodyRow.Collection, Star)
                ),
                Children =
                {
                    Headers.Row(BodyRow.Headers),
                    CollectionView.Row(BodyRow.Collection)
                }
            };
            bool horizontalScrollingEnabled = true;
            if (horizontalScrollingEnabled)
            {
                var scroller = new ScrollView
                {
                    BackgroundColor = cellBorderColor,
                    Orientation = ScrollOrientation.Horizontal,
                    Content = BodyGrid
                }.Row(MainRow.Body);
                body = scroller;
            }
            else
            {
                body = BodyGrid;
            }
            
            MainGrid = new Grid
            {
                RowSpacing = layoutSpacing,
                BackgroundColor = cellBorderColor,
                Padding = layoutPadding,
                Margin = layoutMargin,
                RowDefinitions = Rows.Define(
                    (MainRow.Picker, 50),
                    (MainRow.Body, Star)
                    ),
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadTables();
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
            BodyGrid.WidthRequest = ViewModel.ColumnHeaders.Cells.Count * 200;
            Debug.WriteLine($"{ViewModel.SelectedTable.name} selected, Setting ItemsLayout to span {ViewModel.ColumnHeaders.Cells.Count}");

            Headers.Children.Clear();
            Headers.ColumnDefinitions.Clear();

            int columnNumber = 0;
            foreach (var column in ViewModel.SelectedTable.ColumnInfos)
            {
                Headers.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Headers.Children.Add(new Label { Text = column.Name, BackgroundColor = cellColor, Margin = cellMargin, Padding = cellPadding }.Column(columnNumber));
                columnNumber++;
            }
            CollectionView.ItemsSource = ViewModel.AllCells;
            (CollectionView.ItemsLayout as GridItemsLayout).Span = ViewModel.ColumnHeaders.Cells.Count;
        }

    }
}
