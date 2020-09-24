using System.Windows;

namespace TTTapp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Purchase : Window
    {
        public Purchase()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}