using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Northwind2.Pages
{
    class PageProduit : MenuPage
    {

        public PageProduit() : base("Produits")
        {
            Menu.AddOption("1", "Liste des produits", () => AfficherProduits());
            Menu.AddOption("2", "Créer un produit", CreerProduit);
            Menu.AddOption("3", "Modifier un produit", ModifierProduit);
            Menu.AddOption("4", "Supprimer un produit", SupprimerProduit);


        }

        private void SupprimerProduit()
        {
            AfficherProduits();
            //try
            //{
                int idProduct = Input.Read<int>("saisie id de produit à supprimer");
                Console.WriteLine("Produit supprimé");
                Contexte.SupprimerUnProduit(idProduct);
            //}
            //catch ( SqlException e)
            //{
                
            //}
        }



        private void ModifierProduit()
        {
            Guid idCata = AfficherProduits();

            int idProduct = Input.Read<int>("saisie id de produit");

            Produit prod = new Produit();
            prod = Contexte.GetUnProduit(idProduct);

            string nom = Input.Read<string>("saisie le nouvel nom de produit", prod.nom);

            int idfournisseur = Input.Read<int>("saisie la nouvelle id de fournisseur", prod.idFournisseur);
            decimal prixUnitaire = Input.Read<decimal>("le nouveau Prix unitaire de produit", prod.prixUnitaire);
            Int16 uniteEnStock = Input.Read<Int16>("Unité en stock", prod.uniteEnStock);

            Produit newProduit = new Produit
            {

                idCata = idCata,
                nom = nom,
                idFournisseur = idfournisseur,
                prixUnitaire = prixUnitaire,
                uniteEnStock = uniteEnStock
            };

            Contexte.AjouterModifierProduit(newProduit, 2);


        }

        private void CreerProduit()
        {
            //AfficherProduits();
            Guid idcata = AfficherProduits();


            int idproduct = Input.Read<int>("id de product");
            string nom = Input.Read<string>("saisie le nouvel nom de produit");
            int idfournisseur = Input.Read<int>("id de fournisseur");
            decimal prixUnitaire = Input.Read<decimal>("Prix unitaire de produit");
            Int16 uniteEnStock = Input.Read<Int16>("Unité en stock");
            Produit newProduit = new Produit
            {
                id = idproduct,
                idCata = idcata,
                nom = nom,
                idFournisseur = idfournisseur,
                prixUnitaire = prixUnitaire,
                uniteEnStock = uniteEnStock
            };

            Contexte.AjouterModifierProduit(newProduit, 1);

        }

        private void AfficherCategorie()
        {
            List<Category> categorie = Contexte.GetCategorie();
            ConsoleTable.From(categorie, "categorie").Display("categorie");
        }



        private Guid AfficherProduits()
        {
            AfficherCategorie();
            Guid saisie = Input.Read<Guid>("saisie id de categorie");
            var _produits = Contexte.GetProduit(saisie);
            ConsoleTable.From(Contexte.GetProduit(saisie), "produits").Display("produits");
            return saisie;

        }



    }
}

