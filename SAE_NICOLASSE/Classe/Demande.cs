using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using TD3_BindingBDPension.Model;

namespace SAE_NICOLASSE.Classe
{
    public class Demande : ICrud<Demande>, INotifyPropertyChanged
    {

        private int numDemande;
        private Vin unVin;
        private Employe unEmploye;
        private Commande numCommande;
        private Client numClient;
        private DateTime dateDemande;
        private int quantiteDemande;
        private string accepter;

        public Demande(int numDemande, Vin unVin, Employe unEmploye, Commande numCommande, Client numClient, DateTime dateDemande, int quantiteDemande, string accepter)
        {
            this.NumDemande = numDemande;
            this.UnVin = unVin;
            this.UnEmploye = unEmploye;
            this.NumCommande = numCommande;
            this.NumClient = numClient;
            this.DateDemande = dateDemande;
            this.QuantiteDemande = quantiteDemande;
            this.Accepter = accepter;
        }
        public Demande() { }

        public int NumDemande
        {
            get
            {
                return this.numDemande;
            }

            set
            {
                this.numDemande = value;
            }
        }

        public Vin UnVin
        {
            get
            {
                return this.unVin;
            }

            set
            {
                this.unVin = value;
            }
        }

        public Employe UnEmploye
        {
            get
            {
                return this.unEmploye;
            }

            set
            {
                this.unEmploye = value;
            }
        }

        public Commande NumCommande
        {
            get
            {
                return this.numCommande;
            }

            set
            {
                this.numCommande = value;
            }
        }

        public Client NumClient
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

        public DateTime DateDemande
        {
            get
            {
                return this.dateDemande;
            }

            set
            {
                this.dateDemande = value;
            }
        }

        public int QuantiteDemande
        {
            get
            {
                return this.quantiteDemande;
            }

            set
            {
                this.quantiteDemande = value;
            }
        }

