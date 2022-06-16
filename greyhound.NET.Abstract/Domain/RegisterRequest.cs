namespace greyhound.NET.Domain
{
    public class RegisterRequest
    {
        public RegisterRequest(string host, string port)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new System.ArgumentException($"'{nameof(host)}' cannot be null or empty.", nameof(host));
            }

            if (string.IsNullOrEmpty(port))
            {
                throw new System.ArgumentException($"'{nameof(port)}' cannot be null or empty.", nameof(port));
            }

            Host = host;
            Port = port;
        }

        public string Host { get; }
        public string Port { get; }
    }
}



