using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Media;
using System.Data.OleDb;
using System.Data;

namespace Tamagotchi
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        DispatcherTimer t1;
        int decremento = 2;
        int puntos = 0;
        int puntosIniciales = 0;
        string nombre;
        Boolean comenzar = false;
        int animacionesActivas = 0;
        bool animacionJugarActiva = false;
        bool animacionDescansarActiva = false;
        bool animacionAlimentarActiva = false;
        bool animacionCansado = false;
        bool animacionAburrido = false;
        bool animacionHambriento = false;
        bool heComido, heJugado, heDescansado = false;
        int animacionRecargaTiempo = 0;
        string rutaBBDD;
        OleDbConnection myconect;

        public MainWindow()
        {
            InitializeComponent();
            t1 = new DispatcherTimer();
            t1.Interval = TimeSpan.FromMilliseconds(1000.0);
            t1.Tick += new EventHandler(reloj);
            //t1.Start();
            VentanaBienvenida pantallaInicio = new VentanaBienvenida(this);//lanzar nueva ventana bienvenida. El this es de referencia
            pantallaInicio.ShowDialog(); //a diferencia del show es que no deja interactuar como lo de detras
        }

        private void tercerNivel()
        {
            MessageBoxResult comienzo = MessageBox.Show("!Por fin! Has llegado a desembarco del rey y el trono es tuyo. ¿Seras capaz de " +
                "mantenerlo durante mucho tiempo? Sigue sumando puntos para hacerte mas fuerte, nadie te parara!\n Ya se encuentra disponible un nuevo paisaje," +
                "Desembarco del Rey", "El trono",
                MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void segundoNivel()
        {
            MessageBoxResult comienzo = MessageBox.Show("!Enhorabuena! Has conseguido avanzar de nivel consiguiendo llegar a Aguas Dulces, el trono" +
                " cada vez esta mas cerca ¿Te vas a rendir ahora? ¡A por el trono!\n Ya se encuentra disponible un nuevo paisaje, Aguas Dulces", "Segunda parada, Aguas Dulces",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void comienzo()
        {
            MessageBoxResult comienzo = MessageBox.Show("!Hola! ¿Preparado para esta trepidante aventura? ¡Tendras la oportunidad de hacer" +
                "fuerte a tu dragon con el fin de llegar a Desembarco del Rey desde la gran y fría Invernalia para conseguir el gran preciado" +
                "Trono de Hierro! Deberas hacerte fuerte y recorrer los distintos pasajes que te llevaran hacia tu objetivo, el trono. Consigue " +
                "cada vez mas puntos para avanzar de niveles y hacer mas fuerte a tu dragon para la batalla final. ¿Estas listo?", "La historia comienza",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void primerNivel()
        {
            MessageBoxResult comienzo = MessageBox.Show("Tu aventura ha comenzado, te encuentras en invernalia, un lugar " +
                "frío donde necesitas coger fuerzas para llegar a Aguas Dulces, tu próxima parada, donde te encontraras apenas" +
                " a un paso de Desembarco del Rey donde se encuentra el gran trono de hierro\n Ya se encuentra disponible el fondo con" +
                " el paisaje de invernalia para que puedas adentrarte aun mas en esta historia", "La historia comienza", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private Boolean comprobarJugadorExiste()
        {
            OleDbCommand mycommand;
            mycommand = myconect.CreateCommand();
            mycommand.CommandText = "SELECT * FROM Jugadores WHERE Nombre='" + nombre.ToLower() + "'";
            mycommand.CommandType = CommandType.Text;
            OleDbDataReader DBreader = mycommand.ExecuteReader();
            if (!DBreader.Read())
            {
                mycommand.CommandText = "SELECT MAX(Ranking) as maxRanking FROM Jugadores";
                DBreader.Close();
                OleDbDataReader DBreader2 = mycommand.ExecuteReader();
                if (DBreader2.Read())
                {
                    int maxRanking = Convert.ToInt32(DBreader2["maxRanking"]);
                    mycommand.CommandText = "INSERT INTO Jugadores VALUES ('"+nombre.ToLower()+"', 0, 'Nivel 1',"+(maxRanking+1)+")";
                    DBreader2.Close();
                    OleDbDataReader DBreader3 = mycommand.ExecuteReader();
                }
                return false;
            }
            else
            {
                return true;
            }
        }



        private void reloj(object sender, EventArgs e)
        {
            //var seed = Environment.TickCount;
            //var random = new Random(seed);
            //var value = 1;//random.Next(0, 1);
            animacionRecargaTiempo++;
            this.tbPuntos.Text = "Puntos: " + (puntos);
            if (pbEnergia.Value <= 0 || pbDiversion.Value <= 0 || pbAlimento.Value <= 0)
            {
                this.tbPuntos.Visibility = Visibility.Visible;
                this.btAlimentar.IsEnabled = false;
                this.btDescansar.IsEnabled = false;
                this.btJugar.IsEnabled = false;
                Storyboard sbFinal = (Storyboard)this.Resources["animacionFin"];
                if (animacionesActivas <= 0)
                {
                    Console.WriteLine("ENTRO" + animacionesActivas);
                    t1.Stop();
                    sbFinal.Begin();
                }
            }
            else
            {
                this.pbEnergia.Value -= decremento;
                this.pbDiversion.Value -= decremento;
                this.pbAlimento.Value -= decremento;
                puntos += decremento;
                if ((animacionRecargaTiempo % 10) == 0)
                {
                    actualizarRanking();
                    cargarRanking();
                    actualizarNivel();
                    actualizarLogrosPremios();
                    Storyboard sbRecarga = (Storyboard)this.Resources["animacionRecarga"];
                    sbRecarga.Begin();
                }

            }

        }


        private void btJugar_Click(object sender, RoutedEventArgs e)
        {
            this.btAlimentar.IsEnabled = false;
            this.btDescansar.IsEnabled = false;
            this.btJugar.IsEnabled = false;
            heJugado = true;
            Storyboard sbAnimacionJugar = (Storyboard)this.Resources["animacionJugar"];
            sbAnimacionJugar.Completed += new EventHandler(finJugar);
            sbAnimacionJugar.AutoReverse = true;
            sbAnimacionJugar.Begin(this, true);
            animacionJugarActiva = true;
            animacionesActivas++;
           

            this.pbDiversion.Value += decremento * 2;
            //decremento += 1;
        }

        private void finJugar(object sender, EventArgs e)
        {
            this.btAlimentar.IsEnabled = true;
            this.btDescansar.IsEnabled = true;
            this.btJugar.IsEnabled = true;
            animacionJugarActiva = false;
            animacionesActivas--;
        }

        private void btDescansar_Click(object sender, RoutedEventArgs e)
        {
            this.btAlimentar.IsEnabled = false;
            this.btDescansar.IsEnabled = false;
            this.btJugar.IsEnabled = false;
            heDescansado = true;
            Storyboard sbAnimacionDescansar = (Storyboard)this.Resources["animacionDescansar"];
            sbAnimacionDescansar.Completed += new EventHandler(finDescansar);
            sbAnimacionDescansar.AutoReverse = true;
            sbAnimacionDescansar.Begin();
            animacionDescansarActiva = true;
            animacionesActivas++;

            this.pbEnergia.Value += decremento * 2;
            //decremento += 1;
        }

        private void finDescansar(object sender, EventArgs e)
        {
            this.btAlimentar.IsEnabled = true;
            this.btDescansar.IsEnabled = true;
            this.btJugar.IsEnabled = true;
            animacionDescansarActiva = false;
            animacionesActivas--;
        }

        private void btAlimentar_Click(object sender, RoutedEventArgs e)
        {
            this.btAlimentar.IsEnabled = false;
            this.btDescansar.IsEnabled = false;
            this.btJugar.IsEnabled = false;
            heComido = true;
            Storyboard sbAnimacionComer = (Storyboard)this.Resources["animacionComer"];
            sbAnimacionComer.Completed += new EventHandler(finComer);
            sbAnimacionComer.AutoReverse = true;
            sbAnimacionComer.Begin();
            animacionAlimentarActiva = true;
            animacionesActivas++;
            this.pbAlimento.Value += decremento * 2;
            //decremento += 1;
        }

        private void finComer(object sender, EventArgs e)
        {
            this.btAlimentar.IsEnabled = true;
            this.btDescansar.IsEnabled = true;
            this.btJugar.IsEnabled = true;
            animacionAlimentarActiva = false;
            animacionesActivas--;

        }

        private void cambiarFondo(object sender, MouseButtonEventArgs e)
        {
            imFondo.Source = ((Image)sender).Source; //imFondo2.source pero no es generico
        }

        private void acercaDe(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("Programa realizado por\n\n Alejandro Riquelme");
            MessageBoxResult resultado = MessageBox.Show("Programa realizado por\n\n Alejandro Riquelme", "Acerca De", MessageBoxButton.YesNo);
            switch (resultado)
            {
                case MessageBoxResult.Yes:
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;

            }
        }

        public void setNombre(string nombre)
        {
            this.nombre = nombre;
            tbPuntos.Text = "Bienvenido " + nombre;

        }

        private void inicioArrastrarGorro(object sender, MouseButtonEventArgs e)
        {
            DataObject dobj = new DataObject((Image)sender);
            DragDrop.DoDragDrop((Image)sender, dobj, DragDropEffects.Move);
        }

        private void colocarColeccionable(object sender, DragEventArgs e)
        {
            Image aux = (Image)e.Data.GetData(typeof(Image));
            switch (aux.Name)
            {
                case "imGorro":
                    imComplementoNavidad.Visibility = Visibility.Visible;
                    imComplementoRey.Visibility = Visibility.Hidden;

                    break;
                case "imTatuaje":
                    imComplementoTatuaje.Visibility = Visibility.Visible;
                    break;
                case "imCorona":
                    imComplementoNavidad.Visibility = Visibility.Hidden;
                    imComplementoRey.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void cambioEnergia(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Storyboard sbEnergia = (Storyboard)this.Resources["animacionCansado"];
            if (this.pbEnergia.Value <= 40)
            {
                if(animacionCansado == false)
                {
                    sbEnergia.Begin();
                    animacionCansado = true;
                }
                if (this.pbEnergia.Background == Brushes.White)
                {
                    this.pbEnergia.Background = Brushes.Red;
                }
                else
                {
                    this.pbEnergia.Background = Brushes.White;
                }
            }
            else if (this.pbEnergia.Value > 40)
            {
                if (animacionCansado == true)
                {
                    sbEnergia.Stop();
                    animacionCansado = false;
                }
                this.pbEnergia.Background = Brushes.White;
            }
        }

        private void cambioDiversion(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Storyboard sbDiversion = (Storyboard)this.Resources["animacionAburrido"];

            if (this.pbDiversion.Value <= 40)
            {
                if(animacionAburrido == false)
                {
                    sbDiversion.Begin();
                    animacionAburrido = true;
                }
                if (this.pbDiversion.Background == Brushes.White)
                {
                    this.pbDiversion.Background = Brushes.Red;
                }
                else
                {
                    this.pbDiversion.Background = Brushes.White;
                }
            }
            else if (this.pbDiversion.Value > 40)
            {
                if (animacionAburrido == true)
                {
                    sbDiversion.Stop();
                    animacionAburrido = false;
                }
                this.pbDiversion.Background = Brushes.White;
            }
        }

        private void cambioAlimento(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Storyboard sbAlimento = (Storyboard)this.Resources["animacionHambriento"];

            if (this.pbAlimento.Value <= 40)
            { 
                if(animacionHambriento == false)
                {
                    sbAlimento.Begin();
                    animacionHambriento = true;
                }
                if (this.pbAlimento.Background == Brushes.White)
                {
                    this.pbAlimento.Background = Brushes.Red;
                }
                else
                {
                    this.pbAlimento.Background = Brushes.White;
                }
            }
            else if (this.pbAlimento.Value > 40)
            {
                if (animacionHambriento == true)
                {
                    sbAlimento.Stop();
                    animacionHambriento = false;
                }
                this.pbAlimento.Background = Brushes.White;
            }
        }

        private void realizarRecarga(object sender, MouseButtonEventArgs e)
        {
            this.pbAlimento.Value = this.pbAlimento.Value + 10;
            this.pbDiversion.Value = this.pbDiversion.Value + 10;
            this.pbEnergia.Value = this.pbEnergia.Value + 10;

        }

        private void cambiarDificultad(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            decremento = (int)this.slNiveles.Value;
            tbDificultad.Text = "Dificultad: " + (int)this.slNiveles.Value;
        }

        private void cargarBBDD(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".accdb";
            Nullable<bool> result = ofd.ShowDialog();

            if (result == true && ofd.FileName.Contains(".accdb"))
            {
                rutaBBDD = ofd.FileName;
                myconect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = 
               " + rutaBBDD);
                myconect.Open();
                comenzar = true;
                // C:\Users\aleja\source\repos\Tamagotchi\Tamagotchi\bin\Debug\jugadores.accdb
                if (comprobarJugadorExiste() == false)
                {
                    comienzo();

                }
                actualizarLogrosPremios(); 
                cargarRanking();
                actualizarNivel();

            }
            else
            {
                MessageBoxResult comienzo = MessageBox.Show("Archivo incorrecto.", "Error",
               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cargarRanking()
        {
            int posicionJugador = obtenerPosicionJugador();
            OleDbCommand mycommand;
            mycommand = myconect.CreateCommand();
            mycommand.CommandText = "SELECT Ranking, Nombre, Puntos FROM Jugadores ORDER BY Ranking ASC";
            mycommand.CommandType = CommandType.Text;
            OleDbDataReader DBreader = mycommand.ExecuteReader();
            spRanking.Children.Clear();
            while (DBreader.Read())
            {
                if (Convert.ToInt32(DBreader["Ranking"]) == 1 || (Convert.ToInt32(DBreader["Ranking"]) >= (posicionJugador - 2) && Convert.ToInt32(DBreader["Ranking"]) <= (posicionJugador + 2)))
                {
                    TextBox tbranking = new TextBox();
                    tbranking.Foreground = Brushes.Black;
                    tbranking.FontSize = 16;
                    tbranking.Text = "#" + Convert.ToString(DBreader["Ranking"]) + " " + Convert.ToString(DBreader["Nombre"]) + " -- " + Convert.ToString(DBreader["Puntos"]);
                    spRanking.Children.Add(tbranking);
                                        
                    if ((Convert.ToString(DBreader["Nombre"]).ToLower()) == nombre.ToLower())
                    {
                        tbranking.Background = Brushes.Cyan;
                        if (Convert.ToInt32(DBreader["Ranking"]) == 1)
                        {
                            tbranking.Background = Brushes.Yellow;
                        }
                    }
                    else if(Convert.ToInt32(DBreader["Ranking"]) == 1){
                        tbranking.Background = Brushes.Yellow;
                    }
                }
                

            }
        }
        private int obtenerPosicionJugador()
        {
            OleDbCommand mycommand;
            mycommand = myconect.CreateCommand();
            mycommand.CommandText = "SELECT Ranking FROM Jugadores WHERE Nombre='"+nombre.ToLower()+"'";
            mycommand.CommandType = CommandType.Text;
            OleDbDataReader DBreader = mycommand.ExecuteReader();
            if (DBreader.Read())
            {
                return Convert.ToInt32(DBreader["Ranking"]);
            }
            return -1;

        }

        private int obtenerPuntos()
        {
            int puntosActuales = -1;
            OleDbCommand mycommand;
            mycommand = myconect.CreateCommand();
            mycommand.CommandType = CommandType.Text;
            mycommand.CommandText = "SELECT Puntos FROM Jugadores WHERE Nombre='" + nombre.ToLower() + "'";
            OleDbDataReader DBreader = mycommand.ExecuteReader();
            if (DBreader.Read())
            {
                if(puntosIniciales == 0)
                {
                    puntosIniciales = Convert.ToInt32(DBreader["Puntos"]);
                }
                else
                {
                    puntosActuales = Convert.ToInt32(DBreader["Puntos"]);
                }
                DBreader.Close();
            }
            return puntosActuales;
        }

        private int actualizarPuntosPropios()
        {
            int puntosActuales = -1;
            OleDbCommand mycommand;
            mycommand = myconect.CreateCommand();
            mycommand.CommandType = CommandType.Text;
            //mycommand.CommandText = "SELECT Puntos FROM Jugadores WHERE Nombre='" + nombre.ToLower() + "'";
            //OleDbDataReader DBreader = mycommand.ExecuteReader();
            //if (DBreader.Read())
            //{
                puntosActuales = puntosIniciales + puntos;
                //DBreader.Close();
                mycommand.CommandText = "UPDATE Jugadores SET Puntos=" + puntosActuales + " WHERE Nombre='" + nombre.ToLower() + "'";
                mycommand.ExecuteReader();
                //DBreader.Close();
                
            //}
            return puntosActuales;
        }

        private int actualizarRankingPropio(int puntosActuales)
        {
            int rankingActual = -1;
            OleDbCommand mycommand;
            mycommand = myconect.CreateCommand();
            mycommand.CommandType = CommandType.Text;
            mycommand.CommandText = "SELECT Nombre, Ranking FROM Jugadores WHERE Puntos>" + puntosActuales + " ORDER BY Puntos ASC";
            OleDbDataReader DBreader = mycommand.ExecuteReader();
            if (DBreader.Read())
            {
                Console.WriteLine("ESTE TIENE MAS PUNTOS QUE YO"+Convert.ToString(DBreader["Nombre"])+"ASI QUE ME PONGO"+ (Convert.ToInt32(DBreader["Ranking"]) + 1));
                rankingActual = Convert.ToInt32(DBreader["Ranking"])+1;
                DBreader.Close();
                Console.WriteLine(rankingActual +" "+ obtenerPosicionJugador());
                if (rankingActual != obtenerPosicionJugador())
                {
                    //Console.WriteLine("UPDATE Jugadores SET Ranking=" + rankingActual + " WHERE Nombre='" + nombre.ToLower() + "'");
                    mycommand.CommandText = "UPDATE Jugadores SET Ranking=" + rankingActual + " WHERE Nombre='" + nombre.ToLower() + "'";
                    //Console.WriteLine("ME ACABO DE PONER A ESTO: " + obtenerPosicionJugador());
                    int cambios = mycommand.ExecuteNonQuery();//OleDbDataReader DBreader3 = 
                }
            }
            else //top 1
            {
                if (rankingActual != obtenerPosicionJugador())
                {
                    //Console.WriteLine("UPDATE Jugadores SET Ranking=" + 1 + " WHERE Nombre='" + nombre.ToLower() + "'");
                    mycommand.CommandText = "UPDATE Jugadores SET Ranking=" + 1 + " WHERE Nombre='" + nombre.ToLower() + "'";
                    //Console.WriteLine("ME ACABO DE PONER A ESTO: " + obtenerPosicionJugador());
                    DBreader.Close();
                    int cambios = mycommand.ExecuteNonQuery();//OleDbDataReader DBreader3 = 

                }
            }
            return rankingActual;
        }

        private void actualizarRankingResto(int puntos, int ranking)
        {
            
            OleDbCommand mycommand;
            mycommand = myconect.CreateCommand();
            mycommand.CommandType = CommandType.Text;
            Console.WriteLine("SELECT Nombre, Ranking FROM Jugadores WHERE Puntos<" + puntos);
            mycommand.CommandText = "SELECT Nombre, Ranking FROM Jugadores WHERE Puntos<=" + puntos;
            OleDbDataReader DBreader = mycommand.ExecuteReader();
            while (DBreader.Read())
            {
                int rankingActualJugador = Convert.ToInt32(DBreader["Ranking"]);
                String nombreActualJugador = Convert.ToString(DBreader["Nombre"]).ToLower();
                Console.WriteLine(nombreActualJugador + " " + rankingActualJugador);
                //Console.WriteLine("UPDATE Jugadores SET Ranking=" + (rankingActualJugador + 1) + " WHERE Nombre='" + Convert.ToString(DBreader["Nombre"]).ToLower() + "'");
                OleDbCommand mycommand2;
                mycommand2 = myconect.CreateCommand();
                mycommand2.CommandType = CommandType.Text;
                if((rankingActualJugador+1)<= ranking && Convert.ToString(DBreader["Nombre"]).ToLower() != nombre) //Para que solo se actualicen los que estan entre el nuevo ranking y el anterior
                    // y al haber puesto <= que no se actualice el mismo
                {
                    mycommand2.CommandText = "UPDATE Jugadores SET Ranking=" + (rankingActualJugador + 1) + " WHERE Nombre='" + nombreActualJugador + "'";
                    mycommand2.ExecuteReader();
                }

               // DBreader.Close();
            }
            DBreader.Close();
        }
        private void actualizarRanking()
        {
            //Actualizar Puntos
            int puntos= actualizarPuntosPropios();
            //Actualizar Ranking (Top) propio
            int anteriorRanking = obtenerPosicionJugador();
            int ranking= actualizarRankingPropio(puntos);

            //Actualizar Ranking (Top) del resto
            if (ranking!= anteriorRanking) //para que solo se actualice el resto si ha cambiado realmente ya que si no el top del resto va a subir sin ser necesario
            {
                actualizarRankingResto(puntos, anteriorRanking);
            }
        }

        private void comenzarJuego(object sender, RoutedEventArgs e)
        {
            if (comenzar == true)
            {
                t1.Start();
                btDescansar.IsEnabled = true;
                btJugar.IsEnabled = true;
                btAlimentar.IsEnabled = true;
                obtenerPuntos();
            }
            else
            {
                MessageBoxResult comienzo = MessageBox.Show("Debe cargar antes la base de datos", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void actualizarLogrosPremios()
        {
            if(heComido==true && heDescansado==true && heJugado == true && imTatuaje.Visibility == Visibility.Hidden)
            {
                //desbloquear tatuaje
                tbLogro1.BorderBrush = Brushes.DarkSeaGreen;
                tbLogro1.Background = Brushes.LightGreen;
                imTatuaje.Visibility = Visibility.Visible;

                tbColeccionableTatuaje.BorderBrush = Brushes.DarkSeaGreen;
                tbColeccionableTatuaje.Background = Brushes.LightGreen;

            }

            if(obtenerNivel()=="Nivel 3" && imGorro.Visibility == Visibility.Hidden)
            {
                //desbloquear gorro de navidad
                tbLogro2.BorderBrush = Brushes.DarkSeaGreen;
                tbLogro2.Background = Brushes.LightGreen;
                imGorro.Visibility = Visibility.Visible;

                tbColeccionableGorroNavidad.BorderBrush = Brushes.DarkSeaGreen;
                tbColeccionableGorroNavidad.Background = Brushes.LightGreen;

            }
            if (obtenerPosicionJugador() == 1 && imCorona.Visibility == Visibility.Hidden)
            {
                //desbloquear corona y desembarco
                //logro top 1
                TextBox tbTopUno = new TextBox();
                tbTopUno.Text = "Alcanza el Top 1";
                tbTopUno.Foreground = Brushes.White;
                tbTopUno.FontSize = 16;
                tbTopUno.BorderBrush = Brushes.DarkSeaGreen;
                tbTopUno.Background = Brushes.LightGreen;
                tbTopUno.BorderThickness = new Thickness(5.0);
                tbTopUno.FontWeight = FontWeights.Bold;
                tbTopUno.Height = 57;
                spLogros.Children.Add(tbTopUno);

                //coleccionable corona premio
                tbColeccionableCorona.Text = "Coleccionable: Corona";
                tbColeccionableCorona.Foreground = Brushes.White;
                tbColeccionableCorona.FontSize = 16;
                tbColeccionableCorona.BorderBrush = Brushes.DarkSeaGreen;
                tbColeccionableCorona.Background = Brushes.LightGreen;
                tbColeccionableCorona.BorderThickness = new Thickness(5.0);
                tbColeccionableCorona.FontWeight = FontWeights.Bold;
                //spLogros.Children.Add(tbColeccionableCorona);
                tbColeccionableCorona.Visibility = Visibility.Visible;

                imCorona.Visibility = Visibility.Visible;

            }

        }


        private String obtenerNivel()
        {
            String nivelActual = "";
            OleDbCommand mycommand;
            mycommand = myconect.CreateCommand();
            mycommand.CommandType = CommandType.Text;
            mycommand.CommandText = "SELECT Nivel FROM Jugadores WHERE Nombre='" + nombre.ToLower() + "'";
            OleDbDataReader DBreader = mycommand.ExecuteReader();
            if (DBreader.Read())
            {
                nivelActual = Convert.ToString(DBreader["Nivel"]);
                DBreader.Close();
            }
            return nivelActual;
        }

        private void actualizarNivel()
        {
            String nivel="";
            OleDbCommand mycommand;
            mycommand = myconect.CreateCommand();
            mycommand.CommandType = CommandType.Text;
            int puntos = obtenerPuntos();
            if (puntos > 0 && puntos < 100)
            {
                nivel = "Nivel 1";
                imInvernalia.Visibility = Visibility.Visible;
                if (obtenerNivel()!= nivel)
                {
                    primerNivel();
                }
            }
            else if (puntos >= 100 && puntos < 200)
            {
                nivel = "Nivel 2";
                imInvernalia.Visibility = Visibility.Visible;
                imAguasDulces.Visibility = Visibility.Visible;
                if (obtenerNivel() != nivel)
                {
                    segundoNivel();
                }
            }
            else if(puntos >= 200)
            {
                nivel = "Nivel 3";
                imInvernalia.Visibility = Visibility.Visible;
                imAguasDulces.Visibility = Visibility.Visible;
                imDesembarco.Visibility = Visibility.Visible;
                if (obtenerNivel() != nivel)
                {
                    tercerNivel();
                }
            }
            if (!nivel.Equals(""))
            {
                mycommand.CommandText = "UPDATE Jugadores SET Nivel='" + nivel + "' WHERE Nombre='" + nombre + "'";
                //Console.WriteLine("UPDATE Jugadores SET Nivel='" + nivel + "' WHERE Nombre='" + nombre + "'");
                OleDbDataReader DBreader = mycommand.ExecuteReader();
                DBreader.Close();
            }

        }
    }
}