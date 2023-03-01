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
    /// Interaction logic for CollectionView.xaml
    /// </summary>
    public partial class CollectionView : UserControl
    {
        public CollectionView()
        {
            InitializeComponent();
            HerladenSpellen();
            //datagridSpel.ItemsSource = bordspelRepository.OphalenBordspellen(); (was om te testen of ik iets in datagrid kreeg)
        }
        IBordspelRepository bordspelRepository = new BordspelRepository();
        ICollectieRepository collectieRepository = new CollectieRepository();
        // nieuw spel aanmaken (dus naar ander scherm)
        private void btnNieuw_Click(object sender, RoutedEventArgs e)
        {
            Content = new BordspelView();
        }
        // spel in collectie zetten
        private void btnInCollectie_Click(object sender, RoutedEventArgs e)
        {
            if (datagridSpel.SelectedItem is Bordspel bordspel)
            {
                int id = ((Bordspel)datagridSpel.SelectedItem).id;
                collectieRepository.ZetInCollectie(id);
                lblValidatie.Content = bordspel + " werd toegevoegd aan collectie";
                HerladenSpellen();
            }
            else
            {
                lblValidatie.Content = "Gelieve een spel te selecteren om in je collectie te steken";
            }
        }
        // spel uit collectie halen
        private void btnUitCollectie_Click(object sender, RoutedEventArgs e)
        {
            if (datagridCollectie.SelectedItem is BordspelGebruiker bordspelgebruiker)
            {
                int id = ((BordspelGebruiker)datagridCollectie.SelectedItem).bordspelId;
                collectieRepository.HaalUitCollectie(id);
                lblValidatie.Content = bordspelgebruiker + " werd uit je collectie gehaald";
                HerladenSpellen();
            }
            else
            {
                lblValidatie.Content = "Gelieve een spel te selecteren om uit je collectie te halen";
            }
        }
        // spellen herladen nadat je iets aangepast hebt
        public void HerladenSpellen()
        {
            datagridSpel.ItemsSource = bordspelRepository.OphalenBordspellenNietInCollectie();
            datagridCollectie.ItemsSource = collectieRepository.OphalenCollectie();
        }
        // filteren op spel
        private void txtFilterSpel_KeyUp(object sender, KeyEventArgs e)
        {
            datagridSpel.ItemsSource = bordspelRepository.ZoekenBordspellen(txtFilterSpel.Text);
        }
        // filteren op spel in collectie
        private void txtFilterCollectie_KeyUp(object sender, KeyEventArgs e)
        {
            datagridCollectie.ItemsSource = collectieRepository.ZoekenBordspellen(txtFilterCollectie.Text);
        }
        // verwijderen van een spel
        private void btnVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            if (datagridCollectie.SelectedItem is BordspelGebruiker bordspelgebruiker)
            {
                int id = ((BordspelGebruiker)datagridCollectie.SelectedItem).bordspelId;
                collectieRepository.VerwijderUitDatabank(id);
                lblValidatie.Content = bordspelgebruiker + " werd verwijderd uit de databank :(";
                HerladenSpellen();
            }
            else if (datagridSpel.SelectedItem is Bordspel bordspel)
            {
                int id = ((Bordspel)datagridSpel.SelectedItem).id;
                collectieRepository.VerwijderUitDatabank(id);
                lblValidatie.Content = bordspel + " werd verwijderd uit de databank :(";
                HerladenSpellen();
            }
            else
            {
                lblValidatie.Content = "Gelieve een spel te selecteren om te verwijderen";
            }
        }
        // spel aanpassen (dus naar ander scherm)
        private void btnAanpassen_Click(object sender, RoutedEventArgs e)
        {
            if (datagridCollectie.SelectedItem is BordspelGebruiker bordspelgebruiker)
            {
                int id = ((BordspelGebruiker)datagridCollectie.SelectedItem).bordspelId;
                Content = new BordspelView(id);
            }
            else if (datagridSpel.SelectedItem is Bordspel bordspel)
            {
                int id = ((Bordspel)datagridSpel.SelectedItem).id;
                Content = new BordspelView(id);
            }
            else
            {
                lblValidatie.Content = "Gelieve een spel te selecteren om te bewerken";
            }
        }
    }
}
