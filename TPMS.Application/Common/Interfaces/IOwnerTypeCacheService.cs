using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Common.Interfaces
{
    public interface IOwnerTypeCacheService
    {
        int GetOwnerTypeId(string typeName);
        string GetOwnerTypeName(int id);
        void RefreshCache();
    }
}
