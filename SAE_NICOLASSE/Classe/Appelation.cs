using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_NICOLASSE.Classe
{
    public class Appelation
    {

        private int numappelation;
        private string nomappelation;

        public Appelation(int numappelation, string nomappelation)
        {
            this.Numappelation = numappelation;
            this.Nomappelation = nomappelation;
        }

        public int Numappelation
        {
            get
            {
                return this.numappelation;
            }

            set
            {
                this.numappelation = value;
            }
        }

        public string Nomappelation
        {
            get
            {
                return this.nomappelation;
            }

            set
            {
                this.nomappelation = value;
            }
        }
    }
}
