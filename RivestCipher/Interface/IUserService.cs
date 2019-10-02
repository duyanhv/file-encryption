using RivestCipher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivestCipher.Interface
{
    public interface IUserService: IBaseService<Model.UserModel>
    {
        bool Create(string UserName, string Password);
        UserModel Login(string UserName, string Password);
        bool Logout(Guid userId);
    }
}
