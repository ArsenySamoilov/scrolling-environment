namespace ScrollingEnvironment
{
    public class ETSpawnerStack : System.IDisposable
    {
        public SOStartSimulation StartSimulationData => _distributorEntity.StartSimulationData;
        
        private readonly SOSpawnerStack _spawnerStackData = null;
        
        private readonly ETDistributor _distributorEntity = null;
        private readonly ETSpawner[] _spawnerEntities = null;

        private readonly int _identifier = 0;
        
        public ETSpawnerStack(SOSpawnerStack spawnerStackData, 
            ETDistributor distributorEntity, PLSpawn pool, int identifier)
        {
            _identifier = identifier;
            _distributorEntity = distributorEntity;
            _spawnerStackData = spawnerStackData;
            
            _spawnerEntities = new ETSpawner[_spawnerStackData.SpawnersAmount];
            for (var i = 0; i < _spawnerStackData.SpawnersAmount; ++i)
            {
                _spawnerEntities[i] = 
                    new ETSpawner(_spawnerStackData.SpawnersData[i], this, pool);
            }
        }

        public void Dispose()
        {
            for (var i = 0; i < _spawnerStackData.SpawnersAmount; ++i)
            {
                _spawnerEntities[i].Dispose();
            }
        }

        public ETRoad Distribute()
            => _distributorEntity.Distribute(_identifier).Distribute();
    }
}