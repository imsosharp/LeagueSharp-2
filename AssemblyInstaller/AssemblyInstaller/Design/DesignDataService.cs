using System.Collections.ObjectModel;
using AssemblyInstaller.Model;

namespace AssemblyInstaller.Design
{
    public class DesignDataService : IDataService
    {
        public ObservableCollection<AssemblyEntity> GetChampionData()
        {
            return new ObservableCollection<AssemblyEntity>();
        }

        public ObservableCollection<AssemblyEntity> GetUtilityData()
        {
            return new ObservableCollection<AssemblyEntity>();
        }

        public ObservableCollection<AssemblyEntity> GetLibraryData()
        {
            return new ObservableCollection<AssemblyEntity>();
        }

        public ObservableCollection<LogEntity> GetLogData()
        {
            return new ObservableCollection<LogEntity>();
        }


        public bool IsInitComplete()
        {
            return true;
        }
    }
}
