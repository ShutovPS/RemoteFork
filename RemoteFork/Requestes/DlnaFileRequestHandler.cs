using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Common.Logging;
using RemoteFork.Forms;
using RemoteFork.Server;
using Unosquare.Labs.EmbedIO;
using Unosquare.Net;

namespace RemoteFork.Requestes {
    internal class DlnaFileRequestHandler : BaseRequestHandler {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DlnaFileRequestHandler));

        private const int BufferSize = 1024 * 256;

        public override void Handle(HttpListenerContext context) {
            var file = context.Request.QueryString.GetValues(null)?.FirstOrDefault(s => s.StartsWith(Uri.UriSchemeFile));

            if (!string.IsNullOrEmpty(file)) {
                var fileRequest = FileRequest.Create(context, file);

                if (fileRequest.File.Exists && DlnaConfigurate.CheckAccess(fileRequest.File.FullName)) {
                    var requestId = DateTime.Now.Ticks.ToString("x2");
                    Log.Debug(m => m(
                                  "[{0}] Requested: {1}, Rnages: [{2}]-[{3}]/{4}",
                                  requestId,
                                  fileRequest.File.FullName,
                                  string.Join(",", fileRequest.RangesStartIndexes),
                                  string.Join(",", fileRequest.RangesEndIndexes),
                                  fileRequest.File.Length
                              )
                    );

                    if (ValidateRanges(fileRequest, context.Response) && ValidateModificationDate(fileRequest, context) && ValidateEntityTag(fileRequest, context)) {
                        context.Response.Headers.Add(Constants.HeaderLastModified, fileRequest.File.LastWriteTime.ToString("r"));
                        context.Response.Headers.Add(Constants.HeaderETag, fileRequest.EntityTag);
                        context.Response.Headers.Add(Constants.HeaderAcceptRanges, "bytes");

                        if (!fileRequest.RangeRequest) {
                            context.Response.ContentLength64 = fileRequest.File.Length;
                            context.Response.ContentType = fileRequest.ContentType;
                            context.Response.StatusCode = HttpStatusCode.Ok.ToInteger();

                            if (HttpMethod.ANY.FromString(context.Request.HttpMethod) != HttpMethod.HEAD) {
                                using (var fileStream = fileRequest.File.Open(FileMode.Open, FileAccess.Read, FileShare.Read)) {
                                    long sent = Copy(fileStream, context.Response.OutputStream);

                                    Log.Debug(m => m("[{0}] Sent: {1} bytes", requestId, sent));
                                }
                            }
                        } else {
                            context.Response.ContentLength64 = fileRequest.GetContentLength();

                            if (!fileRequest.MultipartRequest) {
                                context.Response.Headers.Add(Constants.HeaderContentRanges, $"bytes {fileRequest.RangesStartIndexes[0]}-{fileRequest.RangesEndIndexes[0]}/{fileRequest.File.Length}");
                                context.Response.ContentType = fileRequest.ContentType;
                            } else {
                                context.Response.ContentType = $"multipart/byteranges; boundary={fileRequest.Boundary}";
                            }

                            context.Response.StatusCode = HttpStatusCode.PartialContent.ToInteger();
                            if (HttpMethod.ANY.FromString(context.Request.HttpMethod) != HttpMethod.HEAD) {
                                using (var fileStream = fileRequest.File.Open(FileMode.Open, FileAccess.Read, FileShare.Read)) {
                                    for (var i = 0; i < fileRequest.RangesStartIndexes.Length; i++) {
                                        if (fileRequest.MultipartRequest) {
                                            Copy($"--{fileRequest.Boundary}\r\n", context.Response.OutputStream);
                                            Copy($"Content-Type: {fileRequest.ContentType}\r\n", context.Response.OutputStream);
                                            Copy($"Content-Range: bytes {fileRequest.RangesStartIndexes[i]}-{fileRequest.RangesEndIndexes[i]}/{fileRequest.File.Length}\r\n\r\n", context.Response.OutputStream);
                                        }

                                        long sent = Copy(
                                            fileStream,
                                            context.Response.OutputStream,
                                            fileRequest.RangesStartIndexes[i],
                                            fileRequest.RangesEndIndexes[i] - fileRequest.RangesStartIndexes[i] + 1
                                        );

                                        Log.Debug(m => m("[{0}] Sent: {1} bytes", requestId, sent));

                                        if (fileRequest.MultipartRequest) {
                                            Copy("\r\n", context.Response.OutputStream);
                                        }
                                    }

                                    if (fileRequest.MultipartRequest) {
                                        Copy($"--{fileRequest.Boundary}--", context.Response.OutputStream);
                                    }
                                }
                            }
                        }
                    }
                } else {
                    Log.Debug(m => m("File not found: {0}", file));

                    WriteResponse(context.Response, HttpStatusCode.NotFound, $"File not found: {file}");
                }
            } else {
                Log.Debug(m => m("Incorrect parameter: {0}", file));

                WriteResponse(context.Response, HttpStatusCode.NotFound, $"Incorrect parameter: {file}");
            }
        }

        private static long Copy(Stream input, Stream output) {
            long totalRead = 0;
            var buffer = new byte[BufferSize];

            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, BufferSize)) != 0) {
                output.Write(buffer, 0, bytesRead);
                totalRead += bytesRead;
            }

            return totalRead;
        }

        private static long Copy(Stream input, Stream output, long inputOffset, long length) {
            long totalRead = 0;

            if (length > 0) {
                var buffer = new byte[BufferSize];

                if (inputOffset > 0) {
                    input.Seek(inputOffset, SeekOrigin.Begin);
                }

                var bufferLength = buffer.Length;
                var bytesToRead = bufferLength;

                if (length < bufferLength) {
                    bytesToRead = Convert.ToInt32(length);
                }

                int read;
                while (bytesToRead > 0 && 0 != (read = input.Read(buffer, 0, bytesToRead))) {
                    output.Write(buffer, 0, read);

                    totalRead += read;

                    bytesToRead = Convert.ToInt32(Math.Min(length - totalRead, bufferLength));
                }
            }

            return totalRead;
        }

        private static void Copy(string input, Stream output) {
            if (input != null) {
                byte[] buffer = Encoding.UTF8.GetBytes(input);
                output.Write(buffer, 0, buffer.Length);
            }
        }

        private static bool ValidateRanges(FileRequest fileRequest, HttpListenerResponse response) {
            var fileLength = fileRequest.File.Length;

            if (fileLength > int.MaxValue) {
                response.StatusCode = HttpStatusCode.PayloadTooLarge.ToInteger();
                return false;
            }

            for (var i = 0; i < fileRequest.RangesStartIndexes.Length; i++) {
                if ((fileRequest.RangesStartIndexes[i] > fileLength - 1)
                    || (fileRequest.RangesEndIndexes[i] > fileLength - 1)
                    || (fileRequest.RangesStartIndexes[i] < 0)
                    || (fileRequest.RangesEndIndexes[i] < 0)
                    || (fileRequest.RangesEndIndexes[i] < fileRequest.RangesStartIndexes[i])) {
                    response.StatusCode = HttpStatusCode.NotAcceptable.ToInteger();
                    response.AddHeader(Constants.HeaderContentRanges, $"bytes */{fileLength}");

                    return false;
                }
            }

            return true;
        }

        private bool ValidateModificationDate(FileRequest fileRequest, HttpListenerContext context) {
            var modifiedSinceHeader = context.RequestHeader(Constants.HeaderIfModifiedSince);
            if (!string.IsNullOrEmpty(modifiedSinceHeader)) {
                DateTime modifiedSinceDate = FileRequest.ParseHttpDateHeader(context, Constants.HeaderIfModifiedSince);

                if (fileRequest.File.LastWriteTime <= modifiedSinceDate) {
                    context.Response.StatusCode = HttpStatusCode.NotModified.ToInteger();
                    return false;
                }
            }

            var unmodifiedSinceHeader = context.RequestHeader("If-Unmodified-Since");

            if (string.IsNullOrEmpty(unmodifiedSinceHeader)) {
                unmodifiedSinceHeader = context.RequestHeader("Unless-Modified-Since");
            }

            if (!string.IsNullOrEmpty(unmodifiedSinceHeader)) {
                DateTime unmodifiedSinceDate = FileRequest.ParseHttpDateHeader(context, Constants.HeaderIfModifiedSince);

                if (fileRequest.File.LastWriteTime > unmodifiedSinceDate) {
                    context.Response.StatusCode = HttpStatusCode.PreconditionFailed.ToInteger();
                    return false;
                }
            }

            return true;
        }

        private bool ValidateEntityTag(FileRequest fileRequest, HttpListenerContext context) {
            var matchHeader = context.RequestHeader("If-Match");
            if (!string.IsNullOrEmpty(matchHeader) && (matchHeader != "*")) {
                var entitiesTags = matchHeader.Split(FileRequest.CommaSplitArray);
                var entitieTagIndex = 0;

                for (; entitieTagIndex < entitiesTags.Length; entitieTagIndex++) {
                    if (fileRequest.EntityTag == entitiesTags[entitieTagIndex]) {
                        break;
                    }
                }

                if (entitieTagIndex >= entitiesTags.Length) {
                    context.Response.StatusCode = HttpStatusCode.PreconditionFailed.ToInteger();
                    return false;
                }
            }

            var noneMatchHeader = context.RequestHeader(Constants.HeaderIfNotMatch);
            if (!string.IsNullOrEmpty(noneMatchHeader)) {
                if (noneMatchHeader == "*") {
                    context.Response.StatusCode = HttpStatusCode.PreconditionFailed.ToInteger();
                    return false;
                }

                var entitiesTags = noneMatchHeader.Split(FileRequest.CommaSplitArray);
                foreach (var entityTag in entitiesTags) {
                    if (fileRequest.EntityTag == entityTag) {
                        context.Response.Headers.Add(Constants.HeaderETag, fileRequest.EntityTag);
                        context.Response.StatusCode = HttpStatusCode.NotModified.ToInteger();
                        return false;
                    }
                }
            }

            return true;
        }
    }

    sealed class FileRequest {
        private static readonly MD5CryptoServiceProvider Md5 = new MD5CryptoServiceProvider();

        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>(Constants.StandardStringComparer);

        private const string HeaderIfRange = "If-Range";

        private static readonly string[] HttpDateFormats = {"r", "dddd, dd-MMM-yy HH':'mm':'ss 'GMT'", "ddd MMM d HH':'mm':'ss yyyy"};

        internal static readonly char[] CommaSplitArray = {','};

        private static readonly char[] DashSplitArray = {'-'};

        public string Boundary { get; } = "---------------------------" + DateTime.Now.Ticks.ToString("x");

        private readonly HttpListenerContext _context;

        static FileRequest() {
            foreach (var mimeType in Constants.DefaultMimeTypes) {
                MimeTypes[mimeType.Key] = mimeType.Value;
            }

            foreach (var mimeType in Tools.MimeTypes) {
                MimeTypes[mimeType.Key] = mimeType.Value;
            }
        }

        private FileRequest(HttpListenerContext context, string file) {
            _context = context;
            File = new FileInfo(new Uri(file).LocalPath);
        }

        public FileInfo File { get; }

        public bool RangeRequest { get; private set; }

        public bool MultipartRequest { get; private set; }

        public string EntityTag { get; private set; }

        public string ContentType { get; private set; }

        public long[] RangesStartIndexes { get; private set; }
        public long[] RangesEndIndexes { get; private set; }

        private void Parse() {
            EntityTag = GenerateEntityTag();
            ContentType = MimeTypes.ContainsKey(File.Extension) ? MimeTypes[File.Extension] : "application/octet-stream";
            ParseRanges();
        }

        private void ParseRanges() {
            var rangesHeader = _context.RequestHeader(Constants.HeaderRange);
            var ifRangeHeader = _context.RequestHeader(HeaderIfRange);
            var ifRangeHeaderDate = ParseHttpDateHeader(_context, HeaderIfRange);

            if (string.IsNullOrEmpty(rangesHeader)
                || (_context.HasRequestHeader(HeaderIfRange)
                    && (ifRangeHeader != EntityTag
                        || (ifRangeHeaderDate != DateTime.MinValue && File.LastWriteTime > ifRangeHeaderDate)))) {
                RangesStartIndexes = new[] {0L};
                RangesEndIndexes = new[] {File.Length - 1};
                RangeRequest = false;
                MultipartRequest = false;
            } else {
                var ranges = rangesHeader.Replace("bytes=", string.Empty).Split(CommaSplitArray);

                RangesStartIndexes = new long[ranges.Length];
                RangesEndIndexes = new long[ranges.Length];
                RangeRequest = true;
                MultipartRequest = ranges.Length > 1;

                for (var i = 0; i < ranges.Length; i++) {
                    var currentRange = ranges[i].Split(DashSplitArray);

                    if (string.IsNullOrEmpty(currentRange[1])) {
                        RangesEndIndexes[i] = File.Length - 1;
                    } else {
                        RangesEndIndexes[i] = long.Parse(currentRange[1]);
                    }

                    if (string.IsNullOrEmpty(currentRange[0])) {
                        RangesStartIndexes[i] = File.Length - 1 - RangesEndIndexes[i];
                        RangesEndIndexes[i] = File.Length - 1;
                    } else {
                        RangesStartIndexes[i] = long.Parse(currentRange[0]);
                    }
                }
            }
        }

        public long GetContentLength() {
            var contentLength = 0L;

            for (var i = 0; i < RangesStartIndexes.Length; i++) {
                contentLength += Convert.ToInt32(RangesEndIndexes[i] - RangesStartIndexes[i]) + 1;

                if (MultipartRequest) {
                    contentLength +=
                        Boundary.Length
                        + ContentType.Length
                        + RangesStartIndexes[i].ToString().Length
                        + RangesEndIndexes[i].ToString().Length
                        + File.Length.ToString().Length
                        + 49;
                }
            }

            if (MultipartRequest) {
                contentLength += Boundary.Length + 4;
            }

            return contentLength;
        }

        internal static DateTime ParseHttpDateHeader(HttpListenerContext context, string headerName) {
            var ifRangeHeader = context.RequestHeader(headerName);

            DateTime ifRangeHeaderDate;

            if (DateTime.TryParseExact(ifRangeHeader, HttpDateFormats, null, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out ifRangeHeaderDate)) {
                return ifRangeHeaderDate;
            } else {
                return DateTime.MinValue;
            }
        }

        private string GenerateEntityTag() {
            return Convert.ToBase64String(Md5.ComputeHash(Encoding.ASCII.GetBytes($"{File.FullName}|{File.LastWriteTime}")));
        }

        public static FileRequest Create(HttpListenerContext context, string file) {
            var fr = new FileRequest(context, file);

            fr.Parse();

            return fr;
        }
    }
}