using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using XF.LunchTime.Data;

namespace XF.LunchTime.Model
{
    public class Favoritos
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string telefone { get; set; }
    }

    public class FavoritoRepository
    {
        private FavoritoRepository() { }

        private static SQLiteConnection database;
        private static readonly FavoritoRepository instance = new FavoritoRepository();
        public static FavoritoRepository Instance
        {
            get
            {
                if (database == null)
                {
                    database = DependencyService.Get<ISQLite>().GetConexao();
                    database.CreateTable<Favoritos>();
                }
                return instance;
            }
        }

        static object locker = new object();

        public static int SalvarFavorito(Favoritos favorito)
        {
            lock (locker)
            {
                if (favorito.Id != 0)
                {
                    database.Update(favorito);
                    return favorito.Id;
                }
                else return database.Insert(favorito);
            }
        }

        public static IEnumerable<Favoritos> GetFavoritos()
        {
            lock (locker)
            {
                return (from c in database.Table<Favoritos>()
                        select c).ToList();
            }
        }

        public static Favoritos GetFavorito(int Id)
        {
            lock (locker)
            {
                return database.Table<Favoritos>().Where(c => c.Id == Id).FirstOrDefault();
            }
        }

        public static int RemoverFavorito(int Id)
        {
            lock (locker)
            {
                return database.Delete<Favoritos>(Id);
            }
        }
    }
}
