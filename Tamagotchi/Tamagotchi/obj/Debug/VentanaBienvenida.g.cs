﻿#pragma checksum "..\..\VentanaBienvenida.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "C628E73D59AF40E7FE7F95BB99220A5D8CBBD18E6C0EDA21F378B439900FA3C6"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

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
using System.Windows.Shell;
using Tamagotchi;


namespace Tamagotchi {
    
    
    /// <summary>
    /// VentanaBienvenida
    /// </summary>
    public partial class VentanaBienvenida : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\VentanaBienvenida.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblNombreTamagotchi;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\VentanaBienvenida.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbNombreTamagotchi;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\VentanaBienvenida.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btEmpezar;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\VentanaBienvenida.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar pbFuego;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\VentanaBienvenida.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar pbHielo;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Tamagotchi;component/ventanabienvenida.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\VentanaBienvenida.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.lblNombreTamagotchi = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.tbNombreTamagotchi = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.btEmpezar = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\VentanaBienvenida.xaml"
            this.btEmpezar.Click += new System.Windows.RoutedEventHandler(this.enviarNombre);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 32 "..\..\VentanaBienvenida.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.abandonar);
            
            #line default
            #line hidden
            return;
            case 5:
            this.pbFuego = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 6:
            this.pbHielo = ((System.Windows.Controls.ProgressBar)(target));
            
            #line 42 "..\..\VentanaBienvenida.xaml"
            this.pbHielo.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.cargarSegundoPB);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

