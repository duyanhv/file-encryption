using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivestCipher.Model
{
    public class DocumentModel: BaseModel
    {
        public string Name;
        public string Path;
        public bool IsEncrypted;
        public string Password;
    }
}
