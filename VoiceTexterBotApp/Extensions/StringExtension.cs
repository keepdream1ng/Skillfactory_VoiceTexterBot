namespace VoiceTexterBotApp.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Converting string, so it will start crom uppercase letter. 
        /// </summary>
        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}