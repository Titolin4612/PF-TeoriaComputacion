using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Parser
{
    // Configuraciones
    private readonly string[] _clasesValidas = { "GUERRERO", "ARQUERO", "MAGO", "ESPADACHIN" };
    private readonly Dictionary<string, bool> _comandosEsperados = new Dictionary<string, bool>
    {
        { "PERSONAJE", false },      // false = no espera valor después de ':'
        { "NOMBRE", true },         // true = espera valor después de ':'
        { "CLASE", true },
        { "ATRIBUTO", true },       // Nuevo: para definir atributos específicos
        { "INVENTARIO", true }      // Nuevo: para definir inventario
    };

    // Atributos parser
    public string NombrePersonaje { get; private set; }
    public string ClasePersonaje { get; private set; }
    public Dictionary<string, int> AtributosDefinidos { get; private set; }
    public List<string> InventarioDefinido { get; private set; }
    public List<string> Errores { get; private set; }

    // Contador para validacion
    private Dictionary<string, int> _contadorComandos;


    // Constructor
    public Parser()
    {
        Errores = new List<string>();
        AtributosDefinidos = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase); // Case-insensitive para nombres de atributos
        InventarioDefinido = new List<string>();
        _contadorComandos = new Dictionary<string, int>();
    }


    public bool ParsearArchivo(string rutaArchivo)
    {
        // Reiniciar estado para un nuevo parseo
        Errores.Clear();
        AtributosDefinidos.Clear();
        InventarioDefinido.Clear();
        NombrePersonaje = null;
        ClasePersonaje = null;
        _contadorComandos.Clear();
        foreach (var cmd in _comandosEsperados.Keys)
        {
            _contadorComandos[cmd] = 0;
        }


        if (!File.Exists(rutaArchivo))
        {
            Errores.Add($"Error: El archivo de entrada '{rutaArchivo}' no fue encontrado.");
            return false;
        }

        string[] lineas = File.ReadAllLines(rutaArchivo);
        bool personajeDeclarado = false;

        int numeroLinea = 0;
        for (int i = 0; i < lineas.Length; i++)
        {
            string lineaActual = lineas[i].Trim();
            numeroLinea++ ;

            // ANÁLISIS LÉXICO: Ignorar líneas vacías o comentarios
            if (string.IsNullOrWhiteSpace(lineaActual) || lineaActual.StartsWith("//"))
            {
                continue; // ANÁLISIS LÉXICO: Ignorar comentarios y líneas vacías
            }

            // ANÁLISIS LÉXICO: Dividir la línea en clave y valor (si existe)
            string[] partes = lineaActual.Split(new[] { ':' }, 2); // Dividir solo en el primer ':'
            string clave = partes[0].Trim().ToUpper();
            string valor = partes.Length > 1 ? partes[1].Trim() : null;

            // ANÁLISIS SINTÁCTICO y SEMÁNTICO INICIAL
            if (!_comandosEsperados.ContainsKey(clave))
            {
                Errores.Add($"Línea {numeroLinea}: Comando desconocido '{clave}'.");
                continue;
            }

            // Validar que el personaje sea el primer comando
            if (!personajeDeclarado && clave != "PERSONAJE")
            {
                Errores.Add($"Línea {numeroLinea}: Se esperaba el comando 'PERSONAJE' al inicio de la definición.");
                return false;
            }
            if (clave == "PERSONAJE")
            {
                personajeDeclarado = true;
            }
            else if (!personajeDeclarado)
            {
                Errores.Add($"Línea {numeroLinea}: El comando '{clave}' no puede aparecer antes de 'PERSONAJE'.");
                continue;
            }


            // Contar ocurrencias de comandos principales (PERSONAJE, NOMBRE, CLASE)
            if (clave == "PERSONAJE" || clave == "NOMBRE" || clave == "CLASE")
            {
                _contadorComandos[clave]++;
                if (_contadorComandos[clave] > 1)
                {
                    Errores.Add($"Línea {numeroLinea}: El comando '{clave}' solo puede aparecer una vez.");
                    continue;
                }
            }


            // Validar si el comando esperaba un valor y si se proporcionó
            if (_comandosEsperados[clave] && string.IsNullOrWhiteSpace(valor))
            {
                Errores.Add($"Línea {numeroLinea}: El comando '{clave}' requiere un valor después de ':'.");
                continue;
            }
            if (!_comandosEsperados[clave] && !string.IsNullOrWhiteSpace(valor))
            {
                Errores.Add($"Línea {numeroLinea}: El comando '{clave}' no espera un valor, pero se proporcionó '{valor}'.");
                continue;
            }

            // Procesar comandos específicos
            switch (clave)
            {
                case "PERSONAJE":
                    // Ya manejado por la validación de unicidad y orden
                    break;
                case "NOMBRE":
                    NombrePersonaje = valor; // ANÁLISIS SEMÁNTICO: Asignar nombre
                    break;
                case "CLASE":
                    if (_clasesValidas.Contains(valor.ToUpper()))
                    {
                        ClasePersonaje = valor.ToUpper(); // ANÁLISIS SEMÁNTICO: Asignar clase
                    }
                    else
                    {
                        Errores.Add($"Línea {numeroLinea}: Clase '{valor}' no válida. Clases válidas: {string.Join(", ", _clasesValidas)}.");
                    }
                    break;
                case "ATRIBUTO": // ATRIBUTO: Fuerza=10 o ATRIBUTO: Vida = 100
                    string[] partesAtributo = valor.Split(new[] { '=' }, 2);
                    if (partesAtributo.Length == 2)
                    {
                        string nombreAtributo = partesAtributo[0].Trim();
                        if (int.TryParse(partesAtributo[1].Trim(), out int valorAtributo))
                        {
                            // ANÁLISIS SEMÁNTICO: Guardar atributo definido
                            // La validación de si el atributo es válido para la clase se hará después,
                            // una vez que sepamos la clase.
                            AtributosDefinidos[nombreAtributo] = valorAtributo;
                        }
                        else
                        {
                            Errores.Add($"Línea {numeroLinea}: Valor inválido para el atributo '{nombreAtributo}'. Se esperaba un número.");
                        }
                    }
                    else
                    {
                        Errores.Add($"Línea {numeroLinea}: Formato incorrecto para ATRIBUTO. Se esperaba 'NombreAtributo = Valor'. Ejemplo: Fuerza = 10");
                    }
                    break;
                case "INVENTARIO": // INVENTARIO: Espada, Escudo, Pocion
                    string[] items = valor.Split(',').Select(item => item.Trim()).Where(item => !string.IsNullOrWhiteSpace(item)).ToArray();
                    if (items.Any())
                    {
                        InventarioDefinido.AddRange(items); // ANÁLISIS SEMÁNTICO: Guardar ítems
                    }
                    else
                    {
                        Errores.Add($"Línea {numeroLinea}: El comando INVENTARIO no contenía ítems válidos.");
                    }
                    break;
            }
        }

        // Validaciones Sintácticas y Semánticas
        if (_contadorComandos["PERSONAJE"] == 0 && lineas.Any(l => !string.IsNullOrWhiteSpace(l) && !l.StartsWith("//")))
        {
            Errores.Add("Error: El comando 'PERSONAJE' es obligatorio y no se encontró al inicio.");
        }
        if (personajeDeclarado) // Solo validar si se declaró PERSONAJE
        {
            if (_contadorComandos["NOMBRE"] == 0) Errores.Add("Error: El comando 'NOMBRE' es obligatorio y no se encontró.");
            if (_contadorComandos["CLASE"] == 0) Errores.Add("Error: El comando 'CLASE' es obligatorio y no se encontró.");
        }

        return !Errores.Any();
    }
}