using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Proyecto_Final___Reserva_de_Butacas_de_Cine
{
    public partial class FRMReserva : Form
    {
        Sala Reserva = new Sala();  //Se crea una variable "Reserva" de tipo Sala que contiene metodos para agregar y eliminar nodos.

        //Se inicializa en ceros por defecto.
        int[,] matrizlistbox = new int[8, 6];   //Declaración de matriz bidimensional (8 filas y 6 columnas) que almacena valores enteros. 
        
        private string ButacasRes = "";    
        //Declaración de variable "ButacaRes" de tipo string que indica las butacas reservadas.
        private int TotalButacas = 48;     //Declaración de variable "TotalButacas" que indica el total maximo de butacas que hay.
        private int Reservada = 0;    //Declaración de variable "Reservada" para indicar las butacas ya ocupadas.


        public FRMReserva()
        {
            InitializeComponent();

        }

        //EVENTO DE FORMULARIO RESERVA

        private void FRMReserva_Load(object sender, EventArgs e)
        {
            buttonGrabArchivo.Enabled = false;  //El botón se deshabilita porque no hay nada que guardar.
            MostrarMatriz();   //Se muestra la matriz listboxCeros y listboxUnos.
            MostrarLabels();   //Se resetean los labels cada vez que se presiona una butaca. 
        }

        //EVENTO QUE MUESTRA LOS DATOS AL SELECCIONAR UNA BUTACA

        private void MostrarLabels()
        {
            LBLTotal.Text = PrecioButaca();    //Se muestra el precio de cada butaca.

            LBLReservadas.Text = ButacasRes;    //Se muestra las butacas reservadas, segun fila (1-8) y columna (A-F).

            LBLOcupadas.Text = Reservada.ToString();    //Se muestra el numero de butacas reservadas.

            LBLLibres.Text = (TotalButacas - Reservada).ToString();    //Se muestra las butacas libres restando el total de las butacas (en un principio 48) y las butacas ya reservadas.

            LBLFilasO.Text = FilaMasOcupada();  //Se muestra la fila mas ocupada con un metodo.
            
            LBLColumnasO.Text = ColumnaMasOcuapda();    //Se muestra la columna mas ocupada con un metodo.
           
        }

        //EVENTO DE LISTBOXs

        public void MostrarMatriz()    //Método para mostrar la matriz matrizListBox en listBoxCeros y listBoxUnos.
        {
            listBoxCeros.Items.Clear();    //Limpia el listBoxCeros.
            listBoxUnos.Items.Clear();      //Limpia el listBoxUnos.

            string invertido;   //Declaración de variable para cambiar la matriz de ceros a unos con el operador ternario

            for (int i = 0; i < 8; i++)    //Itera a través de las filas de la matriz.
            {
                string filaCeros = "";   //Se crea variable de tipo string llamada filaCeros para guardar la matriz de ceros.
                string filasUnos = "";   //Se crea variable de tipo string llamada filaUnos 

                for (int j = 0; j < 6; j++)    //Itera a través de las columnas de la matriz.
                {
                    filaCeros += matrizlistbox[i, j].ToString() + "    ";  //Se concatena cada elemento de la matrizlistbox convertido a cadena (ToString()) y separado por espacios.
                    invertido = (matrizlistbox[i, j] == 1) ? "0" : "1";    //Se usa el operador ternario para preguntar si cada elemento de la matrizlistbox es 1, como son todos ceros se van a ir cambiando a unos. 
                    filasUnos += invertido + "    ";    //Se concatena cada elemento de "invertido" con un espacio.
                }

                listBoxCeros.Items.Add(filaCeros);   //Se agrega la cadena filaCeros al listBoxCeros mostrando la fila completa de la matriz.
                listBoxUnos.Items.Add(filasUnos);   //Se agrega la cadena filaUnos al listBoxUnos mostrando la fila completa de la matriz.
            }

        }

        //EVENTO DE BOTONES PARA BUTACAS

        private int[] filaColumna(System.Windows.Forms.Button Butaca)   //Se crea un metodo para determinar la posición del boton segun la propiedad NAME, tomando como parametro los Buttons del formulario llamado "Butaca".
        {
            int[] PosButaca = new int[2];   //Se crea una matriz de tipo entera de DOS POSICIONES (FILA | COLUMNA). 
            for(int F = 0; F < 8; F++)    //Itera a través de las filas de la matriz.
            {
                for(int C = 0; C < 6; C++)     //Itera a través de las columnas de la matriz.
                {
                    if(Butaca.Name == "btn_butaca" + F.ToString() + C.ToString())   //Se pregunta si la propiedad NAME de un boton es "btn_butaca" + FILA + COLUMNA.
                    {
                        PosButaca[0] = F;   //Si se cumple se guarda el número de fila (0-7) en la primera posición.
                        PosButaca[1] = C;   //Si se cumple se guarda el número de columna (0-5) en la segunda posición.
                    }
                }
            }
            return PosButaca;   //Devuelve la matriz con esa posición del botón.
        }

        private void DesmarcarCasilla(System.Windows.Forms.Button Butaca, string PosButaca)    //Metodo para desmarcar una casilla (button) y cambiar los datos, tomando como parametro el botón selecioando y la posición de esa butaca.
        {
            CantidadNumericUpDown.Value = CantidadNumericUpDown.Value - 1;    //Se resta 1 al numericUpDown.
            Reservada--;    //Se resta 1 al contador.
            ButacasRes = ButacasRes.Replace(PosButaca + ";", "").Replace(PosButaca, "");    //En "ButacaRes" se guarda una cadena de posiciones de butacas (EJ: 1A;2B;3C) y "PosButaca" representa la posicion de butaca seleccionada (EJ: 2B). "Replace" es un metodo que busca una subcadena en la cadena original y la remplaza por otra subcadena. Acá busca "PosButaca + ';'" ("2B;") y la intenta cambiar por "". Entonces en "ButacaRes" (1A;2B;3C;) se cambiaría a "1A;3C;".   
                                                                //El segundo llamado de Replace es más que nada para seguridad, si hubiera un PosButaca sin ";" se cambiaría igual de todas formas.
            Butaca.BackColor = Color.PeachPuff;    //Se cambia el color del boton seleccionado.
        } 

        private void MarcarCasilla(System.Windows.Forms.Button Butaca, string PosButaca)    //Metodo para marcar casilla (button) y cambiar datos, tomando como parametro el boton selccionado y la posicion de esa butaca.
        {
            ButacasRes = (ButacasRes + PosButaca);    //En "ButacaRes" se guarda la suma de "ButacaRes" (string) y "PosButaca" (string)
            CantidadNumericUpDown.Value = CantidadNumericUpDown.Value + 1;    //Se suma 1 al numericUpDown.
            Reservada++;    //Se suma 1.
            Butaca.BackColor = Color.Red;   //La butaca seleccionada cambia de color a rojo.
        }

        private void btn_butaca48_Click(object sender, EventArgs e)
        {

            //"sender" (de tipo object) es el parámetro que representa el objeto que ha provocado el evento de clic.
            Button clickedButton = (Button)sender;    //En este contexto, sender es convertido explícitamente a tipo Button usando "as Button". SE OBTIENE EL BOTÓN CLICKEADO.
            int[] filaCol = filaColumna(clickedButton);    //Se guarda en la matriz de tipo entera "filacol" la posición obtenida del boton clickeado (EJ: SI SE CLICKEA EL BOTON "btn_butaca00", SE GUARDA EN "filacol" | 0 | 0 |).

            string casillaColor = clickedButton.BackColor.Name.ToString();    //Se crea varibale de tipo string llamada "casillaColor" en la que se guarda el nombre del color del boton seleccionado.
            string posButaca = ((filaCol[0] + 1).ToString() + Convert.ToChar('a' + filaCol[1]).ToString().ToUpper() + ";");    //ESCUCHAME BIEN: Se obtiene el valor de la primera posicion y se le suma 1 para ajustar al formato del usuario (se empieza en 1 (Fila: 1-8)) y se lo convierte a string. Se calcula el carácter segun el valor de la columna, 'a' representa el valor 97 sumando "filacol[1] se obtiene el carácter correspondiente a esa columna. Se convierte a string y a mayúscula. Se concatena todo y al final con un ";""
            //EJ: "filaCol[0]" es 3 y "filaCol[1] es 2" --> filaCol[0] +1 = a '4', 4.ToString() es "'4'". --> 'a' + 2 es igual a 'c' (en cdigo ASCCI 'a' es 97 y sumando 2 es 99, es decir, 'c'), se convierte a cadena y a mayuscula, es decir, "C". --> La concatenación final quedaría como "4C;", representando la fila 4 y la columna C.  
            switch (casillaColor)   //casillaColor es color del boton seleccionado.
            {
                case "Red":    //En el caso de que sea "Red":
                    if(CantidadNumericUpDown.Value != 0)    //Se pregunta si el valor de numericUpDown es distinto de 0.
                    {
                        matrizlistbox[filaCol[0], filaCol[1]] = 0;    //Si se cumple, en esa posición de la matriz se cambia a 0.
                        DesmarcarCasilla(clickedButton, posButaca);    //Se usa un metodo para desmarcar esa casilla.
                        
                    }
                    break;    //Se hace un break.
                case "PeachPuff":   //En el caso de que sea "PeachPuff":
                    matrizlistbox[filaCol[0], filaCol[1]] = 1;    //Si se cumple, en esa posición se cambia a 1.
                    MarcarCasilla(clickedButton, posButaca);    //Se usa un metodo para marcar esa casilla.

                    break;    //Se hace un break.

                case "Green":   //En el caso de que sea "Green":
                    
                    MessageBox.Show("Butaca comprada!");    //Se muestra un mensaje.

                    break;    //Se hace un break.
                default:    //Si ningun caso se cumple:
                    break;    //Se hace un break.
            }
            MostrarMatriz();
            MostrarLabels();

        }
        
        //EVENTO PARA CALCULAR EL PRECIO

        private string PrecioButaca()   //Metodo para sacar el precio de las butacas segun la fila en la que estén.
        {
            int precio = 0;    //Variable de tipo entera "precio". Se va a guardar el total en entero.
            string total = "";    //Variable de tipo string "total". Se va a guardar el total del precio pero como una cadena.
            string[] Butacas = ButacasRes.Split(';');   //Se crea matriz de tipo string "Butacas". Se guardará dividiendo la cadena "ButacaRes" (1A;2B;3C;) por el caraceter ";". 
            //EJ: ButacaRes es "1A;2B;3C". --> La matriz "Butacas" será ["1A" | "2B" | "3C"]
            foreach (string Butaca in Butacas)    //Se itera sobre cada butaca en el arreglo "Butacas".
            {   //EJ Primera Iteración: "Butaca" será "1A".
                if (Butaca != "")   //Se pregunta si la "Butaca" no está vacia.(Evita errores de entradas vacias).
                {
                    int Fila = Int32.Parse(Butaca[0].ToString());   //Se obtiene la fila de la butaca conviertiendo el primer caracter de la cadena a entero.
                    //EJ: Fila será => 1
                    if (Fila > 2)   //Si la fila es mayor a 3, el precio es 70.
                    {   
                        precio = precio + 70;
                    }
                    else   //De lo contrario, es 100.
                    {   //EJ: Como Fila es 1, se suman 100.
                        precio = precio + 100;
                    }
                }
                
            }

            total = "$" + precio.ToString();    //Se convierte el preci total a cadena y se agrega "$"
            return total;   //Retorna el total como cadena.
        }

        //EVENTO DE FILA MAS OCUPADA Y COLUMNA MAS OCUPADA

        private string FilaMasOcupada()    //Metodo para determinar la fila mas ocupada.
        {
            int maxFila = 0;    //Variable para almacenar el número máximo de butacas ocupadas en una fila.
            int maxFilaID = 0;  //Variable para almacenar el ID de la fila más ocupada.


            for (int i = 0; i < 8; i++)    //Itera a través de las 8 filas.
            {
                int Reservadas = 0;    //Contador para el número de butacas ocupadas en la fila actual.
                for (int j = 0; j < 6; j++)    //Itera a través de las 6 columnas.
                {
                    if (matrizlistbox[i, j] == 1 || matrizlistbox[i,j] == 2)    //Se pregunta si la butaca está reservada (valor 1) o comprada (valor 2).
                    {
                        Reservadas++;   //Si se cumple se incrementa el contador de butacas ocupadas.
                    }
                    if (Reservadas > maxFila)   //Se pregunta si el número de butacas ocupadas en la fila actual es mayor que el máximo registrado.
                    {
                        maxFila = Reservadas;    //Actualiza el máximo de butacas ocupadas.
                        maxFilaID = i;    //Actualiza el ID de la fila más ocupada.
                    }
                }
            }
            return ("Fila " + (maxFilaID + 1) + " con " + maxFila + " butacas ocupadas");   //Retorna una cadena con la fila más ocupada y el número de butacas ocupadas.
        }

        private string ColumnaMasOcuapda()
        {
            int maxcolumna = 0;    //Variable para almacenar el número máximo de butacas ocupadas en una columna.
            int maxcolumnaID = 0;   //Variable para almacenar el ID de la columna más ocupada.


            for (int j = 0; j < 6; j++)    //Itera a través de las 6 columnas.
            {
                int Reservadas = 0;    //Contador para el número de butacas ocupadas en la columna actual.


                for (int i = 0; i < 8; i++)    //Itera a través de las 8 filas.
                {
                    if (matrizlistbox[i,j] == 1 || matrizlistbox[i,j] == 2)    //Si la butaca está reservada (valor 1) o comprada (valor 2).
                    {
                        Reservadas++;   //Incrementa el contador de butacas ocupadas.
                    }
                    if (Reservadas > maxcolumna)     //Si el número de butacas ocupadas en la columna actual es mayor que el máximo registrado.
                    {
                        maxcolumna = Reservadas;    //Actualiza el máximo de butacas ocupadas.
                        maxcolumnaID = j;    //Actualiza el ID de la columna más ocupada.
                    }
                }
            }
            return ("Columna " + Convert.ToChar('a' + maxcolumnaID).ToString().ToUpper() + " con " + maxcolumna + " butacas ocupadas");   

        }



        //EVENTO DIRECCION DE LA BUTACA

        private void HabilitarDeshabilitarSeleccion(bool Habilitar)
        {
            string[] Butacas = ButacasRes.Split(';');
            string btn_butaca = "";

            foreach(string Butaca in Butacas)
            {
                if (Butaca != "")
                {
                    int Fila = Int32.Parse(Butaca[0].ToString()) - 1;
                    int Columna = Butaca[1] - 'A';
                    btn_butaca = "btn_butaca"+Fila.ToString()+Columna.ToString();
                    Button ButacaReservada = PanelButaca.Controls.Find(btn_butaca, true).FirstOrDefault() as Button;

                    if(Habilitar == true)
                    {
                        ButacaReservada.Enabled = true;
                        ButacaReservada.BackColor = Color.PeachPuff;
                    }
                    else
                    {
                        ButacaReservada.Enabled=false;
                    }

                }
            }
        }

        //EVENTO DE GRABAR Y RECUPERAR

        private int[,] CargarMatriz()
        {
            string Sala = "Butacas.txt";
            string[] lineas = File.ReadAllLines(Sala);


            int cantidadFilas = lineas.Length;
            int cantidadColumnas = lineas[0].Split(';').Length;

            int[,] matriz = new int[8,6];

            for (int i = 0; i < 8; i++)
            {
                string[] values = lineas[i].Split(';');
                for (int j = 0; j < 6; j++)
                {
                    matriz[i,j] = int.Parse(values[j]);
                }
            }
            return matriz;
        }
        
        private void GuardarButacas()
        {
            FileStream CL = new FileStream("Butacas.txt", FileMode.Create, FileAccess.Write);
            StreamWriter SW = new StreamWriter(CL);

            foreach (string Fila in listBoxCeros.Items)
            {

                string formattedRow = string.Join(";", Fila.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

                SW.WriteLine(formattedRow);
            }

            SW.Close();
            CL.Close();
        }

        private void CargarButacas()
        {
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 6; j++)
                {
                    int valor = matrizlistbox[i, j];
                    string buttonName = "btn_butaca" + i.ToString() + j.ToString();
                    Button button = PanelButaca.Controls.Find(buttonName, true).FirstOrDefault() as Button;

                    if (button != null)
                    {
                        switch (valor)
                        {
                            case 1:
                                Reservada += 1;
                                button.BackColor = Color.Red;
                                break;
                            case 2:
                                Reservada += 1;
                                button.BackColor = Color.Green;
                                break;
                            default:
                                button.BackColor = Color.PeachPuff;
                                break;
                        }
                    }
   
                }
            }
            MostrarLabels();
        }

        private void CargarClientes()
        {
            FileStream CL = new FileStream("Clientes.txt", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader SR = new StreamReader(CL);

            string lineaCL = SR.ReadLine();
            string[] Cliente = new string[0];

            while (lineaCL != null)
            {
                Cliente = lineaCL.Split(';');
                DGVInfo.Rows.Add(Cliente[0], Cliente[1], Cliente[2]);

                ClienteLSE nuevaReseva = new ClienteLSE();

                nuevaReseva.Nombre = Cliente[0];
                nuevaReseva.Butacas = Cliente[1];
                nuevaReseva.TotalAPagar = Cliente[2];

                Reserva.AgregaenListaSE(nuevaReseva);

                lineaCL = SR.ReadLine();
            }
            SR.Close();
            CL.Close();

        }

        private void buttonGrabArchivo_Click(object sender, EventArgs e)
        {
            GuardarButacas();

            FileStream CL = new FileStream("Clientes.txt", FileMode.Create, FileAccess.Write);
            StreamWriter SW = new StreamWriter(CL);

            ClienteLSE nuevaReserva = new ClienteLSE();

            nuevaReserva = Reserva.Primero;

            while(nuevaReserva != null)
            {
                SW.WriteLine(nuevaReserva.Nombre + ";" + nuevaReserva.Butacas.Replace(";", ",") + ";" + nuevaReserva.TotalAPagar);
                nuevaReserva = nuevaReserva.Siguiente;
            }
            SW.Close();
            CL.Close();

            FRMVenta venta = new FRMVenta();
            venta.Show();
            Hide();
        }
        
        private void buttonRecuArchivo_Click(object sender, EventArgs e)
        {
            matrizlistbox = CargarMatriz();
            CargarButacas();
            CargarClientes();
            MostrarMatriz();

            MessageBox.Show("Archivo Cargado!");
        }

        //EVENTO DE BOTÓN DE RESERVAR Y CANCELAR

        private void AgregarItemALista(ClienteLSE nodo)
        {
            if(nodo != null)    //Si hay un nodo, se agrerga al DGVInfo.
            {
                DGVInfo.Rows.Add(nodo.Nombre, nodo.Butacas, nodo.TotalAPagar);
                AgregarItemALista(nodo.Siguiente);
            }
        }

        private void MostrarInfo()
        {
            DGVInfo.Rows.Clear();   //Se limpia el DGVInfo

            if(Reserva.Primero != null)
            {
                AgregarItemALista(Reserva.Primero);
            }
        }

        private void ResetearMatriz(string butacasReservadas)
        {
            string[] butacas = butacasReservadas.Split(';');

            foreach (string butaca in butacas)
            {
                if (!string.IsNullOrWhiteSpace(butaca))
                {
                    int fila = int.Parse(butaca[0].ToString()) - 1;
                    int columna = butaca[1] - 'A';
                    matrizlistbox[fila, columna] = 0; // Resetear la butaca a 0 (desocupada)
                }
            }

            Reservada -= butacas.Length - 1;     // Actualizar las variables de reservación
        }

        private void buttonReservar_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0 && LBLReservadas.Text.Length > 0)
            {
                ClienteLSE nuevaReserva = new ClienteLSE();
                
                nuevaReserva.Nombre = textBox1.Text;
                nuevaReserva.Butacas = ButacasRes;
                nuevaReserva.TotalAPagar = PrecioButaca();

                Reserva.AgregaenListaSE(nuevaReserva);
                HabilitarDeshabilitarSeleccion(false);
                MostrarInfo();

                ButacasRes = "";
                LBLReservadas.Text = ButacasRes;

                buttonGrabArchivo.Enabled = true;
                CantidadNumericUpDown.Value = 0;
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Debe ingresar el nombre y las butacas!");
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0)
            {
                string nombre = textBox1.Text;

                ButacasRes = Reserva.BuscarButacas(Reserva.Primero, nombre);
                Reserva.EliminaenPosición(Reserva.Primero, nombre);
                HabilitarDeshabilitarSeleccion(true);
                LBLReservadas.Text += ButacasRes;

                ResetearMatriz(ButacasRes);
                MostrarMatriz();
                MostrarInfo();

                ButacasRes = "";
                LBLReservadas.Text = "";
            }
            else
            {
                MessageBox.Show("Debe indicar un nombre primero!");
            }
        }
    }
}
