using System;
using System.Collections.Generic;
using PF_TeoriaComputacion.Personajes;

public class Alquimista : PersonajeBase
{
    private Random _rnd = new Random();
    public int Destreza { get; set; }
    public int Ingenio { get; set; }

    // Lista Inventario
    

    public Alquimista()
    {
        Inteligencia = _rnd.Next(1, 100);
        Destreza = _rnd.Next(1, 100);
        Ingenio = _rnd.Next(1, 100);
        Inventario = new List<string> { "Libro de alquimia", "Frascos magicos", "Caldero portatil" };
    }

    // Método para intentar establecer un atributo por nombre
    public override bool PonerAtributos(string nombreAtributo, int valor)
    {
        switch (nombreAtributo.ToUpper())
        {
            case "VIDA": Vida = valor; return true;
            case "INTELIGENCIA": Inteligencia = valor; return true;
            case "DESTREZA": Destreza = valor; return true;
            case "INGENIO": Ingenio = valor; return true;
            default: return false; // Atributo no reconocido para esta clase
        }
    }

    public override void MostrarAtributos()
    {
        Console.WriteLine("Atributos: ");
        Console.WriteLine($"Vida: {Vida}, Inteligencia: {Inteligencia}, Destreza: {Destreza}, Ingenio: {Ingenio}");
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