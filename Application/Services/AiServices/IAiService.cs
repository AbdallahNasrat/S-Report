using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AiServices
{
    public interface IAIService
    {

        Task AnalyzeByImageAsync(int reportId, List<string> imageUrls);


        Task AnalyzeByTextAndVoiceAsync(int reportId, string description, string? voiceUrl);
    }
}
