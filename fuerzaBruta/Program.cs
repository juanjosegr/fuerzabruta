using System;
using System.IO;

class Program
{
    static void Main()
    {
        //Ruta del archivo a leer.
        string filePath = "ruta-del-archivo.txt";

        //comprobar si existe.
        if (File.Exists(filePath))
        {
            //Utilizar el bloque ussing para garantizar el StreamReader se cierre
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
        else
        {
            Console.WriteLine("El archivo no existe.");
        }
    }
}