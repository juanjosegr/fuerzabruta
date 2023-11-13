using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        string contrasenaBuscar = "ma930520";

        BuscarContraseñaEnArchivo(contrasenaBuscar);

        string contraseñaEncriptada = ObtenerHashSHA256(contrasenaBuscar);

        Console.WriteLine($"Contraseña encriptada (SHA-256): {contraseñaEncriptada}");
    }

    static void BuscarContraseñaEnArchivo(string contrasenaBuscar)
    {
        string filePath = "C:\\Users\\juanj\\RiderProjects\\fuerzaBruta\\2151220-passwords.txt";

        if (File.Exists(filePath))
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //Console.WriteLine(line);
                    if (line == contrasenaBuscar)
                    {
                        Console.WriteLine($"Contraseña encontrada:\nLa contraseña es {line}");
                        break;
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("El archivo no existe.");
        }
    }

    static string ObtenerHashSHA256(string contrasena)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            //Convertir entrada a byte
            byte[] bytes = Encoding.UTF8.GetBytes(contrasena);

            //Calcular el hash
            byte[] hashBytes = sha256.ComputeHash(bytes);

            //Convierte el hash en una cadena hexadecimal.
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes[i]; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}