using Project.Scripts.DataBase;

namespace Project.Scripts.Services
{
    public interface IDataBaseService : IService
    {
        SpreadsheetContent Content { get; }
    }
}