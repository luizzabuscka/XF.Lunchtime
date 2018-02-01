using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace XF.LunchTime.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConexao();
    }
}
