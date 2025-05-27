using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PF_TeoriaComputacion.Personajes;

namespace PF_TeoriaComputacion
{
    public class Parser
    {
        // Tabla comandos validos
        private readonly string[] _clasesValidas = { "GUERRERO", "ARQUERO", "MAGO", "ESPADACHIN", "ALQUIMISTA", "DRUIDA", "SANADOR" };
        private readonly string[] _comandosValidos = { "PERSONAJE", "NOMBRE", "CLASE", "ATRIBUTO", "INVENTARIO" };
        private readonly Dictionary<string, string[]> _atributosValidos = new()
        {
            { "GUERRERO", new[] { "VIDA", "INTELIGENCIA", "RABIA", "FUERZA" } },
            { "ARQUERO", new[] { "VIDA", "INTELIGENCIA", "VELOCIDAD", "PRECISION" } }, 
            { "MAGO", new[] { "VIDA", "INTELIGENCIA", "MANA", "FUERZAMAGICA" } },
            { "ESPADACHIN", new[] { "VIDA", "INTELIGENCIA", "ESGRIMA", "GOLPECRITICO" } },
            { "ALQUIMISTA", new[] { "VIDA", "INTELIGENCIA", "DESTREZA", "INGENIO" } },
            { "DRUIDA", new[] { "VIDA", "INTELIGENCIA", "NATURALEZA", "TRANSFORMACION" } },
            { "SANADOR", new[] { "VIDA", "INTELIGENCIA", "ESPIRITU", "CURACION" } }
        };

        // Propiedades
        public List<PersonajeBase> Personajes { get; private set; } = new List<PersonajeBase>();
        public List<string> Errores { get; private set; } = new List<string>();

