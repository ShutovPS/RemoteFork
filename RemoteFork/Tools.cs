using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace RemoteFork {
    internal static class Tools {

        public static IPAddress[] GetIPAddresses(string hostname = "") {
            var hostEntry = Dns.GetHostEntry(hostname);
            var addressList = hostEntry.AddressList;
            return (from iPAddress in addressList
                    let flag = iPAddress.AddressFamily == AddressFamily.InterNetwork
                    where flag
                    select iPAddress)
                .ToArray();
        }

        public static string FSize(long len) {
            float num = len;
            var str = "Байт";
            var flag = num > 102f;
            if (flag) {
                num /= 1024f;
                str = "КБ";
            }
            var flag2 = num > 102f;
            if (flag2) {
                num /= 1024f;
                str = "МБ";
            }
            var flag3 = num > 102f;
            if (flag3) {
                num /= 1024f;
                str = "ГБ";
            }
            return Math.Round(num, 2) + str;
        }

        #region File extensions

        public static readonly IDictionary<string, string> MimeTypes =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
                #region Big freaking list of mime types
                {".323", "text/h323"},
                {".3g2", "video/3gpp2"},
                {".3gp", "video/3gpp"},
                {".3gp2", "video/3gpp2"},
                {".3gpp", "video/3gpp"},
                {".aa", "audio/audible"},
                {".AAC", "audio/aac"},
                {".aax", "audio/vnd.audible.aax"},
                {".ac3", "audio/ac3"},
                {".AddIn", "text/xml"},
                {".ADT", "audio/vnd.dlna.adts"},
                {".ADTS", "audio/aac"},
                {".aif", "audio/x-aiff"},
                {".aifc", "audio/aiff"},
                {".aiff", "audio/aiff"},
                {".art", "image/x-jg"},
                {".asf", "video/x-ms-asf"},
                {".asm", "text/plain"},
                {".asr", "video/x-ms-asf"},
                {".asx", "video/x-ms-asf"},
                {".au", "audio/basic"},
                {".avi", "video/x-msvideo"},
                {".bas", "text/plain"},
                {".bmp", "image/bmp"},
                {".c", "text/plain"},
                {".caf", "audio/x-caf"},
                {".cc", "text/plain"},
                {".cd", "text/plain"},
                {".cdda", "audio/aiff"},
                {".cmx", "image/x-cmx"},
                {".cnf", "text/plain"},
                {".cod", "image/cis-cod"},
                {".contact", "text/x-ms-contact"},
                {".cpp", "text/plain"},
                {".cs", "text/plain"},
                {".csdproj", "text/plain"},
                {".csproj", "text/plain"},
                {".css", "text/css"},
                {".csv", "text/csv"},
                {".cxx", "text/plain"},
                {".dbproj", "text/plain"},
                {".def", "text/plain"},
                {".dib", "image/bmp"},
                {".dif", "video/x-dv"},
                {".disco", "text/xml"},
                {".dll.config", "text/xml"},
                {".dlm", "text/dlm"},
                {".dsw", "text/plain"},
                {".dtd", "text/xml"},
                {".dtsConfig", "text/xml"},
                {".dv", "video/x-dv"},
                {".etx", "text/x-setext"},
                {".exe.config", "text/xml"},
                {".flv", "video/x-flv"},
                {".gif", "image/gif"},
                {".group", "text/x-ms-group"},
                {".gsm", "audio/x-gsm"},
                {".h", "text/plain"},
                {".hdml", "text/x-hdml"},
                {".hpp", "text/plain"},
                {".htc", "text/x-component"},
                {".htm", "text/html"},
                {".html", "text/html"},
                {".htt", "text/webviewhtml"},
                {".hxt", "text/html"},
                {".hxx", "text/plain"},
                {".i", "text/plain"},
                {".ico", "image/x-icon"},
                {".idl", "text/plain"},
                {".ief", "image/ief"},
                {".inc", "text/plain"},
                {".inl", "text/plain"},
                {".ipproj", "text/plain"},
                {".iqy", "text/x-ms-iqy"},
                {".IVF", "video/x-ivf"},
                {".jfif", "image/pjpeg"},
                {".jpe", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".jpg", "image/jpeg"},
                {".jsx", "text/jscript"},
                {".jsxbin", "text/plain"},
                {".lsf", "video/x-la-asf"},
                {".lst", "text/plain"},
                {".lsx", "video/x-la-asf"},
                {".m1v", "video/mpeg"},
                {".m2t", "video/vnd.dlna.mpeg-tts"},
                {".m2ts", "video/vnd.dlna.mpeg-tts"},
                {".m2v", "video/mpeg"},
                {".m3u", "audio/x-mpegurl"},
                {".m3u8", "audio/x-mpegurl"},
                {".m4a", "audio/m4a"},
                {".m4b", "audio/m4b"},
                {".m4p", "audio/m4p"},
                {".m4r", "audio/x-m4r"},
                {".m4v", "video/x-m4v"},
                {".mac", "image/x-macpaint"},
                {".mak", "text/plain"},
                {".map", "text/plain"},
                {".mid", "audio/mid"},
                {".midi", "audio/mid"},
                {".mk", "text/plain"},
                {".mkv", "video/x-matroska"},
                {".mno", "text/xml"},
                {".mod", "video/mpeg"},
                {".mov", "video/quicktime"},
                {".movie", "video/x-sgi-movie"},
                {".mp2", "video/mpeg"},
                {".mp2v", "video/mpeg"},
                {".mp3", "audio/mpeg"},
                {".mp4", "video/mp4"},
                {".mp4v", "video/mp4"},
                {".mpa", "video/mpeg"},
                {".mpe", "video/mpeg"},
                {".mpeg", "video/mpeg"},
                {".mpg", "video/mpeg"},
                {".mpv2", "video/mpeg"},
                {".mqv", "video/quicktime"},
                {".nsc", "video/x-ms-asf"},
                {".odc", "text/x-ms-odc"},
                {".odh", "text/plain"},
                {".odl", "text/plain"},
                {".pbm", "image/x-portable-bitmap"},
                {".pct", "image/pict"},
                {".pgm", "image/x-portable-graymap"},
                {".pic", "image/pict"},
                {".pict", "image/pict"},
                {".pkgdef", "text/plain"},
                {".pkgundef", "text/plain"},
                {".pls", "audio/scpls"},
                {".png", "image/png"},
                {".pnm", "image/x-portable-anymap"},
                {".pnt", "image/x-macpaint"},
                {".pntg", "image/x-macpaint"},
                {".pnz", "image/png"},
                {".ppm", "image/x-portable-pixmap"},
                {".qht", "text/x-html-insertion"},
                {".qhtm", "text/x-html-insertion"},
                {".qt", "video/quicktime"},
                {".qti", "image/x-quicktime"},
                {".qtif", "image/x-quicktime"},
                {".ra", "audio/x-pn-realaudio"},
                {".ram", "audio/x-pn-realaudio"},
                {".ras", "image/x-cmu-raster"},
                {".rc", "text/plain"},
                {".rc2", "text/plain"},
                {".rct", "text/plain"},
                {".rf", "image/vnd.rn-realflash"},
                {".rgb", "image/x-rgb"},
                {".rgs", "text/plain"},
                {".rmi", "audio/mid"},
                {".rpm", "audio/x-pn-realaudio-plugin"},
                {".rqy", "text/x-ms-rqy"},
                {".rtx", "text/richtext"},
                {".s", "text/plain"},
                {".sct", "text/scriptlet"},
                {".sd2", "audio/x-sd2"},
                {".sgml", "text/sgml"},
                {".shtml", "text/html"},
                {".sln", "text/plain"},
                {".smd", "audio/x-smd"},
                {".smx", "audio/x-smd"},
                {".smz", "audio/x-smd"},
                {".snd", "audio/basic"},
                {".sol", "text/plain"},
                {".sor", "text/plain"},
                {".srf", "text/plain"},
                {".SSISDeploymentManifest", "text/xml"},
                {".tif", "image/tiff"},
                {".tiff", "image/tiff"},
                {".tlh", "text/plain"},
                {".tli", "text/plain"},
                {".ts", "video/vnd.dlna.mpeg-tts"},
                {".tsv", "text/tab-separated-values"},
                {".tts", "video/vnd.dlna.mpeg-tts"},
                {".txt", "text/plain"},
                {".uls", "text/iuls"},
                {".user", "text/plain"},
                {".vb", "text/plain"},
                {".vbdproj", "text/plain"},
                {".vbk", "video/mpeg"},
                {".vbproj", "text/plain"},
                {".vbs", "text/vbscript"},
                {".vcf", "text/x-vcard"},
                {".vcs", "text/plain"},
                {".vddproj", "text/plain"},
                {".vdp", "text/plain"},
                {".vdproj", "text/plain"},
                {".vml", "text/xml"},
                {".vsct", "text/xml"},
                {".vsixlangpack", "text/xml"},
                {".vsixmanifest", "text/xml"},
                {".vspscc", "text/plain"},
                {".vsscc", "text/plain"},
                {".vssettings", "text/xml"},
                {".vssscc", "text/plain"},
                {".vstemplate", "text/xml"},
                {".wav", "audio/wav"},
                {".wave", "audio/wav"},
                {".wax", "audio/x-ms-wax"},
                {".wbmp", "image/vnd.wap.wbmp"},
                {".wdp", "image/vnd.ms-photo"},
                {".wm", "video/x-ms-wm"},
                {".wma", "audio/x-ms-wma"},
                {".wml", "text/vnd.wap.wml"},
                {".wmls", "text/vnd.wap.wmlscript"},
                {".wmp", "video/x-ms-wmp"},
                {".wmv", "video/x-ms-wmv"},
                {".wmx", "video/x-ms-wmx"},
                {".wsc", "text/scriptlet"},
                {".wsdl", "text/xml"},
                {".wvx", "video/x-ms-wvx"},
                {".xbm", "image/x-xbitmap"},
                {".xdr", "text/plain"},
                {".xml", "text/xml"},
                {".XOML", "text/plain"},
                {".xpm", "image/x-xpixmap"},
                {".xrm-ms", "text/xml"},
                {".xsd", "text/xml"},
                {".xsf", "text/xml"},
                {".xsl", "text/xml"},
                {".xslt", "text/xml"},
                {".xwd", "image/x-xwindowdump"}

                #endregion
            };

        public static string GetMimeType(string fileName) {
            var extension = MimeTypes.FirstOrDefault(i => fileName.EndsWith(i.Key));
            return extension.Value;
        }

        #endregion File extensions
    }
}