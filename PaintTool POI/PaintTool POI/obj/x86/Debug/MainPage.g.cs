﻿#pragma checksum "C:\Users\ADT\Documents\GitHub\PaintTool-POI\PaintTool POI\PaintTool POI\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5788C8D28AC6DD8E5E7E64F703227AE6138AC96A528F3BDFEFED9D6FC8501B72"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaintTool_POI
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainPage.xaml line 296
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element2 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element2).Click += this.About_Button_Click;
                }
                break;
            case 3: // MainPage.xaml line 301
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element3 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element3).Click += this.DebugButton_Click;
                }
                break;
            case 4: // MainPage.xaml line 283
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element4 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element4).Click += this.ZoomInButton_Click;
                }
                break;
            case 5: // MainPage.xaml line 285
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element5 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element5).Click += this.ZoomOutButton_Click;
                }
                break;
            case 6: // MainPage.xaml line 288
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element6 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element6).Click += this.RotateCWButton_Click;
                }
                break;
            case 7: // MainPage.xaml line 290
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element7 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element7).Click += this.RotateCCWButton_Click;
                }
                break;
            case 8: // MainPage.xaml line 241
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element8 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element8).Click += this.OpenFile_ButtonClick;
                }
                break;
            case 9: // MainPage.xaml line 252
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element9 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element9).Click += this.Exit_Click;
                }
                break;
            case 10: // MainPage.xaml line 22
                {
                    this.Gripper = (global::Windows.UI.Xaml.Controls.ColumnDefinition)(target);
                }
                break;
            case 11: // MainPage.xaml line 209
                {
                    this.mainCanvasScrollViewer = (global::Windows.UI.Xaml.Controls.ScrollViewer)(target);
                }
                break;
            case 12: // MainPage.xaml line 214
                {
                    this.mainCanvasViewBox = (global::Windows.UI.Xaml.Controls.Viewbox)(target);
                }
                break;
            case 13: // MainPage.xaml line 215
                {
                    this.mainCanvasGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 14: // MainPage.xaml line 223
                {
                    global::Windows.UI.Xaml.Controls.InkCanvas element14 = (global::Windows.UI.Xaml.Controls.InkCanvas)(target);
                    ((global::Windows.UI.Xaml.Controls.InkCanvas)element14).PointerReleased += this.InkCanvas_PointerReleased;
                    ((global::Windows.UI.Xaml.Controls.InkCanvas)element14).PointerPressed += this.InkCanvas_PointerPressed;
                }
                break;
            case 15: // MainPage.xaml line 224
                {
                    this.mainCanvasImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 16: // MainPage.xaml line 161
                {
                    this.toolsGridView = (global::Windows.UI.Xaml.Controls.GridView)(target);
                }
                break;
            case 18: // MainPage.xaml line 143
                {
                    this.backColorRectangle = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 19: // MainPage.xaml line 144
                {
                    this.frontColorRectangle = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 20: // MainPage.xaml line 145
                {
                    global::Windows.UI.Xaml.Controls.Button element20 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element20).Click += this.SwapColorButton_Click;
                }
                break;
            case 21: // MainPage.xaml line 133
                {
                    this.mainColorPicker = (global::Windows.UI.Xaml.Controls.ColorPicker)(target);
                    ((global::Windows.UI.Xaml.Controls.ColorPicker)this.mainColorPicker).ColorChanged += this.mainColorPicker_ColorChanged;
                }
                break;
            case 22: // MainPage.xaml line 44
                {
                    this.previewCanvas = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 23: // MainPage.xaml line 45
                {
                    this.previewImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

