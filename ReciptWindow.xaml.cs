using System.Collections.Generic;
using System.Windows;
using TTT.BL;

namespace TTTapp.RW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ReciptWindow : Window
    {
        //User _user;
        private User kayttaja = new User();

        public List<Item> Items { get; set; }
        public string Fullname { get; set; }
        public string FullAddress { get; set; }

        public List<Item> GetItems()
        {
            return Items;
        }

        /// <summary>
        /// Recipt window to show order recipt
        /// </summary>
        public ReciptWindow(User usr)
        {
            kayttaja = usr;
            //_user = usr;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (chbAgree1.IsChecked == true && chbAgree2.IsChecked == true && chbAgree3.IsChecked == true)
            {
                //Hyväksytä ostos
                this.DialogResult = true;
                this.Close();
                Purchase purchase = new Purchase();
                purchase.Show();
            }
            else
            {
                MessageBox.Show("Sinun tulee hyväksyä ehdot, tehdäksesi tilauksen", "Hyväksyntä puuttuu");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgOrder.ItemsSource = GetItems();
            Fullname = kayttaja.Fname + " " + kayttaja.Lname;
            txbName.Text = Fullname;
            FullAddress = kayttaja.Address + " " + kayttaja.Postal + " " + kayttaja.City;
            txbAddr.Text = FullAddress;
            txbEmail.Text = kayttaja.Email;
            txbPhone.Text = kayttaja.Phone;
            //imgContact.Source = new BitmapImage(new Uri("pack://application:,,,/TTTapp;component/info.jpg", UriKind.Absolute));
        }
    }
}