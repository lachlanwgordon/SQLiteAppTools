namespace SQLiteBrowser.Experiment.ViewModels
{
    public class Property
    {
        public object Value { get; set; }
        public double Width { get; set; }
        public string Name => Value.ToString();
    }
}