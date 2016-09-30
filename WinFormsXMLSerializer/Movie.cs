using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsXMLSerializer
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Plot { get; set; }

        public Movie(int id, string title, string plot) 
        {
            this.Id = id;
            this.Title = title;
            this.Plot = plot;
        }

        public Movie() 
        {
        }

        //private string title;
        //private string plot;

        //public string Title
        //{
        //    get { return title; }
        //    set { title = value; }
        //}

        //public string Plot
        //{
        //    get { return plot; }
        //    set { plot = value; }
        //}
    }
}
