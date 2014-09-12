using System.Collections.ObjectModel;

namespace AssemblyInstaller.Model
{
    public interface IDataService
    {
        ObservableCollection<AssemblyEntity> GetChampionData();
        ObservableCollection<AssemblyEntity> GetUtilityData();
        ObservableCollection<AssemblyEntity> GetLibraryData();
        ObservableCollection<LogEntity> GetLogData();
        bool IsInitComplete();
    }
}
