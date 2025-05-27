using System;
using System.Collections.Generic;
using PF_TeoriaComputacion.Personajes;

public class Druida : PersonajeBase
{
    private Random _rnd = new Random();
    public int Naturaleza { get; set; }
    public int Transformacion { get; set; }

    // Lista Inventario
    

    public Druida()
    {
        Inteligencia = _rnd.Next(1, 100);
        Naturaleza = _rnd.Next(1, 100);
        Transformacion = _rnd.Next(1, 100);
        Inventario = new List<string> { "Baston de raices", "pocion de crecimiento", "totem de transformacion" };
    }

    // Método para intentar establecer un atributo por nombre
    public override bool PonerAtributos(string nombreAtributo, int valor)
    {
        switch (nombreAtributo.ToUpper())
        {
            case "VIDA": Vida = valor; return true;
            case "INTELIGENCIA": Inteligencia = valor; return true;
            case "NATURALEZA": Naturaleza = valor; return true;
            case "TRANSFORMACION": Transformacion = valor; return true;
            default: return false; // Atributo no reconocido para esta clase
        }
    }

    public override void MostrarAtributos()
    {
        Console.WriteLine("Atributos: ");
        Console.WriteLine($"Vida: {Vida}, Inteligencia: {Inteligencia}, Naturaleza: {Naturaleza}, Transformacion: {Transformacion}");
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