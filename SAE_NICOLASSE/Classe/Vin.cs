using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
