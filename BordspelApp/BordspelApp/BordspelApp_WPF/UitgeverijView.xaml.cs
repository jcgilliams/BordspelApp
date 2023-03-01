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
    /// Interaction logic for UitgeverijView.xaml
    /// </summary>
    public partial class UitgeverijView : UserControl
    {
        IUitgeverijRepository uitgeverijRepository = new UitgeverijRepository();
        IBordspelRepository bordspelRepository = new BordspelRepository();
        ICollection<BordspelUitgeverij> uitgeverijenLijst = new List<BordspelUitgeverij>();
        ICollection<BordspelUitgeverij> alleBordspelUitgeverijen = new List<BordspelUitgeverij>();
        public int bordspelId { get; set; }
        private int uitgeverijId;
        private Uitgeverij uitgeverij;
        private Bordspel bordspel;

        public UitgeverijView(int bordspelId)
        {
            InitializeComponent();
            cbSelecteerUitgeverij.ItemsSource = uitgeverijRepository.OphalenUitgeverijen();

            if(bordspelId != 0)
            {
                bordspel = bordspelRepository.ZoekenBordspelViaId(bordspelId);
                alleBordspelUitgeverijen = bordspel.BordspelUitgeverij;

                this.bordspelId = bordspelId;

                foreach (var bordspelUitgeverij in alleBordspelUitgeverijen)
                {
                        this.uitgeverijenLijst.Add(bordspelUitgeverij);
                }
                if (uitgeverijenLijst.Count == 0)
                {
                    lblValidatie.Content = "Het spel heeft helaas geen uitgever";
                }
                lbUitgeverij.ItemsSource = uitgeverijenLijst;
            }
        }

        //public UitgeverijView()
        //{
        //    InitializeComponent();
        //    cbSelecteerUitgeverij.ItemsSource = uitgeverijRepository.OphalenUitgeverijen();
        //}

        // nieuwe uitgeverij maken 
        private void btnSaveUitgeverij_Click(object sender, RoutedEventArgs e)
        {
            if (cbSelecteerUitgeverij.SelectedItem == null)
            {
                Uitgeverij uitgeverij = new Uitgeverij();
                uitgeverij.naam = txtNaamUitgeverij.Text;
                uitgeverij.land = txtLandUitgeverij.Text;
                if (uitgeverij.IsGeldig())
                {
                    if (BestaatDeUitgeverijAl() == true)
                    {
                        lblValidatie.Content = "De uitgeverij die u tracht toe te voegen zit al in de databank.";
                    }
                    else
                    {
                        uitgeverijRepository.VoegUitgeverijToeAanDatabank(uitgeverij);
                        cbSelecteerUitgeverij.ItemsSource = uitgeverijRepository.OphalenUitgeverijen();
                        lblValidatie.Content = uitgeverij.naam + " uit " + uitgeverij.land + " werd toegevoegd aan de databank.";
                        txtNaamUitgeverij.Text = "";
                        txtLandUitgeverij.Text = "";
                    }
                }
                else
                {
                    lblValidatie.Content = uitgeverij.Error;
                }
            }
            else
            {
                lblValidatie.Content = "Je kan geen uitgeverij in de databank steken als deze er al inzit, \nje kan deze wel bewerken via de juiste knop.";
            }
        }
        // checken of uitgeverij bestaat
        private bool BestaatDeUitgeverijAl()
        {
            List<Uitgeverij> alleUitgeverijen = (List<Uitgeverij>)uitgeverijRepository.OphalenUitgeverijen();
            foreach (Uitgeverij u in alleUitgeverijen)
            {
                if (u.naam == txtNaamUitgeverij.Text && u.land == txtLandUitgeverij.Text)
                {
                    return true;
                }
            }
            return false;
        }

        private void cbSelecteerUitgeverij_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSelecteerUitgeverij.SelectedItem is Uitgeverij uitgeverij)
            {
                txtNaamUitgeverij.Text = uitgeverij.naam;
                txtLandUitgeverij.Text = uitgeverij.land;
            }
        }
        // uitgeverij verwijderen van spel
        private void btnVerwijderUitgeverij_Click(object sender, RoutedEventArgs e)
        {
            if (lbUitgeverij.SelectedItem != null)
            {
                int uitgeverijId = ((BordspelUitgeverij)lbUitgeverij.SelectedItem).uitgeverijId;
                int bordspelId = ((BordspelUitgeverij)lbUitgeverij.SelectedItem).bordspelId;
                uitgeverijenLijst.Remove((BordspelUitgeverij)lbUitgeverij.SelectedItem);
                lbUitgeverij.Items.Refresh();
                uitgeverijRepository.VerwijderUitgeverijVanBordspel(uitgeverijId, bordspelId);
                lblValidatie.Content = "De ontwerper werd verwijderd van het spel :(";
                if (uitgeverijenLijst.Count == 0)
                {
                    lblValidatie.Content += "\nHet spel heeft helaas geen uitgever";
                }
            }
            else
            {
                lblValidatie.Content = "Gelieve een uitgever te selecteren om te verwijderen";
            }
        }
        // uitgeverij aanpassen
        private void btnUpdateUitgeverij_Click(object sender, RoutedEventArgs e)
        {
            if (cbSelecteerUitgeverij.SelectedItem != null)
            {
                string oudeNaam = cbSelecteerUitgeverij.SelectedItem.ToString();
                uitgeverij = (Uitgeverij)cbSelecteerUitgeverij.SelectedItem;
                uitgeverijId = ((Uitgeverij)cbSelecteerUitgeverij.SelectedItem).id;
                cbSelecteerUitgeverij.SelectedItem = null; //omdat dapper zonder confirm of commit gaat blijft selected item die niet bestaat aanduiden, onvervangbaar zonder dit!

                if (uitgeverij.naam == txtNaamUitgeverij.Text && uitgeverij.land == txtLandUitgeverij.Text)
                {
                    lblValidatie.Content = "Je heb de naam niet veranderd. \nDeze is nog steeds " + oudeNaam;
                }
                else
                {
                    uitgeverij.naam = txtNaamUitgeverij.Text;
                    uitgeverij.land = txtLandUitgeverij.Text;
                    uitgeverijRepository.UpdateUitgeverij(uitgeverij);
                    // omdat hij de uitgeverij in lijst niet wil aanpassen zit onderstaande code er in om dit we gedaan te krijgen; zonder dit moet je van scherm veranderen (update dus van listbox)
                    uitgeverijenLijst.Clear();
                    bordspel = bordspelRepository.ZoekenBordspelViaId(bordspelId);
                    alleBordspelUitgeverijen = bordspel.BordspelUitgeverij;

                    foreach (var bordspelUitgeverij in alleBordspelUitgeverijen)
                    {
                        this.uitgeverijenLijst.Add(bordspelUitgeverij);
                    }

                    cbSelecteerUitgeverij.Items.Refresh();
                    lbUitgeverij.ItemsSource = uitgeverijenLijst;
                    lbUitgeverij.Items.Refresh();
                    lblValidatie.Content = oudeNaam + " kreeg een nieuwe naam: \n" + uitgeverij.naam + " uit " + uitgeverij.land;
                    txtNaamUitgeverij.Text = "";
                    txtLandUitgeverij.Text = "";
                }
            }
            else
            {
                lblValidatie.Content = "Je hebt geen uitgeverij gekozen om te bewerken";
            }
        }
        // voeg uitgeverij aan spel
        private void btnVoegUitgeverijToe_Click(object sender, RoutedEventArgs e)
        {
            if (cbSelecteerUitgeverij.SelectedItem == null)
            {
                lblValidatie.Content = "Gelieve een uitgeverij te selecteren";
            }
            else
            {
                if (cbSelecteerUitgeverij.SelectedItem is Uitgeverij uitgeverij)
                {
                    BordspelUitgeverij bordspelUitgeverij = new BordspelUitgeverij();
                    bordspelUitgeverij.uitgeverij = uitgeverij;
                    bordspelUitgeverij.bordspelId = bordspelId;
                    bordspelUitgeverij.uitgeverijId = uitgeverij.id;
                    if (!uitgeverijenLijst.Contains(bordspelUitgeverij))
                    {
                        uitgeverijenLijst.Add(bordspelUitgeverij);
                        uitgeverijRepository.VoegUitgeverijToeAanBordspel(bordspelUitgeverij);
                        lbUitgeverij.Items.Refresh();
                        lblValidatie.Content = bordspelUitgeverij.ToString() + " werd toegevoegd aan het spel.";
                    }
                    else
                    {
                        lblValidatie.Content = "Uitgeverij staat al in de lijst";
                    }
                }
            }
        }
    }
}
