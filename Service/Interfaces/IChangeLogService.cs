using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IChangeLogService
    {
        Task<string> CreateLog(Guid noteId, string newText);
    }
}
