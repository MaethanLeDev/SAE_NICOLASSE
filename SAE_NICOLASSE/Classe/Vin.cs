using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public string ImagePath => $"/Fichier/Vin{this.UnType.NomType}.png";

        public Vin(int numVin, Fournisseur unFournisseur, TypeVin unType, Appelation uneAppelation, string nomVin, decimal prixVin, string descriptif, int millesime)
        {
            this.NumVin = numVin; this.UnFournisseur = unFournisseur; this.UnType = unType; this.UneAppelation = uneAppelation; this.NomVin = nomVin; this.PrixVin = prixVin; this.Descriptif = descriptif; this.Millesime = millesime;
        }
        public Vin() { }

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<Vin> FindAll(DataAccess dao)
        {
            List<Vin> lesVins = new List<Vin>();
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
                DataTable dt = dao.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    Fournisseur leFournisseur = new Fournisseur(Convert.ToInt32(dr["numfournisseur"]), dr["nomfournisseur"].ToString());
                    TypeVin letypeVin = new TypeVin(Convert.ToInt32(dr["numtype"]), dr["nomtype"].ToString());
                    Appelation appelation = new Appelation(Convert.ToInt32(dr["numtype"]), dr["nomappelation"].ToString());

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
            return lesVins;
        }

        public int Create(DataAccess dao) { throw new NotImplementedException(); }
        public int Delete(DataAccess dao) { throw new NotImplementedException(); }
        public List<Vin> FindBySelection(string criteres, DataAccess dao) { throw new NotImplementedException(); }
        public void Read(DataAccess dao) { throw new NotImplementedException(); }
        public int Update(DataAccess dao) { throw new NotImplementedException(); }
    }
}
