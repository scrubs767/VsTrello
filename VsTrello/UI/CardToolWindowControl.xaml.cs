//------------------------------------------------------------------------------
// <copyright file="CardToolWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace VsTrello.UI
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for CardToolWindowControl.
    /// </summary>
    public partial class CardToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardToolWindowControl"/> class.
        /// </summary>
        public CardToolWindowControl()
        {
            this.InitializeComponent();
            CommandBindings.Add(new CommandBinding(NavigationCommands.GoToPage, (sender, e) => Process.Start((string)e.Parameter)));
        }
    }
}