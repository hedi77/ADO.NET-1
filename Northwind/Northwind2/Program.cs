using Northwind2.Pages;
using Outils.TConsole;

namespace Northwind2
{
    class Program
    {
        static void Main(string[] args)
        {
            Northwind2App app = Northwind2App.Instance;           //singleton
            app.Title = "Northwind2";

            Page accueil = new PageAccueil();
            app.AddPage(accueil);

            app.AddPage(new PageFounisseur());
            app.AddPage(new PageProduit());
            app.NavigateTo(accueil);
            
            app.Run();





        }
    }
}
