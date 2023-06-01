namespace EngLift.DTO.Dictionary
{
    public class DictionaryItemDTO
    {
        public List<PhoneticsItemDTO> phonetics { get; set; }

    }
    public class PhoneticsItemDTO
    {
        public string? audio { get; set; }
        public string? text { get; set; }
    }
}
