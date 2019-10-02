using Redux;
using RivestCipher.Interface;
using RivestCipher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RivestCipher.Action.UserProfileAction;

namespace RivestCipher.Reducer
{
    class UserProfileReducer
    {
        public static UserModel Execute(UserModel previousState, IAction action, IUserService userService, LoginUserParams loginUserParams)
        {
            if(action is LoginAction)
            {
                return userService.Login(loginUserParams.UserName, loginUserParams.Password);
            }
            if(action is LogoutAction)
            {
                userService.Logout(previousState.Id);
                return null;
            }
            return previousState;
        }
    }
}
