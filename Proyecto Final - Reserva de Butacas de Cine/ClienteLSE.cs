using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final___Reserva_de_Butacas_de_Cine
{
    public class ClienteLSE
    {
        public string Nombre { get; set; }
        public string Butacas { get; set; }
        public string TotalAPagar { get; set; }
        public ClienteLSE Siguiente { get; set; }

    }
}
