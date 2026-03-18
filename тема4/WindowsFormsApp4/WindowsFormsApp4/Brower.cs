using System;
using System.IO;

namespace WindowsFormsApp4
{
    public class Brower
    {
        private string name;  
        private int vote;    

        public Brower(string name, int vote) 
        {
            this.name = name;
            this.vote = vote; 
        }

        public string Name { get { return name; } }
        public int Vote
        {
            get { return vote; }
            set { vote = value; }
        }
    }
}