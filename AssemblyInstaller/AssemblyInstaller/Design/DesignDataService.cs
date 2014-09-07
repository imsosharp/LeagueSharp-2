using System.Collections.ObjectModel;
using AssemblyInstaller.Model;

namespace AssemblyInstaller.Design
{
    public class DesignDataService : IDataService
    {
        public ObservableCollectionEx<AssemblyEntity> GetChampionData()
        {
            return new ObservableCollectionEx<AssemblyEntity>();
        }

        public ObservableCollectionEx<AssemblyEntity> GetUtilityData()
        {
            return new ObservableCollectionEx<AssemblyEntity>();
        }

        public ObservableCollectionEx<AssemblyEntity> GetLibraryData()
        {
            return new ObservableCollectionEx<AssemblyEntity>();
        }

        public ObservableCollectionEx<LogEntity> GetLogData()
        {
            return new ObservableCollectionEx<LogEntity>();
        }


        public bool IsInitComplete()
        {
            return true;
        }
    }
}
