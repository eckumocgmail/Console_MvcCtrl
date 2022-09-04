using DataADO;

using System.ComponentModel.DataAnnotations;

namespace DataADO
{
    public class SqlServerConnectionString : BaseService
    {

        [Display(Name = "Сервер")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Server { get; set; }

        [Display(Name = "База данных")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Database { get; set; }

        [Display(Name = "Проверка подлинности Window")]
        [Required(ErrorMessage = "Обязательное поле")]
        public bool TrustedConnection { get; set; } = true;


        [Display(Name = "Пользователь")]
        public string UserID { get; set; }

        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public SqlServerConnectionString() : this("KEST", "tempdb") { }
        public SqlServerConnectionString(string server, string database) : this(server, database, true, "", "") { }
        public SqlServerConnectionString(string server, string database, bool trustedConnection, string userID, string password)
        {
            Server = server;
            Database = database;
            TrustedConnection = trustedConnection;
            UserID = userID;
            Password = password;
        }

        public override string ToString()
        {
            string constr = $"Server={Server};Database={Database};";
            if (TrustedConnection)
            {
                constr = constr + "Trusted_Connection=True;MultipleActiveResultSets=true;";
            }
            else
            {
                constr += $"UID={UserID};PWD={Password};MultipleActiveResultSets=true;";
            }
            return constr;
        }

    }

}