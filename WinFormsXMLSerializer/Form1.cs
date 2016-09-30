using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Dynamic;

namespace WinFormsXMLSerializer
{
    public partial class Form1 : Form
    {
        const string XML_FILE = "bob.xml";

        private List<Movie> listMovie = new List<Movie>();
        private XMLSerializer xml = null;

        public Form1()
        {
            InitializeComponent();
            InitList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            xml = new XMLSerializer(XML_FILE);

            if (!File.Exists(xml.XMLPath))
                xml.SaveData(listMovie);

            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            xml.SaveData(listMovie);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        { 
            listMovie.Clear();
            listMovie = xml.LoadData<List<Movie>>();

            this.dgvMovies.DataSource = listMovie;
            this.dgvMovies.Columns["Id"].Visible = false;
        }

        private void InitList() 
        {
            listMovie.Add(new Movie(1, "Star Trek", "Find mysterious planets..."));
            listMovie.Add(new Movie(2, "Alien", "Kill alien..."));
            listMovie.Add(new Movie(3, "Star Wars", "Destroy evil empire..."));
            listMovie.Add(new Movie(4, "Avengers", "Save world..."));
            listMovie.Add(new Movie(5, "Ghostbusters", "Remove creepy ghosts..."));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            xml.SaveData(listMovie);
        }
    }
}
