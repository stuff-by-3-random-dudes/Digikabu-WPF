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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DigikabuWPF
{
    /// <summary>
    /// Interaktionslogik für MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        static System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        static System.Windows.Threading.DispatcherTimer reloadTimer = new System.Windows.Threading.DispatcherTimer();

        public MenuWindow()
        {
            InitializeComponent();
            this.Hide();
            getAll();
            Termine.IsSelected = true;
            GetSK();
            this.Show();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            reloadTimer.Tick += new EventHandler(reloadTimer_Tick);
            reloadTimer.Interval = new TimeSpan(0, 15, 0);
            reloadTimer.Start();
        }

        private void reloadTimer_Tick(object sender, EventArgs e)
        {
            getAll();
        }
        private void getAll()
        {
            GetSPTM();
            GetWSP();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            string uStart = "8:30", ausgabe = string.Empty;
            int stdDauer = 45, pDauer = 15, pPos = 2, stdAnz = 10;

            DateTime jetzt = DateTime.Now;
            List<DateTime> uhrZeiten = new List<DateTime>();

            uhrZeiten.Add(Convert.ToDateTime(uStart));
            for (int i = 0; i <= stdAnz; i++)
            {
                if (i == pPos)
                {
                    uhrZeiten.Add(uhrZeiten[i].AddMinutes(pDauer));
                }
                else
                {
                    uhrZeiten.Add(uhrZeiten[i].AddMinutes(stdDauer));
                }
            }
            for (int i = 0; i < uhrZeiten.Count - 1; i++)
            {
                if (jetzt >= uhrZeiten[i] && jetzt < uhrZeiten[i + 1])
                {
                    if (i < pPos)
                    {
                        ausgabe = $"{i + 1}";
                    }
                    else if (i == pPos)
                    {
                        ausgabe = "Pause";
                    }
                    else if (i > pPos)
                    {
                        ausgabe = $"{i}";
                    }

                }
            }
            ListViewItem lvi = null;
            ListViewItem lvilast = null;
            switch (ausgabe)
            {
                case "1":
                    SP.SelectedIndex = 0;
                    lvi = SP.ItemContainerGenerator.ContainerFromItem(SP.SelectedItem) as ListViewItem;

                    

                    break;
                case "2":
                    SP.SelectedIndex = 1;
                    lvi = SP.ItemContainerGenerator.ContainerFromItem(SP.SelectedItem) as ListViewItem;
                    lvilast = SP.ItemContainerGenerator.ContainerFromIndex(SP.SelectedIndex - 1) as ListViewItem;
                    break;
                case "Pause":
                    SP.SelectedIndex = 2;
                    lvi = SP.ItemContainerGenerator.ContainerFromItem(SP.SelectedItem) as ListViewItem;
                    lvilast = SP.ItemContainerGenerator.ContainerFromIndex(SP.SelectedIndex - 1) as ListViewItem;
                    break;
                case "3":
                    SP.SelectedIndex = 3;
                    lvi = SP.ItemContainerGenerator.ContainerFromItem(SP.SelectedItem) as ListViewItem;
                    lvilast = SP.ItemContainerGenerator.ContainerFromIndex(SP.SelectedIndex - 1) as ListViewItem;
                    break;
                case "4":
                    SP.SelectedIndex = 4;
                    lvi = SP.ItemContainerGenerator.ContainerFromItem(SP.SelectedItem) as ListViewItem;
                    lvilast = SP.ItemContainerGenerator.ContainerFromIndex(SP.SelectedIndex - 1) as ListViewItem;
                    break;
                case "5":
                    SP.SelectedIndex = 5;
                    lvi = SP.ItemContainerGenerator.ContainerFromItem(SP.SelectedItem) as ListViewItem;
                    lvilast = SP.ItemContainerGenerator.ContainerFromIndex(SP.SelectedIndex - 1) as ListViewItem;
                    break;
                case "6":
                    SP.SelectedIndex = 6;
                    lvi = SP.ItemContainerGenerator.ContainerFromItem(SP.SelectedItem) as ListViewItem;
                    lvilast = SP.ItemContainerGenerator.ContainerFromIndex(SP.SelectedIndex - 1) as ListViewItem;
                    break;
                case "7":
                    SP.SelectedIndex = 7;
                    lvi = SP.ItemContainerGenerator.ContainerFromItem(SP.SelectedItem) as ListViewItem;
                    lvilast = SP.ItemContainerGenerator.ContainerFromIndex(SP.SelectedIndex - 1) as ListViewItem;
                    break;
                case "8":
                    SP.SelectedIndex = 8;
                    lvi = SP.ItemContainerGenerator.ContainerFromItem(SP.SelectedItem) as ListViewItem;
                    lvilast = SP.ItemContainerGenerator.ContainerFromIndex(SP.SelectedIndex - 1) as ListViewItem;
                    break;
                case "9":
                    SP.SelectedIndex = 9;
                    lvi = SP.ItemContainerGenerator.ContainerFromItem(SP.SelectedItem) as ListViewItem;
                    lvilast = SP.ItemContainerGenerator.ContainerFromIndex(SP.SelectedIndex - 1) as ListViewItem;
                    break;
                case "10":
                    SP.SelectedIndex = 10;
                    lvi = SP.ItemContainerGenerator.ContainerFromItem(SP.SelectedItem) as ListViewItem;
                    lvilast = SP.ItemContainerGenerator.ContainerFromIndex(SP.SelectedIndex - 1) as ListViewItem;
                    break;
                default:
                    SP.SelectedIndex = -1;
                    lvilast = SP.ItemContainerGenerator.ContainerFromIndex(10) as ListViewItem;
                    break;
            }
            if (lvi != null)

            {

                lvi.Background = Brushes.DarkGray;

            }
            if(lvilast != null)
            {
                lvilast.Background = Brushes.Transparent;
            }
        }

        private async void GetSK()
        {
            var tosplit = await WebsiteCon.GetUNKL();
            var splittxt = tosplit.Split(';');
            User.Text = splittxt[0];
            Klasse.Text = splittxt[1];
        }
        private async void GetSPTM()
        {
            SP.ItemsSource = await WebsiteCon.getStundenplan();
            TM.ItemsSource = await WebsiteCon.getTermine();
        }
        private async void GetWSP()
        {
            DateTime datum_montag = StartingDateOfWeek(DateTime.Now), datum_freitag = datum_montag.AddDays(4);
            Mondat.Text = datum_montag.ToString("dd.MM.yyyy");
            Frdat.Text = datum_freitag.ToString("dd.MM.yyyy");
            WSP_MON.ItemsSource     =   await WebsiteCon.getStundenplan(datum_montag);
            WSP_DIE.ItemsSource     =   await WebsiteCon.getStundenplan(datum_montag.AddDays(1));
            WSP_MIT.ItemsSource     =   await WebsiteCon.getStundenplan(datum_montag.AddDays(2));
            WSP_DON.ItemsSource     =   await WebsiteCon.getStundenplan(datum_montag.AddDays(3));
            WSP_FR.ItemsSource      =   await WebsiteCon.getStundenplan(datum_montag.AddDays(4));
        }
        private DateTime StartingDateOfWeek(DateTime date)
        {
            DateTime usedDate;
            int dateAdjustment = 0;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dateAdjustment = 1;
                    break;
                case DayOfWeek.Monday:
                    dateAdjustment = 0;
                    break;
                case DayOfWeek.Tuesday:
                    dateAdjustment = -1;
                    break;
                case DayOfWeek.Wednesday:
                    dateAdjustment = -2;
                    break;
                case DayOfWeek.Thursday:
                    dateAdjustment = -3;
                    break;
                case DayOfWeek.Friday:
                    dateAdjustment = -4;
                    break;
                case DayOfWeek.Saturday:
                    dateAdjustment = 2;
                    break;
                default:
                    break;
            }
            usedDate = date.AddDays(Convert.ToDouble(dateAdjustment));
            return usedDate;
        }
        private void ButtonPopUpLogout_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonPopUpLogout_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            WebsiteCon.logout();
            this.Close();
        }

        private void ShowTermine(object sender, MouseButtonEventArgs e)
        {

        }

        private async void listView_Click(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as ListView).SelectedIndex) 
            {
                case 0:
                    TAB.SelectedIndex = 0;
                    Fenster.Text = "Digikabu - Termine";
                    GetSPTM();
                    break;
                case 1:
                    TAB.SelectedIndex = 1;
                    Fenster.Text = "Digikabu - Wochenstundenplan";
                    break;
                case 2:
                    TAB.SelectedIndex = 2;
                    Fenster.Text = "Digikabu - Schulaufgabenplan";
                    break;
                case 3:
                    TAB.SelectedIndex = 3;
                    Fenster.Text = "Digikabu - Speiseplan";
                    break;
                case 4:
                    TAB.SelectedIndex = 4;
                    Fenster.Text = "Digikabu - Entschuldigung";
                    break;
                case 5:
                    TAB.SelectedIndex = 5;
                    Fenster.Text = "Digikabu - Fehlzeiten";
                    break;
                case 6:
                    TAB.SelectedIndex = 6;
                    Fenster.Text = "Digikabu - Einstellungen";
                    break;
            }
            
        }

        private void MinimizeApp(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


    }
}
