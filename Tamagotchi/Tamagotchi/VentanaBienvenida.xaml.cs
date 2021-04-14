using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tamagotchi
{
    /// <summary>
    /// Lógica de interacción para VentanaBienvenida.xaml
    /// </summary>
    public partial class VentanaBienvenida : Window
    {
        MainWindow padre;
        DispatcherTimer t1;
        public VentanaBienvenida(MainWindow padre)
        {
            InitializeComponent();
            this.padre = padre;
            t1= new DispatcherTimer();
            t1.Interval = TimeSpan.FromMilliseconds(100.0);
            t1.Tick += new EventHandler(reloj);
        }

        private void reloj(object sender, EventArgs e)
        {
            pbHielo.Value += 10;
            if (pbHielo.Value == 100)
            {
                pbFuego.Value += 10;
                if (pbFuego.Value == 100)
                {
                    this.Close();
                }
            }
        }


        private void enviarNombre(object sender, RoutedEventArgs e)
        {
            if(tbNombreTamagotchi.Text != "")
            {
                padre.setNombre(tbNombreTamagotchi.Text);
                t1.Start();
            }
            else
            {
                MessageBox.Show("Debe introducir un nombre", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void cargarSegundoPB(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }

        private void abandonar(object sender, RoutedEventArgs e)
        {
            MessageBoxResult abandonar= MessageBox.Show("¿Realmente desea salir?", "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if(abandonar == MessageBoxResult.Yes)
            {
                this.Close();
                padre.Close();
            }
        }
    }
}
