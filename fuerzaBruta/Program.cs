using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Búsqueda de Contraseña Encriptada sin Multihilos:");
        buscarContrasenaEncriptadaSinMultihilos();
        Console.WriteLine("\nBúsqueda de Contraseña Encriptada con Multihilos:");
        buscarContrasenaEncriptadaConMultiHilos();
    }

    private static void buscarContrasenaEncriptadaSinMultihilos()
    {
        string filePath = "2151220-passwords.txt";
        var contrasenaSeleccionada = "~~~~~~";

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var contrasenaEncriptada = EncriptarPaswHash256(contrasenaSeleccionada);
        List<string> listaDeContrasenas = EncriptarDocumentoHash256(filePath);

        MostrarResultado(listaDeContrasenas.Contains(contrasenaEncriptada));

        stopwatch.Stop();
        Console.WriteLine($"Tiempo de ejecución: {stopwatch.Elapsed} segundos");
    }
    
    private static void MostrarResultado(bool contraseñaEncontrada)
    {
        if (contraseñaEncontrada)
        {
            Console.WriteLine("Contraseña encontrada.");
        }
        else
        {
            Console.WriteLine("No encontrada.");
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

            Console.WriteLine($"Contraseña encriptada (SHA-256): {builder}");
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

    private static void BuscarContraseñaEnArchivoSinEncriptar(string contrasenaBuscar, string filePath)
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

    private static void buscarContrasenaEncriptadaConMultiHilos()
    {
        string filePath = "2151220-passwords.txt";
        string contrasenaSeleccionada = "ma930520";

        Stopwatch[] threadStopwatches = new Stopwatch[4];

        string contraseñaEncriptada = EncriptarPaswHash256(contrasenaSeleccionada);
        List<string> listaDeContrasenas = EncriptarDocumentoHash256(filePath);

        bool contrasenaEncontrada = false;
        int numerosThreads = 4;
        int divisionPartes = listaDeContrasenas.Count / numerosThreads;

        Thread[] threads = new Thread[numerosThreads];

        for (int i = 0; i < numerosThreads; i++)
        {
            int inicio = i * divisionPartes;
            int final = (i == numerosThreads - 1) ? listaDeContrasenas.Count : inicio + divisionPartes;

            int threadId = i;

            threads[i] = new Thread(() =>
            {
                threadStopwatches[threadId] = new Stopwatch(); // Inicializa el cronómetro para este hilo
                threadStopwatches[threadId].Start(); // Inicia el cronómetro para este hilo

                if (buscarContrasenaEnLista(inicio, final, listaDeContrasenas, contraseñaEncriptada))
                {
                    Console.WriteLine($"Contraseña encontrada en el hilo {threadId}");
                    contrasenaEncontrada = true;
                }

                threadStopwatches[threadId].Stop();
            });
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        MostrarTiemposEjecucion(threadStopwatches);
        MostrarResultado(contrasenaEncontrada);
    }
    
    private static void MostrarTiemposEjecucion(Stopwatch[] threadStopwatches)
    {
        for (int i = 0; i < threadStopwatches.Length; i++)
        {
            Console.WriteLine($"Tiempo de ejecución en el hilo {i}: {threadStopwatches[i].Elapsed}");
        }
    }

    private static bool buscarContrasenaEnLista(int inicio, int final, List<String> listaContrasenas,
        string contrasenaEncriptada)
    {
        for (int i = inicio; i < final; i++)
        {
            if (listaContrasenas[i] == contrasenaEncriptada)
            {
                return true;
            }
        }

        return false;
    }
}