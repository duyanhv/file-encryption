using RivestCipher.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RivestCipher.Repository
{
    public class DocumentRepository
    {
        private string _connectionString;
        XmlSerializer _serializer;
        public DocumentRepository(string connectionString)
        {
            _connectionString = connectionString;
            _serializer = new XmlSerializer(typeof(List<DocumentModel>));
        }
        public List<DocumentModel> GetAll(List<Guid> Ids = default)
        {
            try
            {
                using (FileStream fs = new FileStream(_connectionString, FileMode.Open))
                {
                    var documents = (List<DocumentModel>)_serializer.Deserialize(fs);
                    if(Ids == null)
                    {
                        return documents.Where(d => d.IsActive).ToList();
                    }
                    else
                    {
                        return documents.Where(d => d.IsActive && Ids.Contains(d.Id)).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool Create(string path, bool isEncrypted)
        {
            try
            {
                var docs = GetAll();
                if (docs == null || docs.Count < 1)
                {
                    docs = new List<DocumentModel>();
                }
                var writer = new StreamWriter(_connectionString);
                var doc = new DocumentModel
                {
                    Id = Guid.NewGuid(),
                    Name = Path.GetFileName(path),
                    Path = path,
                    IsEncrypted = isEncrypted,
                };
                docs.Add(doc);
                _serializer.Serialize(writer, docs);
                writer.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateOrUpdate(DocumentModel createDocumentParams)
        {
            try { 
            var documents = GetAll();
            var writer = new StreamWriter(_connectionString);
            var isUpdate = false;
            if (documents == null || documents.Count < 1)
            {
                documents = new List<DocumentModel>();
            }

            documents.ToList().ForEach(document =>
            {
                 if (Guid.Equals(createDocumentParams.Id, document.Id))
                 {
                    document = createDocumentParams;
                    isUpdate = true;
                 }
            });
            if(!isUpdate)
            {
               documents.Add(createDocumentParams);
            }
            _serializer.Serialize(writer, documents);
            writer.Close();
            return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }


        public bool IsDocumentExists(string filename)
        {
            try
            {
                using (FileStream fs = new FileStream(_connectionString, FileMode.Open))
                {
                    var users = (List<DocumentModel>)_serializer.Deserialize(fs);
                    return users.ToList().Where(doc => String.Equals(doc.Name, filename, StringComparison.OrdinalIgnoreCase)).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
