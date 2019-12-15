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
        MessageSent=8,
        MessageSentFailed =9,
        SavedSettings = 10,
        SaveSettingsFailed = 11,
        ReviewSaved = 12,
        ReviewSaveFailed = 13,

    }

    public enum LgsUserStatus
    {
        UserActive = 1,
        UserBlocked = 2,
        UserDeleted = 3,
        UserDoesnotExist = 4,
        InvalidRequest = 5
    }

    public enum LgsCompanySettingEnum
    {

    }

    public enum LgsNotificationEnum
    {
        Sms = 1,
        Email = 2,
        Both = 3
    }

    public enum LgsNotificationIntervalEnum
    {
        Weekly = 0,
        Monthly = 1,
    }
}