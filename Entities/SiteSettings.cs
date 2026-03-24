namespace ReactMaterialUIShowcaseApi.Entities
{
    public class SiteSettings
    {
        public int Id { get; set; }

        public bool ProfileInvisibleMode { get; set; }

        public bool AccountsSlack { get; set; }
        public bool AccountsSpotify { get; set; }
        public bool AccountsAtlassian { get; set; }
        public bool AccountsAsana { get; set; }

        public bool NotifMentionsEmail { get; set; }
        public bool NotifMentionsPush { get; set; }
        public bool NotifMentionsSms { get; set; }

        public bool NotifCommentsEmail { get; set; }
        public bool NotifCommentsPush { get; set; }
        public bool NotifCommentsSms { get; set; }

        public bool NotifFollowsEmail { get; set; }
        public bool NotifFollowsPush { get; set; }
        public bool NotifFollowsSms { get; set; }

        public bool NotifLoginEmail { get; set; }
        public bool NotifLoginPush { get; set; }
        public bool NotifLoginSms { get; set; }
    }
}