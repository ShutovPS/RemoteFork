using Unosquare.Net;

namespace RemoteFork.Requestes {
    internal interface IRequestHandler {
        void Handle(HttpListenerContext context);
    }
}