using System.Collections.Generic;

namespace TD3_BindingBDPension.Model
{
    public interface ICrud<T>
    {
        int Create();
        void Read();
        int Update();
        int Delete();
        List<T> FindAll();
        List<T> FindBySelection(string criteres);
    }
}
