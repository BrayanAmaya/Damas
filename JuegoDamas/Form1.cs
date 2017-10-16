using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JuegoDamas
{
    public partial class Form1 : Form
    {
        int Turno = 0;
        bool MovExtra = false;
        PictureBox Seleccionado = null;

        List<PictureBox> Azules = new List<PictureBox>();
        List<PictureBox> Rojas = new List<PictureBox>();

        public Form1()
        {
            InitializeComponent();
        }
        private void CargarListas()
        {
            Azules.Add(azul1);
            Azules.Add(azul2);
            Azules.Add(azul3);
            Azules.Add(azul4);
            Azules.Add(azul5);
            Azules.Add(azul6);
            Azules.Add(azul7);
            Azules.Add(azul8);
            Azules.Add(azul9);
            Azules.Add(azul10);
            Azules.Add(azul11);
            Azules.Add(azul12);

            Rojas.Add(roja1);
            Rojas.Add(roja2);
            Rojas.Add(roja3);
            Rojas.Add(roja4);
            Rojas.Add(roja5);
            Rojas.Add(roja6);
            Rojas.Add(roja7);
            Rojas.Add(roja8);
            Rojas.Add(roja9);
            Rojas.Add(roja10);
            Rojas.Add(roja11);
            Rojas.Add(roja12);
        }
        public void Seleccion(object Objeto)
        {
            try { Seleccionado.BackColor = Color.Black; }
            catch
            {

            }
            PictureBox Ficha = (PictureBox)Objeto;
            Seleccionado = Ficha;
            Seleccionado.BackColor = Color.Lime;
        }
        private void CuadroClick(object sender, MouseEventArgs e)
        {
            Movimiento((PictureBox)sender);
        }
        private void Movimiento(PictureBox Cuadro)
        {
            if (Seleccionado != null)
            {
                string color = Seleccionado.Name.ToString().Substring(0, 4);
                if (Validacion(Seleccionado, Cuadro, color))
                {
                    Point Anterior = Seleccionado.Location;
                    Seleccionado.Location = Cuadro.Location;
                    int Avance = Anterior.Y - Cuadro.Location.Y;

                    if (!MovimientosExtras(color) | Math.Abs(Avance) == 50) //Verificar Movimientos Extras
                    {
                        Turno++;
                        Seleccionado.BackColor = Color.Black;
                        Seleccionado = null;
                    }
                    else
                    {
                        MovExtra = true;
                    }
                }
            }
        }

        private bool MovimientosExtras(string color)
        {
            List<PictureBox> BandoContrario = color == "roja" ? Azules : Rojas;
            List<Point> Posiciones = new List<Point>();
            int SigPosicion = color == "roja" ? -100 : 100;

            Posiciones.Add(new Point(Seleccionado.Location.X + 100, Seleccionado.Location.Y + SigPosicion));
            Posiciones.Add(new Point(Seleccionado.Location.X - 100, Seleccionado.Location.Y + SigPosicion));
            if (Seleccionado.Tag == "Queen")
            {
                Posiciones.Add(new Point(Seleccionado.Location.X + 100, Seleccionado.Location.Y + SigPosicion));
                Posiciones.Add(new Point(Seleccionado.Location.X - 100, Seleccionado.Location.Y + SigPosicion));
            }
            bool Resultado = false;
            for (int i = 0; i < Posiciones.Count; i++)
            {
                if (Posiciones[i].X > 50 && Posiciones[i].X <= 400 && Posiciones[i].Y >= 50 && Posiciones[i].Y <= 400)
                {
                    if (!Ocupado(Posiciones[i], Rojas) && !Ocupado(Posiciones[i], Azules))
                    {
                        Point PuntoMedio = new Point(Promedio(Posiciones[i].X, Seleccionado.Location.X), Promedio(Posiciones[i].Y, Seleccionado.Location.Y));
                        if (Ocupado(PuntoMedio, BandoContrario))
                        {
                            Resultado = true;
                        }
                    }
                }
            }
            return Resultado;
        }

        private bool Ocupado(Point punto, List<PictureBox> Bando)
        {
            for (int i = 0; i < Bando.Count; i++)
            {
                if (punto == Bando[i].Location)
                {
                    return true;
                }
            }
            return false;
        }
        private int Promedio(int n1, int n2)
        {
            int Resultado = n1 + n2;
            Resultado = Resultado / 2;
            return Math.Abs(Resultado);
        }

        private bool Validacion(PictureBox Origen, PictureBox Destino, string color)
        {
            Point PuntoOrigen = Origen.Location;
            Point PuntoDestino = Destino.Location;
            int Avance = PuntoOrigen.Y - PuntoDestino.Y;
            Avance = color == "roja" ? Avance : (Avance * -1);
            Avance = Seleccionado.Tag == "queen" ? Math.Abs(Avance) : Avance;
            if (Avance == 50)
            {
                return true;
            }
            else if (Avance == 100)
            {
                Point PuntoMedio = new Point(Promedio(PuntoDestino.X, PuntoOrigen.X), Promedio(PuntoDestino.Y, PuntoOrigen.Y));
                List<PictureBox> BandoContrario = color == "roja" ? Azules : Rojas;
                for (int i = 0; i < BandoContrario.Count; i++)
                {
                    if (BandoContrario[i].Location == PuntoMedio)
                    {
                        BandoContrario[i].Location = new Point(0, 0);
                        BandoContrario[i].Visible = false;
                        return true;
                    }
                }
            }
            return false;
        }

        private void IfQueen(string color)
        {
            if (color == "azul" && Seleccionado.Location.Y == 400)
            {
                Seleccionado.BackgroundImage = Properties.Resources.reyna_azul;
                Seleccionado.Tag = "Queen";
            }
            else if (color == "roja" && Seleccionado.Location.Y == 50)
            {
                Seleccionado.BackgroundImage = Properties.Resources.reyna_roja;
                Seleccionado.Tag = "Queen";
            }
        }
        private void SeleccionRoja(object sender, MouseEventArgs e)
        {
            Seleccion(sender);
        }

        private void SeleccionAzul(object sender, MouseEventArgs e)
        {
            Seleccion(sender);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
