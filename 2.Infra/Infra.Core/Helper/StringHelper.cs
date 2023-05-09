using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SFF.Infra.Core.Helper
{
    public static class StringHelper
    {

        public static bool IsNull(this string value) => value == null;

        public static bool IsNotNull(this string value) => !value.IsNull();

        public static bool IsEmpty(this string value) => value.IsNull() || (value.Trim() == "");

        public static bool IsNotEmpty(this string value) => !value.IsEmpty();

        public static bool IsEqual(this string value, string value2)
        {
            return value?.Trim()?.ToLower() == value2?.Trim()?.ToLower();
        }

        public static bool IsNotEqual(this string value, string value2) => !value.IsEqual(value2);

        public static string FormaterNullInEmpty(this string value) => value.IsEmpty() ? string.Empty : value;

        public static bool StartsWithAny(this string value, params string[] values)
        {
            if (value.IsEmpty() || values.IsEmpty()) return false;

            foreach (var v in values)
            {
                if (value.ToLower().StartsWith(v.ToLower()))
                    return true;
            }

            return false;
        }

        public static bool StartsNotWithAny(this string value, params string[] values) => !value.StartsWithAny(values);

        public static bool EndsWithAny(this string value, params string[] values)
        {
            if (value.IsEmpty() || values.IsEmpty()) return false;

            foreach (var v in values)
            {
                if (value.ToLower().EndsWith(v.ToLower()))
                    return true;
            }

            return false;
        }

        public static bool EndsNotWithAny(this string value, params string[] values) => !value.EndsWithAny(values);

        public static IEnumerable<string> Chunk(this string str, int size)
        {
            if (str.IsNotEmpty())
            {
                for (var i = 0; i < str.Length; i += size)
                {
                    yield return str.Substring(i, Math.Min(size, str.Length - i));
                }
            }
        }

        public static IEnumerable<string> Decode(this IEnumerable<string> values) => values.SelectOrDefault(value => WebUtility.HtmlDecode(value));

        public static string[] SplitBySemicolon(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return Array.Empty<string>();

            return str.Replace(" ", "").Split(';');
        }

        public static string RemoveSpecialCharacters(this string value)
        {
            if (value.IsEmpty()) return string.Empty;

            return new Regex("[^0-9a-zA-Z]+").Replace(value, string.Empty);
        }

        public static string RemoveIndexes(this string value)
        {
            if (value.IsEmpty()) return string.Empty;

            var replaced = value.ToCharArray().Where(p => char.IsLetter(p)).Aggregate("", (a, b) => a + b);

            return replaced;
        }

        public static string EndSubString(this string value, int size)
        {
            if (value.IsEmpty() || value.Length < size)
                return string.Empty;

            return value.Substring(value.ToString().Length - size, size);
        }
        public static Dictionary<string, string> DeserializeJsonAndFlatten(this string json)
        {
            var dict = new Dictionary<string, string>();
            var token = JToken.Parse(json == "" ? "{}" : json);
            FillDictionaryFromJToken(dict, token, "");
            return dict;
        }

        private static void FillDictionaryFromJToken(Dictionary<string, string> dict, JToken token, string prefix)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (var prop in token.Children<JProperty>())
                    {
                        var name = prop.Name;
                        var join = string.IsNullOrEmpty(prefix) ? name : prefix + "." + name;

                        FillDictionaryFromJToken(dict, prop.Value, join);
                    }
                    break;

                case JTokenType.Array:
                    var index = 0;
                    foreach (var value in token.Children())
                    {
                        var name = $"[{index}]";
                        var join = string.IsNullOrEmpty(prefix) ? name : prefix + "." + name;

                        FillDictionaryFromJToken(dict, value, join);
                        index++;
                    }
                    break;

                default:
                    dict.Add(prefix, ((JValue)token).Value?.ToString());
                    break;
            }
        }


        public static string EncodeToBase64(this string texto)
        {
            try
            {
                byte[] textoAsBytes = Encoding.ASCII.GetBytes(texto);
                string resultado = Convert.ToBase64String(textoAsBytes);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string DecodeFrom64(this string dados)
        {
            try
            {
                byte[] dadosAsBytes = Convert.FromBase64String(dados);
                string resultado = ASCIIEncoding.ASCII.GetString(dadosAsBytes);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string FormatCNPJ(this string CNPJ)
        {
            return string.IsNullOrEmpty(CNPJ) ? "" : Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
        }

        public static string FormatCPF(this string CPF)
        {
            return string.IsNullOrEmpty(CPF) ? "" : Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
        }
    }
}
