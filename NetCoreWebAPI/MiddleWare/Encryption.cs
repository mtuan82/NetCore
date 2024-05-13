using Core.Configurations;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace NetCoreWebAPI.MiddleWare
{
    public class Encryption
    {
        private readonly RequestDelegate _next;
        private readonly EncryptionSettings settings;
        public Encryption(RequestDelegate next, IOptions<EncryptionSettings> appSettings)
        {
            _next = next;
            settings = appSettings.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            List<string> excludeURL = GetExcludeURLList();
            if (!excludeURL.Contains(httpContext.Request.Path.Value))
            {
                httpContext.Request.Body = DecryptStream(httpContext.Request.Body);
                httpContext.Response.Body = EncryptStream(httpContext.Response.Body);
                if (httpContext.Request.QueryString.HasValue)
                {
                    string decryptedString = DecryptString(httpContext.Request.QueryString.Value.Substring(1));
                    httpContext.Request.QueryString = new QueryString($"?{decryptedString}");
                }
            }
            await _next(httpContext);
        }

        private CryptoStream EncryptStream(Stream responseStream)
        {
            Aes aes = GetEncryptionAlgorithm();
            ToBase64Transform base64Transform = new ToBase64Transform();
            CryptoStream base64EncodedStream = new CryptoStream(responseStream, base64Transform, CryptoStreamMode.Write);
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            CryptoStream cryptoStream = new CryptoStream(base64EncodedStream, encryptor, CryptoStreamMode.Write);
            return cryptoStream;
        }
        static byte[] Encrypt(string plainText)
        {
            byte[] encrypted;
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            return encrypted;
        }

        // This are main functions that we decrypt the payload and  parameter which we pass from the angular service.
        private Stream DecryptStream(Stream cipherStream)
        {
            Aes aes = GetEncryptionAlgorithm();
            FromBase64Transform base64Transform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces);
            CryptoStream base64DecodedStream = new CryptoStream(cipherStream, base64Transform, CryptoStreamMode.Read);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            CryptoStream decryptedStream = new CryptoStream(base64DecodedStream, decryptor, CryptoStreamMode.Read);
            return decryptedStream;
        }
        private string DecryptString(string cipherText)
        {
            Aes aes = GetEncryptionAlgorithm();
            byte[] buffer = Convert.FromBase64String(cipherText);
            MemoryStream memoryStream = new MemoryStream(buffer);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            StreamReader streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        // We have to use same KEY and IV as we use for encryption in angular side.
        private Aes GetEncryptionAlgorithm()
        {
            Aes aes = Aes.Create();
            var secret_key = Encoding.UTF8.GetBytes(settings.SecretKey);
            var initialization_vector = Encoding.UTF8.GetBytes(settings.InitializationVector);
            aes.Key = secret_key;
            aes.IV = initialization_vector;
            return aes;
        }

        // This are excluded URL from encrypt- decrypt that already we added in angular side and as well as in ASP.NET CORE side.
        private List<string> GetExcludeURLList()
        {
            List<string> excludeURL = new List<string>();
            excludeURL.Add("/swagger");
            return excludeURL;
        }
    }
}
