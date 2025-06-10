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
        public List<Client> FindAll()
        {
            List<Client> lesClients = new List<Client>();
            try
            {
                string query = "SELECT numclient, nomclient, prenomclient, mailclient FROM client ORDER BY nomclient, prenomclient;";

                using (NpgsqlCommand cmdSelect = new NpgsqlCommand(query))
                {
                    DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                    foreach (DataRow dr in dt.Rows)
                    {
                        
                        Client nouveauClient = new Client(
                            Convert.ToInt32(dr["numclient"]),
                            dr["nomclient"].ToString(),
                            dr["prenomclient"].ToString(),
                            dr["mailclient"].ToString()
                        );

                        lesClients.Add(nouveauClient);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des clients : " + ex.Message);
            }

            return lesClients;
        }

    }
}
