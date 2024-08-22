using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final___Reserva_de_Butacas_de_Cine
{
    public class Sala
    {
        public ClienteLSE Primero {  get; set; }

        public string BuscarButacas(ClienteLSE Nodo, string Cliente)
        {
            if (Primero.Nombre == Cliente)
            {
                // El nodo de inicio tiene el nombre que se quiere eliminar, actualiza el inicio
                Primero = Primero.Siguiente;
                return Nodo.Butacas;
            }

            while (Nodo.Siguiente != null)
            {
                if (Nodo.Siguiente.Nombre == Cliente)
                {
                    return Nodo.Siguiente.Butacas;
                }
                Nodo = Nodo.Siguiente;
            }

            return "";
        }



        private ClienteLSE BuscarUltimo(ClienteLSE unNodo)
        {
            if(unNodo.Siguiente == null)
            {
                return unNodo;
            }
            else
            {
                return BuscarUltimo(unNodo.Siguiente);
            }
        }


        public void AgregaenListaSE(ClienteLSE nuevoCliente)
        {
          
            if(Primero == null)
            {
                Primero = nuevoCliente;
            }
            else
            {
                ClienteLSE aux = BuscarUltimo(Primero);
                aux.Siguiente = nuevoCliente;

            }
        }

        public void EliminaenPosición(ClienteLSE Nodo, string nombre)
        {
            if (Primero == null)
            {
                // La lista está vacía, no hay nodos que eliminar
                return;
            }

            if (Primero.Nombre == nombre)
            {
                // El nodo de inicio tiene el nombre que se quiere eliminar, actualiza el inicio
                Primero = Primero.Siguiente;
                return;
            }

            ClienteLSE actual = Primero;

            while(actual.Siguiente != null)
            {
                if (actual.Siguiente.Nombre == nombre)
                {
                    actual.Siguiente = actual.Siguiente.Siguiente;
                    return;

                }
                actual = actual.Siguiente;

            }
        }





    }
}
