using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using TD3_BindingBDPension.Model;

namespace SAE_NICOLASSE.Classe
{
    public class Vin : ICrud<Vin>, INotifyPropertyChanged
    {
        public int NumVin { get; set; }
        public Fournisseur UnFournisseur { get; set; }
        public TypeVin UnType { get; set; }
        public Appelation UneAppelation { get; set; }
        public string NomVin { get; set; }
        public decimal PrixVin { get; set; }
        public string Descriptif { get; set; }
        public int Millesime { get; set; }

        public string ImagePath => UnType != null ? $"/Fichier/Vin{this.UnType.NomType}.png" : "/Fichier/VinRouge.png";

        public event PropertyChangedEventHandler? PropertyChanged;

        public Vin(int numVin, Fournisseur unFournisseur, TypeVin unType, Appelation uneAppelation, string nomVin, decimal prixVin, string descriptif, int millesime)
        {
            this.NumVin = numVin;
            this.UnFournisseur = unFournisseur;
            this.UnType = unType;
            this.UneAppelation = uneAppelation;
            this.NomVin = nomVin;
            this.PrixVin = prixVin;
            this.Descriptif = descriptif;
            this.Millesime = millesime;
        }
        public Vin() { }

        public List<Vin> FindAll()
        {
            List<Vin> lesVins = new List<Vin>();
            try
            {
                string sql = @"
                SELECT 
                    v.numvin, v.nomvin, v.prixvin, v.descriptif, v.millesime,
                    f.numfournisseur, f.nomfournisseur,
                    t.numtype, t.nomtype,
                    a.numtype, a.nomappelation
                FROM vin v
                JOIN fournisseur f ON v.numfournisseur = f.numfournisseur
                JOIN typevin t ON v.numtype = t.numtype
                JOIN appelation a ON v.numtype2 = a.numtype
                ORDER BY v.nomvin;";

                using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
                {
                    DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Fournisseur leFournisseur = new Fournisseur(
                            Convert.ToInt32(dr["numfournisseur"]),
                            dr["nomfournisseur"].ToString()
                        );
                        TypeVin letypeVin = new TypeVin(
                            Convert.ToInt32(dr["numtype"]),
                            dr["nomtype"].ToString()
                        );
                        Appelation appelation = new Appelation(
                            Convert.ToInt32(dr["numtype"]),
                            dr["nomappelation"].ToString()
                        );

                        Vin leVin = new Vin(
                            Convert.ToInt32(dr["numvin"]),
                            leFournisseur,
                            letypeVin,
                            appelation,
                            dr["nomvin"].ToString(),
                            Convert.ToDecimal(dr["prixvin"]),
                            dr["descriptif"].ToString(),
                            Convert.ToInt32(dr["millesime"])
                        );
                        lesVins.Add(leVin);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des vins : " + ex.Message);
            }
            return lesVins;
        }

        public int Create() { throw new NotImplementedException(); }
        public int Delete() { throw new NotImplementedException(); }
        public List<Vin> FindBySelection(string criteres) { throw new NotImplementedException(); }
        public void Read() { throw new NotImplementedException(); }
        public int Update() { throw new NotImplementedException(); }
    }
}
