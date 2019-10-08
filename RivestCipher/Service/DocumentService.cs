using RivestCipher.Helper;
using RivestCipher.Interface;
using RivestCipher.Model;
using RivestCipher.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivestCipher.Service
{
    public class DocumentService : IDocumentService
    {

        DocumentRepository documentRepository;
        public DocumentService(string connectionString)
        {
            documentRepository = new DocumentRepository(connectionString);
        }
        public bool Activate(Guid Id)
        {
            throw new NotImplementedException();
        }

        public bool Create(string path, bool isEncrypted)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new Exception("Required Fields Are Blank");
            }
            var editedPath = AddNumberToFileName.GetUniqueFilePath(path);
            
            return documentRepository.Create(editedPath, isEncrypted);
        }

        public bool CreateOrUpdate(DocumentModel createDocumentParams)
        {
            return documentRepository.CreateOrUpdate(createDocumentParams);
        }

        public bool Deactivate(Guid Id)
        {
            throw new NotImplementedException();
        }

        public List<DocumentModel> GetAll()
        {
            return documentRepository.GetAll();
        }

        public DocumentModel GetById(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
