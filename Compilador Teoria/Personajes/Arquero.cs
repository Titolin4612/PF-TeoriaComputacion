using System;
using System.Collections.Generic;
using PF_TeoriaComputacion.Personajes;

public class Arquero : PersonajeBase
{
    private Random _rnd = new Random();
    public int Velocidad { get; set; }
    public int Precision { get; set; }

    // Lista Inventario
    

    public Arquero()
    {
        Inteligencia = _rnd.Next(1, 100);
        Velocidad = _rnd.Next(1, 100);
        Precision = _rnd.Next(1, 100);
        Inventario = new List<string> { "arco mortal", "escudo de hierro", "flechas" };
    }

    // Método para intentar establecer un atributo por nombre
    public override bool PonerAtributos(string nombreAtributo, int valor)
    {
        switch (nombreAtributo.ToUpper())
        {
            case "VIDA": Vida = valor; return true;
            case "INTELIGENCIA": Inteligencia = valor; return true;
            case "VELOCIDAD": Velocidad = valor; return true;
            case "PRECISION": Precision = valor; return true;
            default: return false; // Atributo no reconocido para esta clase
        }
    }

    public override void MostrarAtributos()
    {
        Console.WriteLine("Atributos: ");
        Console.WriteLine($"Vida: {Vida}, Inteligencia: {Inteligencia}, Velocidad: {Velocidad}, Precision: {Precision}");
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