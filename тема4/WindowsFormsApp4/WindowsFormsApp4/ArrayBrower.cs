using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting; 

namespace WindowsFormsApp4
{
    public class ArrayBrower
    {
         List<Brower> Z; 
        List<double> procent; 
         string nameDiagram; 

        public ArrayBrower()
        {
            Z = new List<Brower>(); 
            procent = new List<double>(); 
            nameDiagram = "Популярность браузеров"; 
        }

        public string NameDiagram { get { return nameDiagram; } set { nameDiagram = value; } }
        public int Count { get { return Z.Count; } }
        public List<double> Procent { get { return procent; } }

        public void Add(Brower B) { Z.Add(B); }
        public Brower this[int i]
        {
            get { return (i >= 0 && i < Z.Count) ? Z[i] : null; }
        }

        public void AllProcent()
        {
            if (Z.Count == 0) return;
            int S = 0;
            procent.Clear(); 
            foreach (var b in Z) S += b.Vote; 
            foreach (var b in Z) procent.Add(b.Vote / (double)S); 
        }

        public void SaveToFile(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine(nameDiagram); 
                sw.WriteLine(Z.Count);      
                foreach (var b in Z)
                {
                    sw.WriteLine(b.Name);   
                    sw.WriteLine(b.Vote);   
                }
            }
        }

        public void OpenFile(string fileName)
        {
            if (!File.Exists(fileName)) return;
            Z.Clear();
            using (StreamReader sr = new StreamReader(fileName))
            {
                nameDiagram = sr.ReadLine();
                int count = int.Parse(sr.ReadLine());
                for (int i = 0; i < count; i++)
                {
                    Z.Add(new Brower(sr.ReadLine(), int.Parse(sr.ReadLine())));
                }
            }
            AllProcent();
        }

        public void Diagram(Chart chart)
        {
            chart.Series.Clear(); 
           chart.Titles.Clear(); 
            chart.Titles.Add(nameDiagram); 
            chart.Titles[0].Font = new Font("Arial", 14); 

            Series s = new Series("Browsers") { ChartType = SeriesChartType.Pie };
            chart.ChartAreas[0].Area3DStyle.Enable3D = true; 

            foreach (var b in Z)
            {
                s.Points.AddXY(b.Name, b.Vote); 
            }
            chart.Series.Add(s);
        }
    }
}