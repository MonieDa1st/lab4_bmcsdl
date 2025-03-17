namespace QLSVNhom
{
    public static class LoggedInUser
    {
        public static string Manv { get; set; } = string.Empty;
        public static string PubKey { get; set; } = string.Empty;

        public static void Logout()
        {
            Manv = string.Empty;
            PubKey = string.Empty;
        }
    }
}
