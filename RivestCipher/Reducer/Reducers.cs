using Redux;
using RivestCipher.Helper;
using RivestCipher.Interface;
using RivestCipher.Model;
using RivestCipher.Service;
using RivestCipher.State;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RivestCipher.Action.DocumentAction;
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
                UserConnectionString = previousState.UserConnectionString,
                DocumentConnectionString = previousState.DocumentConnectionString,
                DocumentFolder = previousState.DocumentFolder,
                Documents = DocumentsReducer(previousState.Documents, action)
            };
        }

        public static UserModel LoginReducer(UserModel previousState, LoginAction action)
        {
            var userService = new UserService(App.Store.GetState().UserConnectionString);
            return userService.Login(action.loginUserParams.UserName, action.loginUserParams.Password);
        }
        public static UserModel RegisterReducer(UserModel previousState, RegisterAction action)
        {
            var userService = new UserService(App.Store.GetState().UserConnectionString);
            userService.Create(action.registerUserParams.UserName, action.registerUserParams.Password);
            return previousState;
        }
        public static UserModel LogoutReducer(UserModel previousState, LogoutAction action)
        {
            var userService = new UserService(App.Store.GetState().UserConnectionString);
            userService.Logout(action.userId);
            return null;
        }
        public static UserModel GetLoggedInUserReducer(UserModel previousState, GetLoggedInUserAction action)
        {
            var userService = new UserService(App.Store.GetState().UserConnectionString);
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

        public static List<DocumentModel> EncryptDocumentReducer(List<DocumentModel> previousState, EncryptAction action)
        {
            var isUserLoggedIn = App.Store.GetState().UserProfile != null;
            var documentService = new DocumentService(App.Store.GetState().DocumentConnectionString);
            var userService = new UserService(App.Store.GetState().UserConnectionString);
            var newListDocuments = previousState;
            foreach (var document in action.createDocumentParams)
            {
                if (!File.Exists(document.Path) || !CanReadFile.Check(document.Path))
                {
                    continue;
                }
                var rc4 = new RC4(document.Password.Trim(), File.ReadAllBytes(document.Path));
                var encryptedFileRaw = rc4.Encrypt();
                var newDocumentName = string.Join("_", Path.GetFileNameWithoutExtension(document.Path), string.Format("{0:yyyy-MM-dd_hh-mm-ss-fff}", DateTime.Now));
                var encryptedFilePath = Path.Combine(App.Store.GetState().DocumentFolder, newDocumentName);
                File.WriteAllBytes(encryptedFilePath, encryptedFileRaw);
                var documentId = document.Id == Guid.Empty ? Guid.NewGuid() : document.Id;
                var encyptedDocument = new DocumentModel
                {
                    Id = documentId,
                    Path = encryptedFilePath,
                    IsEncrypted = true,
                    Name = Path.GetFileName(newDocumentName),
                    Password = document.Password.Trim(),
                    FileExt = Path.GetExtension(document.Path)
                };
                if (isUserLoggedIn)
                {
                    documentService.CreateOrUpdate(encyptedDocument);
                    userService.UpdateUserDocument(App.Store.GetState().UserProfile.Id, documentId);
                }
                else
                {
                    newListDocuments.Add(encyptedDocument);
                }
            }
            return newListDocuments;
        }
        public static List<DocumentModel> DecryptDocumentReducer(List<DocumentModel> previousState, DecryptAction action)
        {
            var isUserLoggedIn = App.Store.GetState().UserProfile != null;
            var documentService = new DocumentService(App.Store.GetState().DocumentConnectionString);
            var userService = new UserService(App.Store.GetState().UserConnectionString);
            var newListDocuments = previousState;
            foreach (var document in action.createDocumentParams)
            {
                if (!File.Exists(document.Path) || !CanReadFile.Check(document.Path))
                {
                    continue;
                }
                var rc4 = new RC4(document.Password.Trim(), File.ReadAllBytes(document.Path));
                var newDocumentName = string.Join("_", Path.GetFileNameWithoutExtension(document.Path), string.Format("{0:yyyy-MM-dd_hh-mm-ss-fff}{1}", DateTime.Now, document.FileExt));
                var decryptedFileRaw = rc4.Decrypt(document.Path);
                var decryptedFilePath = Path.Combine(App.Store.GetState().DocumentFolder, newDocumentName);
                File.WriteAllBytes(decryptedFilePath, decryptedFileRaw);
                var documentId = document.Id == Guid.Empty ? Guid.NewGuid() : document.Id;
                var decyptedDocument = new DocumentModel
                {
                    Id = documentId,
                    Path = decryptedFilePath,
                    IsEncrypted = false,
                    Name = Path.GetFileName(newDocumentName),
                    Password = document.Password.Trim(),
                    FileExt = document.FileExt
                };
                if (isUserLoggedIn)
                {
                    documentService.CreateOrUpdate(decyptedDocument);
                    userService.UpdateUserDocument(App.Store.GetState().UserProfile.Id, documentId);
                }
                else
                {
                    newListDocuments.Add(decyptedDocument);
                }
            }
            return newListDocuments;
        }
        public static List<DocumentModel> GetDocumentsReducer(List<DocumentModel> previousState, GetDocumentsAction action)
        {
            var documentService = new DocumentService(App.Store.GetState().DocumentConnectionString);
            if(App.Store.GetState().UserProfile != null)
            {
                return documentService.GetAll(App.Store.GetState().UserProfile != null ? App.Store.GetState().UserProfile.Documents : null);
            }
            return previousState;
        }
        public static List<DocumentModel> DeactivationDocumentReducer(List<DocumentModel> previousState, DeactivateDocumentAction action)
        {
            var documentService = new DocumentService(App.Store.GetState().DocumentConnectionString);
            var newDocument = action.deactivateDocumentParams;
            newDocument.IsActive = false;
            documentService.CreateOrUpdate(newDocument);
            return previousState;
        }
        public static List<DocumentModel> ActivationDocumentReducer(List<DocumentModel> previousState, ActivateDocumentAction action)
        {
            var documentService = new DocumentService(App.Store.GetState().DocumentConnectionString);
            var newDocument = action.deactivateDocumentParams;
            newDocument.IsActive = true;
            documentService.CreateOrUpdate(newDocument);
            return previousState;
        }

        public static List<DocumentModel> DocumentsReducer(List<DocumentModel> previousState, IAction action)
        {
            if(action is EncryptAction)
            {
                return EncryptDocumentReducer(previousState, (EncryptAction) action);
            }
            if(action is DecryptAction)
            {
                return DecryptDocumentReducer(previousState, (DecryptAction)action);
            }
            if(action is GetDocumentsAction)
            {
                return GetDocumentsReducer(previousState, (GetDocumentsAction)action);
            }
            if(action is DeactivateDocumentAction)
            {
                return DeactivationDocumentReducer(previousState, (DeactivateDocumentAction)action);
            }
            if (action is ActivateDocumentAction)
            {
                return ActivationDocumentReducer(previousState, (ActivateDocumentAction)action);
            }
            return previousState;
        }
    }
}
