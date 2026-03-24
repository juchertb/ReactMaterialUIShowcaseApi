namespace ReactMaterialUIShowcaseApi.Dtos
{
    public class SiteSettingsDto
    {
        public int Id { get; set; }
        public bool ProfileInvisibleMode { get; set; }

        // Accounts
        public bool AccountsSlack { get; set; }
        public bool AccountsSpotify { get; set; }
        public bool AccountsAtlassian { get; set; }
        public bool AccountsAsana { get; set; }

        // Notification Mentions
        public bool NotifMentionsEmail { get; set; }
        public bool NotifMentionsPush { get; set; }
        public bool NotifMentionsSms { get; set; }

        // Notification Comments
        public bool NotifCommentsEmail { get; set; }
        public bool NotifCommentsPush { get; set; }
        public bool NotifCommentsSms { get; set; }

        // Notification Follows
        public bool NotifFollowsEmail { get; set; }
        public bool NotifFollowsPush { get; set; }
        public bool NotifFollowsSms { get; set; }

        // Notification Login
        public bool NotifLoginEmail { get; set; }
        public bool NotifLoginPush { get; set; }
        public bool NotifLoginSms { get; set; }
    }
}
