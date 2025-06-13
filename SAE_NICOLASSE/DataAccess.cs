// ========================================================================
// FICHIER : DataAccess.cs
// DÉCISION : Je conserve ta version ("moi"), car elle est la plus à jour.
//            Elle contient la bonne chaîne de connexion pour ton serveur
//            (srv-peda-new:5433) et utilise la bonne logique de
//            connexion directe que nous avons mise en place.
// ========================================================================

using Npgsql;
using System;
using System.Data;
using System.Windows;

namespace SAE_NICOLASSE
{
    public class DataAccess
    {
        private static DataAccess instance;
        private NpgsqlConnection connection;

        private DataAccess(string user, string password)
        {
            // Chaîne de connexion corrigée avec vos informations
            string connectionString = $"Host=srv-peda-new;Port=5433;Username={user};Password={password};Database=nicolas.bd;Options='-c search_path=leshema'";
            try
            {
                this.connection = new NpgsqlConnection(connectionString);
                this.connection.Open();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Échec de la tentative de connexion pour l'utilisateur : " + user);
                throw;
            }
        }

        public static void CreerInstance(string user, string password)
        {
            if (instance == null)
            {
                instance = new DataAccess(user, password);
            }
        }

        public static DataAccess Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new InvalidOperationException("L'instance de DataAccess n'a pas été initialisée. Appelez CreerInstance au préalable.");
                }
                return instance;
            }
        }

        public DataTable ExecuteSelect(NpgsqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            cmd.Connection = this.connection;
            using (var adapter = new NpgsqlDataAdapter(cmd))
            {
                adapter.Fill(dataTable);
            }
            return dataTable;
        }

        public int ExecuteInsert(NpgsqlCommand cmd)
        {
            cmd.Connection = this.connection;
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int ExecuteSet(NpgsqlCommand cmd)
        {
            cmd.Connection = this.connection;
            return cmd.ExecuteNonQuery();
        }

        public NpgsqlConnection GetConnection()
        {
            return this.connection;
        }
    }
}
