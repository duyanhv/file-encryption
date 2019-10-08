using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivestCipher.Model
{
    public class DocumentModel: BaseModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsEncrypted { get; set; }
        public string Password { get; set; }
        public string FileExt { get; set; }
    }
}
