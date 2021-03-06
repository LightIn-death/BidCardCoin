﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BidCardCoin.Models;
using bidCardCoin.ORM;

namespace BidCardCoin.Vue.CRUD
{
    public partial class AddUtilisateurView : UserControl
    {
        private readonly List<Adresse> _listeAdresses;
        private readonly List<string> _listeMotClef;
        private readonly Utilisateur _user;
        private readonly Window _win;

        public AddUtilisateurView(Window win = null, Utilisateur user = null)
        {
            InitializeComponent();
            _win = win;

            _user = user ?? new Utilisateur();
            _listeMotClef = new List<string>();
            _listeAdresses = new List<Adresse>();
        }


        private void CreateNewUtilisateur(object sender, RoutedEventArgs e)
        {
            if (InputAge.Text != "" || InputEmail.Text != "" || InputFixe.Text != "" || InputNom.Text != "")
            {
                var uuid = Guid.NewGuid().ToString();
                var idPersonne = Guid.NewGuid().ToString();


                _user.IdUtilisateur = uuid;
                _user.IdPersonne = idPersonne;
                _user.Nom = InputNom.Text;
                _user.Prenom = InputPrenom.Text;
                _user.Age = int.TryParse(InputAge.Text, out _) ? int.Parse(InputAge.Text) : 20;
                _user.Email = InputEmail.Text;
                _user.Password = InputPassword.Password;
                _user.IdentityExist = InputIdentity.IsChecked ?? false;
                _user.IsSolvable = InputSolvable.IsChecked ?? false;
                _user.IsRessortissant = InputRessortissant.IsChecked ?? false;
                _user.TelephoneFixe = InputFixe.Text;
                _user.TelephoneMobile = InputMobile.Text;
                _user.ListeMotClef = _listeMotClef;
                _user.Adresses = _listeAdresses;


                UtilisateurORM.AddUtilisateur(_user);
                _win.Close();
            }
            else
            {
                MessageBox.Show("Veulliez completer tout les champs");
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            _win.Close();
        }

        private void ViewMotsClefs(object sender, RoutedEventArgs e)
        {
            var window = new Window
            {
                Title = "Selection mots clés",

                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#393C43"),
                Icon = new BitmapImage(new Uri("pack://application:,,,/ressources/CRUDimg/adresse.png",
                    UriKind.RelativeOrAbsolute))
            };
            window.Content = new ListeMotsClefsView(window, _listeMotClef);
            window.ShowDialog();
        }

        private void SelectAdresses(object sender, RoutedEventArgs e)
        {
            var window = new Window
            {
                Title = "Liste des adresses",

                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#393C43"),
                Icon = new BitmapImage(new Uri("pack://application:,,,/ressources/CRUDimg/Adresse.png",
                    UriKind.RelativeOrAbsolute))
            };

            window.Content = new ListeAdressesView(window, _listeAdresses);
            window.ShowDialog();
        }
    }
}