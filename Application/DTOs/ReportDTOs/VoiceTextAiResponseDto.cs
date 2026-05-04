using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization; 


namespace Application.DTOs.ReportDTOs
{

    public class VoiceTextAiResponseDto
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("priority")]
        public string Priority { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("final_text")]
        public string Final_Text { get; set; }
    }
    public class VoiceTextAiWrapperDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("result")]
        public VoiceTextAiResponseDto Result { get; set; } // هنا الداتا الحقيقية
    }
}
