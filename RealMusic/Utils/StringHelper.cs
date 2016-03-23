using System.Globalization;
using System.Linq;
using System.Text;

namespace RealMusic.Utils
{
    public static class StringHelper
    {
        public static string RemoveDiacriticsAndLowerCase(this string str)
        {
            if (str == null) return null;
            var chars =
                from c in str.Normalize(NormalizationForm.FormD).ToCharArray()
                let uc = CharUnicodeInfo.GetUnicodeCategory(c)
                where uc != UnicodeCategory.NonSpacingMark
                select c;

            var cleanStr = new string(chars.ToArray()).Normalize(NormalizationForm.FormC).ToLower().Replace("đ", "d");

            return cleanStr;
        }
    }
}
