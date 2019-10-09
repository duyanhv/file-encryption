using RivestCipher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivestCipher.Interface
{
    public interface IDocumentService: IBaseService<DocumentModel>
    {
        bool Create(string path, bool isEncrypted);
        bool CreateOrUpdate(DocumentModel createDocumentParams);
    }
}