        public string Accepter
        {
            get
            {
                return this.accepter;
            }

            set
            {
                this.accepter = value;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            throw new NotImplementedException();
        }

        public int Delete()
        {
            throw new NotImplementedException();
        }

        public List<Demande> FindAll()
        {
            List<Demande> lesDemandes = new List<Demande>();
            try
            {
                // La requête SQL est complexe car elle doit récupérer les informations pour tous les objets imbriqués.
                // On utilise des alias (ex: 'demandeur_nom') pour éviter les conflits de noms de colonnes.
                string sql = @"
                SELECT
                    -- Champs de la Demande
                    d.numdemande, d.datedemande, d.quantitedemande, d.accepter,

                    -- Champs du Client lié à la Demande
                    cl.numclient, cl.nomclient, cl.prenomclient, cl.mailclient,

                    -- Champs de l'Employé qui a enregistré la Demande
                    e_dem.numemploye    AS demandeur_num,
                    e_dem.nom           AS demandeur_nom,
                    e_dem.prenom        AS demandeur_prenom,
                    e_dem.login         AS demandeur_login,
                    e_dem.mdp           AS demandeur_mdp,
                    r_dem.numrole       AS demandeur_role_num,
                    r_dem.nomrole       AS demandeur_role_nom,

                    -- Champs du Vin lié à la Demande
                    v.numvin, v.nomvin, v.prixvin, v.descriptif, v.millesime,
                    f.numfournisseur, f.nomfournisseur,
                    tv.numtype, tv.nomtype,
                    v.numtype2          AS appelation_num, 
                    a.nomappelation,

                    -- Champs de la Commande liée (peut être NULL)
                    c.numcommande, c.datecommande, c.valider, c.prixtotal,

                    -- Champs de l'Employé de la Commande (peut être NULL)
                    e_cde.numemploye    AS cde_employe_num,
                    e_cde.nom           AS cde_employe_nom,
                    e_cde.prenom        AS cde_employe_prenom,
                    e_cde.login         AS cde_employe_login,
                    e_cde.mdp           AS cde_employe_mdp,
                    r_cde.numrole       AS cde_role_num,
                    r_cde.nomrole       AS cde_role_nom

                FROM
                    DEMANDE d
                    -- Jointures obligatoires
                    JOIN CLIENT cl ON d.numclient = cl.numclient
                    JOIN VIN v ON d.numvin = v.numvin
                    JOIN EMPLOYE e_dem ON d.numemploye = e_dem.numemploye
                    -- Sous-jointures obligatoires
                    JOIN FOURNISSEUR f ON v.numfournisseur = f.numfournisseur
                    JOIN TYPEVIN tv ON v.numtype = tv.numtype
                    JOIN APPELATION a ON v.numtype2 = a.numtype
                    JOIN ROLE r_dem ON e_dem.numrole = r_dem.numrole
                    -- Jointures optionnelles (car une demande n'a pas toujours de commande)
                    LEFT JOIN COMMANDE c ON d.numcommande = c.numcommande
                    LEFT JOIN EMPLOYE e_cde ON c.numemploye = e_cde.numemploye
                    LEFT JOIN ROLE r_cde ON e_cde.numrole = r_cde.numrole

                ORDER BY d.datedemande DESC;";

                using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
                {
                    // Exécution de la requête via votre couche d'accès aux données
                    DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);

                    foreach (DataRow dr in dt.Rows)
                    {
                        // 1. Construire l'objet Client
                        Client leClient = new Client(
                            Convert.ToInt32(dr["numclient"]),
                            dr["nomclient"].ToString(),
                            dr["prenomclient"].ToString(),
                            dr["mailclient"].ToString()
                        );

                        // 2. Construire l'objet Employe (celui qui a fait la demande)
                        Role roleDemandeur = new Role(
                            Convert.ToInt32(dr["demandeur_role_num"]),
                            dr["demandeur_role_nom"].ToString()
                        );
                        Employe employeDemandeur = new Employe(
                            Convert.ToInt32(dr["demandeur_num"]),
                            roleDemandeur,
                            dr["demandeur_nom"].ToString(),
                            dr["demandeur_prenom"].ToString(),
                            dr["demandeur_login"].ToString(),
                            dr["demandeur_mdp"].ToString()
                        );

                        // 3. Construire l'objet Vin
                        Fournisseur leFournisseur = new Fournisseur(Convert.ToInt32(dr["numfournisseur"]), dr["nomfournisseur"].ToString());
                        TypeVin leTypeVin = new TypeVin(Convert.ToInt32(dr["numtype"]), dr["nomtype"].ToString());
                        Appelation lAppelation = new Appelation(Convert.ToInt32(dr["appelation_num"]), dr["nomappelation"].ToString());
                        Vin leVin = new Vin(
                            Convert.ToInt32(dr["numvin"]),
                            leFournisseur,
                            leTypeVin,
                            lAppelation,
                            dr["nomvin"].ToString(),
                            Convert.ToDecimal(dr["prixvin"]),
                            dr["descriptif"].ToString(),
                            Convert.ToInt32(dr["millesime"])
                        );

                        // 4. Construire l'objet Commande (qui peut être null)
                        Commande laCommande = null;
                        if (dr["numcommande"] != DBNull.Value)
                        {
                            Role roleCommande = new Role(
                                Convert.ToInt32(dr["cde_role_num"]),
                                dr["cde_role_nom"].ToString()
                            );
                            Employe employeCommande = new Employe(
                                 Convert.ToInt32(dr["cde_employe_num"]),
                                 roleCommande,
                                 dr["cde_employe_nom"].ToString(),
                                 dr["cde_employe_prenom"].ToString(),
                                 dr["cde_employe_login"].ToString(),
                                 dr["cde_employe_mdp"].ToString()
                            );
                            laCommande = new Commande(
                                Convert.ToInt32(dr["numcommande"]),
                                employeCommande,
                                Convert.ToDateTime(dr["datecommande"]),
                                Convert.ToBoolean(dr["valider"]),
                                Convert.ToDecimal(dr["prixtotal"])
                            );
                        }

                        // 5. Finalement, construire l'objet Demande
                        Demande laDemande = new Demande(
                            Convert.ToInt32(dr["numdemande"]),
                            leVin,
                            employeDemandeur,
                            laCommande, // Cet objet peut être null, ce qui est correct
                            leClient,
                            Convert.ToDateTime(dr["datedemande"]),
                            Convert.ToInt32(dr["quantitedemande"]),
                            dr["accepter"].ToString()
                        );

                        lesDemandes.Add(laDemande);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des demandes : " + ex.Message);
            }

            return lesDemandes;
        }

        public List<Demande> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        

        public int Update()
        {
            string sql = @"UPDATE DEMANDE 
                       SET ACCEPTER = @accepter 
                       WHERE NUMDEMANDE = @numdemande";

            using (var cmdUpdate = new NpgsqlCommand(sql))
            {

                cmdUpdate.Parameters.AddWithValue("@accepter", this.Accepter);
                cmdUpdate.Parameters.AddWithValue("@numdemande", this.NumDemande);


                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }
    }
}
