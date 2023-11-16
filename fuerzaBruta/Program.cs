using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

internal class Program
{
    private static void Main()
    {
        var contrasenaBuscar = "ma930520";
        string filePath = "C:\\Users\\juanj\\RiderProjects\\fuerzaBruta\\2151220-passwords.txt";

        BuscarContraseñaEnArchivo(contrasenaBuscar, filePath);

        var contraseñaEncriptada = EncriptarPaswHash256(contrasenaBuscar);
        Console.WriteLine($"Contraseña encriptada (SHA-256): {contraseñaEncriptada}");

        List<string> contraseñasEncriptadas = EncriptarDocumentoHash256(filePath);
        if (contraseñasEncriptadas.Contains(contraseñaEncriptada))
        {
            Console.WriteLine("Contraseña encontrada.");
        }
        else
        {
            Console.WriteLine("No encontrada.");
        }
    }

    private static void BuscarContraseñaEnArchivo(string contrasenaBuscar, string filePath)
    {
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

    private static string EncriptarPaswHash256(string contrasena)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            //Convertir entrada a byte
            byte[] bytes = Encoding.UTF8.GetBytes(contrasena);

            //Calcular el hash
            byte[] hashBytes = sha256.ComputeHash(bytes);

            //Convierte el hash en una cadena hexadecimal.
            StringBuilder builder = new StringBuilder();
            
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }

    private static List<string> EncriptarDocumentoHash256(String filePath)
    {
        List<string> hashes = new List<string>();

        using (SHA256 sha256 = SHA256.Create())
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(line);

                    byte[] hashBytes = sha256.ComputeHash(bytes);

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        builder.Append(hashBytes[i].ToString("x2"));
                    }

                    hashes.Add(builder.ToString());
                }
            }
        }

        return hashes;
    }
    
    
    
}