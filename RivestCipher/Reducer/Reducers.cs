using Redux;
using RivestCipher.Interface;
using RivestCipher.Model;
using RivestCipher.Service;
using RivestCipher.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RivestCipher.Action.UserProfileAction;

namespace RivestCipher.Reducer
{
    class Reducers
    {
        public static ApplicationState ReduceApplication(ApplicationState previousState, IAction action)
        {
            return new ApplicationState
            {
                UserProfile = UserProfileReducer(previousState.UserProfile, action),
                ConnectionString = previousState.ConnectionString,
            };
        }

        public static UserModel LoginReducer(UserModel previousState, LoginAction action)
        {
            var userService = new UserService(App.Store.GetState().ConnectionString);
            return userService.Login(action.loginUserParams.UserName, action.loginUserParams.Password);
        }
        public static UserModel RegisterReducer(UserModel previousState, RegisterAction action)
        {
            var userService = new UserService(App.Store.GetState().ConnectionString);
            userService.Create(action.registerUserParams.UserName, action.registerUserParams.Password);
            return previousState;
        }
        public static UserModel LogoutReducer(UserModel previousState, LogoutAction action)
        {
            var hey = App.Store.GetState().ConnectionString;
            var userService = new UserService(App.Store.GetState().ConnectionString);
            userService.Logout(action.userId);
            return null;
        }
        public static UserModel GetLoggedInUserReducer(UserModel previousState, GetLoggedInUserAction action)
        {
            var hey = App.Store.GetState().ConnectionString;
            var userService = new UserService(App.Store.GetState().ConnectionString);
            return userService.GetLoggedInUser();
        }
        public static UserModel UserProfileReducer(UserModel previousState, IAction action)
        {
            if(action is LoginAction)
            {
                return LoginReducer(previousState, (LoginAction)action);
            }
            if(action is LogoutAction)
            {
                return LogoutReducer(previousState, (LogoutAction)action);
            }
            if(action is GetLoggedInUserAction)
            {
                return GetLoggedInUserReducer(previousState, (GetLoggedInUserAction)action);
            }
            if(action is RegisterAction)
            {
                return RegisterReducer(previousState, (RegisterAction)action);
            }
            return previousState;
        }
    }
}
