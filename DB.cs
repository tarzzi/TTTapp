using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using TTT.BL;

namespace TTT.DB
{
    internal class DBConnect
    {
        private MySqlConnection connection;

        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            //Hakee mysql connection stringin
            string connect = System.Configuration.ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
            connection = new MySqlConnection(connect);
        }

        private bool OpenConnection()
        {
            try
            {
                //Avaa yhteyden
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //0: Ei yhteyttä serveriin.
                //1045: Väärä käyttäjänimi tai salasana.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Yhteyttä ei saatu muodostettua, ota yhteys asiakaspalveluun. Virhe 0");
                        break;

                    case 1045:
                        MessageBox.Show("Yhteyttä ei saatu muodostettua, ota yhteys asiakaspalveluun. Virhe 1045");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                //Sulkee yhteyden
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public List<Pet> GetPets()
        {
            string query = "SELECT name, age, colour, gender, url, breed FROM PET";
            //Create a list to store the result
            List<Pet> pets = new List<Pet>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    Pet pet = new Pet();
                    pet.Name = dataReader.GetString(0);
                    pet.Age = dataReader.GetInt32(1);
                    pet.Colour = dataReader.GetString(2);
                    pet.Gender = dataReader.GetString(3);
                    pet.Url = dataReader.GetString(4);
                    pet.Breed = dataReader.GetString(5);
                    pets.Add(pet);
                }
                dataReader.Close();
                this.CloseConnection();
                return pets;
            }
            else
            {
                MessageBox.Show("Please connect to LabraNet to get list");
                return pets;
            }
        }

        public List<Item> GetItems()
        {
            string query = "SELECT itemName, price, category, stockCount, url, itemID FROM ITEM where stockCount > 10";
            List<Item> items = new List<Item>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Item item = new Item();
                    item.Name = dataReader.GetString(0);
                    item.Price = Math.Round(dataReader.GetDouble(1), 2);
                    item.Category = dataReader.GetString(2);
                    item.InStock = dataReader.GetInt32(3);
                    item.Url = dataReader.GetString(4);
                    item.ID = dataReader.GetInt32(5);
                    items.Add(item);
                }

                dataReader.Close();
                this.CloseConnection();
                return items;
            }
            else
            {
                MessageBox.Show("Please connect to LabraNet to get list");
                return items;
            }
        }

        public List<Item> GetDiscountItems()
        {
            string query = "SELECT itemName, price, category, stockCount, url, itemID FROM ITEM where stockCount < 10";
            List<Item> items = new List<Item>();
            double discount = 0.85f;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Item item = new Item();
                    item.Name = dataReader.GetString(0);
                    item.Price = Math.Round(dataReader.GetDouble(1) * discount, 2);
                    item.Category = dataReader.GetString(2);
                    item.InStock = dataReader.GetInt32(3);
                    item.Url = dataReader.GetString(4);
                    item.ID = dataReader.GetInt32(5);
                    items.Add(item);
                }
                dataReader.Close();
                this.CloseConnection();
                return items;
            }
            else
            {
                MessageBox.Show("Please connect to LabraNet to get list");
                return items;
            }
        }

        public bool InsertPurchase(User user)
        {
            bool success = false;
            bool cliinfo = false;
            bool purch = false;
            bool hasitm = false;
            bool inv = false;
            //Syötä asiakkaan tiedot

            long clientId = 0;
            if (this.OpenConnection() == true)
            {
                string insClient = $"INSERT INTO CLIENT (fname, lname, address, city, postal, phone, email) values ('{user.Fname}', '{user.Lname}', '{user.Address}', '{user.City}', '{user.Postal}', '{user.Phone}', '{user.Email}') ";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(insClient, connection);
                    cmd.ExecuteNonQuery();
                    clientId = cmd.LastInsertedId; //Hae syötetyn asiakkaan asiakas/tilausnumero
                    this.CloseConnection();
                    cliinfo = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Purchase failed, error: " + ex.Message);
                    cliinfo = false;
                }
            }
            else
            {
                MessageBox.Show("Please connect to LabraNet to get list");
            }

            //Syötä asiakkaan id tietokantaan uuden oston kohdalle, ja hae samalla tapahtuvan oston numero
            long ostoId = 0;
            string purchase = $"INSERT INTO PURCHASE (Client_clientID) values ('{clientId}') ";
            if (this.OpenConnection() == true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(purchase, connection);
                    cmd.ExecuteNonQuery();
                    ostoId = cmd.LastInsertedId; //hae viim autoincr
                    this.CloseConnection();
                    purch = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Purchase failed, error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please connect to LabraNet to get list");
                purch = false;
            }

            //Syötä item:ien numerot ostos-id:n alle
            //Syötä ostoskorin tiedot
            foreach (Item item in user.Cart)
            {
                string insCartItem = $"INSERT INTO PURCHASE_has_ITEM (PURCHASE_purchaseID, ITEM_itemID) values ({ostoId}, {item.ID}); ";
                if (this.OpenConnection() == true)
                {
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand(insCartItem, connection);
                        cmd.ExecuteNonQuery();
                        this.CloseConnection();
                        hasitm = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Purchase failed, error: " + ex.Message);
                    }
                    hasitm = true;
                }
                else
                {
                    MessageBox.Show("Please connect to LabraNet to get list");
                    success = false;
                }
            }

            //Päivitä db inventory määrä
            foreach (Item item in user.Cart)
            {
                string updateStock = $"UPDATE ITEM SET stockCount = stockCount - {item.Amount} where itemID = {item.ID}; ";
                if (this.OpenConnection() == true)
                {
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand(updateStock, connection);
                        cmd.ExecuteNonQuery();
                        this.CloseConnection();
                        inv = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Purchase failed, error: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Please connect to LabraNet to get list");
                    inv = false;
                }
            }
            //Jos kaikki on onnistunutta palauttaa true, jos virhetilanne, palauttaa false
            if (hasitm && cliinfo && purch && inv)
            {
                success = true;
                return success;
            }
            else
            {
                //Mikäli virhe jossakin syötössä, kertoo sen palauttaessa
                if (inv == false)
                { MessageBox.Show("Ongelma varaston päivityksessä, ota yhteys liikkeeseen!"); }
                if (cliinfo == false)
                { MessageBox.Show("Ongelma asiakastietojen syöttämisessä, ota yhteys liikkeeseen!"); }
                if (hasitm == false)
                { MessageBox.Show("Ongelma tuotteiden lisäyksessä tietokantaan, ota yhteys liikkeeseen!"); }
                if (purch == false)
                { MessageBox.Show("Ongelma ostotapahtuman käsittelyssä, ota yhteys liikkeeseen!"); }
                return success;
            }
        }
    }
}