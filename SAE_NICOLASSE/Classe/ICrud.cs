using SAE_NICOLASSE; // Ajout du using pour DataAccess
using System.Collections.Generic;

namespace TD3_BindingBDPension.Model
{
    public interface ICrud<T>
    {
        // Chaque m�thode qui interagit avec la base de donn�es doit maintenant
        // recevoir l'instance de DataAccess (le "badge de s�curit�").
        public int Create(DataAccess dao);
        public void Read(DataAccess dao);
        public int Update(DataAccess dao);
        public int Delete(DataAccess dao);
        public List<T> FindAll(DataAccess dao);
        public List<T> FindBySelection(string criteres, DataAccess dao);
    }
}
