using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using ClosedXML.Graphics;
using EngLift.Common;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.DTO.Response;
using EngLift.DTO.Word;
using EngLift.Service.Extensions;
using EngLift.Service.Interfaces;
using EngLift.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
internal class NoGraphicsEngine : IXLGraphicEngine
{
    private NoGraphicsEngine() { }
    public static NoGraphicsEngine Instance { get; } = new();
    public double GetDescent(IXLFontBase font, double dpiY) => default;
    public GlyphBox GetGlyphBox(ReadOnlySpan<int> graphemeCluster, IXLFontBase font, Dpi dpi) => default;
    public double GetMaxDigitWidth(IXLFontBase font, double dpiX) => default;
    public XLPictureInfo GetPictureInfo(Stream imageStream, XLPictureFormat expectedFormat) => default;
    public double GetTextHeight(IXLFontBase font, double dpiY) => default;
    public double GetTextWidth(string text, IXLFontBase font, double dpiX) => default;
}

namespace EngLift.WebAPI.Controllers.Admin
{
    [Authorize]
    [ApiController]
    [Route("Api/v1/[controller]s/Admin")]
    public class WordController : ControllerApiBase<WordController>
    {
        private readonly IWordService _wordService;
        private IGooglePublisher _publisherService;
        public WordController(
            ILogger<WordController> logger,
            IWordService wordService,
            IGooglePublisherFactory factory
            ) : base(logger)
        {
            _wordService = wordService;
            _publisherService = factory.Init(GoogleConstant.PubSubTopicImport);
        }

        [HttpGet("Lesson")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CAN_VIEW_WORD)]
        public async Task<IActionResult> GetAllWordByLessonId([FromQuery] WordRequest request)
        {
            try
            {
                var result = await _wordService.GetAllWordByLessonId(request);
                return Success<DataList<WordItemDTO>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> GetAllWordByLessonId Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpGet("{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CAN_VIEW_WORD)]
        public async Task<IActionResult> GetWordDetail(Guid Id)
        {
            try
            {
                var result = await _wordService.GetWordDetail(Id);
                return Success<WordItemDTO>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> GetWordDetail Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpPost("")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CREATE_VIEW_WORD)]
        public async Task<IActionResult> CreateWord([FromBody] WordCreateDTO dto)
        {
            try
            {
                var result = await _wordService.CreateWord(dto);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> CreateWord Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpPut("{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_EDIT_VIEW_WORD)]
        public async Task<IActionResult> UpdateWord(Guid Id, [FromBody] WordUpdateDTO dto)
        {
            try
            {
                var result = await _wordService.UpdateWord(Id, dto);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> UpdateWord Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpDelete("{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_DELETE_VIEW_WORD)]
        public async Task<IActionResult> DeleteWord([FromQuery] Guid LessonId, Guid Id)
        {
            try
            {
                var result = await _wordService.DeleteWord(Id, LessonId);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> DeleteWord Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpPost("Import")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CREATE_VIEW_WORD)]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File not exist");
                }
                List<WordCreateExcelDTO> Words = new List<WordCreateExcelDTO>();
                var loadOptions = new LoadOptions
                {
                    GraphicEngine = NoGraphicsEngine.Instance,
                };
                using (var workbook = new XLWorkbook(file.OpenReadStream(), loadOptions))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);
                    /*                    if (worksheet.RowsUsed().Count() > 200)
                                        {
                                            throw new ServiceExeption(HttpStatusCode.BadRequest, "Limit import are 200 record");
                                        }*/
                    var count = worksheet.RowsUsed().Count();
                    for (int i = 2; i <= worksheet.RowsUsed().Count(); i++)
                    {
                        var word = new WordCreateExcelDTO();
                        var Content = worksheet.Cell(i, 1).Value.ToString();
                        word.Content = Content;
                        if (String.IsNullOrEmpty(Content)) word.MessageError += "Thiếu từ vựng, ";

                        var Trans = worksheet.Cell(i, 2).Value.ToString();
                        word.Trans = Trans;
                        if (String.IsNullOrEmpty(Trans)) word.MessageError += "Thiếu dịch nghĩa, ";

                        var Example = worksheet.Cell(i, 3).Value.ToString();
                        word.Example = Example;
                        if (String.IsNullOrEmpty(Example)) word.MessageError += "Thiếu ví dụ,";

                        var Position = worksheet.Cell(i, 4).Value.ToString();
                        word.Position = Position;

                        var Phonetic = worksheet.Cell(i, 5).Value.ToString();
                        word.Phonetic = Phonetic;

                        var China = worksheet.Cell(i, 6).Value.ToString();
                        word.China = China;

                        var LessonId = worksheet.Cell(i, 7).Value.ToString();
                        word.LessonId = LessonId;
                        if (String.IsNullOrEmpty(LessonId)) word.MessageError += "Thiếu LessonId";

                        Words.Add(word);

                        worksheet.Cell(i, 8).Value = string.IsNullOrEmpty(word.MessageError) ? "Thành công" : word.MessageError;
                        if (worksheet.Cell(i, 8).Value.ToString() != "Thành công") worksheet.Cell(i, 7).Style.Fill.BackgroundColor = XLColor.Red;
                    }
                    var list = Words.Where(x => String.IsNullOrEmpty(x.MessageError)).ToList();
                    await _wordService.CheckExistLesson(list);
                    await _publisherService.SendMessageAsync<List<WordCreateExcelDTO>>(list);

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "WordsImportResult.xlsx");
                    }
                }
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> UploadExcel Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        /// <summary>
        /// Delete file
        /// </summary>
        [HttpGet("TempleteImport")]
        public IActionResult DownLoadImportWords()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "excel", "sampleimport.xlsx");
            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest("Not found Excel");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "WordSample.xlsx");
        }
    }
}
