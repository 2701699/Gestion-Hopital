
using System;

using System.Collections.Generic;

using System.Collections.ObjectModel;

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

using static projetGestionHopital.MainWindow;
 
namespace projetGestionHopital

{

    /// <summary>

    /// Interaction logic for MainWindow.xaml

    /// </summary>

    public partial class MainWindow : Window

    {

        public MainWindow()

        {

            InitializeComponent();

            //Initialiser les listes des spécialités et des médecins

            SpecialitesListe = new ObservableCollection<Specialite>();

            MedecinListe = new ObservableCollection<Medecin>();

        }
        //Declarer la liste des spécialités

        public ObservableCollection<Specialite> SpecialitesListe { get; set; }

        int indexSpecialite;

        //Declarer la liste des médecins

        public ObservableCollection<Medecin> MedecinListe { get; set; }

        int indexMedecin;

        // Cette méthode est appelée lorsque l'utilisateur clique sur la fenêtre avec le bouton gauche de la souris et maintient le clic.
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // La méthode DragMove() permet de déplacer la fenêtre en maintenant le bouton de la souris enfoncé et en la déplaçant.
            this.DragMove();
        }

        // Cette méthode pour le bouton de fermeture.
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // La méthode Close() ferme la fenêtre actuelle.
            this.Close();
        }

        // Afficher le MenuItem "Médecin" dans le menu
        private void MenuItemMedecin_Click(object sender, RoutedEventArgs e)

        {

            // Afficher la page correspondante dans le TabControl

            if (tabControlPrincipal != null)

            {

                tabControlPrincipal.SelectedIndex = 1;

            }

        }
        // Afficher le MenuItem "Spécialité" dans le menu
        private void MenuItemSpecialite_Click(object sender, RoutedEventArgs e)

        {

            // Afficher la page correspondante dans le TabControl

            if (tabControlPrincipal != null)

            {

                tabControlPrincipal.SelectedIndex = 0;

            }

        }
        // Afficher le MenuItem "Consultation" dans le menu
        private void MenuItemConsultation_Click(object sender, RoutedEventArgs e)

        {

            // Afficher la page correspondante dans le TabControl

            if (tabControlPrincipal != null)

            {

                tabControlPrincipal.SelectedIndex = 2;

            }

        }


        // Méthode pour gérer l'ajout d'un nouveau médecin

        private void btnAjouterMedecin_Click(object sender, RoutedEventArgs e)

        {

            // Vérifier si tous les champs sont remplis

            if (string.IsNullOrWhiteSpace(tbIdentifiant.Text) ||

                string.IsNullOrWhiteSpace(tbNomMedecin.Text) ||

                string.IsNullOrWhiteSpace(tbPrenomMedecin.Text) ||

                cmbSpecialiteMedecin.SelectedItem == null ||

                string.IsNullOrWhiteSpace(tbTelephoneMedecin.Text) ||

                string.IsNullOrWhiteSpace(tbSalaireMedecin.Text))

            {

                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                return;

            }

            // Vérifier si l'identifiant contient uniquement des chiffres

            if (!System.Text.RegularExpressions.Regex.IsMatch(tbIdentifiant.Text, "^[0-9]*$"))

            {

                MessageBox.Show("Veuillez saisir l'identifiant en entier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                tbIdentifiant.Text = string.Empty;

                return;

            }

            // Vérifier si le numéro de téléphone contient uniquement des chiffres

            if (!System.Text.RegularExpressions.Regex.IsMatch(tbTelephoneMedecin.Text, "^[0-9]*$"))

            {

                MessageBox.Show("Veuillez saisir le numéro de téléphone en entier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                tbTelephoneMedecin.Text = string.Empty;

                return;

            }

            // Vérifier si le salaire est un nombre valide

            if (!double.TryParse(tbSalaireMedecin.Text, out double salaire))

            {

                MessageBox.Show("Veuillez entrer le salaire en chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                tbSalaireMedecin.Text = string.Empty;

                return;

            }

            // Les champs sont remplis et valides, continuer avec l'ajout du médecin

            try

            {

                // Création d'une instance de la classe Medecin

                Medecin medecin = new Medecin();

                // Remplissage des propriétés du médecin avec les valeurs des champs

                medecin.Identifiant = int.Parse(tbIdentifiant.Text);

                medecin.Nom = tbNomMedecin.Text;

                medecin.Prenom = tbPrenomMedecin.Text;

                medecin.Specialite = cmbSpecialiteMedecin.SelectedItem.ToString();

                medecin.Telephone = long.Parse(tbTelephoneMedecin.Text);

                medecin.Salaire = salaire; // Utilisation du salaire validé

                // Vérifier si l'identifiant est unique

                bool identifiantUnique = true;

                foreach (var item in dgMedecin.Items)

                {

                    dynamic med = item;

                    if (med.IdentifiantMedecin == medecin.Identifiant)

                    {

                        identifiantUnique = false;

                        break;

                    }

                }

                if (!identifiantUnique)

                {

                    MessageBox.Show("L'identifiant existe déjà.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;

                }

                // Ajouter la nouvelle spécialité à la liste des médecins disponibles

                MedecinListe.Add(medecin);

                // Ajout du médecin à la DataGrid

                dgMedecin.Items.Add(new

                {

                    IdentifiantMedecin = medecin.Identifiant,

                    NomMedecin = medecin.Nom,

                    PrenomMedecin = medecin.Prenom,

                    SpecialiteMedecin = medecin.Specialite,

                    TelephoneMedecin = medecin.Telephone,

                    SalaireMedecin = medecin.Salaire,

                });

                statusMessage.Text = "Médecin ajouté avec succès";

                statusAction.Foreground = Brushes.Green;

                statusAction.Text = "OK";

            }

            catch (FormatException exp)

            {

                statusMessage.Text = exp.ToString();

                statusAction.Text = "erreur";

                statusAction.Foreground = Brushes.Red;

            }

            // Mettre à jour la liste des médecins

            UpdateListeMedecins();

            // Mettre à jour la DataGrid

            UpdateDataGridListeMedecin();

            // Mettre à jour la liste des médecins pour les consultations

            UpdateConsultationMedecinList();

            // Réinitialisation des champs après l'ajout

            tbIdentifiant.Text = "";

            tbNomMedecin.Text = "";

            tbPrenomMedecin.Text = "";

            cmbSpecialiteMedecin.SelectedItem = null;

            tbTelephoneMedecin.Text = "";

            tbSalaireMedecin.Text = "";

        }


        // Méthode  pour modifier un médecin 
        private void btnModifierMedecin_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si un médecin est sélectionné dans la DataGrid
            if (dgMedecin.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un médecin à modifier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Créer une nouvelle instance de Medecin pour stocker les données modifiées
            Medecin medecinAmodifier = new Medecin();
            // Récupérer les valeurs des champs de saisie et les attribuer à l'objet Medecin.
            medecinAmodifier.Identifiant = int.Parse(tbIdentifiant.Text);
            medecinAmodifier.Nom = tbNomMedecin.Text;
            medecinAmodifier.Prenom = tbPrenomMedecin.Text;
            medecinAmodifier.Specialite = cmbSpecialiteMedecin.SelectedItem.ToString();
            medecinAmodifier.Telephone = long.Parse(tbTelephoneMedecin.Text);
            medecinAmodifier.Salaire = double.Parse(tbSalaireMedecin.Text);

            // Vérifier si l'identifiant est unique
            bool identifiantUnique = true;
            foreach (var item in dgMedecin.Items)
            {
                dynamic med = item;
                if (med.IdentifiantMedecin == medecinAmodifier.Identifiant)
                {
                    identifiantUnique = false;
                    break;
                }
            }
            // Afficher un message d'erreur si l'identifiant n'est pas unique et quitter la méthode.
            if (!identifiantUnique)
            {
                MessageBox.Show("L'identifiant existe déjà.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Supprimer l'élément à l'index de l'élément à modifier dans la DataGrid.
            dgMedecin.Items.RemoveAt(indexMedecin);
            // Insérer les nouvelles données modifiées dans la DataGrid à l'index de l'élément modifié.
            dgMedecin.Items.Insert(indexMedecin, new
            {
                IdentifiantMedecin = medecinAmodifier.Identifiant,
                NomMedecin = medecinAmodifier.Nom,
                PrenomMedecin = medecinAmodifier.Prenom,
                SpecialiteMedecin = medecinAmodifier.Specialite,
                TelephoneMedecin = medecinAmodifier.Telephone,
                SalaireMedecin = medecinAmodifier.Salaire,
            });
            // Ajouter le médecin modifié à la liste des médecins.
            MedecinListe.Add(medecinAmodifier);
            // Mettre à jour le ComboBox cbListeMedecin
            UpdateListeMedecins();
            UpdateDataGridListeMedecin();
            // Rafraîchir les éléments visuels.
            dgMedecin.Items.Refresh();
            cmbSpecialiteMedecin.Items.Refresh();

            // Afficher un message de succès dans la barre d'état.
            statusMessage.Text = "Médecin modifié avec succès";

            statusAction.Foreground = Brushes.Green;

            statusAction.Text = "OK";
            // Réinitialisation des champs après la modification
            tbIdentifiant.Text = "";
            tbNomMedecin.Text = "";
            tbPrenomMedecin.Text = "";
            cmbSpecialiteMedecin.SelectedItem = null;
            tbTelephoneMedecin.Text = "";
            tbSalaireMedecin.Text = "";
        }



        // Méthode de suppression d'un médecin
        private void btnSupprimerMedecin_Click(object sender, RoutedEventArgs e)

        {
            // Récupérer le médecin sélectionné dans la DataGrid
            dynamic selectedMedecin = dgMedecin.SelectedItem;

            if (selectedMedecin != null)

            {

                // Supprimer le médecin de la liste MedecinListe

                Medecin medecinToRemove = MedecinListe.FirstOrDefault(m => m.Identifiant == selectedMedecin.IdentifiantMedecin);

                if (medecinToRemove != null)

                {

                    MedecinListe.Remove(medecinToRemove);

                }

                // Supprimer l'élément de la DataGrid

                dgMedecin.Items.RemoveAt(indexMedecin);

                // Mettre à jour le ComboBox des spécialités pour les médecins

                cmbSpecialiteMedecin.ItemsSource = SpecialitesListe;

                // Mettre à jour le ComboBox cmbSpecialiteMedecin

                UpdateComboBoxSpecialitesMedecin();

                // Mettre à jour le ComboBox cbMedecinSpecialite

                UpdateCboBoxMedecinSpecialite();

                // Rafraîchir le ComboBox cmbSpecialiteMedecin
                cmbSpecialiteMedecin.Items.Refresh();

                // Mettre à jour la liste des médecins pour les consultations

                UpdateConsultationMedecinList();

            }

        }

        //métohe d'ajout d'une spécialité
        private void btnAjouterSpecialite_Click(object sender, RoutedEventArgs e)

        {

            // Créer une nouvelle instance de la spécialité

            Specialite specialite = new Specialite();

            try

            {

                // Vérifier si le champ de l'identifiant est vide

                if (string.IsNullOrEmpty(tbIdentifiantSpecialite.Text))

                {

                    MessageBox.Show("Veuillez entrer un identifiant pour la spécialité.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;

                }

                // Vérifier si l'identifiant est déjà utilisé

                int identifiant;

                if (!int.TryParse(tbIdentifiantSpecialite.Text, out identifiant))

                {

                    MessageBox.Show("Veuillez entrer un identifiant valide pour la spécialité.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;

                }

                foreach (var existingSpecialite in SpecialitesListe)

                {

                    if (existingSpecialite.IdentifiantS == identifiant)

                    {

                        MessageBox.Show("L'identifiant est déjà utilisé merci d'enter un autre .", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                        return;

                    }

                }

                // Vérifier si le champ du nom est vide

                if (string.IsNullOrEmpty(tbNomSpecialite.Text))

                {

                    MessageBox.Show("Veuillez entrer un nom pour la spécialité.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;

                }

                // Vérifier si le nom est déjà utilisé

                foreach (var existingSpecialite in SpecialitesListe)

                {

                    if (existingSpecialite.NomS == tbNomSpecialite.Text)

                    {

                        MessageBox.Show("La spécialité existe deja merci de mentionner une autre .", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                        return;

                    }

                }

                // Remplir les propriétés de la spécialité avec les valeurs des champs

                specialite.IdentifiantS = identifiant;

                specialite.NomS = tbNomSpecialite.Text;

                specialite.Description = tbDescription.Text;

                // Ajouter la spécialité à la liste des spécialités

                SpecialitesListe.Add(specialite);

                // Mettre à jour les ComboBox et le DataGrid avec les nouvelles données

                cmbSpecialiteMedecin.ItemsSource = SpecialitesListe;

                cbMedecinSpecialite.ItemsSource = SpecialitesListe;
                // Insérer les nouvelles données  dans la DataGridé.
                dgSpecialite.Items.Add(new

                {

                    IdentifiantSpecialite = specialite.IdentifiantS,

                    NomSpecialite = specialite.NomS,

                    DescriptionSpecialite = specialite.Description,

                });
                // Afficher un message de succès dans la barre d'état.
                statusMessage.Text = "Spécialité ajoutée avec succès";

                statusAction.Foreground = Brushes.Green;

                statusAction.Text = "OK";

            }

            catch (FormatException)

            {

                MessageBox.Show("Veuillez entrer un identifiant valide pour la spécialité.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            // Mettre à jour les ComboBox

            UpdateComboBoxSpecialitesMedecin();

            UpdateCboBoxMedecinSpecialite();


            UpdateConsultationMedecinList();

            // Réinitialiser les champs après l'ajout

            tbIdentifiantSpecialite.Text = "";

            tbNomSpecialite.Text = "";

            tbDescription.Text = "";

        }

        // Méthode de modification d'une spécialité.
        private void btnModifierSpecialite_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si aucune spécialité n'est sélectionnée dans la DataGrid dgSpecialite
            if (dgSpecialite.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une spécialité à modifier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Créer une instance de Specialite pour stocker les informations de la spécialité à modifier
            Specialite specialiteAmodifier = new Specialite();
            // Récupérer les valeurs des champs et les assigner à l'instance de Specialite
            specialiteAmodifier.IdentifiantS = int.Parse(tbIdentifiantSpecialite.Text);
            specialiteAmodifier.NomS = tbNomSpecialite.Text;
            specialiteAmodifier.Description = tbDescription.Text;

            // Supprimer l'ancienne spécialité de la DataGrid
            dgSpecialite.Items.RemoveAt(indexSpecialite);

            // Insérer la spécialité modifiée dans la DataGrid
            dgSpecialite.Items.Insert(indexSpecialite, new
            {
                IdentifiantSpecialite = specialiteAmodifier.IdentifiantS,
                NomSpecialite = specialiteAmodifier.NomS,
                DescriptionSpecialite = specialiteAmodifier.Description,
            });

            // Rafraîchir la DataGrid
            dgSpecialite.Items.Refresh();

            // Mettre à jour le ComboBox cmbSpecialiteMedecin
            UpdateComboBoxSpecialitesMedecin();
            // Mettre à jour le ComboBox cbMedecinSpecialite
            UpdateCboBoxMedecinSpecialite();

            // Rafraîchir le ComboBox cmbSpecialiteMedecin
            cmbSpecialiteMedecin.Items.Refresh();
            // Mettre à jour le ComboBox des spécialités pour les consultations
            cbMedecinSpecialite.Items.Refresh();

            // Réinitialisation des champs après la modification
            tbIdentifiantSpecialite.Text = "";
            tbNomSpecialite.Text = "";
            tbDescription.Text = "";
        }


        //méthode permet la suppression d'une spécialité
        private void btnSupprimerSpecialite_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedSpecialite = dgSpecialite.SelectedItem;
            if (selectedSpecialite != null)
            {
                // Vérifier si des médecins exercent encore cette spécialité
                bool medecinsExist = MedecinListe.Any(m => m.Specialite == selectedSpecialite.NomSpecialite);
                if (medecinsExist)
                {
                    MessageBox.Show("Des médecins exercent encore cette spécialité. Impossible de supprimer la spécialité.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Supprimer la spécialité de la liste SpecialitesListe
                Specialite specialiteASupprimer = SpecialitesListe.FirstOrDefault(s => s.IdentifiantS == selectedSpecialite.IdentifiantSpecialite);
                if (specialiteASupprimer != null)
                {
                    SpecialitesListe.Remove(specialiteASupprimer);
                }

                // Rafraîchir la DataGrid des spécialités
                dgSpecialite.Items.Refresh();

                // Mettre à jour les ComboBox des spécialités pour les médecins
                cmbSpecialiteMedecin.ItemsSource = SpecialitesListe;
                cbMedecinSpecialite.ItemsSource = SpecialitesListe;

                // Supprimer les médecins associés à la spécialité
                List<Medecin> medecinsASupprimer = MedecinListe.Where(m => m.Specialite == selectedSpecialite.NomSpecialite).ToList();
                foreach (var medecin in medecinsASupprimer)
                {
                    MedecinListe.Remove(medecin);
                }

                // Supprimer les données associées à la spécialité de la DataGrid dgMedecin
                dgMedecin.Items.Refresh();

                // Mettre à jour la source de données de la DataGrid de consultation
                UpdateConsultationMedecinList();

                // Supprimer la spécialité de la DataGrid dgSpecialite
                dgSpecialite.Items.Remove(selectedSpecialite);

                statusMessage.Text = "Spécialité supprimée avec succès";
                statusAction.Foreground = Brushes.Green;
                statusAction.Text = "OK";
            }
        }
        //méthode permet la recherche d'un médecin en sélectionnant la spécialité
        private void btnRecherche_Click(object sender, RoutedEventArgs e)

        {

            // Récupérer la spécialité sélectionnée dans le ComboBox cbMedecinSpecialite

            string specialiteSelectionnee = cbMedecinSpecialite.SelectedItem?.ToString();

            // Vérifier si une spécialité est sélectionnée

            if (!string.IsNullOrEmpty(specialiteSelectionnee))

            {

                // Filtrer les médecins correspondant à la spécialité sélectionnée

                var medecinsSpecialite = MedecinListe.Where(medecin => medecin.Specialite == specialiteSelectionnee).ToList();

                // Mettre à jour la source de données de la DataGrid

                dgListeMedecins.ItemsSource = medecinsSpecialite;

            }

            else

            {

                // Si aucune spécialité n'est sélectionnée, afficher tous les médecins

                dgListeMedecins.ItemsSource = MedecinListe;

            }

        }

        //Méthode exécutée lorsqu'on clique sur le bouton de réinitialisation.
        private void btnReset_Click(object sender, RoutedEventArgs e)

        {

            // Effacez le contenu des champs de medecins

            tbIdentifiant.Text = "";

            tbNomMedecin.Text = "";

            tbPrenomMedecin.Text = "";

            cmbSpecialiteMedecin.SelectedItem = null;

            tbTelephoneMedecin.Text = "";

            tbSalaireMedecin.Text = "";

            // Effacez le contenu des champs de specialite

            tbIdentifiantSpecialite.Text = "";

            tbNomSpecialite.Text = "";

            tbDescription.Text = "";

        }
        //Méthode permet de mettre a jour la liste médecin
        private void UpdateConsultationMedecinList()

        {

            // Mettre à jour la source de données de la DataGrid des médecins pour la consultation

            dgListeMedecins.ItemsSource = MedecinListe;

        }

        //méthode permet de mettre à jour le combobox de liste spécialité dans consultation
        private void UpdateCboBoxMedecinSpecialite()

        {

            /*// Récupérer la liste des noms de spécialités de la DataGrid

            List<string> specialites = new List<string>();

            foreach (var item in dgSpecialite.Items)

            {

                dynamic specialite = item;

                specialites.Add(specialite.SpecialiteMedecin.ToString());

            }*/

            // Mettre à jour la source de données du ComboBox cbMedecinSpecialite

            cbMedecinSpecialite.ItemsSource = cmbSpecialiteMedecin.ItemsSource;

        }
        //methode qui met à jour le combobox de liste spécialité 
        private void UpdateComboBoxSpecialitesMedecin()

        {

            // Récupérer la liste des noms de spécialités de la DataGrid

            List<string> specialites = new List<string>();

            foreach (var item in dgSpecialite.Items)

            {

                dynamic specialite = item;

                specialites.Add(specialite.NomSpecialite.ToString());

            }

            // Mettre à jour la source de données du ComboBox cmbSpecialiteMedecin

            cmbSpecialiteMedecin.ItemsSource = specialites;

        }

        private void UpdateDataGridListeMedecin()

        {

            // Récupérer la spécialité sélectionnée dans le ComboBox

            string specialiteSelectionnee = cbMedecinSpecialite.SelectedItem?.ToString();

            // Vérifier si une spécialité est sélectionnée

            if (!string.IsNullOrEmpty(specialiteSelectionnee))

            {

                // Filtrer les médecins correspondant à la spécialité sélectionnée

                List<Medecin> medecinsSpecialite = new List<Medecin>();

                foreach (var medecin in MedecinListe)

                {

                    if (medecin.Specialite == specialiteSelectionnee)

                    {

                        medecinsSpecialite.Add(medecin);

                    }

                }

                dgMedecin.Items.Refresh();

                // Mettre à jour la source de données de la DataGrid avec les médecins correspondants

                dgListeMedecins.ItemsSource = medecinsSpecialite;

            }

        }



        private void UpdateListeMedecins()

        {

            // Récupérer la liste des noms de médecins

            List<string> medecins = new List<string>();

            foreach (var item in dgMedecin.Items)

            {

                dynamic medecin = item;

                medecins.Add(medecin.NomMedecin.ToString());

            }

            // Mettre à jour le Datagrid des médecins pour les consultations

            dgListeMedecins.ItemsSource = MedecinListe;

        }


        // Ajouter une nouvelle classe pour gerer les médecins.

        public class Medecin

        {

            public int Identifiant { get; set; }

            public string Nom { get; set; }

            public string Prenom { get; set; }

            public string Specialite { get; set; }

            public long Telephone { get; set; }

            public double Salaire { get; set; }

        }

        public class Specialite

        {

            public int IdentifiantS { get; set; }

            public string NomS { get; set; }

            public string Description { get; set; }

        }

        // Méthode appelée lorsqu'un élément est sélectionné dans la DataGrid des spécialités.
        private void selectedSpecialite(object sender, SelectionChangedEventArgs e)
        {
            // Récupérer la ligne sélectionnée dans la DataGrid
            dynamic ligneSelectionnee = dgSpecialite.SelectedItem;

            // Vérifier si une ligne est sélectionnée
            if (ligneSelectionnee != null)
            {
                // Remplir les champs avec les informations de la ligne sélectionnée
                tbIdentifiantSpecialite.Text = ligneSelectionnee.IdentifiantSpecialite.ToString();
                tbNomSpecialite.Text = ligneSelectionnee.NomSpecialite.ToString();
                tbDescription.Text = ligneSelectionnee.DescriptionSpecialite.ToString();

                // Conserver l'index de la ligne sélectionnée
                indexSpecialite = dgSpecialite.SelectedIndex;
            }
        }

        // Méthode appelée lorsqu'un élément est sélectionné dans la DataGrid des médecins.
        private void selectedMedecin(object sender, SelectionChangedEventArgs e)
        {
            // Récupérer la ligne sélectionnée dans la DataGrid
            dynamic ligneSelectionnee = dgMedecin.SelectedItem;

            // Vérifier si une ligne est sélectionnée
            if (ligneSelectionnee != null)
            {
                // Remplir les champs avec les informations de la ligne sélectionnée
                tbIdentifiant.Text = ligneSelectionnee.IdentifiantMedecin.ToString();
                tbNomMedecin.Text = ligneSelectionnee.NomMedecin.ToString();
                tbPrenomMedecin.Text = ligneSelectionnee.PrenomMedecin.ToString();
                cmbSpecialiteMedecin.Text = ligneSelectionnee.SpecialiteMedecin.ToString();
                tbTelephoneMedecin.Text = ligneSelectionnee.TelephoneMedecin.ToString();
                tbSalaireMedecin.Text = ligneSelectionnee.SalaireMedecin.ToString();

                // Conserver l'index de la ligne sélectionnée
                indexMedecin = dgMedecin.SelectedIndex;
            }
        }


    }

}

   
