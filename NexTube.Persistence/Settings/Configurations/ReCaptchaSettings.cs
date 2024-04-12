namespace NexTube.Persistence.Settings.Configurations {
    public class ReCaptchaSettings {
        // link to captcha service api
        public string RemoteAddress { get; set; } = String.Empty;

        // an amount of score, that assigned to each user action
        public double AcceptableScore { get; set; }

        // credentials for using third version of protocol
        public string ApplicationKey_V3 { get; set; } = String.Empty;
        public string PrivateKey_V3 { get; set; } = String.Empty;

        // credentials for using second version of protocol
        public string ApplicationKey_V2 { get; set; } = String.Empty;
        public string PrivateKey_V2 { get; set; } = String.Empty;
    }
}
