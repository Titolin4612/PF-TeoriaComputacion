using System;
using System.Collections.Generic;
using PF_TeoriaComputacion.Personajes;
using System.Text;
using System.Threading.Tasks;

namespace PF_TeoriaComputacion.Personajes
{
    public class PersonajeBase
    {
        public string Nombre { get; set; }
        public int Vida { get; set; } = 100;
        public int Inteligencia { get; set; }
        public List<string> Inventario { get; set; }

        public virtual bool PonerAtributos(string nombreAtributo, int valor)
        {
            return false;
        }

        public virtual void MostrarAtributos()
        {

        }

        public virtual void MostrarInventario()
        {

        }
    }
}
