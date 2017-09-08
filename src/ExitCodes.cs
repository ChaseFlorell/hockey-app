namespace HockeyApp
{
    public static class ExitCodes
    {
        /// <summary>
        /// Used when command fails to execute successfully
        /// </summary>
        public static int CommandFailed => 1;

        /// <summary>
        /// Used when the user specified an invalid command
        /// </summary>
        public static int InvalidCommand => 2;
    }
}