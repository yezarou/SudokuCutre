/* =======================================================
 * AUTOR:       Ruben Zúñiga García
 * FECHA:       25/05/2018
 * VERSIÓN:     1.0
 * DESCRIPCIÓN: Sudoku
 * ======================================================= */

using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace WPF_Tema9Ejercicio27
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // ------------------------------------ Campos ------------------------------------
        string[,] tablero = new string[9, 9];
        string[,] solucion = new string[9,9];

        // ------------------------------------ Constructor ------------------------------------
        public MainWindow() {
            InitializeComponent();
            InicializarTablero();
        }

        // ------------------------------------ Métodos ------------------------------------
        private void InicializarTablero() {
            LeerFichero();

            grdCuadricula.Children.Clear();

            for (int i = 0; i < grdCuadricula.RowDefinitions.Count; i++)
                for (int j = 0; j < grdCuadricula.ColumnDefinitions.Count; j++) {
                    // Creamos un texbox para cada cuadricula y lo formateamos.
                    TextBox txt = new TextBox();

                    txt.Margin = new Thickness(1);
                    txt.FontSize = 20;

                    txt.FontWeight = FontWeights.Bold;
                    txt.MaxLength = 1;

                    txt.VerticalContentAlignment = VerticalAlignment.Center;
                    txt.HorizontalContentAlignment = HorizontalAlignment.Center;

                    if (tablero[i, j] != " ") { // Para que no le añada el espacio, será mas cómodo al escribir.
                        txt.IsEnabled = false;
                        txt.Text = tablero[i, j];
                    }
                    else                        // Si tiene la solución escrita, escribe la solución.
                        txt.Text = solucion[i, j];

                    txt.TextChanged += txt_ComprobarCelda;
                    grdCuadricula.Children.Add(txt);

                    Grid.SetRow(txt, i);
                    Grid.SetColumn(txt, j);
                }
        }

        private void LeerFichero() {
            string[] fichero;

            if (cboxDificultad.Text[0] == '2')
                fichero = File.ReadAllLines("dificil.txt");

            else
                fichero = File.ReadAllLines("facil.txt");

            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < 9; j++) {
                    tablero[i, j] = fichero[i].Split(',')[j];
                }
            }
        }

        private void LeerSolucion() {
            string[] fichero = File.ReadAllLines("tablero.txt");

            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < 9; j++) {
                    solucion[i, j] = fichero[i+10].Split(',')[j];
                }
            }
        }

        // ------------------------------------ Eventos ------------------------------------

        private void txt_ComprobarCelda(object sender, TextChangedEventArgs e) {
            TextBox txt = (TextBox)sender;
            int contador = 0;

            int fila = Grid.GetRow(txt);
            int columna = Grid.GetColumn(txt);

            int filaCuadrante = fila / 3;
            int columnaCuadrante = columna / 3;

            while (contador < 9 && txt.Text != null) {
                if (tablero[fila,contador] == txt.Text || tablero[contador,columna] == txt.Text)
                    txt.Text = "";
                contador++;
            }

            for (int i = 3 * filaCuadrante; i < 3 * (1 + filaCuadrante); i++)
                for (int j = 3 * columnaCuadrante; j < 3 * (1 + columnaCuadrante); j++)
                    if (tablero[i,j] == txt.Text && (i != fila || j != columna))
                        txt.Text = "";

            tablero[fila,columna] = txt.Text;
        }

        private void cboxDificultad_DropDownClosed(object sender, EventArgs e) {
            solucion = new string[9, 9];
            InicializarTablero();
        }

        private void btnSolucion_Click(object sender, RoutedEventArgs e) {
            LeerSolucion();
            InicializarTablero();
        }

        private void btnReiniciar_Click(object sender, RoutedEventArgs e) {
            solucion = new string[9, 9];
            InicializarTablero();
        }
    }
}
