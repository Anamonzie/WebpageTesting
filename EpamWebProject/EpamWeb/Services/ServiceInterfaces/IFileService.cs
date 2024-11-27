namespace EpamWeb.Services.ServiceInterfaces
{
    public interface IFileService
    {
        void WriteToFile<T>(string filePath, T data);
        T ReadFromFile<T>(string filePath);
    }
}