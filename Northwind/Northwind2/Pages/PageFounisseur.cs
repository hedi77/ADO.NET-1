using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Northwind2.Pages
{
    public class PageFounisseur : MenuPage
    {
        public PageFounisseur() : base("Founisseurs")
        {
            Menu.AddOption("1", "Liste des pays", AfficherPays);
            Menu.AddOption("2", "Liste des fournisseur", AfficherListeFounisseur);
            Menu.AddOption("3", "Nb de produits par pays", AfficherNbProduitsParPays);
        }

        private void AfficherNbProduitsParPays()
        {
            string saisie = Input.Read<string>("de quel pays souhaitez-vous afficher le nombre de produits?");
            int nb = Contexte._GetNbProduits(saisie);
            Console.WriteLine(nb);
            // Output.WriteLine(ConsoleColor.Cyan.nb.ToString() + "produit");
        }

        private void AfficherListeFounisseur()
        {
            string saisie = Input.Read<string>("de quel pays souhaitez-vous afficher la liste des fournisseurs?");
            List<Fournisseur> _fournisseurs = Contexte.GetFournisseurs(saisie);
            ConsoleTable.From(_fournisseurs, "company").Display("fournisseurs");
        }

        private void AfficherPays()
        {
            List<string> _pays = Contexte.GetPaysFournisseurs();
            ConsoleTable.From(_pays, "pays").Display("pays");
        }
    }
}
