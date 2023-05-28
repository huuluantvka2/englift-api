namespace EngLift.Service.Interfaces
{
    public interface IDictionaryService
    {
        Task<string?> GetLinkAudio(string word);
    }
}
