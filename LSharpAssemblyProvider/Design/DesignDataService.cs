using System.Collections.ObjectModel;
using LSharpAssemblyProvider.Model;

namespace LSharpAssemblyProvider.Design
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
        
        public ObservableCollectionEx<LogEntity> GetL#_IssuesData()
        {
            return new ObservableCollectionEx<L#_IssuesgEntity>();
        }

        public bool IsInitComplete()
        {
            return true;
        }
    }
}
