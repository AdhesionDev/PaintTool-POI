using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace PaintTool_POI.UIElements
{
    public delegate T GetItem<T>();
    public class PaintToolItemGrid : Grid
    {
        private IconElement iconElement;
        private TextBlock descripution;
        public PaintToolItemGrid(string iconString, string descriputionString) : this(() =>
        {
            FontIcon icon = new FontIcon()
            {
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Glyph = iconString
            };
            return icon;
        },
            () =>
            {
                TextBlock descriputionText = new TextBlock()
                {
                    Text = descriputionString
                };
                return descriputionText;
            })
        {
        }
        public PaintToolItemGrid(IconElement icon, TextBlock descripution)
        {
            this.SetIconAndText(icon, descripution);
        }

        public PaintToolItemGrid(GetItem<IconElement> getIcon, GetItem<TextBlock> getText)
        {
            IconElement iconElement = getIcon?.Invoke();
            TextBlock textBlock = getText?.Invoke();
            this.SetIconAndText(iconElement, textBlock);
        }


        private void SetIconAndText(IconElement icon, TextBlock descripution)
        {
            this.iconElement = icon;
            this.descripution = descripution;

            this.iconElement.SetValue(Grid.ColumnProperty, 0);
            this.iconElement.VerticalAlignment = VerticalAlignment.Center;

            this.descripution.SetValue(Grid.ColumnProperty, 1);
            this.descripution.TextWrapping = TextWrapping.WrapWholeWords;
            this.descripution.FontSize = 8;

            ColumnDefinition iconColumn = new ColumnDefinition();
            this.ColumnDefinitions.Add(iconColumn);
            ColumnDefinition descriputionColumn = new ColumnDefinition();
            this.ColumnDefinitions.Add(descriputionColumn);
            this.SetValue(MarginProperty, new Thickness(10, 0, 10, 0));
            this.Width = 64;

            this.Children.Add(icon);
            this.Children.Add(descripution);
        }
    }
}
