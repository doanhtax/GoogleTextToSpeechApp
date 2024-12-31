using Google.Cloud.TextToSpeech.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace GoogleTextToSpeechApp.Pages.Home
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string InputText { get; set; } = string.Empty; // Gán giá trị mặc định là chuỗi rỗng

        [BindProperty]
        public string Language { get; set; } = "en-US"; // Default to English (United States)

        [BindProperty]
        public string Gender { get; set; } = "NEUTRAL"; // Default to Neutral voice

        [BindProperty]
        public float Speed { get; set; } = 1.0f; // Default to normal speed

        public string AudioFileName { get; set; } = string.Empty; // Gán giá trị mặc định là chuỗi rỗng

        public IActionResult OnPostConvertText()
        {
          

            // Google Text-to-Speech Client
            var client = TextToSpeechClient.Create();

            // Build the SynthesisInput object
            var input = new SynthesisInput
            {
                Text = InputText
            };
            // Voice configuration
            var voiceSelection = new VoiceSelectionParams
            {
                LanguageCode = Language, // Ngôn ngữ từ form
                Name = $"{Language}-Wavenet-A", // Giọng Wavenet-A mặc định
                SsmlGender = Gender.ToUpper() switch
                {
                    "FEMALE" => SsmlVoiceGender.Female,
                    "MALE" => SsmlVoiceGender.Male,
                    _ => SsmlVoiceGender.Neutral
                }
            };

            // Set AudioConfig parameters
            var audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3,
                SpeakingRate = Speed
            };

            // Generate audio
            var response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

            //    Save audio to wwwroot/audio
            AudioFileName = $"output-{Guid.NewGuid()}.mp3";
            var audioFilePath = Path.Combine("wwwroot/audio", AudioFileName);
            System.IO.File.WriteAllBytes(audioFilePath, response.AudioContent.ToByteArray());

            return Page();
        }

        public IActionResult OnGetDownloadFile(string fileName)
        {
            // if (string.IsNullOrEmpty(fileName))
            // {
            //     return BadRequest("File name is required.");
            // }

            var filePath = Path.Combine("wwwroot/audio", fileName);

            // if (!System.IO.File.Exists(filePath))
            // {
            //     return NotFound("File not found.");
            // }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "audio/mpeg", fileName);
        }

    }
}
