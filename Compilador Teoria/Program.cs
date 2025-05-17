using System;
using System.IO;
using System.Linq;
using PF_TeoriaComputacion.Personajes;

public class Program
{
    private static readonly string _baseProyecto = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName ?? throw new Exception("No se pudo determinar el directorio raíz del proyecto.");
    private static readonly string _rutaArchivoInput = Path.Combine(_baseProyecto, "input.txt");
    private static readonly string _rutaArchivoOutput = Path.Combine(_baseProyecto, "output.txt");

    static void Main(string[] args)
    {
        Parser parser = new Parser();

        Console.WriteLine($"Intentando parsear el archivo: {_rutaArchivoInput}");
        bool exitoParseo = parser.ParsearArchivo(_rutaArchivoInput);

        if (exitoParseo)
        {
            Console.WriteLine("Parseo exitoso.");
            Console.WriteLine($"Nombre: {parser.NombrePersonaje}");
            Console.WriteLine($"Clase: {parser.ClasePersonaje}");

            // Crear el personaje
            Personaje personaje = null; 

            switch (parser.ClasePersonaje)
            {
                case "GUERRERO":
                    personaje = new Guerrero();
                    break;
                case "MAGO":
                    personaje = new Mago();
                    break;
                case "ARQUERO":
                    personaje = new Arquero();
                    break;
                case "Espadachin":
                    personaje = new Espadachin();
                    break;
            }

            if (personaje != null)
            {
                // ANÁLISIS SEMÁNTICO: Verificar si los atributos son válidos para la clase y aplicarlos.
                foreach (var p in parser.AtributosDefinidos)
                {
                    bool aplicado = personaje.PonerAtributos(p.Key, p.Value);
                    if (!aplicado)
                    {
                        Console.WriteLine($"Advertencia Semántica: El atributo '{p.Key}' no es válido o no se pudo aplicar a la clase {parser.ClasePersonaje}.");
                        // Podrías añadir esto a una lista de advertencias si quieres
                    }
                }

                // Aplicar inventario definido (si se especificó, reemplaza el por defecto)
                if (parser.InventarioDefinido.Any())
                {
                    personaje.Inventario = parser.InventarioDefinido;
                }

                Console.WriteLine("\n--- Información del Personaje Creado ---");
                // Mostrar Info en Consola
                System.Console.WriteLine($"PERSONAJE: {parser.NombrePersonaje} ({parser.ClasePersonaje})");
                personaje.MostrarAtributos();
                personaje.MostrarInventario();

                // Escribir en Archivo
                EscribirArchivo(personaje, parser.NombrePersonaje, parser.ClasePersonaje);
                Console.WriteLine($"\nInformación del personaje escrita en: {_rutaArchivoOutput}");
            }
        }
        else
        {
            Console.WriteLine("\nErrores durante el parseo:");
            foreach (string error in parser.Errores)
            {
                Console.WriteLine(error);
            }
        }
    }

    public static void EscribirArchivo(dynamic personaje, string nombre, string clase)
    {
        using (StreamWriter sw = new StreamWriter(_rutaArchivoOutput))
        {
            sw.WriteLine("PERSONAJE"); // Ya no se lee del input, se escribe directamente
            sw.WriteLine($"NOMBRE: {nombre}");
            sw.WriteLine($"CLASE: {clase}");

            // Simplificado usando las propiedades públicas directamente
            if (clase.Equals("GUERRERO"))
            {
                var p = (Guerrero)personaje;
                sw.WriteLine("ATRIBUTOS:");
                sw.WriteLine($"  Vida: {p.Vida}");
                sw.WriteLine($"  Inteligencia: {p.Inteligencia}");
                sw.WriteLine($"  Rabia: {p.Rabia}");
                sw.WriteLine($"  Fuerza: {p.Fuerza}");
                sw.WriteLine("INVENTARIO:");
                foreach (var item in p.Inventario) sw.WriteLine($"- {item}");
            }
            else if (clase.Equals("MAGO"))
            {
                var p = (Mago)personaje;
                sw.WriteLine("ATRIBUTOS:");
                sw.WriteLine($"  Vida: {p.Vida}");
                sw.WriteLine($"  Inteligencia: {p.Inteligencia}");
                sw.WriteLine($"  Mana: {p.Mana}");
                sw.WriteLine($"  Fuerza Magica: {p.FuerzaMagica}");
                sw.WriteLine("INVENTARIO:");
                foreach (var item in p.Inventario) sw.WriteLine($"- {item}");
            }
            else if (clase.Equals("ARQUERO"))
            {
                var p = (Arquero)personaje;
                sw.WriteLine("ATRIBUTOS:");
                sw.WriteLine($"  Vida: {p.Vida}");
                sw.WriteLine($"  Inteligencia: {p.Inteligencia}");
                sw.WriteLine($"  Precision: {p.Precision}");
                sw.WriteLine($"  Velocidad: {p.Velocidad}");
                sw.WriteLine("INVENTARIO:");
                foreach (var item in p.Inventario) sw.WriteLine($"- {item}");
            }
        }
    }
}