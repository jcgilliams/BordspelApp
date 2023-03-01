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
    /// Interaction logic for ArtiestView.xaml
    /// </summary>
    public partial class ArtiestView : UserControl
    {
        IPersoonRepository persoonRepository = new PersoonRepository();
        IBordspelRepository bordspelRepository = new BordspelRepository();
        ICollection<BordspelPersoon> personenLijst = new List<BordspelPersoon>();
        ICollection<BordspelPersoon> alleBordspelPersonen = new List<BordspelPersoon>();
        public int bordspelId { get; set; }
        private int persoonId;
        private Persoon persoon;
        private Bordspel bordspel;
        public ArtiestView(int bordspelId)
        {
            InitializeComponent();
            cbSelecteerArtiest.ItemsSource = persoonRepository.OphalenPersonen();

            if (bordspelId != 0)
            {
                bordspel = bordspelRepository.ZoekenBordspelViaId(bordspelId);
                alleBordspelPersonen = bordspel.BordspelPersoon;

                this.bordspelId = bordspelId;

                foreach (var bordspelPersoon in alleBordspelPersonen)
                {
                    if (bordspelPersoon.rol.id == 2)
                    {
                        this.personenLijst.Add(bordspelPersoon);
                    }
                }
                if (personenLijst.Count == 0)
                {
                    lblValidatie.Content = "Het spel heeft helaas geen designer";
                }
                lbArtiest.ItemsSource = personenLijst;
            }
        }
        public ArtiestView()
        {
            InitializeComponent();
            cbSelecteerArtiest.ItemsSource = persoonRepository.OphalenPersonen();
        }        
        // nieuwe artiest toevoegen aan databank
        private void btnSaveArtiest_Click(object sender, RoutedEventArgs e)
        {
            if (cbSelecteerArtiest.SelectedItem == null)
            {
                Persoon persoon = new Persoon();
                persoon.naam = txtNaamArtiest.Text;
                persoon.voornaam = txtVoornaamArtiest.Text;
                if (persoon.IsGeldig())
                {
                    if (BestaatDePersoonAl() == true)
                    {
                        lblValidatie.Content = "De persoon die u tracht toe te voegen zit al in de databank.";
                    }
                    else
                    {
                        persoonRepository.VoegPersoonToeAanDatabank(persoon);
                        cbSelecteerArtiest.ItemsSource = persoonRepository.OphalenPersonen();
                        lblValidatie.Content = persoon.naam + " " + persoon.voornaam + " werd toegevoegd aan de databank.";
                        txtNaamArtiest.Text = "";
                        txtVoornaamArtiest.Text = "";
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

        // checken of persoon al bestaat?
        private bool BestaatDePersoonAl()
        {
            List<Persoon> allePersonen = (List<Persoon>)persoonRepository.OphalenPersonen();
            foreach (Persoon p in allePersonen)
            {
                if (p.naam == txtNaamArtiest.Text && p.voornaam == txtVoornaamArtiest.Text)
                {
                    return true;
                }
            }
            return false;
        }

        private void cbSelecteerArtiest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSelecteerArtiest.SelectedItem is Persoon persoon)
            {
                txtNaamArtiest.Text = persoon.naam;
                txtVoornaamArtiest.Text = persoon.voornaam;
            }
        }
        // artiest verwijderen van bordspel
        private void btnVerwijderArtiest_Click(object sender, RoutedEventArgs e)
        {
            if (lbArtiest.SelectedItem != null)
            {
                int persoonId = ((BordspelPersoon)lbArtiest.SelectedItem).persoonId;
                int bordspelId = ((BordspelPersoon)lbArtiest.SelectedItem).bordspelId;
                personenLijst.Remove((BordspelPersoon)lbArtiest.SelectedItem);
                lbArtiest.Items.Refresh();
                persoonRepository.VerwijderPersoonVanBordspel(persoonId, bordspelId);
                lblValidatie.Content = "De artiest werd verwijderd van het spel :(";
                if (personenLijst.Count == 0)
                {
                    lblValidatie.Content += "\nHet spel heeft helaas geen artiest";
                }
            }
            else
            {
                lblValidatie.Content = "Gelieve een ontwerper te selecteren om te verwijderen";
            }
        }
        // artiest aanpassen (update)
        private void btnUpdateArtiest_Click(object sender, RoutedEventArgs e)
        {
            if (cbSelecteerArtiest.SelectedItem != null)
            {
                string oudeNaam = cbSelecteerArtiest.SelectedItem.ToString();
                persoon = (Persoon)cbSelecteerArtiest.SelectedItem;
                persoonId = ((Persoon)cbSelecteerArtiest.SelectedItem).id;
                cbSelecteerArtiest.SelectedItem = null; //omdat dapper zonder confirm of commit gaat blijft selected item die niet bestaat aanduiden, onvervangbaar zonder dit!

                if (persoon.naam == txtNaamArtiest.Text && persoon.voornaam == txtVoornaamArtiest.Text)
                {
                    lblValidatie.Content = "Je heb de naam niet veranderd. \nDeze is nog steeds " + oudeNaam;
                }
                else
                {
                    persoon.naam = txtNaamArtiest.Text;
                    persoon.voornaam = txtVoornaamArtiest.Text;
                    persoonRepository.UpdatePersoon(persoon);
                    // omdat hij de persoon in lijst niet wil aanpassen zit onderstaande code er in om dit we gedaan te krijgen; zonder dit moet je van scherm veranderen (update dus van listbox)
                    personenLijst.Clear();
                    bordspel = bordspelRepository.ZoekenBordspelViaId(bordspelId);
                    alleBordspelPersonen = bordspel.BordspelPersoon;

                    foreach (var bordspelPersoon in alleBordspelPersonen)
                    {
                        if (bordspelPersoon.rol.id == 2)
                        {
                            this.personenLijst.Add(bordspelPersoon);
                        }
                    }

                    cbSelecteerArtiest.Items.Refresh();
                    lbArtiest.ItemsSource = personenLijst;
                    lbArtiest.Items.Refresh();
                    lblValidatie.Content = oudeNaam + " kreeg een nieuwe naam: \n" + persoon.naam + " " + persoon.voornaam;
                    txtNaamArtiest.Text = "";
                    txtVoornaamArtiest.Text = "";
                }
            }
            else
            {
                lblValidatie.Content = "Je hebt geen ontwerper gekozen om te bewerken";
            }
        }
        // artiest toevoegen aan bordspel
        private void btnVoegArtiestToe_Click(object sender, RoutedEventArgs e)
        {
            if (cbSelecteerArtiest.SelectedItem == null)
            {
                lblValidatie.Content = "Gelieve een artiest te selecteren";
            }
            else
            {
                if (cbSelecteerArtiest.SelectedItem is Persoon persoon)
                {
                    BordspelPersoon bordspelPersoon = new BordspelPersoon();
                    bordspelPersoon.persoon = persoon;
                    bordspelPersoon.persoonId = persoon.id;
                    bordspelPersoon.rolId = 2;
                    bordspelPersoon.bordspelId = bordspelId;
                    if (!personenLijst.Contains(bordspelPersoon))
                    {
                        personenLijst.Add(bordspelPersoon);
                        persoonRepository.VoegPersoonToeAanBordspel(bordspelPersoon);
                        lbArtiest.Items.Refresh();
                        lblValidatie.Content = bordspelPersoon.ToString() + " werd toegevoegd aan het spel.";
                    }
                    else
                    {
                        lblValidatie.Content = "Artiest staat al in de lijst";
                    }
                }
            }
        }
    }
}
