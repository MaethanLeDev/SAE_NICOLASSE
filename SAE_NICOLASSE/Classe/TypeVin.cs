using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_NICOLASSE.Classe
{
    public class TypeVin
    {
        private int numType;
        private string nomType;

        public TypeVin(int numType, string nomType)
        {
            this.NumType = numType;
            this.NomType = nomType;
        }

        public int NumType
        {
            get
            {
                return this.numType;
            }

            set
            {
                this.numType = value;
            }
        }

        public string NomType
        {
            get
            {
                return this.nomType;
            }

            set
            {
                this.nomType = value;
            }
        }
    }
}
