namespace greyhound.NET.Domain
{
    public class RegisterResponse
    {
        public RegisterResponse(string registrationId)
        {
            RegistrationId = registrationId ?? throw new System.ArgumentNullException(nameof(registrationId));
        }

        public string RegistrationId { get; }
    }
}

