using System;
using System.IO;
using System.Linq;
using PF_TeoriaComputacion.Personajes;

namespace PF_TeoriaComputacion
{
    public class Program
    {
        private static readonly string _baseProyecto = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName
            ?? throw new InvalidOperationException("No se pudo determinar el directorio raíz del proyecto.");
        private static readonly string _rutaArchivoInput = Path.Combine(_baseProyecto, "input.txt");
        private static readonly string _rutaArchivoOutput = Path.Combine(_baseProyecto, "output.txt");

        static void Main(string[] args)
        {
            Console.WriteLine($"Procesando archivo: {_rutaArchivoInput}");

            // Mirar si existe el archivo
            if (!File.Exists(_rutaArchivoInput))
            {
                Console.WriteLine($"Error, No se encontro el archivo en: {_rutaArchivoInput}");
                return;
            }

            if (!File.ReadAllLines(_rutaArchivoInput).Any())
            {
                Console.WriteLine($"\n\nError, Archivo vacio");
                return;
            }

            Parser parser = new Parser();
            bool bien = parser.ParsearArchivo(_rutaArchivoInput);

            if (bien)
            {
                Console.WriteLine("\nParseo bien. Personajes creados:");
                foreach (var personaje in parser.Personajes)
                {
                    Console.WriteLine($"\nNombre: {personaje.Nombre} - Clase: {personaje.GetType().Name}");
                    personaje.MostrarAtributos();
                    personaje.MostrarInventario();
                }

                try
                {
                    EscribirArchivo(parser.Personajes);
                    Console.WriteLine($"\nResultados escritos en: {_rutaArchivoOutput}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"\nError al escribir en '{_rutaArchivoOutput}': {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nErrore, No se hay personajes:");
                foreach (string error in parser.Errores)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine("\nCorrige los errores en input.txt y vuelva a hacerle.");
            }
        }

        private static void EscribirArchivo(List<PersonajeBase> personajes)
        {
            using (StreamWriter sw = new StreamWriter(_rutaArchivoOutput))
            {
                foreach (var personaje in personajes)
                {
                    sw.WriteLine("PERSONAJE");
                    sw.WriteLine($"NOMBRE: {personaje.Nombre}");
                    sw.WriteLine($"CLASE: {personaje.GetType().Name.ToUpper()}");
                    sw.WriteLine("ATRIBUTOS:");
                    switch (personaje)
                    {
                        case Guerrero p:
                            var guerrero = (Guerrero)personaje;
                            sw.WriteLine($"  VIDA: {guerrero.Vida}");
                            sw.WriteLine($"  INTELIGENCIA: {guerrero.Inteligencia}");
                            sw.WriteLine($"  RABIA: {guerrero.Rabia}");
                            sw.WriteLine($"  FUERZA: {guerrero.Fuerza}");
                            break;

                        case Arquero p:
                            var arquero = (Arquero)personaje;
                            sw.WriteLine($"  VIDA: {arquero.Vida}");
                            sw.WriteLine($"  INTELIGENCIA: {arquero.Inteligencia}");
                            sw.WriteLine($"  VELOCIDAD: {arquero.Velocidad}");
                            sw.WriteLine($"  PRECISION: {arquero.Precision}");
                            break;

                        case Mago p:
                            var mago = (Mago)personaje;
                            sw.WriteLine($"  VIDA: {mago.Vida}");
                            sw.WriteLine($"  INTELIGENCIA: {mago.Inteligencia}");
                            sw.WriteLine($"  MANA: {mago.Mana}");
                            sw.WriteLine($"  FUERZAMAGICA: {mago.FuerzaMagica}");
                            break;

                        case Espadachin p:
                            var espadachin = (Espadachin)personaje;
                            sw.WriteLine($"  VIDA: {espadachin.Vida}");
                            sw.WriteLine($"  INTELIGENCIA: {espadachin.Inteligencia}");
                            sw.WriteLine($"  ESGRIMA: {espadachin.Esgrima}");
                            sw.WriteLine($"  GOLPECRITICO: {espadachin.GolpeCritico}");
                            break;

                        case Alquimista p:
                            var alquimista = (Alquimista)personaje;
                            sw.WriteLine($"  VIDA: {alquimista.Vida}");
                            sw.WriteLine($"  INTELIGENCIA: {alquimista.Inteligencia}");
                            sw.WriteLine($"  DESTREZA: {alquimista.Destreza}");
                            sw.WriteLine($"  INGENIO: {alquimista.Ingenio}");
                            break;

                        case Druida p:
                            var druida = (Druida)personaje;
                            sw.WriteLine($"  VIDA: {druida.Vida}");
                            sw.WriteLine($"  INTELIGENCIA: {druida.Inteligencia}");
                            sw.WriteLine($"  NATURALEZA: {druida.Naturaleza}");
                            sw.WriteLine($"  TRANSFORMACION: {druida.Transformacion}");
                            break;

                        case Sanador p:
                            var sanador = (Sanador)personaje;
                            sw.WriteLine($"  VIDA: {sanador.Vida}");
                            sw.WriteLine($"  INTELIGENCIA: {sanador.Inteligencia}");
                            sw.WriteLine($"  ESPIRITU: {sanador.Espiritu}");
                            sw.WriteLine($"  CURACION: {sanador.Curacion}");
                            break;

                        default:
                            sw.WriteLine($"  VIDA: {personaje.Vida}");
                            sw.WriteLine($"  INTELIGENCIA: {personaje.Inteligencia}");
                            break;
                    }
                    sw.WriteLine("INVENTARIO:");
                    if (personaje.Inventario.Any())
                    {
                        foreach (var item in personaje.Inventario)
                        {
                            sw.WriteLine($"  - {item}");
                        }
                    }
                    else
                    {
                        sw.WriteLine("  Vacío");
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}