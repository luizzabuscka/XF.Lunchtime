using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using XF.LunchTime.Data;
using Xamarin.Forms;

namespace XF.LunchTime.Model
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }




    public class UserRepository
    {
        private UserRepository() { }

        private static SQLiteConnection database;
        private static readonly UserRepository instance = new UserRepository();
        public static UserRepository Instance
        {
            get
            {
                if (database == null)
                {
                    database = DependencyService.Get<ISQLite>().GetConexao();
                    database.CreateTable<User>();
                }
                return instance;
            }
        }

        static object locker = new object();

        public static int SalvarUsuario(User user)
        {
            lock (locker)
            {
                if (user.Id != 0)
                {
                    database.Update(user);
                    return user.Id;
                }
                else return database.Insert(user);
            }
        }

        public static IEnumerable<User> GetUsuarios()
        {
            lock (locker)
            {
                return (from c in database.Table<User>()
                        select c).ToList();
            }
        }

        public static User GetUsuario(string email,string senha)
        {
            lock (locker)
            {
                return database.Table<User>().Where(c => c.Email == email && c.Senha == senha).FirstOrDefault();
            }
        }

        public static int RemoverUsuario(int Id)
        {
            lock (locker)
            {
                return database.Delete<User>(Id);
            }
        }


        public static bool IsAutorizado(User paramLogin)
        {
            return (paramLogin.Nome == "usuario" && paramLogin.Senha == "1234");
        }
    }
}
