using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Threading;

namespace T2SDP.FactorialHilos
{
    public class FactorialHilos
    {       
        static Random rd = new Random();
        int valorFinal;
        BigInteger valorFactorial;
        public static int numHilos = rd.Next(4,10);
        static int valorAsignado = rd.Next(1250, 1500) * numHilos;
        static long[,] tiemposEjecucion = new long[2,numHilos];
        static Stopwatch[] stArray = new Stopwatch[numHilos];


        public FactorialHilos(int valorFinal, BigInteger valorFactorial) {
            this.valorFinal = valorFinal;
            this.valorFactorial = valorFactorial;
        }
        public static FactorialHilos fh;

        public static void Letrero(int valorAsignado) {
            Console.WriteLine("________________________________________________________________________________________________________________________________");
            Console.WriteLine("\n Guillermo Adrián Alonso Arámbula MAT: 1812367 Hora: V5 (1,3,5)");
            Console.WriteLine(" Tarea 03 - Programa que calcula e IMPRIME un número factorial mayor a 5000 y mide tiempos de ejecución... (Secuencial e Hilos)");
            Console.WriteLine("\n El valor a sacar su factorial es: " + valorAsignado);
            Console.WriteLine("________________________________________________________________________________________________________________________________");
        }

        public static BigInteger calculoFactorial(BigInteger valorFactorial, int valorActual, int valorFinal)
        {
            int i;
            for (i = valorActual; i < valorFinal; i++)
            {
                valorFactorial *= (i + 1);
            }
            return valorFactorial;
        }

        static void run(object i) {
            stArray[(int)i] = Stopwatch.StartNew();
            fh.valorFactorial = calculoFactorial(fh.valorFactorial, fh.valorFinal * ((int) i / numHilos), fh.valorFinal * (((int)i + 1) / numHilos));
            tiemposEjecucion[1, (int)i] = stArray[(int)i].ElapsedTicks * 100;
            stArray[(int)i].Stop();
        }

        public static void mainThreads() {
            fh = new FactorialHilos(valorAsignado, 1);
            Thread[] hilos = new Thread[numHilos];
            for (int i = 0; i < hilos.Length; i++)
            {
                hilos[i] = new Thread(new ParameterizedThreadStart(run));
                hilos[i].Start(i);
            }
        }

        public static void Main(string[] args)
        { 
                Letrero(valorAsignado);
                Stopwatch st = new Stopwatch();

                fh = new FactorialHilos(valorAsignado, 1);

            if (fh.valorFinal > 5000)
            {
                //SECCIÓN SECUENCIAL-ITERATIVA
                st = Stopwatch.StartNew();
                for (int i = 0; i < numHilos; i++)
                {
                    for (int n = fh.valorFinal * (i / numHilos); n < fh.valorFinal * ((i + 1) / numHilos); n++)
                    {
                        fh.valorFactorial *= (n + 1);
                    }
                    tiemposEjecucion[0, i] = st.ElapsedTicks * 100;
                    Console.WriteLine("\nTiempo total de ejecución secuencial [" + (i + 1) + "]  : " + st.ElapsedTicks * 100 + " ns.");
                }
                st.Stop();

                //SECCIÓN DE HILOS               
                mainThreads();
                for (int i = 0; i < numHilos; i++)
                {
                    Console.WriteLine("\nTiempo total de ejecución de hilo [" + ((int)i + 1) + "]: " + tiemposEjecucion[1, (int)i] + " ns.");
                }
            }
            else
            {
                st = Stopwatch.StartNew();
                Console.WriteLine("El número factorial a calcular no es mayor a 5000");
                st.Stop();
                Console.WriteLine("Tiempo de ejecución secuencial: " + st.ElapsedTicks * 100 + " ns.\n");
            }
        }
    }
}
