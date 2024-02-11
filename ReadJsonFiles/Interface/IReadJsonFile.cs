namespace ReadJsonFiles.Interface
{
    public interface IReadJsonFile
    {
        Task<string> ReadJsonFileAsync();
        Task<string> ReadLEIJsonFileAsync();
        Task<string> ReadEcpJsonFileAsync();
        Task<string> CreateRRRJsonFile();
    }
}
