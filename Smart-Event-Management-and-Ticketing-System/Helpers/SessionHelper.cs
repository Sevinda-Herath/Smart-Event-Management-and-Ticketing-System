namespace Smart_Event_Management_and_Ticketing_System.Helpers
{
    /// <summary>
    /// Helper class for managing user sessions (login state)
    /// Provides simple session-based authentication without ASP.NET Identity
    /// </summary>
    public static class SessionHelper
    {
        // Session key constants
        private const string SESSION_MEMBER_ID = "MemberId";
        private const string SESSION_MEMBER_NAME = "MemberName";
        private const string SESSION_MEMBER_EMAIL = "MemberEmail";

        /// <summary>
        /// Set member session data after successful login
        /// </summary>
        public static void SetMemberSession(ISession session, int memberId, string memberName, string memberEmail)
        {
            session.SetInt32(SESSION_MEMBER_ID, memberId);
            session.SetString(SESSION_MEMBER_NAME, memberName);
            session.SetString(SESSION_MEMBER_EMAIL, memberEmail);
        }

        /// <summary>
        /// Get logged-in member's ID from session
        /// </summary>
        public static int? GetMemberId(ISession session)
        {
            return session.GetInt32(SESSION_MEMBER_ID);
        }

        /// <summary>
        /// Get logged-in member's name from session
        /// </summary>
        public static string? GetMemberName(ISession session)
        {
            return session.GetString(SESSION_MEMBER_NAME);
        }

        /// <summary>
        /// Get logged-in member's email from session
        /// </summary>
        public static string? GetMemberEmail(ISession session)
        {
            return session.GetString(SESSION_MEMBER_EMAIL);
        }

        /// <summary>
        /// Check if a member is currently logged in
        /// </summary>
        public static bool IsLoggedIn(ISession session)
        {
            return session.GetInt32(SESSION_MEMBER_ID).HasValue;
        }

        /// <summary>
        /// Clear member session data (logout)
        /// </summary>
        public static void ClearSession(ISession session)
        {
            session.Clear();
        }
    }
}
