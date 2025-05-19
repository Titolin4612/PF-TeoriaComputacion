using System;
using System.Collections.Generic;
using PF_TeoriaComputacion.Personajes;

public class Mago : PersonajeBase
{
    private Random _rnd = new Random();

    public int Mana { get; set; }
    public int FuerzaMagica { get; set; }

    // Lista Inventario
    

    public Mago()
    {
        Inteligencia = _rnd.Next(1, 100);
        Mana = _rnd.Next(1, 100);
        FuerzaMagica = _rnd.Next(1, 100);
        Inventario = new List<string> { "Baston magico", "Libro de hechizos", "Pocion de vida avanzada" };
    }

    // Método para intentar establecer un atributo por nombre
    public override bool PonerAtributos(string nombreAtributo, int valor)
    {
        switch (nombreAtributo.ToUpper())
        {
            case "VIDA": Vida = valor; return true;
            case "INTELIGENCIA": Inteligencia = valor; return true;
            case "MANA": Mana = valor; return true;
            case "FUERZAMAGICA": FuerzaMagica = valor; return true;
            default: return false; // Atributo no reconocido para esta clase
        }
    }

    public override void MostrarAtributos()
    {
        Console.WriteLine("Atributos: ");
        Console.WriteLine($"Vida: {Vida}, Inteligencia: {Inteligencia}, Mana: {Mana}, FuerzaMagica: {FuerzaMagica}");
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