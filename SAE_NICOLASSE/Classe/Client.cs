using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using TD3_BindingBDPension.Model;

namespace SAE_NICOLASSE.Classe
{
    public class Client : ICrud<Client>, INotifyPropertyChanged
    {
        public int NumClient { get; set; }
        public string NomClient { get; set; }
        public string PrenomClient { get; set; }
        public string MailClient { get; set; }

        public Client(int numClient, string nomClient, string prenomClient, string mailClient)
        {
            this.NumClient = numClient;
            this.NomClient = nomClient;
            this.PrenomClient = prenomClient;
            this.MailClient = mailClient;
        }
        public Client() { }

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<Client> FindAll()
        {
            List<Client> lesClients = new List<Client>();
            string query = "SELECT numclient, nomclient, prenomclient, mailclient FROM client ORDER BY nomclient, prenomclient;";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(query))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesClients.Add(new Client(
                        Convert.ToInt32(dr["numclient"]),
                        dr["nomclient"].ToString(),
                        dr["prenomclient"].ToString(),
                        dr["mailclient"].ToString()
                    ));
                }
            }
            return lesClients;
        }

        public int Create()
        {
            // Requête pour insérer un nouveau client et retourner son ID auto-généré
            string sql = @"INSERT INTO CLIENT (nomclient, prenomclient, mailclient) 
                   VALUES (@nom, @prenom, @mail) 
                   RETURNING numclient;";

            using (var cmd = new NpgsqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@nom", this.NomClient);
                cmd.Parameters.AddWithValue("@prenom", this.PrenomClient);
                cmd.Parameters.AddWithValue("@mail", this.MailClient);

                // On utilise ExecuteInsert qui est fait pour récupérer un ID en retour
                return DataAccess.Instance.ExecuteInsert(cmd);
            }
        }
        public int Delete() { throw new NotImplementedException(); }
        public List<Client> FindBySelection(string criteres) { throw new NotImplementedException(); }
        public void Read() { throw new NotImplementedException(); }
        public int Update() { throw new NotImplementedException(); }
    }
}
