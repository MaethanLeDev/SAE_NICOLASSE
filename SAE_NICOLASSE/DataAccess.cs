using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace SAE_NICOLASSE
{
    public class DataAccess
    {
        private static DataAccess instance;
        private readonly string connectionString;

        private DataAccess(string user, string password)
        {
            this.connectionString = $"Host=srv-peda-new;Port=5433;Username={user};Password={password};Database=nicolas.bd;Options='-c search_path=leshema'";
        }

        public static void CreerInstance(string user, string password)
        {
            if (instance == null)
            {
                instance = new DataAccess(user, password);
            }
        }

        // Méthode pour "oublier" la connexion de l'utilisateur précédent
        public static void ResetInstance()
        {
            instance = null;
        }

        public static DataAccess Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new InvalidOperationException("L'instance de DataAccess n'a pas été initialisée.");
                }
                return instance;
            }
        }

        // Chaque méthode ci-dessous ouvre et ferme sa propre connexion.
        // C'est la méthode la plus sûre.
        public DataTable ExecuteSelect(NpgsqlCommand cmd)
        {
            using (var conn = new NpgsqlConnection(this.connectionString))
            {
                conn.Open();
                cmd.Connection = conn;
                DataTable dataTable = new DataTable();
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
                return dataTable;
            }
        }

        public int ExecuteInsert(NpgsqlCommand cmd)
        {
            using (var conn = new NpgsqlConnection(this.connectionString))
            {
                conn.Open();
                cmd.Connection = conn;
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int ExecuteSet(NpgsqlCommand cmd)
        {
            using (var conn = new NpgsqlConnection(this.connectionString))
            {
                conn.Open();
                cmd.Connection = conn;
                return cmd.ExecuteNonQuery();
            }
        }
    }
}