using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivestCipher.Interface
{
    public interface IBaseService<T>
    {
        bool Deactivate(Guid Id);
        bool Activate(Guid Id);
        List<T> GetAll(List<Guid> Id = default);
        T GetById(Guid Id);
    }
}
