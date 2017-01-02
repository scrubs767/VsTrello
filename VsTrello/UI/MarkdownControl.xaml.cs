using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VsTrello.UI
{
    /// <summary>
    /// Interaction logic for MarkdownControl.xaml
    /// </summary>
    public partial class MarkdownControl : UserControl
    {
        public MarkdownControl()
        {
            InitializeComponent();
        }

        public FlowDocument Document
        {
            get { return (FlowDocument)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof(FlowDocument), typeof(MarkdownControl), new PropertyMetadata(OnDocumentChanged));

        private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MarkdownControl control = (MarkdownControl)d;
            if (e.NewValue == null)
                control.RTB.Document = new FlowDocument(); //Document is not amused by null :)

            control.RTB.Document = control.Document;
        }
    }
}
