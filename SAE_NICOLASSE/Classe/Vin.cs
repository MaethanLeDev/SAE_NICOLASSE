using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SAE_NICOLASSE.Classe
{
    public class Vin
    {
        private int numVin;
        private Fournisseur unFournisseur;
        private TypeVin unType;
        private Appelation uneAppelation;
        private string nomVin;
        private decimal prixVin;
        private string descriptif;
        private int annee;

        public Vin(int numVin, Fournisseur unFournisseur, TypeVin unType, Appelation uneAppelation, string nomVin, decimal prixVin, string descriptif, int annee)
        {
            this.NumVin = numVin;
            this.UnFournisseur = unFournisseur;
            this.UnType = unType;
            this.UneAppelation = uneAppelation;
            this.NomVin = nomVin;
            this.PrixVin = prixVin;
            this.Descriptif = descriptif;
            this.Annee = annee;
        }

        public int NumVin
        {
            get
            {
                return this.numVin;
            }

            set
            {
                this.numVin = value;
            }
        }

        public Fournisseur UnFournisseur
        {
            get
            {
                return this.unFournisseur;
            }

            set
            {
                this.unFournisseur = value;
            }
        }

        public TypeVin UnType
        {
            get
            {
                return this.unType;
            }

            set
            {
                this.unType = value;
            }
        }

        public Appelation UneAppelation
        {
            get
            {
                return this.uneAppelation;
            }

            set
            {
                this.uneAppelation = value;
            }
        }

        public string NomVin
        {
            get
            {
                return this.nomVin;
            }

            set
            {
                this.nomVin = value;
            }
        }

        public decimal PrixVin
        {
            get
            {
                return this.prixVin;
            }

            set
            {
                this.prixVin = value;
            }
        }

        public string Descriptif
        {
            get
            {
                return this.descriptif;
            }

            set
            {
                this.descriptif = value;
            }
        }

        public int Annee
        {
            get
            {
                return this.annee;
            }

            set
            {
                this.annee = value;
            }
        }
        public List<Vin> FindAllVins()
        {
            List<Vin> lesVins = new List<Vin>();
            try
            {
                
                string query = @"
            SELECT 
                v.numvin, v.nomvin, v.prixvin, v.descriptif, v.millesime,
                f.numfournisseur, f.nomfournisseur,                  -- Infos Fournisseur
                t.numtype, t.nomtype,                               -- Infos TypeVin
                a.numappelation, a.nomappelation                    -- Infos Appelation
            FROM vin v
            JOIN fournisseur f ON v.numfournisseur = f.numfournisseur
            JOIN typevin t ON v.numtype = t.numtype
            JOIN appelation a ON v.numtype2 = a.numappelation -- Supposition ici
            ORDER BY v.nomvin;";

                using (NpgsqlCommand cmdSelect = new NpgsqlCommand(query))
                {
                    DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                    foreach (DataRow dr in dt.Rows)
                    {
                        lesVins.Add(new Vin(
                            Convert.ToInt32(dr["numvin"]),

                            new Fournisseur(Convert.ToInt32(dr["numfournisseur"]), dr["nomfournisseur"].ToString()),

                            new TypeVin(Convert.ToInt32(dr["numtype"]), dr["nomtype"].ToString()),

                            new Appelation(Convert.ToInt32(dr["numappelation"]), dr["nomappelation"].ToString()),

                            dr["nomvin"].ToString(),
                            Convert.ToDecimal(dr["prixvin"]),
                            dr["descriptif"].ToString(),
                            Convert.ToInt32(dr["millesime"])
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des vins : " + ex.Message);
            }

            return lesVins;
        }

    }
}
