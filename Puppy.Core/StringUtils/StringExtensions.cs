﻿#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → TopCore </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TopCore </Project>
//     <File>
//         <Name> StringExtensions.cs </Name>
//         <Created> 28 Apr 17 2:52:15 PM </Created>
//         <Key> e00e427c-6798-4957-a7c2-bcd2f38b2b4a </Key>
//     </File>
//     <Summary>
//         StringExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Microsoft.AspNetCore.WebUtilities;
using Puppy.Core.AssemblyUtils;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Puppy.Core.StringUtils
{
    public static class StringExtensions
    {
        public static string EnsureLeadingSlash(this string url)
        {
            if (!url.StartsWith("/"))
            {
                return "/" + url;
            }

            return url;
        }

        public static string EnsureTrailingSlash(this string url)
        {
            if (!url.EndsWith("/"))
            {
                return url + "/";
            }

            return url;
        }

        public static string RemoveLeadingSlash(this string url)
        {
            if (url != null && url.StartsWith("/"))
            {
                url = url.Substring(1);
            }

            return url;
        }

        public static string RemoveTrailingSlash(this string url)
        {
            if (url != null && url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return url;
        }

        public static string CleanUrlPath(this string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                url = "/";
            }

            if (url != "/" && url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return url;
        }

        public static bool IsLocalUrl(this string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            if (url[0] == 47 && (url.Length == 1 || url[1] != 47 && url[1] != 92))
            {
                return true;
            }

            if (url.Length > 1 && url[0] == 126)
            {
                return url[1] == 47;
            }

            return false;
        }

        public static bool IsUrl(this string value)
        {
            bool isUrl = Uri.TryCreate(value, UriKind.Absolute, out var uriResult) && (uriResult.Scheme.ToLower() == "http" || uriResult.Scheme.ToLower() == "https");

            return isUrl;
        }

        public static string AddQueryString(this string url, string query)
        {
            if (!url.Contains("?"))
            {
                url += "?";
            }
            else if (!url.EndsWith("&"))
            {
                url += "&";
            }

            return url + query;
        }

        public static string GetOrigin(this string url)
        {
            if (url != null && (url.StartsWith("http://") || url.StartsWith("https://")))
            {
                var num = url.IndexOf("//", StringComparison.Ordinal);

                if (num > 0)
                {
                    var length = url.IndexOf("/", num + 2, StringComparison.Ordinal);

                    if (length >= 0)
                    {
                        url = url.Substring(0, length);
                    }

                    return url;
                }
            }
            return null;
        }

        /// <summary>
        ///     Get full physical path, if absolute path then return. Else combine with Current
        ///     Directory of Application.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFullPath(this string path)
        {
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var pathUri))
            {
                throw new ArgumentException($"Invalid path {path}");
            }

            if (pathUri.IsAbsoluteUri)
            {
                return path;
            }

            path = Path.Combine(Directory.GetCurrentDirectory(), path);

            return path;
        }

        /// <summary>
        ///     <para> Get full physical path, if absolute path then return. </para>
        ///     <para>
        ///         Else combine with Assembly Directory - use <c> typeof({class}).GetAssembly() </c>
        ///         to get Assembly you want.
        ///     </para>
        ///     <para>
        ///         if null data, will use root caller assembly - by <c> Assembly.GetEntryAssembly() </c>.
        ///     </para>
        /// </summary>
        /// <param name="path">     Relative or absolute path </param>
        /// <param name="assembly">
        ///     use <c> typeof({class}).GetAssembly() </c> to get Assembly you want. If null data,
        ///     will use root caller assembly - by <c> Assembly.GetEntryAssembly() </c>.
        /// </param>
        /// <returns></returns>
        public static string GetFullPath(this string path, Assembly assembly)
        {
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var pathUri))
            {
                throw new ArgumentException($"Invalid path {path}");
            }

            if (pathUri.IsAbsoluteUri)
            {
                return path;
            }

            string assemblyDirectory = assembly?.GetDirectoryPath() ?? Assembly.GetEntryAssembly().GetDirectoryPath();

            path = Path.Combine(assemblyDirectory, path);

            return path;
        }

        public static bool IsBase64(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            try
            {
                var byteArray = Convert.FromBase64String(value);

                return byteArray != null;
            }
            catch
            {
                return false;
            }
        }

        public static string EncodeBase64(this string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);

            string base64Encode = Convert.ToBase64String(bytes);

            return base64Encode;
        }

        public static string EncodeBase64Url(this string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);

            string base64Encode = WebEncoders.Base64UrlEncode(bytes);

            return base64Encode;
        }

        /// <summary>
        ///     Anti Javascript, Css, Html injection by encode html 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AntiInjection(this string value)
        {
            return value.EncodeHtml();
        }

        public static string EncodeHtml(this string value)
        {
            value = System.Net.WebUtility.HtmlEncode(value);
            return value;
        }

        public static string DecodeHtml(this string value)
        {
            value = System.Net.WebUtility.HtmlDecode(value);

            return value;
        }

        public static string DecodeBase64(this string value)
        {
            byte[] bytes = Convert.FromBase64String(value);

            string base64Decode = Encoding.ASCII.GetString(bytes);

            return base64Decode;
        }

        public static string DecodeBase64Url(this string value)
        {
            byte[] bytes = WebEncoders.Base64UrlDecode(value);

            string base64Decode = Encoding.ASCII.GetString(bytes);

            return base64Decode;
        }

        public static string GetSha256(this string value)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));

                var hash = BitConverter.ToString(hashBytes).Replace("-", "");

                return hash;
            }
        }

        public static string GetSha512(this string value)
        {
            using (var sha512 = SHA512.Create())
            {
                byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(value));

                var hash = BitConverter.ToString(hashBytes).Replace("-", "");

                return hash;
            }
        }

        public static string GetHmacSha256(this string value, string key)
        {
            var keyBytes = Convert.FromBase64String(key);

            var valueBytes = Encoding.UTF8.GetBytes(value);

            using (var shaAlgorithm = new HMACSHA256(keyBytes))
            {
                var hashBytes = shaAlgorithm.ComputeHash(valueBytes);

                var hash = BitConverter.ToString(hashBytes).Replace("-", "");
                return hash;
            }
        }

        public static string GetHmacSha512(this string value, string key)
        {
            var keyBytes = Convert.FromBase64String(key);

            var valueBytes = Encoding.UTF8.GetBytes(value);

            using (var shaAlgorithm = new HMACSHA512(keyBytes))
            {
                var hashBytes = shaAlgorithm.ComputeHash(valueBytes);

                var hash = BitConverter.ToString(hashBytes).Replace("-", "");

                return hash;
            }
        }

        public static string HashPassword(this string value, string salt, int iterations = 100000)
        {
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);

            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(valueBytes, saltBytes, iterations))
            {
                var hashBytes = rfc2898DeriveBytes.GetBytes(32);

                var hashString = Convert.ToBase64String(hashBytes);

                return hashString;
            }
        }

        public static string HashPassword(this string value, out string salt, int iterations = 100000)
        {
            salt = StringHelper.GenerateSalt();

            return value.HashPassword(salt, iterations);
        }

        public static string Encrypt(this string value, string key)
        {
            byte[] clearBytes = Encoding.ASCII.GetBytes(value);

            using (var encrypt = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                encrypt.Key = pdb.GetBytes(32);

                encrypt.IV = pdb.GetBytes(16);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                    }
                    value = Safe64Encoding.EncodeBytes(ms.ToArray());
                }
            }
            return value;
        }

        public static string Decrypt(this string value, string key)
        {
            byte[] cipherBytes = Safe64Encoding.DecodeBytes(value);

            using (var encrypt = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                encrypt.Key = pdb.GetBytes(32);

                encrypt.IV = pdb.GetBytes(16);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);

                        cs.Dispose();
                    }
                    value = Encoding.ASCII.GetString(ms.ToArray());
                }
            }
            return value;
        }

        public static bool TryDecrypt(this string value, string key, out string result)
        {
            try
            {
                result = value.Decrypt(key);

                return true;
            }
            catch
            {
                result = null;

                return false;
            }
        }

        public static object ParseTo(this string value, Type type)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(type);

            return converter.ConvertFrom(value);
        }
    }
}