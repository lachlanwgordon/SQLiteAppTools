The repo is currently in the experimental phase and the API is changing frequently. This will be available as a nuget when it's ready. This readme reflects how the code should end up – not as it currently is.

# SQLite Browser
SQLite Browser lets you look inside the database inside your Xamarin Forms app to inspect and debug the contents. There's no need to dig through folders deeply buried in macOS with GUIDs for names or a command line to dig around in Android. Just a nuget with a GUI write in your app.

SQLite Browser works on any Xamarin Forms app using sqlite-net-pcl and can be used on simulators, emulators and devices.

# Getting Started

1. Add SQLiteBrowser to your .NetStandard library from Nuget

2. In your App.xaml.cs set `MainPage = new BrowserPage(YourSQLiteConnection);`(and comment out where you were setting MainPage).
Make sure you have initialized your SQLiteConnection with the correct path and have called CreateTable<>() for all your types.

3. Run your App and explore your database.

If your SQLiteConnection isn't ready yet at startup, you want to open the Browser after the app has fully started, you're not using a XamarinForms App or for some other reason the above doesn't work for you – see **Alternate Setup Methods**

# Alternate Setup Methods
There are several ways to open the SQLiteBrowser to work with different startup flows and when you want to access the browser.

All these options require you start by installing SQLiteBrowser from Nuget.

## Initialize on startup and Navigate Later
If you want to Initialise the SQLite browser when your app starts but wait until later when then app is running to browse the data:

1. Initialize in App.xaml.cs, or your Database class etc. wherever you have access to the connection and once you have called CreateTable();
call `SQLiteBrowser.Init.Init(YourSQLiteConnection);`

2. Add a BrowserPage somewhere in your app's navigation.  This could be in your MasterDetailPage, Shell or you could navigate from anywhere in your app just like you would with any XamarinForms Page. Once you've call Init() there's no need to pass the connection to the BrowserPage again, you can call `new BrowserPage()` with no parameters.

## Initialize with Database Path and Types
If you don't have access to your SQLiteConnection where you want to initialize the SQLite Browser or if you don't call CreateTable for each type until you use them(or only once when app first runs) you can instead pass in a Database Path and any types you want to be able to browse.
1. call `SQLiteBrowser.Init.Init("full/path/data.db3",typeof(Person),typeof(Cat),...);`
2. Add a BrowserPage somewhere in your app's navigation.  This could be in your MasterDetailPage, Shell or you could navigate from anywhere in your app just like you would with any XamarinForms Page. Once you've call Init() there's no need to pass the connection to the BrowserPage again, you can call `new BrowserPage()` with no parameters.

## Use the BrowserApp From Native Code
If you're not using a Xamarin Forms App, or you want to be able to quickly boot into SQLiteBrowser without initializing your Forms App you can add a BrowserApp from your AppDelegate or MainActivity.
1. Where you would normally call `LoadApplication(new App());` call `LoadApplication(new BrowserApp(SqliteConnection));` or `SQLiteBrowser.Init.Init("full/path/data.db3",typeof(Person),typeof(Cat),...);`

See **Initialize with Database Path and Types** for details of using a connection vs Path and Types.

If you weren't already using Xamarin Forms you will also need to add `Xamarin.Forms.Forms.Init(this, savedInstanceState);` on Android or `Xamarin.Forms.Forms.Init();` on iOS.

# Using the Browser
Once you've got the browser up and runnning all the functionality happens within the app. To get started open the Table picker, select a picker and scroll through the records.

Tap on any record to see details.

Long press on any field to copy the contents – This will be useful for searching, executing SQL and Navigation

## The tool bar
Under the picker is a tool bar which lets you search, filter, execute SQL etc.

### Search
Click the magnifiying glass and enter text to search. By default it will search all fields in the current table. You can change this to only search certain fields to speed it up or you can search all tables to slow it down.

### Execute SQL
Want to write your own query or run a script to seed the database? Go right ahead, tap the `SQL` button and type away. This is useful for debuggin your queries without rebuilding your app every time or if you're feeling analytical.

### Add Item
Click the plus button and fill in the details to add a new record.

## Detailed View
When you tap on an item it's details open up. From the details page you can edit, navigate and view resources.

## Edit
Tap the pencil to make all fields editable. Click save to update them in the database.

## Navigation
SQLite-Net-PCL doesn't support foreign keys but we all use them anyway and reconstruct the data. Tap on a record to see the details, the tap on anyfield that has a foreign key in it. If the FieldName is PersonId or Person and you have a table called Person it will open the table and filter by the ID. If you have we can't find a matching table it will ask you to select a table.

## Images
If a resource looks like a path to an image we'll try to load it and display it.

## URLs
If you've got URLs in your database linking(e.g. a pdf in blob storage or an external link) tap on the URL to open it in the devices default browser.








