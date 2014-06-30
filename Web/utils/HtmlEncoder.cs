namespace HawaiiDBEDT.Web.utils
{
    using System.Web;

    public class HtmlEncoder
    {
        public static string BetterHtmlDecode(string toDecode)
        {
            if (string.IsNullOrEmpty(toDecode))
            {
                return toDecode;
            }

            return HttpUtility.HtmlDecode(toDecode).Replace("&apos;", "'");
        }
    }
}