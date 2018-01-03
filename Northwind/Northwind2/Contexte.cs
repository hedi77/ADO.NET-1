using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    public class Contexte
    {

        public static void SupprimerUnProduit(int idProduct)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = "delete Product from Product where ProductId = @id";
            command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = idProduct });

            using (var cnx = new SqlConnection(Settings1.Default.NorthwindConnection))
            {
                command.Connection = cnx;
                cnx.Open();
                command.ExecuteNonQuery();
            }
        }



        public static void AjouterModifierProduit(Produit newProduit, int operation)
        {
            SqlCommand command = new SqlCommand();

            using (var cnx = new SqlConnection(Settings1.Default.NorthwindConnection))
            {
                if (operation == 1)
                {
                    command.CommandText = @"insert Product ( CategoryId,SupplierId, Name, UnitPrice, UnitsInStock) 
                                    values(@CID, @SID, @Name, @UnitPrice,@UnitsInStock)";
                }
                else if (operation == 2)
                {
                    command.CommandText = @"update Product
                                    set Name=@Name,CategoryId=@CID,UnitPrice=@UnitPrice,UnitsInStock=@UnitsInStock
                                    from Product
                                    where ProductId=@PID";
                    command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@PID", Value = newProduit.id });
                }

                command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@CID", Value = newProduit.idCata });
                command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@SID", Value = newProduit.idFournisseur });
                command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@Name", Value = newProduit.nom });
                command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Decimal, ParameterName = "@UnitPrice", Value = newProduit.prixUnitaire });
                command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.SmallInt, ParameterName = "@UnitsInStock", Value = newProduit.uniteEnStock });

                command.Connection = cnx;
                cnx.Open();
                command.ExecuteNonQuery();

            }

        }

        //public static void MisAJour(Produit produitToModify)
        //{
        //    SqlCommand command = new SqlCommand();
        //    command.CommandText = @"update Product
        //                            set Name=@Name,CategoryId=@CID,UnitPrice=@UnitPrice,UnitsInStock=@UnitsInStock
        //                            from Product
        //                            where ProductId=@PID";

        //    command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@PID", Value = produitToModify.id });
        //    command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@CID", Value = produitToModify.idCata });
        //    command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@SID", Value = produitToModify.idFournisseur });
        //    command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@Name", Value = produitToModify.nom });
        //    command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Decimal, ParameterName = "@UnitPrice", Value = produitToModify.prixUnitaire });
        //    command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.SmallInt, ParameterName = "@UnitsInStock", Value = produitToModify.uniteEnStock });

        //    using (var cnx = new SqlConnection(Settings1.Default.NorthwindConnection))
        //    {
        //        command.Connection = cnx;
        //        cnx.Open();
        //        command.ExecuteNonQuery();
        //    }

        //    Output.WriteLine(ConsoleColor.Green, "Produit modifié avec succès");

        //}

        //public static void AjouterProduit(Produit newProduit)
        //{
        //    SqlCommand command = new SqlCommand();
        //    command.CommandText = @"insert Product ( CategoryId,SupplierId, Name, UnitPrice, UnitsInStock) 
        //                            values(@CID, @SID, @Name, @UnitPrice,@UnitsInStock)";

        //    command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@CID", Value = newProduit.idCata });
        //    command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@SID", Value = newProduit.idFournisseur });
        //    command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@Name", Value = newProduit.nom });
        //    command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Decimal, ParameterName = "@UnitPrice", Value = newProduit.prixUnitaire });
        //    command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.SmallInt, ParameterName = "@UnitsInStock", Value = newProduit.uniteEnStock });

        //    using (var cnx = new SqlConnection(Settings1.Default.NorthwindConnection))
        //    {
        //        command.Connection = cnx;
        //        cnx.Open();
        //        command.ExecuteNonQuery();
        //    }

        //    Output.WriteLine(ConsoleColor.Green, "Produit créé avec succès");

        //}


        public static List<Category> GetCategorie()
        {
            List<Category> listCategory = new List<Category>();
            SqlCommand command = new SqlCommand();
            command.CommandText = "select CategoryId,Name,Description from Category";

            using (var cnx = new SqlConnection(Settings1.Default.NorthwindConnection))
            {
                command.Connection = cnx;
                cnx.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var categorie = new Category();
                        categorie.id = (Guid)reader["CategoryId"];
                        categorie.nom = (string)reader["Name"];
                        categorie.description = (string)reader["Description"];

                        listCategory.Add(categorie);
                    }
                }
            }
            return listCategory;
        }

        public static List<Produit> GetProduit(Guid idCata)
        {
            List<Produit> listProduit = new List<Produit>();
            SqlCommand command = new SqlCommand();
            command.CommandText = "select ProductId,Name,UnitPrice,UnitsInStock from Product where CategoryId= @idCata order by ProductId";


            command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@idCata", Value = idCata });

            using (var cnx = new SqlConnection(Settings1.Default.NorthwindConnection))
            {
                command.Connection = cnx;
                cnx.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var produit = new Produit();
                        produit.id = (int)reader["ProductId"];
                        produit.nom = (string)reader["Name"];
                        produit.prixUnitaire = (decimal)reader["UnitPrice"];
                        produit.uniteEnStock = (Int16)reader["UnitsInStock"];

                        listProduit.Add(produit);
                    }
                }
            }
            return listProduit;
        }

        public static Produit GetUnProduit(int idProduct)
        {

            SqlCommand command = new SqlCommand();
            command.CommandText = "select Name,UnitPrice,UnitsInStock from Product where ProductId= @id ";


            command.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = idProduct });
            var produit = new Produit();
            using (var cnx = new SqlConnection(Settings1.Default.NorthwindConnection))
            {

                command.Connection = cnx;
                cnx.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        //produit.id = (int)reader["ProductId"];
                        produit.nom = (string)reader["Name"];
                        produit.prixUnitaire = (decimal)reader["UnitPrice"];
                        produit.uniteEnStock = (Int16)reader["UnitsInStock"];


                    }
                }
            }
            return produit;
        }



        //public static List<Commande> GetClientsCommandes(string customerID)
        //{
        //    SqlCommand command = new SqlCommand();
        //    command.CommandText = @"select  c.CustomerId,c.CompanyName,o.OrderId,o.OrderDate,o.ShippedDate,
        //                            o.Freight,count( *)nbProduitDiff ,floor(sum(od.UnitPrice*od.Quantity*(1-od.Discount)))CA
        //                            from Customer c
        //                            inner join Orders o on o.CustomerId=c.CustomerId
        //                            inner join OrderDetail od on od.OrderId=o.OrderId
        //                            group by c.CustomerId,c.CompanyName,o.OrderId,o.OrderDate,o.ShippedDate,o.Freight";

        //    var par = new SqlParameter
        //    {
        //        SqlDbType = SqlDbType.NVarChar,
        //        ParameterName = "@id",
        //        Value = customerID
        //    };

        //    command.Parameters.Add(par);

        //    using (var cnx = new SqlConnection(Settings1.Default.NorthwindConnection))
        //    {

        //        command.Connection = cnx;
        //        cnx.Open();


        //    }
        //}


        public static int _GetNbProduits(string pays)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = "select dbo.ufn_GetNbProduits(@id)";

            var par = new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@id",
                Value = pays
            };

            command.Parameters.Add(par);

            using (var cnx = new SqlConnection(Settings1.Default.NorthwindConnection))
            {

                command.Connection = cnx;
                cnx.Open();
                return (int)command.ExecuteScalar();

            }


        }

        public static int GetNbProduits(string pays)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = @"select count(*)
                                from Product p
                                inner join Supplier s on s.SupplierId=p.SupplierId
                                inner join Address a on a.AddressId=s.AddressId
                                where a.Country= @id ";

            var par = new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@id",
                Value = pays
            };

            command.Parameters.Add(par);

            using (var cnx = new SqlConnection(Settings1.Default.NorthwindConnection))
            {

                command.Connection = cnx;
                cnx.Open();
                return (int)command.ExecuteScalar();

            }


        }

        public static List<string> GetPaysFournisseurs()
        {
            var listPays = new List<string>();

            var cmd = new SqlCommand();
            cmd.CommandText = @"select distinct A.Country 
                                from Address A 
                                inner join Supplier S on S.AddressId = A.AddressId
                                order by 1";

            using (var cnx = new SqlConnection(Settings1.Default.NorthwindConnection))
            {
                cmd.Connection = cnx;
                cnx.Open();


            }
            return listPays;
        }

        public static List<Fournisseur> GetFournisseurs(string pays)
        {
            var listFournisseurs = new List<Fournisseur>();

            var cmd = new SqlCommand();
            cmd.CommandText = @"select s.SupplierId,s.CompanyName
                                from address a 
                                inner join Supplier s on a.AddressId=s.AddressId
                                where Country = @id ";

            //creation d'un paremetre
            var param = new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@id",
                Value = pays
            };

            cmd.Parameters.Add(param);


            using (var cnx = new SqlConnection(Settings1.Default.NorthwindConnection))
            {

                cmd.Connection = cnx;
                cnx.Open();

                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    // On lit les lignes de résultat une par une
                    while (sdr.Read())
                    {
                        //...et pour chacune on crée un objet qu'on ajoute à la liste
                        var cat = new Fournisseur();
                        cat.id = (int)sdr["SupplierId"];
                        cat.nom = (string)sdr["CompanyName"];

                        listFournisseurs.Add(cat);
                    }
                }

            }

            return listFournisseurs;
        }
    }

    public class Produit
    {
        [Display(ShortName = "Id")]
        public int id { get; set; }
        [Display(ShortName = "Produit")]
        public string nom { get; set; }
        [Display(ShortName = "Unité en stock")]
        public int uniteEnStock { get; set; }
        [Display(ShortName = "Prix unitaire")]
        public decimal prixUnitaire { get; set; }
        [Display(ShortName = "None")]
        public Guid idCata { get; set; }
        [Display(ShortName = "None")]
        public int idFournisseur { get; set; }

    }

    public class Category
    {
        public Guid id { get; set; }
        public string nom { get; set; }
        public string description { get; set; }

    }

    public class Fournisseur
    {
        public int id { get; set; }
        public string nom { get; set; }
    }

    public enum OperationEnum { Ajout, Modifier }

}


