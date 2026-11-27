using System.Collections.Generic;

namespace DnevnikNastroeniya.Models.Api
{
    public class DeepSeekRequest
    {
        public string model { get; set; } = "deepseek-chat";
        public List<ApiMessage> messages { get; set; } = new();
        public float temperature { get; set; } = 0.7f;
    }
}