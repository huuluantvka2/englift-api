using EngLift.DTO.Dictionary;
using EngLift.Service.Interfaces;
using Newtonsoft.Json;

namespace EngLift.Service.Implements
{
    public class DictionaryService : IDictionaryService
    {
        private const string BaseUrl = "https://api.dictionaryapi.dev/api/v2/entries/en";
        public DictionaryService() { }

        public async Task<string?> GetLinkAudio(string word)
        {
            List<DictionaryItemDTO> dictionarys = new List<DictionaryItemDTO>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseUrl}/{word}");
                string apiResponse = await response.Content.ReadAsStringAsync();
                dictionarys = JsonConvert.DeserializeObject<List<DictionaryItemDTO>>(apiResponse);
                foreach (var dictionary in dictionarys)
                {
                    foreach (var phonetic in dictionary.phonetics)
                    {
                        if (!string.IsNullOrEmpty(phonetic.audio)) return phonetic.audio;
                    }
                }
            }
            return null;
        }
    }
}
