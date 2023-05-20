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

namespace NodedUI
{
    /// <summary>
    /// Interaction logic for StartModule.xaml
    /// </summary>
    public partial class StartModule : UserControl
    {
        // Using a DependencyProperty as the backing store for NodeName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodeNameProperty =
            DependencyProperty.Register("NodeName", typeof(string), typeof(StartModule), new PropertyMetadata(String.Empty));

        public string NodeName
        {
            get { return (string)GetValue(NodeNameProperty); }
            set { SetValue(NodeNameProperty, value); }
        }

        public StartModule()
        {
            InitializeComponent();
        }

        
    }
}
