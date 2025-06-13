// ========================================================================
// FICHIER : Classe/DetailCommande.cs
// DÉCISION : Je conserve la version de ton collègue ("lui").
//            Elle contient la méthode Create() qui permet d'enregistrer
//            un détail de commande en base de données, ce qui est essentiel.
//            Ta version ne contenait que les propriétés.
// ========================================================================

using Npgsql;
using System;
using System.Collections.Generic;
using TD3_BindingBDPension.Model;

namespace SAE_NICOLASSE.Classe
{
    public class DetailCommande : ICrud<DetailCommande>
    {
        private Commande uneCommande;
        private Vin unVin;
        private int quantite;
        private decimal prix;

        public DetailCommande(Commande uneCommande, Vin unVin, int quantite, decimal prix)
        {
            this.UneCommande = uneCommande;
            this.UnVin = unVin;
            this.Quantite = quantite;
            this.Prix = prix;
        }

        public Commande UneCommande
        {
            get { return this.uneCommande; }
            set { this.uneCommande = value; }
        }

        public Vin UnVin
        {
            get { return this.unVin; }
            set { this.unVin = value; }
        }

        public int Quantite
        {
            get { return this.quantite; }
            set { this.quantite = value; }
        }

        public decimal Prix
        {
            get { return this.prix; }
            set { this.prix = value; }
        }

        public int Create()
        {
            string sql = @"
            INSERT INTO DETAILCOMMANDE (numcommande, numvin, quantite, prix)
            VALUES (@numcommande, @numvin, @quantite, @prix);";

            using (var cmd = new NpgsqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@numcommande", this.UneCommande.Numcommande);
                cmd.Parameters.AddWithValue("@numvin", this.UnVin.NumVin);
                cmd.Parameters.AddWithValue("@quantite", this.Quantite);
                cmd.Parameters.AddWithValue("@prix", this.Prix);

                // On utilise ExecuteSet car cette table n'a pas d'ID auto-généré à retourner.
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        // Les autres méthodes de l'interface ne sont pas implémentées mais doivent être présentes.
        public int Delete() { throw new NotImplementedException(); }
        public List<DetailCommande> FindAll() { throw new NotImplementedException(); }
        public List<DetailCommande> FindBySelection(string criteres) { throw new NotImplementedException(); }
        public void Read() { throw new NotImplementedException(); }
        public int Update() { throw new NotImplementedException(); }
    }
}
