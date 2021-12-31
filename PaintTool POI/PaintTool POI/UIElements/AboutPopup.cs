using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PaintTool_POI.UIElements
{
    internal class AboutPopup
    {
        public static async void ShowAbout()
        {

            ContentDialog aboutDialog = new ContentDialog()
            {
                Title = "About",
                Content = "PaintTool POI\nVersion: Test",
                CloseButtonText = "OK",
                PrimaryButtonText = "Gua",
            };
            await aboutDialog.ShowAsync();
        }
    }
}
