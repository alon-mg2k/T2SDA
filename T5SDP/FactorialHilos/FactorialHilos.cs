using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T5SDP.FactorialHilos
{
    public class FactorialHilos
    {       
        static Random rd = new Random();
        int valorFinal;
        BigInteger valorFactorial;
        public static int numHilos = rd.Next(4,20);
        public static int valorAsignado = rd.Next(1000, 1250) * numHilos;
        static int inicio, final;
        public static long[,] tiemposEjecucion = new long[2,numHilos];
        static BigInteger[,] facArray = new BigInteger[2, numHilos];
        static Stopwatch[] stArray = new Stopwatch[numHilos];

        public FactorialHilos(int valorFinal, BigInteger valorFactorial) {
            this.valorFinal = valorFinal;
            this.valorFactorial = valorFactorial;
        }
        public static FactorialHilos fh  = new FactorialHilos(valorAsignado, 1);

        public static void Letrero(int valorAsignado) {
            Console.WriteLine("________________________________________________________________________________________________________________________________");
            Console.WriteLine("\n Guillermo Adrián Alonso Arámbula MAT: 1812367 Hora: V5 (1,3,5)");
            Console.WriteLine(" Tarea 03 - Programa que calcula e IMPRIME un número factorial mayor a 5000 y mide tiempos de ejecución... (Secuencial e Hilos)");
            Console.WriteLine("\n El valor a sacar su factorial es: " + valorAsignado);
            Console.WriteLine("_________________________________________________________________________________________________________________________________");
        }

        public static BigInteger calculoFactorial(BigInteger valorFactorial, int valorActual, int valorFinal)
        {
            for (int i = valorActual; i < valorFinal; i++)
            {
                valorFactorial *= (i + 1);
            }
            return valorFactorial;
        }

        public static void mainThreads() {
            Parallel.For(0, numHilos, i => {
                stArray[(int)i] = new Stopwatch();
                stArray[(int)i].Start();

                int inicio = (valorAsignado / numHilos) * ((int)i);
                int final = (valorAsignado / numHilos) * ((int)i + 1);

                if ((int)i == 0)
                {
                    BigInteger factorial = 1;
                    fh.valorFactorial = calculoFactorial(factorial, inicio, final);
                }
                else
                {
                    BigInteger factorial = facArray[0, (int)i - 1];
                    fh.valorFactorial = calculoFactorial(factorial, inicio, final);
                }

                facArray[1, (int)i] = fh.valorFactorial;

                stArray[(int)i].Stop();
                tiemposEjecucion[1, (int)i] = stArray[(int)i].ElapsedTicks * 100;
            });
        }

        static void ConsoleLog() {
            for (int i = 0; i < numHilos; i++) {
                Console.WriteLine("\nSecuencia Factorial [" + i + "] Tiempo: " + tiemposEjecucion[0, i] + " ns.");
                Console.WriteLine("Secuencia Factorial [" + i + "] Resultado: " + facArray[0, i] + "\n");
            }
            for (int i = 0; i < numHilos; i++)
            {
                Console.WriteLine("\nHilo Factorial [" + i + "] Tiempo: " + tiemposEjecucion[1, i] + " ns.");
                Console.WriteLine("Hilo Factorial [" + i + "] Resultado: " + facArray[1, i] + "\n");
            }
        }

        public static void MainRun()
        { 
            Letrero(valorAsignado);
            Stopwatch st = new Stopwatch();
            st.Start();

            if (fh.valorFinal > 5000)
            {
                //SECCIÓN SECUENCIAL-ITERATIVA

                for (int i = 0; i < numHilos; i++)
                {
                    inicio = (fh.valorFinal / numHilos) * i;
                    final = (fh.valorFinal / numHilos) * (i + 1);

                    fh.valorFactorial = calculoFactorial(fh.valorFactorial, inicio, final);

                    facArray[0, i] = fh.valorFactorial;
                    tiemposEjecucion[0, i] = st.ElapsedTicks * 100;
                }
                st.Stop();

                //SECCIÓN DE HILOS
                fh = new FactorialHilos(valorAsignado, 1);
                mainThreads();

                //SECCIÓN DE IMPRESIÓN EN CONSOLA
                ConsoleLog();
            }
            else
            {
                Console.WriteLine("El número factorial a calcular no es mayor a 5000");
                MessageBox.Show("El número factorial [" + valorAsignado + "] no se puede calcular","Error");
                st.Stop();
                Console.WriteLine("Tiempo de ejecución secuencial: " + st.ElapsedTicks * 100 + " ns.\n");
            }
        }
    }
}
