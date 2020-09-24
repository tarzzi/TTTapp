using TTTapp;

namespace TTT.BL
{
    public class Item
    {
        public Item()
        {
        }

        public Item(string name, double price, string category, int inStock, string url, int id)
        {
            Name = name;
            Price = price;
            Category = category;
            InStock = inStock;
            Url = url;
            ID = id;
        }

        public Item(string name, double price, string category, int amount, int id)
        {
            Name = name;
            Price = price;
            Category = category;
            Amount = amount;
            ID = id;
        }

        [MainWindow.ColumnName("Tuote")]
        public string Name { get; set; }

        [MainWindow.ColumnName("Hinta €")]
        public double Price { get; set; }

        [MainWindow.ColumnName("Osasto")]
        public string Category { get; set; }

        [MainWindow.ColumnName("Jäljellä")]
        public int InStock { get; set; }

        public string Url { get; set; }

        [MainWindow.ColumnName("Kpl")]
        public int Amount { get; set; }

        public int ID { get; set; }
    }
}