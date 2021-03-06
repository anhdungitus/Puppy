﻿#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Puppy </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Puppy </Project>
//     <File>
//         <Name> HtmlHelper.cs </Name>
//         <Created> 17/10/17 4:02:35 PM </Created>
//         <Key> 91ba3f3b-4117-4bc6-abe7-dca2ce5cbc66 </Key>
//     </File>
//     <Summary>
//         HtmlHelper.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Puppy.Core.FileUtils;
using Puppy.Core.StringUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Puppy.Web.HtmlUtils
{
    public class HtmlHelper
    {
        internal class ToolEnvironment
        {
            public string TempFolderPath { get; set; }

            public string ToolPath { get; set; }

            public int Timeout { get; set; }

            public bool Debug { get; set; }
        }

        #region Exception

        public class HtmlConvertException : Exception
        {
            public HtmlConvertException(string msg) : base(msg)
            {
            }
        }

        public class HtmlConvertTimeoutException : HtmlConvertException
        {
            public HtmlConvertTimeoutException() : base("HTML to PDF conversion process has not finished in the given period.")
            {
            }
        }

        #endregion

        #region Html to PDF

        /// <summary>
        ///     Convert Html to Pdf 
        /// </summary>
        /// <param name="url">     File path or HTTP URL </param>
        /// <param name="pdfPath"></param>
        public static void ToPdf(string url, string pdfPath)
        {
            var pdfOutput = new HtmlToPdf.PdfOutput { OutputFilePath = pdfPath };

            ToPdf(new HtmlToPdf.PdfDocument { Url = url }, pdfOutput);
        }

        public static void ToPdfFromHtml(string html, string pdfPath)
        {
            // Generate Temp File from Html Content
            var url = FileHelper.CreateTempFile(".html");
            File.WriteAllText(url, html);

            try
            {
                // Convert
                ToPdf(url, pdfPath);
            }
            finally
            {
                // Remove Temp
                FileHelper.SafeDelete(url);
            }
        }

        /// <summary>
        ///     Convert Html to Pdf 
        /// </summary>
        /// <param name="url"> File path or HTTP URL </param>
        public static byte[] ToPdf(string url)
        {
            byte[] fileBytes = null;

            var pdfOutput = new HtmlToPdf.PdfOutput
            {
                OutputCallback = (document, bytes) =>
                {
                    fileBytes = bytes;
                }
            };

            ToPdf(new HtmlToPdf.PdfDocument { Url = url }, pdfOutput);

            return fileBytes;
        }

        public static byte[] ToPdfFromHtml(string html)
        {
            // Generate Temp File from Html Content
            var url = FileHelper.CreateTempFile(".html");
            File.WriteAllText(url, html);

            try
            {
                // Convert
                var bytes = ToPdf(url);
                return bytes;
            }
            finally
            {
                // Remove Temp
                FileHelper.SafeDelete(url);
            }
        }

        public static void ToPdf(HtmlToPdf.PdfDocument document, HtmlToPdf.PdfOutput pdfOutput)
        {
            var environment = new ToolEnvironment
            {
                TempFolderPath = Path.GetTempPath(),
                ToolPath = $"{nameof(HtmlUtils)}/wkhtml/wkhtmltopdf.exe".GetFullPath(null),
                Timeout = 60000
            };

            string outputPdfFilePath;
            bool delete;
            if (pdfOutput.OutputFilePath != null)
            {
                outputPdfFilePath = pdfOutput.OutputFilePath;
                delete = false;
            }
            else
            {
                outputPdfFilePath = Path.Combine(environment.TempFolderPath, $"{Guid.NewGuid():N}.pdf");
                delete = true;
            }

            if (!File.Exists(environment.ToolPath))
            {
                throw new HtmlConvertException($"File '{environment.ToolPath}' not found. Check if wkhtmltopdf application is installed.");
            }

            var paramsBuilder = new StringBuilder();

            paramsBuilder.AppendFormat("--page-size {0} ", document.PaperType);

            if (!string.IsNullOrWhiteSpace(document.HeaderUrl))
            {
                paramsBuilder.AppendFormat("--header-html {0} ", document.HeaderUrl);
                paramsBuilder.Append("--margin-top 25 ");
                paramsBuilder.Append("--header-spacing 5 ");
            }
            if (!string.IsNullOrWhiteSpace(document.FooterUrl))
            {
                paramsBuilder.AppendFormat("--footer-html {0} ", document.FooterUrl);
                paramsBuilder.Append("--margin-bottom 25 ");
                paramsBuilder.Append("--footer-spacing 5 ");
            }
            if (!string.IsNullOrWhiteSpace(document.HeaderLeft))
            {
                paramsBuilder.AppendFormat("--header-left \"{0}\" ", document.HeaderLeft);
            }

            if (!string.IsNullOrWhiteSpace(document.HeaderCenter))
            {
                paramsBuilder.AppendFormat("--header-center \"{0}\" ", document.HeaderCenter);
            }

            if (!string.IsNullOrWhiteSpace(document.HeaderRight))
            {
                paramsBuilder.AppendFormat("--header-right \"{0}\" ", document.HeaderRight);
            }

            if (!string.IsNullOrWhiteSpace(document.FooterLeft))
            {
                paramsBuilder.AppendFormat("--footer-left \"{0}\" ", document.FooterLeft);
            }

            if (!string.IsNullOrWhiteSpace(document.FooterCenter))
            {
                paramsBuilder.AppendFormat("--footer-center \"{0}\" ", document.FooterCenter);
            }

            if (!string.IsNullOrWhiteSpace(document.FooterRight))
            {
                paramsBuilder.AppendFormat("--footer-right \"{0}\" ", document.FooterRight);
            }

            if (!string.IsNullOrWhiteSpace(document.HeaderFontSize))
            {
                paramsBuilder.AppendFormat("--header-font-size \"{0}\" ", document.HeaderFontSize);
            }

            if (!string.IsNullOrWhiteSpace(document.FooterFontSize))
            {
                paramsBuilder.AppendFormat("--footer-font-size \"{0}\" ", document.FooterFontSize);
            }

            if (!string.IsNullOrWhiteSpace(document.HeaderFontName))
            {
                paramsBuilder.AppendFormat("--header-font-name \"{0}\" ", document.HeaderFontName);
            }

            if (!string.IsNullOrWhiteSpace(document.FooterFontName))
            {
                paramsBuilder.AppendFormat("--footer-font-name \"{0}\" ", document.FooterFontName);
            }

            if (document.ExtraParams != null)
            {
                foreach (var extraParam in document.ExtraParams)
                {
                    paramsBuilder.AppendFormat("--{0} {1} ", extraParam.Key, extraParam.Value);
                }
            }

            if (document.Cookies != null)
            {
                foreach (var cookie in document.Cookies)
                {
                    paramsBuilder.AppendFormat("--cookie {0} {1} ", cookie.Key, cookie.Value);
                }
            }

            paramsBuilder.AppendFormat("\"{0}\" \"{1}\"", document.Url, outputPdfFilePath);

            try
            {
                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = environment.ToolPath;
                    process.StartInfo.Arguments = paramsBuilder.ToString();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardInput = true;

                    using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                    using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                    {
                        DataReceivedEventHandler outputHandler = (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                outputWaitHandle.Set();
                            }
                            else
                            {
                                output.AppendLine(e.Data);
                            }
                        };

                        DataReceivedEventHandler errorHandler = (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                errorWaitHandle.Set();
                            }
                            else
                            {
                                error.AppendLine(e.Data);
                            }
                        };

                        process.OutputDataReceived += outputHandler;
                        process.ErrorDataReceived += errorHandler;

                        try
                        {
                            process.Start();
                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();

                            if (process.WaitForExit(environment.Timeout) && outputWaitHandle.WaitOne(environment.Timeout) && errorWaitHandle.WaitOne(environment.Timeout))
                            {
                                if (process.ExitCode != 0 && !File.Exists(outputPdfFilePath))
                                {
                                    throw new HtmlConvertException($"Html to PDF conversion of '{document.Url}' failed. Wkhtmltopdf output: \r\n{error}");
                                }
                            }
                            else
                            {
                                if (!process.HasExited)
                                {
                                    process.Kill();
                                }

                                throw new HtmlConvertTimeoutException();
                            }
                        }
                        finally
                        {
                            process.OutputDataReceived -= outputHandler;
                            process.ErrorDataReceived -= errorHandler;
                        }
                    }
                }

                if (pdfOutput.OutputStream != null)
                {
                    using (Stream fs = new FileStream(outputPdfFilePath, FileMode.Open))
                    {
                        byte[] buffer = new byte[32 * 1024];
                        int read;

                        while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            pdfOutput.OutputStream.Write(buffer, 0, read);
                        }
                    }
                }

                if (pdfOutput.OutputCallback != null)
                {
                    byte[] pdfFileBytes = File.ReadAllBytes(outputPdfFilePath);
                    pdfOutput.OutputCallback(document, pdfFileBytes);
                }
            }
            finally
            {
                if (delete && File.Exists(outputPdfFilePath))
                    FileHelper.SafeDelete(outputPdfFilePath);
            }
        }

        public class HtmlToPdf
        {
            public static class PaperTypes
            {
                public static string A0 = "A0";
                public static string A1 = "A1";
                public static string A2 = "A2";
                public static string A3 = "A3";
                public static string A4 = "A4";
                public static string A5 = "A5";
                public static string A6 = "A6";
                public static string A7 = "A7";
                public static string A8 = "A8";
                public static string A9 = "A9";
                public static string B0 = "B0";
                public static string B1 = "B1";
                public static string B10 = "B10";
                public static string B2 = "B2";
                public static string B3 = "B3";
                public static string B4 = "B4";
                public static string B5 = "B5";
                public static string B6 = "B6";
                public static string B7 = "B7";
                public static string B8 = "B8";
                public static string B9 = "B9";
                public static string C5E = "C5E";
                public static string Comm10E = "Comm10E";
                public static string Dle = "DLE";
                public static string Executive = "Executive";
                public static string Folio = "Folio";
                public static string Ledger = "Ledger";
                public static string Legal = "Legal";
                public static string Letter = "Letter";
                public static string Tabloid = "Tabloid";
            }

            public class PdfOutput
            {
                /// <summary>
                ///     Store in temp folder if this blank 
                /// </summary>
                public string OutputFilePath { get; set; }

                public Stream OutputStream { get; set; }

                public Action<PdfDocument, byte[]> OutputCallback { get; set; }
            }

            public class PdfDocument
            {
                public string PaperType { get; set; } = PaperTypes.A4;

                /// <summary>
                ///     File path or HTTP URL 
                /// </summary>
                public string Url { get; set; }

                public string HeaderUrl { get; set; }

                public string FooterUrl { get; set; }

                public string HeaderLeft { get; set; }

                public string HeaderCenter { get; set; }

                public string HeaderRight { get; set; }

                public string FooterLeft { get; set; }

                public string FooterCenter { get; set; }

                public string FooterRight { get; set; }

                public object State { get; set; }

                public Dictionary<string, string> Cookies { get; set; }

                public Dictionary<string, string> ExtraParams { get; set; }

                public string HeaderFontSize { get; set; }

                public string FooterFontSize { get; set; }

                public string HeaderFontName { get; set; }

                public string FooterFontName { get; set; }
            }
        }

        #endregion

        #region Docx to Html

        public static byte[] FromDocx(string docxPath)
        {
            // Generate Temp File for Html
            var htmlPath = FileHelper.CreateTempFile(".html");

            try
            {
                // Convert
                FromDocx(docxPath, htmlPath);

                // Result
                var bytes = File.ReadAllBytes(htmlPath);
                return bytes;
            }
            finally
            {
                // Remove Temp
                FileHelper.SafeDelete(htmlPath);
            }
        }

        public static void FromDocx(string docxPath, string htmlPath)
        {
            var environment = new ToolEnvironment
            {
                TempFolderPath = Path.GetTempPath(),
                ToolPath = $"{nameof(HtmlUtils)}/docx2html/Puppy.DocxToHtml.exe".GetFullPath(null),
                Timeout = 60000
            };

            var paramsBuilder = new StringBuilder();

            docxPath = docxPath?.GetFullPath();
            htmlPath = htmlPath?.GetFullPath();

            paramsBuilder.Append($"{docxPath} {htmlPath}");

            try
            {
                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = environment.ToolPath;
                    process.StartInfo.Arguments = paramsBuilder.ToString();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardInput = true;

                    using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                    using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                    {
                        DataReceivedEventHandler outputHandler = (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                outputWaitHandle.Set();
                            }
                            else
                            {
                                output.AppendLine(e.Data);
                            }
                        };

                        DataReceivedEventHandler errorHandler = (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                errorWaitHandle.Set();
                            }
                            else
                            {
                                error.AppendLine(e.Data);
                            }
                        };

                        process.OutputDataReceived += outputHandler;
                        process.ErrorDataReceived += errorHandler;

                        try
                        {
                            process.Start();
                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();

                            if (process.WaitForExit(environment.Timeout) &&
                                outputWaitHandle.WaitOne(environment.Timeout) &&
                                errorWaitHandle.WaitOne(environment.Timeout))
                            {
                                if (process.ExitCode != 0 && !File.Exists(htmlPath))
                                {
                                    throw new HtmlConvertException($"Html to PDF conversion of '{docxPath}' failed. Puppy.DocxToHtml output: \r\n{error}");
                                }
                            }
                            else
                            {
                                if (!process.HasExited)
                                {
                                    process.Kill();
                                }

                                throw new HtmlConvertTimeoutException();
                            }
                        }
                        finally
                        {
                            process.OutputDataReceived -= outputHandler;
                            process.ErrorDataReceived -= errorHandler;
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        #endregion
    }
}