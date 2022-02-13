using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T5SDP
{
    public partial class LineChart : Form
    {
        public LineChart()
        {
            InitializeComponent();
            FactorialHilos.FactorialHilos.MainRun();
            runChart.Titles["Details"].Text = "Número de secuencias/hilos en el programa:" + FactorialHilos.FactorialHilos.numHilos + 
                "\nNúmero Factorial a Calcular: " + FactorialHilos.FactorialHilos.valorAsignado;
            dataGet();
            infoLabel = dataPrint(infoLabel);
        }

        public void dataGet() {
            for (int i = 0; i < FactorialHilos.FactorialHilos.numHilos; i++) {
                runChart.Series["Secuencial"].Points.AddXY((i + 1), FactorialHilos.FactorialHilos.tiemposEjecucion[0, i]);
                runChart.Series["Hilos"].Points.AddXY((i+1), FactorialHilos.FactorialHilos.tiemposEjecucion[1,i]);
            }
        }

        public Label dataPrint(Label lbl) {
            String letrero = "Tiempo de ejecución secuencial: \n";
            for (int i = 0; i < FactorialHilos.FactorialHilos.numHilos; i++) { 
                letrero += "Secuencia [" + (i+1) + "]: " + FactorialHilos.FactorialHilos.tiemposEjecucion[0, i] + " ns.\n";
            }
            letrero += "\nTiempo de ejecución hilos: \n";
            for (int i = 0; i < FactorialHilos.FactorialHilos.numHilos; i++)
            {
                letrero += "Hilo [" + (i + 1) + "]: " + FactorialHilos.FactorialHilos.tiemposEjecucion[1, i] + " ns.\n";
            }
            lbl.Text = letrero;
            return lbl;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void runChart_Click(object sender, EventArgs e)
        {

        }
    }
}
