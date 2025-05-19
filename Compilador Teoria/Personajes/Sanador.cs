using System;
using System.Collections.Generic;
using PF_TeoriaComputacion.Personajes;

public class Sanador : PersonajeBase
{
    private Random _rnd = new Random();
    public int Espiritu { get; set; }
    public int Curacion { get; set; }

    // Lista Inventario
    public Sanador()
    {
        Inteligencia = _rnd.Next(1, 100);
        Espiritu = _rnd.Next(1, 100);
        Curacion = _rnd.Next(1, 100);
        Inventario = new List<string> { "Baculo sanador", "Amuleto sagrado", "Posicion de resurreccion" };
    }

    // Método para intentar establecer un atributo por nombre
    public override bool PonerAtributos(string nombreAtributo, int valor)
    {
        switch (nombreAtributo.ToUpper())
        {
            case "VIDA": Vida = valor; return true;
            case "INTELIGENCIA": Inteligencia = valor; return true;
            case "ESPIRITU": Espiritu = valor; return true;
            case "CURACION": Curacion = valor; return true;
            default: return false; // Atributo no reconocido para esta clase
        }
    }

    public override void MostrarAtributos()
    {
        Console.WriteLine("Atributos: ");
        Console.WriteLine($"Vida: {Vida}, Inteligencia: {Inteligencia}, Espiritu: {Espiritu}, Curacion: {Curacion}");
    }

    public override void MostrarInventario()
    {
        Console.WriteLine("Inventarios: ");
        if (Inventario.Any())
        {
            foreach (string item in Inventario)
            {
                Console.WriteLine($"- {item}");
            }
        }
        else
        {
            Console.WriteLine("Vacío");
        }
    }
}