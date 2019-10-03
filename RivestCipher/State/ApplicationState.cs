using RivestCipher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivestCipher.State
{
    public class ApplicationState
    {
        public UserModel UserProfile { get; set; }
        public string ConnectionString { get; set; }
    }
}