        public bool ParsearArchivo(string rutaArchivo)
        {
            // Limpiar por si acaso
            Personajes.Clear();
            Errores.Clear();

            // Verificar que el input exista
            if (!File.Exists(rutaArchivo))
            {
                Errores.Add($"Error: El archivo '{rutaArchivo}' no existe.");
                return false;
            }

            // Atributos
            string[] lineas = File.ReadAllLines(rutaArchivo);
            PersonajeBase personaje = null;
            bool tieneNombre = false;
            bool tieneClase = false;
            List<string> nombresUsados = new List<string>();
            int lineaInicio = 0;

            // Leer las lineas
            for (int i = 0; i < lineas.Length; i++)
            {
                string linea = lineas[i].Trim();
                int numeroLinea = i + 1;

                // Saltar vacios o comentarios
                if (string.IsNullOrWhiteSpace(linea) || linea.StartsWith("//"))
                    continue;

                // .Split para separar con :                                                              ANALISIS LEXICO
                string[] partes = linea.Split(new[] { ':' }, 2);
                string comando = partes[0].Trim().ToUpper();
                string valor = partes.Length > 1 ? partes[1].Trim() : "";

                // Validar comando
                if (!_comandosValidos.Contains(comando))
                {
                    Errores.Add($"Linea {numeroLinea}: Comando desconocido '{comando}'.");
                    return false;
                }

                // Verificar personaje y sus sintaxis                                                     ANALISIS SINTACTICO
                if (comando == "PERSONAJE")
                {
                    if (personaje != null)
                    {
                        if (!tieneNombre)
                            Errores.Add($"Linea {lineaInicio}: Falta el comando 'NOMBRE'.");
                        if (!tieneClase)
                            Errores.Add($"Linea {lineaInicio}: Falta el comando 'CLASE'.");
                        if (Errores.Any())
                            return false;
                        Personajes.Add(personaje);
                    }

                    // Resetear pa volver a empezar
                    personaje = null;
                    tieneNombre = false;
                    tieneClase = false;
                    lineaInicio = numeroLinea;
                    continue;
                }

                // Verificar que este el personaje
                if (lineaInicio == 0)
                {
                    Errores.Add($"Linea {numeroLinea}: Se esperaba 'PERSONAJE' primero.");
                    return false;
                }

                // Logica comandos                                                                        ANALISIS SEMANTICO
                if (comando == "NOMBRE")
                {
                    if (string.IsNullOrWhiteSpace(valor))
                    {
                        Errores.Add($"Linea {numeroLinea}: El nombre no puede estar vacío.");
                        return false;
                    }
                    if (nombresUsados.Contains(valor))                                                  
                    {
                        Errores.Add($"Linea {numeroLinea}: El nombre '{valor}' ya está en uso.");
                        return false;
                    }
                    if (tieneNombre)
                    {
                        Errores.Add($"Linea {numeroLinea}: Solo se permite un 'NOMBRE' por personaje.");
                        return false;
                    }

                    nombresUsados.Add(valor);

                    tieneNombre = true;

                    if (personaje != null)
                        personaje.Nombre = valor;
                }
                else if (comando == "CLASE")
                {
                    if (string.IsNullOrWhiteSpace(valor))
                    {
                        Errores.Add($"Linea {numeroLinea}: La clase no puede estar vacía.");
                        return false;
                    }
                    string claseUpper = valor.ToUpper();
                    if (!_clasesValidas.Contains(claseUpper))
                    {
                        Errores.Add($"Linea {numeroLinea}: Clase '{valor}' no válida.");
                        return false;
                    }
                    if (tieneClase)
                    {
                        Errores.Add($"Linea {numeroLinea}: Solo se permite una 'CLASE' por personaje.");
                        return false;
                    }
                    tieneClase = true;
                    personaje = CrearPersonaje(claseUpper);  // REPRE INTERMEDIA
                    if (tieneNombre)
                        personaje.Nombre = nombresUsados[nombresUsados.Count - 1];
                }
                else if (comando == "ATRIBUTO")
                {
                    if (string.IsNullOrWhiteSpace(valor))
                    {
                        Errores.Add($"Linea {numeroLinea}: El atributo requiere un valor.");
                        return false;
                    }

                    // Dividir la cadena en =
                    string[] partesAtributo = valor.Split('=');

                    // Verificar que haya exactamente dos partes pa nombre y pues pa valor
                    if (partesAtributo.Length != 2)
                    {
                        Errores.Add($"Linea {numeroLinea}: Formato incorrecto para ATRIBUTO.");
                        return false;
                    }

                    // Sacar el nombre y valor y limpiar espacios en blanco
                    string nombreAtributo = partesAtributo[0].Trim();
                    string stringValorAtributo = partesAtributo[1].Trim();

                    // Validar que el nombre del atributo no este vacio
                    if (string.IsNullOrWhiteSpace(nombreAtributo))
                    {
                        Errores.Add($"Linea {numeroLinea}: El nombre del atributo no puede estar vacío.");
                        return false;
                    }
                    // Validar que el valor del atributo no este vacio
                    if (string.IsNullOrWhiteSpace(stringValorAtributo))
                    {
                        Errores.Add($"Linea {numeroLinea}: El valor del atributo '{nombreAtributo}' no puede estar vacío.");
                        return false;
                    }

                    int valorAtributo;
                    // Intentar convertir el valor del atributo a un número 
                    if (!int.TryParse(stringValorAtributo, out valorAtributo))
                    {
                        Errores.Add($"Linea {numeroLinea}: El valor del atributo '{nombreAtributo}' debe ser un número.");
                        return false;
                    }

                    if (valorAtributo <= 0) 
                    {
                        Errores.Add($"Linea {numeroLinea}: El valor del atributo '{nombreAtributo}' debe ser un número positivo (se encontró '{valorAtributo}').");
                        return false; 
                    }

                    // Verificar que ya se haya definido una clase pa el personaje
                    if (personaje == null)
                    {
                        Errores.Add($"Linea {numeroLinea}: Defina una CLASE antes de intentar asignar un ATRIBUTO.");
                        return false;
                    }

                    // Verificar si el atributo es válido pa la clase
                    string clasePersonajeActual = personaje.GetType().Name.ToUpper();
                    bool atributoValidoParaClase = false;
                    foreach (string atrValido in _atributosValidos[clasePersonajeActual])
                    {
                        if (atrValido.Equals(nombreAtributo, StringComparison.OrdinalIgnoreCase))
                        {
                            atributoValidoParaClase = true;
                            break;
                        }
                    }

                    if (!atributoValidoParaClase)
                    {
                        Errores.Add($"Linea {numeroLinea}: El atributo '{nombreAtributo}' no es válido para la clase '{personaje.GetType().Name}'.");
                        return false; 
                    }

                    // Intentar ponerle el atributo al personaje
                    if (!personaje.PonerAtributos(nombreAtributo, valorAtributo))
                    {
                        Errores.Add($"Linea {numeroLinea}: No se pudo asignar el atributo '{nombreAtributo}' con valor '{valorAtributo}'. verifique las reglas del personaje.");
                        return false; 
                    }
                }
                else if (comando == "INVENTARIO")
                {
                    if (string.IsNullOrWhiteSpace(valor))
                    {
                        Errores.Add($"Linea {numeroLinea}: El inventario requiere items separados por comas.");
                        return false; 
                    }

                    string[] itemsSinProcesar = valor.Split(',');
                    List<string> itemsLimpios = new List<string>();

                    for (int k = 0; k < itemsSinProcesar.Length; k++)
                    {
                        string itemActual = itemsSinProcesar[k].Trim();
                        if (!string.IsNullOrWhiteSpace(itemActual))
                        {
                            itemsLimpios.Add(itemActual);
                        }
                    }

                    if (itemsLimpios.Count == 0) 
                    {
                        Errores.Add($"Linea {numeroLinea}: El inventario no contiene items validos despues de procesar '{valor}'.");
                        return false; 
                    }

                    if (personaje == null)
                    {
                        Errores.Add($"Linea {numeroLinea}: Define 'CLASE' antes de intentar asignar un 'INVENTARIO'.");
                        return false; 
                    }

                    personaje.Inventario.Clear();
                    // Añadir cada item limpio a la lista de inventario del pj
                    for (int k = 0; k < itemsLimpios.Count; k++)
                    {
                        personaje.Inventario.Add(itemsLimpios[k]);
                    }

                }
            }

            // Validar y almacenar el personaje 
            if (personaje != null)
            {
                if (!tieneNombre)
                    Errores.Add($"Linea {lineaInicio}: Falta el comando 'NOMBRE'.");
                if (!tieneClase)
                    Errores.Add($"Linea {lineaInicio}: Falta el comando 'CLASE'.");
                if (Errores.Any())
                    return false;
                Personajes.Add(personaje);
            }

            // Si no hay perosajes pues false
            if (!Personajes.Any() && lineas.Any(l => !string.IsNullOrWhiteSpace(l) && !l.StartsWith("//")))
            {
                Errores.Add("No se encontraron personajes.");
                return false;
            }

            return true;
        }

        // Instanciar los personajes REPRESENTACION INTERMEDIA
        private PersonajeBase CrearPersonaje(string clase)
        {
            if (clase == "GUERRERO") return new Guerrero();
            if (clase == "ARQUERO") return new Arquero();
            if (clase == "MAGO") return new Mago();
            if (clase == "ESPADACHIN") return new Espadachin();
            if (clase == "ALQUIMISTA") return new Alquimista();
            if (clase == "DRUIDA") return new Druida();
            if (clase == "SANADOR") return new Sanador();
            throw new Exception($"Clase no valida: {clase}");
        }
    }
}