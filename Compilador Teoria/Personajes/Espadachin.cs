using System;
using System.Collections.Generic;
using PF_TeoriaComputacion.Personajes;
public class Espadachin : PersonajeBase
{
    private Random _rnd = new Random();

    
    
    
    public int Esgrima { get; set; }
    public int GolpeCritico { get; set; }

    // Lista Inventario
    

    public Espadachin()
    {
        Inteligencia = _rnd.Next(1, 100);
        Esgrima = _rnd.Next(1, 100);
        GolpeCritico = _rnd.Next(1, 100);
        Inventario = new List<string> { "Espada magica", "Armadura pesada", "Pocion de vida avanzada" };
    }

    // Método para intentar establecer un atributo por nombre
    public override bool PonerAtributos(string nombreAtributo, int valor)
    {
        switch (nombreAtributo.ToUpper())
        {
            case "VIDA": Vida = valor; return true;
            case "INTELIGENCIA": Inteligencia = valor; return true;
            case "ESGRIMA": Esgrima = valor; return true;
            case "GOLPECRITICO": GolpeCritico = valor; return true;
            default: return false; // Atributo no reconocido para esta clase
        }
    }

    public override void MostrarAtributos()
    {
        Console.WriteLine("Atributos: ");
        Console.WriteLine($"Vida: {Vida}, Inteligencia: {Inteligencia}, Esgrima: {Esgrima}, GolpeCritico: {GolpeCritico}");
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