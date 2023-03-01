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
    /// Interaction logic for OntwerperView.xaml
    /// </summary>
    public partial class OntwerperView : UserControl
    {
        IPersoonRepository persoonRepository = new PersoonRepository();
        IBordspelRepository bordspelRepository = new BordspelRepository();
        ICollection<BordspelPersoon> personenLijst = new List<BordspelPersoon>();
        ICollection<BordspelPersoon> alleBordspelPersonen = new List<BordspelPersoon>();
        public int bordspelId { get; set; }
        private int persoonId;
        private Persoon persoon;
        private Bordspel bordspel;

        public OntwerperView()
        {
            InitializeComponent();
            cbSelecteerOntwerper.ItemsSource = persoonRepository.OphalenPersonen();
        }
        public OntwerperView(int bordspelId)
        {
            InitializeComponent();
            cbSelecteerOntwerper.ItemsSource = persoonRepository.OphalenPersonen();

            if (bordspelId != 0)
            {
                bordspel = bordspelRepository.ZoekenBordspelViaId(bordspelId);
                alleBordspelPersonen = bordspel.BordspelPersoon;

                this.bordspelId = bordspelId;

                foreach (var bordspelPersoon in alleBordspelPersonen)
                {
                    if (bordspelPersoon.rol.id == 1)
                    {
                        this.personenLijst.Add(bordspelPersoon);
                    }
                }
                if (personenLijst.Count == 0)
                {
                    lblValidatie.Content = "Het spel heeft helaas geen designer";
                }
                lbOntwerper.ItemsSource = personenLijst;
            }
        }
        // nieuwe ontwerper aanmaken
        private void btnSaveOntwerper_Click(object sender, RoutedEventArgs e)
        {
            if(cbSelecteerOntwerper.SelectedItem == null)
            {
                Persoon persoon = new Persoon();
                persoon.naam = txtNaamOntwerper.Text;
                persoon.voornaam = txtVoornaamOntwerper.Text;
                if (persoon.IsGeldig()) 
                {
                    if (BestaatDePersoonAl() == true)
                    {
                        lblValidatie.Content = "De persoon die u tracht toe te voegen zit al in de databank.";
                    }
                    else
                    {
                        persoonRepository.VoegPersoonToeAanDatabank(persoon);
                        cbSelecteerOntwerper.ItemsSource = persoonRepository.OphalenPersonen();
                        lblValidatie.Content = persoon.naam + " " + persoon.voornaam + " werd toegevoegd aan de databank.";
                        txtNaamOntwerper.Text = "";
                        txtVoornaamOntwerper.Text = "";
                    }
                }
                else
                {
                    lblValidatie.Content = persoon.Error;
                }
            }
            else
            {
                lblValidatie.Content = "Je kan geen persoon in de databank steken als deze er al inzit, \nje kan hem wel bewerken via de juiste knop.";
            }
        }
        // checken of persoon al bestaat
        private bool BestaatDePersoonAl()
        {
            List<Persoon> allePersonen = (List<Persoon>)persoonRepository.OphalenPersonen();
            foreach (Persoon p in allePersonen)
            {
                if (p.naam == txtNaamOntwerper.Text && p.voornaam == txtVoornaamOntwerper.Text)
                {
                    return true;
                }
            }
            return false;
        }

        private void cbSelecteerOntwerper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSelecteerOntwerper.SelectedItem is Persoon persoon)
            {
                txtNaamOntwerper.Text = persoon.naam;
                txtVoornaamOntwerper.Text = persoon.voornaam;
            }
        }
        // ontwerper verwijderen van spel
        private void btnVerwijderOntwerper_Click(object sender, RoutedEventArgs e)
        {
            if (lbOntwerper.SelectedItem != null)
            {
                int persoonId = ((BordspelPersoon)lbOntwerper.SelectedItem).persoonId;
                int bordspelId = ((BordspelPersoon)lbOntwerper.SelectedItem).bordspelId;
                personenLijst.Remove((BordspelPersoon)lbOntwerper.SelectedItem);
                lbOntwerper.Items.Refresh();
                persoonRepository.VerwijderPersoonVanBordspel(persoonId, bordspelId);
                lblValidatie.Content = "De ontwerper werd verwijderd van het spel :(";
                if (personenLijst.Count == 0)
                {
                    lblValidatie.Content += "\nHet spel heeft helaas geen designer";
                }             
            }
            else
            {
                lblValidatie.Content = "Gelieve een ontwerper te selecteren om te verwijderen";
            }
        }
        // ontwerper toevoegen aan spel
        private void btnVoegOntwerperToe_Click(object sender, RoutedEventArgs e)
        {
            if (cbSelecteerOntwerper.SelectedItem == null)
            {
                lblValidatie.Content = "Gelieve een ontwerper te selecteren";
            }
            else
            {     
                if (cbSelecteerOntwerper.SelectedItem is Persoon persoon)
                {
                    BordspelPersoon bordspelPersoon = new BordspelPersoon();
                    bordspelPersoon.persoon = persoon;
                    bordspelPersoon.persoonId = persoon.id;
                    bordspelPersoon.rolId = 1;
                    bordspelPersoon.bordspelId = bordspelId;
                    if (!personenLijst.Contains(bordspelPersoon))
                    {
                        personenLijst.Add(bordspelPersoon);
                        persoonRepository.VoegPersoonToeAanBordspel(bordspelPersoon);
                        lbOntwerper.Items.Refresh();
                        lblValidatie.Content = bordspelPersoon.ToString() + " werd toegevoegd aan het spel.";
                    }
                    else
                    {
                        lblValidatie.Content = "Ontwerper staat al in de lijst";
                    }
                }                
            }
        }
        // ontwerper updaten
        private void btnUpdateOntwerper_Click(object sender, RoutedEventArgs e)
        {
            if (cbSelecteerOntwerper.SelectedItem != null)
            {
                string oudeNaam = cbSelecteerOntwerper.SelectedItem.ToString();
                persoon = (Persoon)cbSelecteerOntwerper.SelectedItem;
                persoonId = ((Persoon)cbSelecteerOntwerper.SelectedItem).id;
                cbSelecteerOntwerper.SelectedItem = null; //omdat dapper zonder confirm of commit gaat blijft selected item die niet bestaat aanduiden, onvervangbaar zonder dit!

                if (persoon.naam == txtNaamOntwerper.Text && persoon.voornaam == txtVoornaamOntwerper.Text)
                {
                    lblValidatie.Content = "Je heb de naam niet veranderd. \nDeze is nog steeds " + oudeNaam;
                }
                else
                {
                    persoon.naam = txtNaamOntwerper.Text;
                    persoon.voornaam = txtVoornaamOntwerper.Text;
                    persoonRepository.UpdatePersoon(persoon);
                    // omdat hij de persoon in lijst niet wil aanpassen zit onderstaande code er in om dit we gedaan te krijgen; zonder dit moet je van scherm veranderen (update dus van listbox)
                    personenLijst.Clear();
                    bordspel = bordspelRepository.ZoekenBordspelViaId(bordspelId);
                    alleBordspelPersonen = bordspel.BordspelPersoon;

                    foreach (var bordspelPersoon in alleBordspelPersonen)
                    {
                        if (bordspelPersoon.rol.id == 1)
                        {
                            this.personenLijst.Add(bordspelPersoon);
                        }
                    }

                    cbSelecteerOntwerper.Items.Refresh();
                    lbOntwerper.ItemsSource = personenLijst;
                    lbOntwerper.Items.Refresh();
                    lblValidatie.Content = oudeNaam + " kreeg een nieuwe naam: \n" + persoon.naam + " " + persoon.voornaam;
                    txtNaamOntwerper.Text = "";
                    txtVoornaamOntwerper.Text = "";                    
                }
            }
            else
            {
                lblValidatie.Content = "Je hebt geen ontwerper gekozen om te bewerken";
            }
        }
    }
}
