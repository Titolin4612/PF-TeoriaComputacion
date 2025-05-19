using System;
using System.Collections.Generic;
using PF_TeoriaComputacion.Personajes;

public class Guerrero : PersonajeBase
{
    private Random _rnd = new Random();

    
    
    
    public int Rabia { get; set; }
    public int Fuerza { get; set; }

    // Lista Inventario
    

    public Guerrero()
    {
        Inteligencia = _rnd.Next(1, 100);
        Rabia = _rnd.Next(1, 100);
        Fuerza = _rnd.Next(1, 100);
        Inventario = new List<string> { "Espada básica", "Escudo de madera", "Pocion de vida simple" };
    }

    // Método para intentar establecer un atributo por nombre
    public override bool PonerAtributos(string nombreAtributo, int valor)
    {
        switch (nombreAtributo.ToUpper())
        {
            case "VIDA": Vida = valor; return true;
            case "INTELIGENCIA": Inteligencia = valor; return true;
            case "RABIA": Rabia = valor; return true;
            case "FUERZA": Fuerza = valor; return true;
            default: return false; // Atributo no reconocido para esta clase
        }
    }

    public override void MostrarAtributos()
    {
        Console.WriteLine("Atributos: ");
        Console.WriteLine($"Vida: {Vida}, Inteligencia: {Inteligencia}, Rabia: {Rabia}, Fuerza: {Fuerza}");
    }

    public override void MostrarInventario()
    {
        Console.WriteLine("Inventario: ");
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