using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TTT.BL;
using TTT.DB;
using TTTapp.RW;

namespace TTTapp
{
    /// <summary>
    /// Programmer: Tarmo Urrio
    /// Programmed: Finished 26.4.2020
    /// For: TTOS0300 - User interfaces programming
    /// </summary>
    public partial class MainWindow : Window
    {
        private User user = new User();
        private List<Item> items = new List<Item>();
        private SolidColorBrush green = Brushes.Green;
        private SolidColorBrush red = Brushes.PaleVioletRed;
        private SolidColorBrush white = Brushes.White;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void dgDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object obj = dgDisplay.SelectedItem;
            ShowProductInfo(obj);
        }

        private void dgViewCart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txbSelectedItem.Text = "";
            txbSelectedPrice.Text = "";
        }

        private void ShowProductInfo(Object obj)
        {
            txbAction.Background = white;
            txbAction.Text = "";
            if (obj is Pet)  // Jos valittuna on lemmikki
            {
                Pet pet = (Pet)obj;

                try
                {
                    imgDisplay.DataContext = pet;
                }
                catch (Exception)
                {
                    imgDisplay.Source = new BitmapImage(new Uri($"pack://application:,,,/TTTapp;component/missing.jpg", UriKind.Absolute));
                }
            }

            if (obj is Item) // Jos valittuna on tuote
            {
                Item item = (Item)obj;
                try
                {
                    imgDisplay.DataContext = item;
                }
                catch (Exception)
                {
                    imgDisplay.Source = new BitmapImage(new Uri($"pack://application:,,,/TTTapp;component/missing.jpg", UriKind.Absolute));
                }
                txbSelectedItem.DataContext = item;
                txbSelectedPrice.DataContext = item;
            }
        }

        private void btnItemAll_Click(object sender, RoutedEventArgs e)
        {
            ViewAllProducts();
            lblInfo.Visibility = Visibility.Hidden;
        }

        private void btnItemDiscount_Click(object sender, RoutedEventArgs e)
        {
            lblInfo.Visibility = Visibility.Hidden;
            SetBtnVisibility(true);
            try
            {
                DBConnect db = new DBConnect();
                dgDisplay.ItemsSource = db.GetDiscountItems();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnGetAPet_Click(object sender, RoutedEventArgs e)
        {
            lblInfo.Visibility = Visibility.Visible;
            dgViewCart.Visibility = Visibility.Hidden;
            txbCart.Visibility = Visibility.Hidden;
            imgDisplay.Visibility = Visibility.Visible;
            SetBtnVisibility(false);
            DBConnect db = new DBConnect();
            try
            {
                dgDisplay.ItemsSource = db.GetPets();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tapahtui virhe, yritä uudelleen tai ota yhteys asiakaspalveluun" + ex.Message);
            }
        }

        private void btnContact_Click(object sender, RoutedEventArgs e)
        {
            ViewStoreInfo();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            imgContact.Visibility = Visibility.Hidden;
            wrpLogin.Visibility = Visibility.Visible;
        }

        private void btnInsertCred_Click(object sender, RoutedEventArgs e)
        {
            //Syötetyn tekstin tarkistus, onko tyhjiä kohtia kriittisissä henkilötiedoissa
            if (string.IsNullOrWhiteSpace(txtFname.Text) || string.IsNullOrWhiteSpace(txtLname.Text) || string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtCity.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Syötä vähintään Nimesi, Osoitteesi ja Sähköpostiosoitteesi jatkaaksesi tilausta");
            }
            else
            {
                user.Fname = txtFname.Text;
                user.Lname = txtLname.Text;
                user.Address = txtAddress.Text;
                user.City = txtCity.Text;
                user.Postal = txtPostal.Text;
                user.Email = txtEmail.Text;
                user.Phone = txtPhone.Text;
                wrpLogin.Visibility = Visibility.Hidden;
                ViewStoreInfo();
                btnLogin.Content = $"Tervetuloa {user.Fname} {user.Lname}!";
            }
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            Object obj = null; // Alemmat if lauseet tarkastaa kummasta dg:stä lisätään tuotetta
            if (dgDisplay.SelectedItem != null && dgDisplay.SelectedCells.Count > 0)
            {
                obj = dgDisplay.SelectedItem;
            }
            if (dgViewCart.SelectedItem != null && dgViewCart.SelectedCells.Count > 0)
            {
                obj = dgViewCart.SelectedItem;
            }
            if (int.TryParse(txtAmount.Text, out int a))
            {
                if (a > 0)
                {
                    if (obj == null)
                    {
                        MessageBox.Show("Valitse ensin lisättävä tuote!");
                    }
                    else
                    {
                        try
                        {
                            Item itemSelected = (Item)obj;
                            Item itemToCart = new Item(itemSelected.Name, itemSelected.Price, itemSelected.Category, a, itemSelected.ID);
                            items.Add(itemToCart);
                            user.Cart = items;
                            RefreshCart();
                            CountCart();
                            txbAction.Text = $"Tuote {itemToCart.Name} lisätty!";
                            txbAction.Background = green;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Virhe lisäyksessä!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Syötä positiivinen määrä");
                }
            }
            else
            {
                MessageBox.Show("Antamasi luku ei ole numero");
            }
        }

        private void RefreshCart()
        {
            dgViewCart.DataContext = null;
            dgViewCart.DataContext = user;
        }

        private void btnViewCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                user.Cart.Count();
            }
            catch (Exception)
            {
                MessageBox.Show("Ostoskorisi on tyhjä! Lisää ensin tuotteita.");
            }
            try
            {
                dgViewCart.DataContext = user;
                CountCart();
                imgDisplay.Visibility = Visibility.Hidden;
                dgViewCart.Visibility = Visibility.Visible;
                txbCart.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                MessageBox.Show("Ostoskorisi on tyhjä! Lisää ensin tuotteita.");
            }
        }

        private void btnRemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if (dgViewCart.SelectedItem == null)
            {
                MessageBox.Show("Valitse ensin poistettava tuote");
            }
            else
            {
                try
                {
                    Object obj2 = dgViewCart.SelectedItem;
                    Item itemToRemove = (Item)obj2;
                    user.Cart.Remove(itemToRemove);
                    txbAction.Text = $"Tuote {itemToRemove.Name} poistettu!";
                    txbAction.Background = red;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Virhe: " + ex);
                }
                finally
                {
                    RefreshCart();
                    CountCart();
                }
            }
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            //Ostotapahtuma
            int? c = user.Cart?.Count(); //? tarkistaa onko kori tyhjä / null-arvo
            if (c == 0 || c == null)
            {
                MessageBox.Show("Ostoskori on tyhjä!", "Valitse ensin tuotteita", MessageBoxButton.OK);
            }
            else if (string.IsNullOrWhiteSpace(user.Fname) || string.IsNullOrWhiteSpace(user.Lname) || string.IsNullOrWhiteSpace(user.Address) || string.IsNullOrWhiteSpace(user.City) || string.IsNullOrWhiteSpace(user.Email))
            {
                MessageBox.Show("Syötä vähintään Nimesi, Osoitteesi ja Sähköpostisi tehdäksesi tilauksen");
            }
            else
            {
                ReciptWindow recipt = new ReciptWindow(user);
                recipt.Items = user.Cart;
                Nullable<bool> purchaseCompleted = recipt.ShowDialog(); //Hakee true/false arvon kuittiosiosta

                if (purchaseCompleted == true)
                {
                    DBConnect db = new DBConnect();
                    db.InsertPurchase(user);
                    user.Cart = null;
                    items = null;
                    RefreshCart();
                    CountCart();
                    dgViewCart.DataContext = null;
                }
                else if (purchaseCompleted == null)
                {
                    MessageBox.Show("Ostotapahtuma peruutettu");
                }
                else
                {
                    MessageBox.Show("Ostotapahtuma peruutettu");
                }
            }
        }

        private void btnShowPic_Click(object sender, RoutedEventArgs e)
        {
            //Näyttää tuotekuvan
            dgViewCart.Visibility = Visibility.Hidden;
            txbCart.Visibility = Visibility.Hidden;
            imgDisplay.Visibility = Visibility.Visible;
        }

        private void CountCart()
        {
            int x = 0;
            double total = 0;
            try
            {
                x = user.Cart.Count();
            }
            catch (Exception)
            {
                x = 0;
            }
            int z = 0;
            if (x != 0)
            {
                foreach (Item i in user.Cart)
                {
                    z += i.Amount;
                }
                x *= z;
                foreach (Item y in user.Cart)
                {
                    total += y.Price * y.Amount;
                }
            }
            txbCartItems.Text = z.ToString() + " kpl"; // Laskee korin tuotteiden määrän.
            txbCartTotal.Text = total.ToString() + "€"; // Laskee korin sisällön hinnan
        }

        private void ViewAllProducts()
        {
            SetBtnVisibility(true);
            try
            {
                DBConnect db = new DBConnect();
                dgDisplay.ItemsSource = db.GetItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Virhe hakiessa tietoja" + ex.Message);
            }
        }

        private void SetBtnVisibility(bool show)
        {
            // Piilottaa/Näyttää nappuloita kun niiden painaminen on sallittu
            if (show)
            {
                btnAddToCart.IsEnabled = true;
                btnRemoveFromCart.IsEnabled = true;
                btnViewCart.IsEnabled = true;
                btnBuy.IsEnabled = true;
                btnShowPic.IsEnabled = true;
                txtAmount.IsEnabled = true;
                txbSelectedItem.Visibility = Visibility.Visible;
                txbSelectedPrice.Visibility = Visibility.Visible;
                btnUp.Visibility = Visibility.Visible;
                btnDown.Visibility = Visibility.Visible;
            }
            else
            {
                btnAddToCart.IsEnabled = false;
                btnRemoveFromCart.IsEnabled = false;
                btnViewCart.IsEnabled = false;
                btnBuy.IsEnabled = false;
                btnShowPic.IsEnabled = false;
                txtAmount.IsEnabled = false;
                txbSelectedItem.Visibility = Visibility.Hidden;
                txbSelectedPrice.Visibility = Visibility.Hidden;
                btnUp.Visibility = Visibility.Hidden;
                btnDown.Visibility = Visibility.Hidden;
            }
        }

        public class ColumnNameAttribute : System.Attribute
        {
            //Taustaluokka datagridien otsikkojen käsittelyyn.
            public ColumnNameAttribute(string Name) { this.Name = Name; }

            public string Name { get; set; }
        }

        private void dgDisplay_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor; //Kuvaa ominaisuutta
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute; //Määrittää propertyn nimen otsikkoatribuutiksi
            if (att != null)
            {
                e.Column.Header = att.Name;
            }
            if (e.Column.Header.ToString() == "Tuote")
            {
                e.Column.Width = new DataGridLength(e.Column.ActualWidth + 250); //Muokkaa autogeneroitujen otsikkojen leveyttä
            }
            if (e.Column.Header.ToString() == "Hinta €")
            {
                e.Column.Width = new DataGridLength(e.Column.ActualWidth + 110);
            }
            if (e.Column.Header.ToString() == "Jäljellä")
            {
                e.Column.Width = new DataGridLength(e.Column.ActualWidth + 80);
            }
            if (e.Column.Header.ToString() == "Url")
            {
                e.Cancel = true;
            }
            if (e.Column.Header.ToString() == "Kpl")
            {
                e.Cancel = true;
            }
            if (e.Column.Header.ToString() == "ID")
            {
                e.Cancel = true;
            }
            if (e.Column.Header.ToString() == "Href")
            {
                e.Cancel = true;
            }
        }

        private void dgViewCart_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
            }
            if (e.Column.Header.ToString() == "Tuote")
            {
                e.Column.Width = new DataGridLength(e.Column.ActualWidth + 150);
            }
            if (e.Column.Header.ToString() == "Hinta €")
            {
                e.Column.Width = new DataGridLength(e.Column.ActualWidth + 110);
            }
            if (e.Column.Header.ToString() == "Jäljellä")
            {
                e.Cancel = true;
            }
            if (e.Column.Header.ToString() == "Url")
            {
                e.Cancel = true;
            }
            if (e.Column.Header.ToString() == "Kpl")
            {
                e.Column.Width = new DataGridLength(e.Column.ActualWidth + 80);
            }
            if (e.Column.Header.ToString() == "ID")
            {
                e.Cancel = true;
            }
        }

        private void ViewStoreInfo()
        {
            imgContact.Visibility = Visibility.Visible;
            imgContact.Source = new BitmapImage(new Uri("pack://application:,,,/TTTapp;component/info.jpg", UriKind.Absolute));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewAllProducts();
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtAmount.Text, out int x);
            x++;
            txtAmount.Text = x.ToString();
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtAmount.Text, out int x);
            if (x >= 1)
            {
                x--;
                txtAmount.Text = x.ToString();
            }
        }
    }
}