using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Aria2Manager
{
    public class WindowBoundsSettings
    {
        private readonly string _filePath = "settings.json";
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        private readonly string _propertyName = nameof(WindowBounds);

        public WindowBounds GetWindowBounds()
        {
            if (!File.Exists(_filePath))
                return GetDefaultWindowBounds();

            try
            {
                string jsonString = File.ReadAllText(_filePath);
                using JsonDocument doc = JsonDocument.Parse(jsonString);
                if (doc.RootElement.TryGetProperty(_propertyName, out JsonElement boundsElement))
                {
                    return boundsElement.Deserialize<WindowBounds>() ?? GetDefaultWindowBounds();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"读取文件时发生错误: {ex.Message}");
            }

            return GetDefaultWindowBounds();
        }

        public void SaveWindowBounds(WindowBounds bounds)
        {
            try
            {
                string jsonString;
                if (File.Exists(_filePath))
                {
                    // 如果文件存在，读取现有内容并更新 windowBounds 属性
                    string existingJson = File.ReadAllText(_filePath);
                    JsonNode? rootNode = null;
                    try
                    {
                        rootNode = JsonNode.Parse(existingJson);
                    }
                    catch (JsonException)
                    {
                        // 解析失败，保持 null
                    }
                    rootNode ??= JsonNode.Parse("{}");

                    JsonNode boundsNode = JsonSerializer.SerializeToNode(bounds);
                    rootNode[_propertyName] = boundsNode;

                    jsonString = rootNode.ToJsonString(_options);
                }
                else
                {
                    // 如果文件不存在，创建新的 JSON 内容
                    var newJson = new Dictionary<string, object>
                    {
                        [_propertyName] = bounds
                    };
                    jsonString = JsonSerializer.Serialize(newJson, _options);
                }

                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"保存文件时发生错误: {ex.Message}");
            }
        }

        private static WindowBounds GetDefaultWindowBounds()
        {
            return new()
            {
                Width = 800,
                Height = 500,
                Left = 100,
                Top = 100
            };
        }
    }
}
