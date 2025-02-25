using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class GPTClient
{
    private const string ApiUrl = "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions";  // DashScope API 地址
    private const string ApiKey = "sk-acfa03e737f74376bf571351cee9e314";  // 替换为你的 API Key

    private const string SystemPrompt =
        "You are a character generator. Based on description, output exactly a JSON object " +
        "with the keys \"speed\", \"jumpForce\", and \"attackSpeed\". " +
        "Each value should be between 1 and 10. Do not include any extra text. " +
        "Example: Input: \"A swift and agile fighter with balanced power.\" " +
        "Output: { \"speed\": 5, \"jumpForce\": 8, \"attackSpeed\": 6 }";

    public async Task<PlayerStats> GenerateCharacterStats(string description)
    {
        try
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var requestPayload = new
                {
                    model = "qwen-plus",
                    messages = new[]
                    {
                        new { role = "system", content = SystemPrompt },
                        new { role = "user", content = description }
                    },
                    temperature = 0.3,
                    max_tokens = 100
                };

                var jsonRequest = JsonConvert.SerializeObject(requestPayload);

                // 创建 HttpRequestMessage
                var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl)
                {
                    Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
                };

                // 设置 Authorization 头
                request.Headers.Add("Authorization", $"Bearer {ApiKey}");

                // 发送请求
                var response = await httpClient.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                // 检查 API 响应状态
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"API Error: {response.StatusCode} - {responseBody}");
                }

                // 解析 JSON 响应
                var jsonResponse = JsonConvert.DeserializeObject<QwenResponse>(responseBody);
                string generatedJson = jsonResponse.Choices[0].Message.Content;

                // 转换为 PlayerStats 对象
                return ParseCharacterStats(generatedJson);
            }
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Network error: " + ex.Message);
        }
        catch (JsonException ex)
        {
            throw new Exception("JSON parsing error: " + ex.Message);
        }
    }

    private PlayerStats ParseCharacterStats(string json)
    {
        try
        {
            var cleanJson = json.Replace("\n", "").Replace("```json", "").Replace("```", "").Trim();
            var result = JsonConvert.DeserializeObject<PlayerStats>(cleanJson);

            // 校验属性范围
            if (result.Speed < 1 || result.Speed > 10 ||
                result.JumpForce < 1 || result.JumpForce > 10 ||
                result.AttackSpeed < 1 || result.AttackSpeed > 10)
            {
                throw new Exception("Invalid stat values returned by GPT.");
            }

            return result;
        }
        catch
        {
            throw new Exception("Failed to parse character data");
        }
    }
}

// JSON 响应类
internal class QwenResponse
{
    [JsonProperty("choices")] public Choice[] Choices { get; set; }
}

internal class Choice
{
    [JsonProperty("message")] public Message Message { get; set; }
}

internal class Message
{
    [JsonProperty("content")] public string Content { get; set; }
}

// PlayerStats 类
public class PlayerStats
{
    [JsonProperty("speed")] public float Speed { get; set; }
    [JsonProperty("jumpForce")] public float JumpForce { get; set; }
    [JsonProperty("attackSpeed")] public float AttackSpeed { get; set; }
}
