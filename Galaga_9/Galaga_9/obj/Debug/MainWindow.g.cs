﻿#pragma checksum "..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "69FAB218D809D2CA50622227F8EF1C73"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3603
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Galaga_9;
using Microsoft.Expression.Interactivity.Core;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Galaga_9 {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 7 "..\..\MainWindow.xaml"
        internal Galaga_9.MainWindow Window;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\MainWindow.xaml"
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\MainWindow.xaml"
        internal Galaga_9.TowersControl towersControl;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\MainWindow.xaml"
        internal System.Windows.Controls.Grid UI;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\MainWindow.xaml"
        internal System.Windows.Controls.Grid Progress;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\MainWindow.xaml"
        internal System.Windows.Controls.Grid Gold;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\MainWindow.xaml"
        internal System.Windows.Controls.Grid talk;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\MainWindow.xaml"
        internal System.Windows.Shapes.Path mc_talkBG;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\MainWindow.xaml"
        internal System.Windows.Controls.TextBlock t_good;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\MainWindow.xaml"
        internal Galaga_9.explosion_effControl explosion_eff;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\MainWindow.xaml"
        internal Galaga_9.Tower_overControl UI_Tower_menu;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Galaga_9;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Window = ((Galaga_9.MainWindow)(target));
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.towersControl = ((Galaga_9.TowersControl)(target));
            return;
            case 4:
            this.UI = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.Progress = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.Gold = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.talk = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.mc_talkBG = ((System.Windows.Shapes.Path)(target));
            return;
            case 9:
            this.t_good = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.explosion_eff = ((Galaga_9.explosion_effControl)(target));
            return;
            case 11:
            this.UI_Tower_menu = ((Galaga_9.Tower_overControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

