using System.Collections.Generic;

namespace AppEngine.Database
{
    public interface IDatabase<TClass>
    {
        TClass Select(int id);
        TClass Select(string name);
        int Insert(TClass tclass);
        int Alter(TClass tclass);
        int Delete(TClass tclass);
        int Update(TClass tclass);
        List<TClass> SelectMany(string column, string value);
        List<TClass> SelectAll();
        void SetObjectValues(int id);
        bool Exists(TClass tclass);
    }
}
