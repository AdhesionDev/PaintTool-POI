﻿#pragma checksum "C:\Users\ADT\Documents\GitHub\AdhesionTekPaintTool\AdhesionTekPaintTool\AdhesionTekPaintTool\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3D5D01439C5C153528B4FA7286799874A8955E12C139F30FB489FDE6B74FB266"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdhesionTekPaintTool
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
            case 2: // MainPage.xaml line 14
                {
                    this.toolsButtons = (global::Windows.UI.Xaml.Data.CollectionViewSource)(target);
                }
                break;
            case 3: // MainPage.xaml line 288
                {
                    global::Windows.UI.Xaml.Controls.Button element3 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element3).Click += this.Debug_Button_Click;
                }
                break;
            case 4: // MainPage.xaml line 279
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element4 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element4).Click += this.About_Button_Click;
                }
                break;
            case 5: // MainPage.xaml line 248
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element5 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element5).Click += this.Exit_Click;
                }
                break;
            case 6: // MainPage.xaml line 36
                {
                    this.Gripper = (global::Windows.UI.Xaml.Controls.ColumnDefinition)(target);
                }
                break;
            case 7: // MainPage.xaml line 142
                {
                    global::Windows.UI.Xaml.Controls.ScrollViewer element7 = (global::Windows.UI.Xaml.Controls.ScrollViewer)(target);
                    ((global::Windows.UI.Xaml.Controls.ScrollViewer)element7).ViewChanged += this.ScrollViewer_ViewChanged;
                }
                break;
            case 8: // MainPage.xaml line 220
                {
                    this.mainCanvas = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 9: // MainPage.xaml line 175
                {
                    this.toolsGridView = (global::Windows.UI.Xaml.Controls.GridView)(target);
                }
                break;
            case 11: // MainPage.xaml line 154
                {
                    this.colorPicker = (global::Windows.UI.Xaml.Controls.ColorPicker)(target);
                    ((global::Windows.UI.Xaml.Controls.ColorPicker)this.colorPicker).ColorChanged += this.colorPicker_ColorChanged;
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

