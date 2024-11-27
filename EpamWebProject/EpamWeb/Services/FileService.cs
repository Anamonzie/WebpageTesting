using EpamWeb.Services.ServiceInterfaces;
using System.Text.Json;

namespace EpamWeb.Services
{
    public class FileService : IFileService
    {
        public void WriteToFile<T>(string filePath, T data)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }

        public T ReadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var jsonData = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonData)
                ?? throw new InvalidOperationException($"Failed to deserialize data from {filePath}");
        }
    }
}
