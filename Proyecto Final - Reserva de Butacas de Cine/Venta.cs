using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Proyecto_Final___Reserva_de_Butacas_de_Cine
{
    public partial class FRMVenta : Form
    {

        private int[,] MatrizListBox = new int[8, 6];
        private int TotalButacas = 48;
        private int Reservadas = 0;


        public FRMVenta()
        {
            InitializeComponent();
        }

        private void FRMVenta_Load(object sender, EventArgs e)
        {
            MatrizListBox = CargarMatriz();
            MostrarMatriz();
            CargarButacas();
            MostrarSala();

            BTNConfirmar.Enabled = false;
        }

        private int[,] CargarMatriz()
        {
            string Sala = "Butacas.txt";
            string[] lineas = File.ReadAllLines(Sala);

            int cantFilas = lineas.Length;
            int cantColu = lineas[0].Split(';').Length;

            int[,] matriz = new int[8, 6];

            for (int i = 0; i < 8; i++)
            {
                string[] values = lineas[i].Split(';');
                for (int j = 0; j < 6; j++)
                {
                    matriz[i, j] = int.Parse(values[j]);
                }
            }
            return matriz;
        }

        private void MostrarMatriz()
        {
            listBoxCeros.Items.Clear();
            listBoxUnos.Items.Clear();

            string Invertido;
            //muestro la matriz en los listbox
            for (int i = 0; i < 8; i++)
            {
                string FilaNormal = "";
                string FilaInvertida = "";
                for (int j = 0; j < 6; j++)
                {
                    FilaNormal += MatrizListBox[i, j].ToString() + "    ";
                    Invertido = (MatrizListBox[i, j] == 1) ? "0" : "1";
                    FilaInvertida += Invertido + "    ";
                }
                listBoxCeros.Items.Add(FilaNormal);
                listBoxUnos.Items.Add(FilaInvertida);
            }
        }

        private void CargarButacas()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    int valor = MatrizListBox[i, j];
                    string buttonName = "btn_butaca" + i.ToString() + j.ToString();
                    Button button = PanelButaca.Controls.Find(buttonName, true).FirstOrDefault() as Button;

                    if (button != null)
                    {
                        switch (valor)
                        {
                            case 1:
                                Reservadas += 1;
                                button.BackColor = Color.Red;
                                break;
                            case 2:
                                Reservadas += 1;
                                button.BackColor = Color.Green;
                                break;
                            default:
                                button.BackColor = Color.PeachPuff;
                                break;
                        }
                    }
                }
            }
        }

        private void GenerarVenta()
        {
            FileStream BT = new FileStream("Butacas.txt", FileMode.Create, FileAccess.Write);
            FileStream VE = new FileStream("Vendidas.txt", FileMode.Create, FileAccess.Write);
            FileStream CL = new FileStream("Clientes.txt", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(CL);
            StreamWriter SWV = new StreamWriter(VE);
            StreamWriter SWB = new StreamWriter(BT);


            foreach (string Fila in listBoxCeros.Items)
            {
                SWB.WriteLine("0;0;0;0;0;0;");
            }
            string lineaCL = SR.ReadLine();
            while (lineaCL != null)
            {
                SWV.WriteLine(lineaCL);
                lineaCL = SR.ReadLine();
            }

            SR.Close();
            SWB.Close();
            SWV.Close();
            BT.Close();
            File.Delete("Clientes.txt");
        }

        private void MostrarSala()
        {
            FileStream CL = new FileStream("Clientes.txt", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader SR = new StreamReader(CL);
            string lineaNT = SR.ReadLine();
            string[] Reserva = new string[0];
            while (lineaNT != null)
            {
                Reserva = lineaNT.Split(';');
                DGVVenta.Rows.Add(Reserva[0], Reserva[1]);
                lineaNT = SR.ReadLine();
            }

            SR.Close();
            CL.Close();
        }

        private string EstadisticasVenta()
        {
            string textoEstadistico = "Quedaron " + (TotalButacas - Reservadas) + " Libres y " + Reservadas + " Ocupadas\n";
            for (int i = 0; i < 8; i++)
            {
                int Facturacion = 0;

                for (int j = 0; j < 6; j++)
                {
                    if (MatrizListBox[i, j] == 2)
                    {
                        if (i > 2)
                        {
                            Facturacion = Facturacion + 70;
                        }
                        else
                        {
                            Facturacion = Facturacion + 100;
                        }
                    }
                }
                if (Facturacion != 0)
                {
                    textoEstadistico = textoEstadistico + "La fila " + (i+1) + " Facturo: " + Facturacion.ToString() + "\n";
                }

            }
            return textoEstadistico;
        }

        private int[] FilaColumna(System.Windows.Forms.Button Butaca)
        {
            int[] PosButaca = new int[2];
            for (int F = 0; F < 8; F++)
            {
                for (int C = 0; C < 6; C++)
                {
                    if (Butaca.Name == "btn_butaca" + F.ToString() + C.ToString())
                    {
                        PosButaca[0] = F;
                        PosButaca[1] = C;
                    }
                }
            }
            return PosButaca;
        }

        private void btn_butaca44_Click(object sender, EventArgs e)
        {
            Button buttonchecked = (Button)sender;
            int[] FilaCol = FilaColumna(buttonchecked);

            string CasillaColor = buttonchecked.BackColor.Name.ToString();

            switch (CasillaColor)
            {
                case "Red":
                    buttonchecked.Enabled = false;
                    MatrizListBox[FilaCol[0], FilaCol[1]] = 2;
                    buttonchecked.BackColor = Color.Green;
                    BTNConfirmar.Enabled = true;
                    break;

                case "Green":
                    buttonchecked .Enabled = false;
                    MessageBox.Show("Butaca confirmada!");
                    break;

                default:
                    break;
            }
            MostrarMatriz();
        }

        private void BTNConfirmar_Click(object sender, EventArgs e)
        {
            LBDatos.Items.Clear();

            string estadisticas = EstadisticasVenta();

            string[] lineasEstadisticas = estadisticas.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (string linea in lineasEstadisticas)
            {
                if (!string.IsNullOrEmpty(linea))
                {
                    LBDatos.Items.Add(linea);
                }
            }

            GenerarVenta();

            PanelButaca.Enabled = false;
            BTNConfirmar.Enabled=false;
            MessageBox.Show("Confirmación guardada!");
        }
    }
}

