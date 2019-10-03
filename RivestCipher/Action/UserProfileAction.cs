using Redux;
using RivestCipher.Model;
using System;

namespace RivestCipher.Action
{
    class UserProfileAction
    {
        public class LoginAction: IAction
        {
            public LoginUserParams loginUserParams { get; set; }      
        }
        public class LogoutAction : IAction {
            public Guid userId { get; set; }
        }
        public class GetLoggedInUserAction : IAction
        {
        }
        public class RegisterAction : IAction
        {
            public LoginUserParams registerUserParams { get; set; }
        }
    }
}
