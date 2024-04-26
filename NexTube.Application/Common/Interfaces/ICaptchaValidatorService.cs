namespace NexTube.Application.Common.Interfaces {
    public interface ICaptchaValidatorService {
        Task<bool> IsCaptchaPassedAsync(string token);
    }
}
