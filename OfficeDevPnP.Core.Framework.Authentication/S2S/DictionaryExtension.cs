using Microsoft.AspNet.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Authentication.S2S
{
    internal static class DictionaryExtension
    {
        public const char DefaultSeparator = '&';

        public const char DefaultKeyValueSeparator = '=';

        public static DictionaryExtension.Encoder DefaultDecoder;

        public static DictionaryExtension.Encoder DefaultEncoder;

        public static DictionaryExtension.Encoder NullEncoder;

        static DictionaryExtension()
        {
            DictionaryExtension.DefaultDecoder = new DictionaryExtension.Encoder(Base64UrlDecode);
            DictionaryExtension.DefaultEncoder = new DictionaryExtension.Encoder(Base64UrlEncode);
            DictionaryExtension.NullEncoder = new DictionaryExtension.Encoder(DictionaryExtension.NullEncode);
        }

        public static void Decode(this IDictionary<string, string> self, string encodedDictionary)
        {
            self.Decode(encodedDictionary, '&', '=', DictionaryExtension.DefaultDecoder, DictionaryExtension.DefaultDecoder, false);
        }

        public static void Decode(this IDictionary<string, string> self, string encodedDictionary, DictionaryExtension.Encoder decoder)
        {
            self.Decode(encodedDictionary, '&', '=', decoder, decoder, false);
        }

        public static void Decode(this IDictionary<string, string> self, string encodedDictionary, char separator, char keyValueSplitter, bool endsWithSeparator)
        {
            self.Decode(encodedDictionary, separator, keyValueSplitter, DictionaryExtension.DefaultDecoder, DictionaryExtension.DefaultDecoder, endsWithSeparator);
        }

        public static void Decode(this IDictionary<string, string> self, string encodedDictionary, char separator, char keyValueSplitter, DictionaryExtension.Encoder keyDecoder, DictionaryExtension.Encoder valueDecoder, bool endsWithSeparator)
        {
            if (encodedDictionary == null)
            {
                throw new ArgumentNullException("encodedDictionary");
            }
            if (keyDecoder == null)
            {
                throw new ArgumentNullException("keyDecoder");
            }
            if (valueDecoder == null)
            {
                throw new ArgumentNullException("valueDecoder");
            }
            if (endsWithSeparator && encodedDictionary.LastIndexOf(separator) == encodedDictionary.Length - 1)
            {
                encodedDictionary = encodedDictionary.Substring(0, encodedDictionary.Length - 1);
            }
            string[] strArrays = encodedDictionary.Split(new char[] { separator });
            for (int i = 0; i < (int)strArrays.Length; i++)
            {
                string str = strArrays[i];
                string[] strArrays1 = str.Split(new char[] { keyValueSplitter });
                if (((int)strArrays1.Length == 1 || (int)strArrays1.Length > 2) && !string.IsNullOrEmpty(strArrays1[0]))
                {
                    throw new ArgumentException("The request is not properly formatted.", "encodedDictionary");
                }
                if ((int)strArrays1.Length != 2)
                {
                    throw new ArgumentException("The request is not properly formatted.", "encodedDictionary");
                }
                string str1 = keyDecoder(strArrays1[0].Trim());
                string str2 = strArrays1[1].Trim();
                char[] chrArray = new char[] { '\"' };
                string str3 = valueDecoder(str2.Trim(chrArray));
                try
                {
                    self.Add(str1, str3);
                }
                catch (ArgumentException argumentException)
                {
                    CultureInfo invariantCulture = CultureInfo.InvariantCulture;
                    object[] objArray = new object[] { str1 };
                    throw new ArgumentException(string.Format(invariantCulture, "The request is not properly formatted. The parameter '{0}' is duplicated.", objArray), "encodedDictionary");
                }
            }
        }

        public static void DecodeFromJson(this IDictionary<string, string> self, string encodedDictionary)
        {
            Dictionary<string, object> strs = JsonConvert.DeserializeObject<Dictionary<string, object>>(encodedDictionary);
            if (strs == null)
            {
                throw new ArgumentException("Invalid request format.", "encodedDictionary");
            }
            foreach (KeyValuePair<string, object> keyValuePair in strs)
            {
                if (keyValuePair.Value == null)
                {
                    self.Add(keyValuePair.Key, null);
                }
                else if (!(keyValuePair.Value is object[]))
                {
                    self.Add(keyValuePair.Key, keyValuePair.Value.ToString());
                }
                else
                {
                    self.Add(keyValuePair.Key, JsonConvert.SerializeObject(keyValuePair.Value));
                }
            }
        }

        public static string Encode(this IDictionary<string, string> self)
        {
            return self.Encode('&', '=', DictionaryExtension.DefaultEncoder, DictionaryExtension.DefaultEncoder, false);
        }

        public static string Encode(this IDictionary<string, string> self, DictionaryExtension.Encoder encoder)
        {
            return self.Encode('&', '=', encoder, encoder, false);
        }

        public static string Encode(this IDictionary<string, string> self, char separator, char keyValueSplitter, bool endsWithSeparator)
        {
            return self.Encode(separator, keyValueSplitter, DictionaryExtension.DefaultEncoder, DictionaryExtension.DefaultEncoder, endsWithSeparator);
        }

        public static string Encode(this IDictionary<string, string> self, char separator, char keyValueSplitter, DictionaryExtension.Encoder keyEncoder, DictionaryExtension.Encoder valueEncoder, bool endsWithSeparator)
        {
            if (keyEncoder == null)
            {
                throw new ArgumentNullException("keyEncoder");
            }
            if (valueEncoder == null)
            {
                throw new ArgumentNullException("valueEncoder");
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in self)
            {
                if (stringBuilder.Length != 0)
                {
                    stringBuilder.Append(separator);
                }
                stringBuilder.AppendFormat("{0}{1}{2}", keyEncoder(keyValuePair.Key), keyValueSplitter, valueEncoder(keyValuePair.Value));
            }
            if (endsWithSeparator)
            {
                stringBuilder.Append(separator);
            }
            return stringBuilder.ToString();
        }

        public static string EncodeToJson(this IDictionary<string, string> self)
        {
            return JsonConvert.SerializeObject(self);
        }

        private static string NullEncode(string value)
        {
            return value;
        }
        private static string Base64UrlEncode(string value)
        {
            return WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(value));
        }

        private static string Base64UrlDecode(string value)
        {
            return System.Text.Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(value));
        }

        public delegate string Encoder(string input);
    }
}
