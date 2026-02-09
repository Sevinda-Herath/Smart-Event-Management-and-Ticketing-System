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
        private const string SESSION_MEMBER_ROLE = "MemberRole";

        /// <summary>
        /// Set member session data after successful login
        /// </summary>
        public static void SetMemberSession(ISession session, int memberId, string memberName, string memberEmail, string role)
        {
            session.SetInt32(SESSION_MEMBER_ID, memberId);
            session.SetString(SESSION_MEMBER_NAME, memberName);
            session.SetString(SESSION_MEMBER_EMAIL, memberEmail);
            session.SetString(SESSION_MEMBER_ROLE, role);
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
        /// Get logged-in member's role from session
        /// </summary>
        public static string? GetMemberRole(ISession session)
        {
            return session.GetString(SESSION_MEMBER_ROLE);
        }

        /// <summary>
        /// Check if a member is currently logged in
        /// </summary>
        public static bool IsLoggedIn(ISession session)
        {
            return session.GetInt32(SESSION_MEMBER_ID).HasValue;
        }

        /// <summary>
        /// Check if the logged-in user is an admin
        /// </summary>
        public static bool IsAdmin(ISession session)
        {
            return GetMemberRole(session) == "Admin";
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
