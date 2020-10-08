using System;
using Android.Content;
using Android.Runtime;
using SQLiteAppTools.Droid.Renderer;
using Xamarin.Forms;

//[assembly: ExportRenderer(typeof(Label), typeof(CustomLabelRenderer))]
namespace SQLiteAppTools.Droid.Renderer
{
    public class CustomLabelRenderer : Xamarin.Forms.Platform.Android.FastRenderers.LabelRenderer
    {
        public CustomLabelRenderer(Context context) : base(context)
        {

        }

        [Obsolete]
        public CustomLabelRenderer(IntPtr handle, JniHandleOwnership transfer)
        {

        }
    }
}
