using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace NettoyagePCApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DirectoryInfo winTemp;
        public DirectoryInfo appTemp;
        public MainWindow()
        {
            InitializeComponent();
            winTemp = new DirectoryInfo(@"C:\Windows\Temp");
            appTemp = new DirectoryInfo(System.IO.Path.GetTempPath());
            
        }

        // Calcul de la taille d'un dossier
        public long DirSize(DirectoryInfo dir)
        {
            return dir.GetFiles().Sum(fi => fi.Length) + dir.GetDirectories().Sum(di => DirSize(di));
        }

        // Vider un dossier
        public void ClearTempData(DirectoryInfo di)
        {
            foreach (FileInfo file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                    Console.WriteLine(file.FullName);
                    
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                    Console.WriteLine(dir.FullName);

                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        private void Button_ANALYSER_Click(object sender, RoutedEventArgs e)
        {
            AnalyseFolders();
        }

        private void Button_CLEAN_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Nettoyage en cours...");
            btnClean.Content = "NETTOYAGE EN COURS";
            // Nettoyage du presse papier
            Clipboard.Clear();
            // Nettoyage des dossiers temp
            try
            {
                ClearTempData(winTemp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            try
            {
                ClearTempData(appTemp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            btnClean.Content = "NETTOYAGE TERMINER";
            title.Content = "Nettoyage effectuée";
            space.Content = "0 Mb";
        }

        private void Button_MAJ_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Votre logiciel est à jour", "Mise à jour", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_HISTORIQUE_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TODO : Créer page historique", "Historique", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_WEB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("http://geoffrey-lusitano.fr")
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

        }

        public void AnalyseFolders()
        {
            Console.WriteLine("Debut de l'analyse...");
            long totalSize = 0;
            try
            {
                totalSize += DirSize(winTemp) / 1000000;
                totalSize += DirSize(appTemp) / 1000000;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Analyse impossible : " + ex.Message);
            }


            space.Content = totalSize + " Mb";
            title.Content = "Analyse effectuée";
            date.Content = DateTime.Today;
        }
    }
}
