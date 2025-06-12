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
        private int numVin;
        private Fournisseur unFournisseur;
        private TypeVin unType;
        private Appelation uneAppelation;
        private string nomVin;
        private decimal prixVin;
        private string descriptif;
        private int millesime;

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

        public int Millesime
        {
            get
            {
                return this.millesime;
            }

            set
            {
                this.millesime = value;
            }
        }


        public string ImagePath
        {
            get
            {
                Console.WriteLine($"/Fichier/Vin{this.UnType.NomType}.png");
                return $"/Fichier/Vin{this.UnType.NomType}.png";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            throw new NotImplementedException();
        }

        public int Delete()
        {
            throw new NotImplementedException();
        }

        public List<Vin> FindAll()
        {
            List<Vin> lesVins = new List<Vin>();
            try
            {

                string sql = @"
            SELECT 
                v.numvin, v.nomvin, v.prixvin, v.descriptif, v.millesime,
                f.numfournisseur, f.nomfournisseur,                  -- Infos Fournisseur
                t.numtype, t.nomtype,                               -- Infos TypeVin
                a.numtype AS cde_appelation, a.nomappelation                    -- Infos Appelation
            FROM vin v
            JOIN fournisseur f ON v.numfournisseur = f.numfournisseur
            JOIN typevin t ON v.numtype = t.numtype
            JOIN appelation a ON v.numtype2 = a.numtype -- Supposition ici
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
                            Convert.ToInt32(dr["cde_appelation"]),
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

        public List<Vin> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }

    }
}
