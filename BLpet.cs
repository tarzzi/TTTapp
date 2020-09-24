using TTTapp;

namespace TTT.BL
{
    internal class Pet
    {
        public Pet()
        {
        }

        public Pet(string name, string gender, int age, string colour, string breed, string url)
        {
            Name = name;
            Gender = gender;
            Age = age;
            Colour = colour;
            Breed = breed;
            Url = url;
        }

        [MainWindow.ColumnName("Nimi")]
        public string Name { get; set; }

        [MainWindow.ColumnName("Sukupuoli")]
        public string Gender { get; set; }

        [MainWindow.ColumnName("Ikä")]
        public int Age { get; set; }

        [MainWindow.ColumnName("Väri")]
        public string Colour { get; set; }

        [MainWindow.ColumnName("Rotu")]
        public string Breed { get; set; }

        [MainWindow.ColumnName("Href")]
        public string Url { get; set; }
    }
}