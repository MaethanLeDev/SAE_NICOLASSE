using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_NICOLASSE
{
    public class appelation
    {

        private int numappelation;
        private string nomappelation;

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
