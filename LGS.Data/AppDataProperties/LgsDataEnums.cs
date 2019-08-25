using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGS.Data.AppDataProperties
{
    public enum LgsUserStatus
    {
        UserActive = 1,
        UserBlocked = 2,
        UserDeleted = 3,
        UserDoesnotExist = 4,
        InvalidRequest = 5
    }
}
