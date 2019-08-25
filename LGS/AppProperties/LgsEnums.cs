using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGS.AppProperties
{
    public enum LgsAlertEnums
    {
        UserExist = 1,
        SuccessfulRegistration = 2,
        InvalidModel = 3,
        SuccessfulUpdate = 4,
        SuccessfulDelete = 5,
        SuccessfulBlock = 6,
        SuccessfulUnBlock = 7,

    }

    public enum LgsUserStatus
    {
        UserActive = 1,
        UserBlocked = 2,
        UserDeleted = 3,
        UserDoesnotExist = 4,
        InvalidRequest = 5
    }
}