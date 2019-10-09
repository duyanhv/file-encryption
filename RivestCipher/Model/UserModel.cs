using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivestCipher.Model
{
    public class UserModel: BaseModel
    {
        public string UserName;
        public string Password;
        public List<Guid> Documents;
        public bool IsLoggedIn = false;
    }
}
