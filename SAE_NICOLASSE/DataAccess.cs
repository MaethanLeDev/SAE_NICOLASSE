

using System.Collections.Generic;
using System.Data;
using System.Windows;
using Microsoft.Extensions.Logging;
using Npgsql;


namespace SAE_NICOLASSE
{

    public class DataAccess
    {
        private readonly string connectionString;
        private NpgsqlConnection connection;

        public DataAccess(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "La chaîne de connexion ne peut pas être vide.");
            }
            this.connectionString = connectionString;
            this.connection = new NpgsqlConnection(this.connectionString);
        }

        public NpgsqlConnection GetConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb de connexion GetConnection \n" + connectionString);
                throw;
            }
            return connection;
        }

        public DataTable ExecuteSelect(NpgsqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            try
            {
                cmd.Connection = GetConnection();
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL : " + cmd.CommandText);
                throw;
            }
            return dataTable;
        }

        public int ExecuteInsert(NpgsqlCommand cmd)
        {
            try
            {
                cmd.Connection = GetConnection();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete insert " + cmd.CommandText);
                throw;
            }
        }

        public int ExecuteSet(NpgsqlCommand cmd)
        {
            try
            {
                cmd.Connection = GetConnection();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete set " + cmd.CommandText);
                throw;
            }
        }

        public void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}



