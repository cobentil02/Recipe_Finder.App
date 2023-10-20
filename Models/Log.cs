using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models
{
    public class UserCredentials
    {
        [PrimaryKey]
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    
    }
}
