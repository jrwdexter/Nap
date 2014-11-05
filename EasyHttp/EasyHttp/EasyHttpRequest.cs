using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Newtonsoft.Json;

namespace EasyHttp
{
    public class EasyHttpRequest
    {
        public string Url { get; set; }

        public RequestFormat Format { get; set; }

        public HttpMethod Method { get; set; }

        public List<KeyValuePair<string, string>> QueryParameters = new List<KeyValuePair<string, string>>();

        public List<KeyValuePair<string, string>> Headers = new List<KeyValuePair<string, string>>();

        public string Body { get; set; }

        public EasyHttpRequest WithQueryParameter(string key, string value)
        {
            this.QueryParameters.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }
        public EasyHttpRequest WithBody(object body)
        {
            if (this.Format == RequestFormat.Json)
                this.Body = JsonConvert.SerializeObject(body);
            if (this.Format == RequestFormat.Xml)
            {
                var xmlSerializer = new XmlSerializer(body.GetType());
                var textWriter = new StringWriter();
                xmlSerializer.Serialize(textWriter, body);
                this.Body = textWriter.ToString();
            }
            if (this.Format == RequestFormat.Form)
            {
                var sb = new StringBuilder();
                foreach (var prop in body.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    sb.AppendFormat("{0}{1}&", prop.Name, prop.GetValue(body));
                }
                this.Body = sb.ToString().TrimEnd('&');
            }
            return this;
        }

        public EasyHttpRequest WithHeader(string headerName, string value)
        {
            this.Headers.Add(new KeyValuePair<string, string>(headerName, value));
            return this;
        }
        public async Task<string> ExecuteASync()
        {
            var client = new HttpClient();
            var content = new StringContent(this.Body);
            string urlTemp = this.Url;
            if(this.QueryParameters.Any())
            {
                urlTemp = this.Url.Contains("?") ? this.Url : this.Url+"?";
                urlTemp += string.Join("&", this.QueryParameters.Select(x=> string.Format("{0}={1}", x.Key, WebUtility.UrlEncode(x.Value))));
            }

            HttpResponseMessage response = null;
            if(this.Method == HttpMethod.Get)
                response = await client.GetAsync(new Uri(urlTemp));
            if(this.Method == HttpMethod.Post)
                response = await client.PostAsync(new Uri(urlTemp), content);
            if(this.Method == HttpMethod.Put)
                response = await client.PutAsync(new Uri(urlTemp), content);
            if(this.Method == HttpMethod.Delete)
                response = await client.DeleteAsync(new Uri(urlTemp)); 
            if(response == null)
                return default(string);
            throw new NotImplementedException();
        }
        public async Task<T> ExecuteASync<T>()
        {
            return (T) await this.ExecuteASync();
        }

    }
}