using Redux;
using RivestCipher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivestCipher.Action
{
    class DocumentAction
    {
        public class EncryptAction: IAction
        {
            public List<DocumentModel> createDocumentParams { get; set; }
        }
        public class DecryptAction : IAction
        {
            public List<DocumentModel> createDocumentParams { get; set; }
        }
    }
}
