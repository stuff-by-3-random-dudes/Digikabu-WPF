﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DigikabuWPF.Properties;
namespace DigikabuWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if(Properties.Settings.Default.UN != string.Empty)
            {
                Nutzer.Text = Properties.Settings.Default.UN;
            }
            if (Properties.Settings.Default.PW != string.Empty)
            {
                PW.Password = Properties.Settings.Default.PW;
            }
            if (Properties.Settings.Default.keepData == true)
            {
                LoginKeep.IsChecked = true;
            }
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Nutzer.Text != string.Empty)
            {
                if(PW.Password != string.Empty)
                {
                    string retval = await WebsiteCon.Einloggen(Nutzer.Text, PW.Password);
                    if (retval.Contains("Erfolgreich"))
                    {
                        if (LoginKeep.IsChecked == true)
                        {
                            Properties.Settings.Default.UN = Nutzer.Text;
                            Properties.Settings.Default.PW = PW.Password;
                            Properties.Settings.Default.keepData = true;
                            Properties.Settings.Default.Save();
                        }
                        MenuWindow mw = new MenuWindow();
                        this.Close();
                        mw.Show();
                    }
                    else
                    {
                        MessageBox.Show(retval, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
            }
            

        }

    }
}
