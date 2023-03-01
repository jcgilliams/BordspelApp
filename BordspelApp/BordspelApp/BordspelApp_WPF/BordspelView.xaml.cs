using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BordspelApp_DAL;
using BordspelApp_Models;

namespace BordspelApp_WPF
{
    /// <summary>
    /// Interaction logic for BordspelView.xaml
    /// </summary>
    public partial class BordspelView : UserControl
    {
        IBordspelRepository bordspelRepository = new BordspelRepository();
        private int opgehaaldeId;
        private Bordspel bordspel = new Bordspel();

        public BordspelView()
        {
            InitializeComponent();
            btnUpdateSpel.Visibility = Visibility.Hidden;
        }
        public BordspelView(int id)
        {
            InitializeComponent();
            ContentWindow.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Hidden;
            opgehaaldeId = id;
            HaalDataOp(id);
        }
        // knop om terug naar eerste scherm te gaan
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Content = new CollectionView();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string geselecteerdTabblad = (tabControl.SelectedItem as TabItem).Name;
            switch (geselecteerdTabblad)
            {
                case "tabArtiest":
                    ContentWindow.Content = new ArtiestView(bordspel.id);
                    break;
                case "tabUitgeverij":
                    ContentWindow.Content = new UitgeverijView(bordspel.id);
                    break;
                default:
                    ContentWindow.Content = new OntwerperView(bordspel.id);
                    break;
            }
        }
        // nieuw bordspel aanmaken
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Bordspel bordspel = new Bordspel();

            bordspel.naam = txtBordspelNaam.Text;
            if (int.TryParse(txtRelease.Text, out int release) || string.IsNullOrEmpty(txtRelease.Text))
            {
                bordspel.jaar = release;
            }
            if (int.TryParse(txtMinSpelers.Text, out int minSpelers) || string.IsNullOrEmpty(txtMinSpelers.Text))
            {
                bordspel.minAantalSpelers = minSpelers;
            }
            if (int.TryParse(txtMaxSpelers.Text, out int maxSpelers) || string.IsNullOrEmpty(txtMaxSpelers.Text))
            {
                bordspel.maxAantalSpelers = maxSpelers;
            }
            if (int.TryParse(txtMinSpeeltijd.Text, out int minSpeeltijd) || string.IsNullOrEmpty(txtMinSpeeltijd.Text))
            {
                bordspel.minSpeeltijd = minSpeeltijd;
            }
            if (int.TryParse(txtMaxSpeeltijd.Text, out int maxSpeeltijd) || string.IsNullOrEmpty(txtMaxSpeeltijd.Text))
            {
                bordspel.maxSpeeltijd = maxSpeeltijd;
            }
            if (int.TryParse(txtLeeftijd.Text, out int leeftijd) || string.IsNullOrEmpty(txtLeeftijd.Text))
            {
                bordspel.leeftijd = leeftijd;
            }
            bordspel.beschrijving = txtBeschrijving.Text;

            if (bordspel.IsGeldig())
            {
                this.bordspel= bordspelRepository.VoegBordspelToeAanDatabank(bordspel);
                lblValidatie.Content = "Het bordspel werd met succes toegevoegd.";
                ContentWindow.Content = new OntwerperView(this.bordspel.id);
                ContentWindow.Visibility = Visibility.Visible;
            }
            else
            {
                lblValidatie.Content = bordspel.Error;
            }            
        }
        // data ophalen van bordspel om aan te passen of te bekijken
        private void HaalDataOp(int id)
        {
            bordspel = bordspelRepository.ZoekenBordspelViaId(id);
            txtBordspelNaam.Text = bordspel.naam;
            txtRelease.Text = bordspel.jaar.ToString();
            txtMinSpelers.Text = bordspel.minAantalSpelers.ToString();
            txtMaxSpelers.Text = bordspel.maxAantalSpelers.ToString();
            txtMinSpeeltijd.Text = bordspel.minSpeeltijd.ToString();
            txtMaxSpeeltijd.Text = bordspel.maxSpeeltijd.ToString();
            txtLeeftijd.Text = bordspel.leeftijd.ToString();
            txtBeschrijving.Text = bordspel.beschrijving;
            lblValidatie.Content = "";
        }
        // bordspel aanpassen
        private void btnUpdateSpel_Click(object sender, RoutedEventArgs e)
        {
            bordspel.naam = txtBordspelNaam.Text;
            if (int.TryParse(txtRelease.Text, out int release) || string.IsNullOrEmpty(txtRelease.Text))
            {
                bordspel.jaar = release;
            }
            if(int.TryParse(txtMinSpelers.Text, out int minSpelers) || string.IsNullOrEmpty(txtMinSpelers.Text))
            {
                bordspel.minAantalSpelers = minSpelers;
            }
            if (int.TryParse(txtMaxSpelers.Text, out int maxSpelers) || string.IsNullOrEmpty(txtMaxSpelers.Text))
            {
                bordspel.maxAantalSpelers = maxSpelers;
            }
            if (int.TryParse(txtMinSpeeltijd.Text, out int minSpeeltijd) || string.IsNullOrEmpty(txtMinSpeeltijd.Text))
            {
                bordspel.minSpeeltijd= minSpeeltijd;
            }
            if (int.TryParse(txtMaxSpeeltijd.Text, out int maxSpeeltijd) || string.IsNullOrEmpty(txtMaxSpeeltijd.Text))
            {
                bordspel.maxSpeeltijd = maxSpeeltijd;
            }
            if (int.TryParse(txtLeeftijd.Text, out int leeftijd) || string.IsNullOrEmpty(txtLeeftijd.Text))
            {
                bordspel.leeftijd = leeftijd;
            }
            bordspel.beschrijving = txtBeschrijving.Text;

            if (bordspel.IsGeldig())
            {
                bordspelRepository.UpdateBordspel(bordspel);
                lblValidatie.Content = "Het bordspel werd met succes aangepast.";
            }
            else
            {
                lblValidatie.Content = bordspel.Error;
            }
        }
    }
}
