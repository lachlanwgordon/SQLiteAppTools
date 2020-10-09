# SQLite App Tools
SQLite App Tools lets you browse the tables of a SQLite databse, inside your Xamarin Forms app. 

No need to dig through folders deeply buried in macOS with GUIDs for names, or digging around the command line to get your db3 off your Android. Just a nuget with a GUI right in your app.

SQlite App Tools works on any platform that runs Xamarin Forms and sqlite-net. You can run it on simulators, emulator, phones, tablets – any where you run your app.

SQLite Browser works on any Xamarin Forms app using sqlite-net-pcl and can be used on simulators, emulators and devices.

![SQLite App Tools running on and iPad Simulator and Android Pixel 3A](img/demo.gif "test")


# Getting Started

1. Add SQLiteAppTools to your .NetStandard library from Nuget. The preview version is currently available on an Azure Devops feed at `https://pkgs.dev.azure.com/lachlanwgordon/SQLiteAppTools/_packaging/SQLiteBrowser/nuget/v3/index.json`.

It is a public feed but you will need to sign in with a Microsoft account. The package will be moving to nuget.org shortly.

2. In your `App.xaml.cs` set your MainPage to a `new SQLiteAppTools.BrowserPage(path)`. The path needs to be the same path you use when creating a `SQLiteConnection`.

You main need to remove(or comment out) the line where you previously set your MainPage.

```
protected override async void OnStart()
{
    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.db3");
    MainPage = new SQLiteAppTools.BrowserPage(path);
}
```

3. Select a Table from the Picker, then scroll, click and search around your database.

If you want to pass in the path at startup, and navigate to the page later, or through a Shell, TabbedPage etc., see [Initialize on startup, navigate later](#Initialize-on-startup,-navigate-later)


# Features

The navigation and filtering features are likely to change quite a bit but the code you need to write to get this running shouldn't need to change much.

## Select Table
The Picker has a list of Tables found in your database. Select one and it will load the data. Scroll around.

## Search
Type 3 or more characters in the search box and it will search all cells in the table and display the matching rows.

## Navigate to Foreign Key
Click on a foreign key in a table, if the column name is of the format `{TableName}Id`, the table will be opened and it will search for the Id.

## Open URL
Click on a cell with a URL in it, the URL will open in the default browser.

## Initialize on startup, navigate later
I usually like to initialize my database at startup, and then forget about what the path is and I don't want to mess around with passing the path into the view or view model where I navigate to the SQLite App Tools Browser Page.

To initialize without opening the page, call `TableService.Init()`:

```
protected override async void OnStart()
{
    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.db3");
    SQLiteAppTools.TableService.Init(path);
}
```

When you navigate to the page, you don't need to include a `path` parameter.

```
Shell.Current.Navigation.PushAsync(new SQLiteAppTools.BrowserPage());
```

Or you can include it in a Shell by including the namespace `xmlns:sqiteapptools="clr-namespace:SQLiteAppTools;assembly=SQLiteAppTools"`

and adding the `BrowserPage` the same way you would with any other content page.

```
<sqiteapptools:BrowserPage Title="DB"></sqiteapptools:BrowserPage>
```

# Contributing

Found a bug or have a good feature idea? Create an issue.

PRs welcome!








