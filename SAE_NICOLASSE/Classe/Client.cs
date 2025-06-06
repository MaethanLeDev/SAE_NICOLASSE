using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_NICOLASSE.Classe
{
    public class Client
    {

        private int numClient;
        private string nomClient;
        private string prenomClient;
        private string mailClient;

        public Client(int numClient, string nomClient, string prenomClient, string mailClient)
        {
            this.NumClient = numClient;
            this.NomClient = nomClient;
            this.PrenomClient = prenomClient;
            this.MailClient = mailClient;
        }

        public int NumClient
        {
            get
            {
                return this.numClient;
            }

            set
            {
                this.numClient = value;
            }
        }

        public string NomClient
        {
            get
            {
                return this.nomClient;
            }

            set
            {
                this.nomClient = value;
            }
        }

        public string PrenomClient
        {
            get
            {
                return this.prenomClient;
            }

            set
            {
                this.prenomClient = value;
            }
        }

        public string MailClient
        {
            get
            {
                return this.mailClient;
            }

            set
            {
                this.mailClient = value;
            }
        }
    }
}
