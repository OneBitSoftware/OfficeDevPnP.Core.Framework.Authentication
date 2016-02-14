using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Authentication.S2S
{
    public abstract class OAuth2Message
    {
        private Dictionary<string, string> _message = new Dictionary<string, string>(StringComparer.Ordinal);

        protected string this[string index]
        {
            get
            {
                return this.GetValue(index);
            }
            set
            {
                this._message[index] = value;
            }
        }

        protected IEnumerable<string> Keys
        {
            get
            {
                return this._message.Keys;
            }
        }

        public Dictionary<string, string> Message
        {
            get
            {
                return this._message;
            }
        }

        protected OAuth2Message()
        {
        }

        protected bool ContainsKey(string key)
        {
            return this._message.ContainsKey(key);
        }

        protected void Decode(string message)
        {
            this._message.Decode(message);
        }

        protected void DecodeFromJson(string message)
        {
            this._message.DecodeFromJson(message);
        }

        protected string Encode()
        {
            return this._message.Encode();
        }

        protected string EncodeToJson()
        {
            return this._message.EncodeToJson();
        }

        protected string GetValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("The input string parameter is either null or empty.", "key");
            }
            string str = null;
            this._message.TryGetValue(key, out str);
            return str;
        }

        public override string ToString()
        {
            return this.Encode();
        }
    }
}
