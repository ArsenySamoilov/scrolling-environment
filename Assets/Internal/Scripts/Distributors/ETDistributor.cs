using System;
using UniRx;

namespace ScrollingEnvironment
{
    public class ETDistributor : IDisposable
    {
        public SOStartSimulation StartSimulationData => _distributorData.StartSimulationData;
        
        private readonly SODistributor _distributorData = null;
        
        private readonly ETSpawnerStack[] _spawnerStackEntities = null;
        private readonly ETRoadStack[] _roadStackEntities = null;

        private readonly int _spawnerStacksAmount = 0;
        private readonly int _roadStacksAmount = 0;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public ETDistributor(SODistributor distributorData, PLSpawn spawnPool)
        {
            _distributorData = distributorData;
            _spawnerStacksAmount = _distributorData.SpawnerStacksData.Length;
            _roadStacksAmount = _distributorData.RoadStacksData.Length;
            
            SimulateStart();

            _spawnerStackEntities = new ETSpawnerStack[_spawnerStacksAmount];
            for (var i = 0; i < _spawnerStacksAmount; ++i)
            {
                _spawnerStackEntities[i] =
                    new ETSpawnerStack(_distributorData.SpawnerStacksData[i], this, spawnPool, i);
            }

            _roadStackEntities = new ETRoadStack[_roadStacksAmount];
            for (var i = 0; i < _roadStacksAmount; ++i)
            {
                _roadStackEntities[i] = 
                    new ETRoadStack(_distributorData.RoadStacksData[i], this);
            }
        }

        public void Dispose()
        {
            _disposable.Clear();
            
            for (var i = 0; i < _spawnerStacksAmount; ++i)
            {
                _spawnerStackEntities[i].Dispose();
            }

            for (var i = 0; i < _roadStacksAmount; ++i)
            {
                _roadStackEntities[i].Dispose();
            }
        }

        public bool CheckHasAnyRoadStackCrossedStartPoint()
        {
            for (var i = 0; i < _roadStacksAmount; ++i)
            {
                if (!_roadStackEntities[i].CheckHasAnyRoadCrossedStartPoint())
                {
                    return false;
                }
            }

            return true;
        }

        public ETRoadStack Distribute(int spawnerStackIndex)
            => _roadStackEntities[_distributorData.SelectRandomRoadStackIndex(spawnerStackIndex)];

        private void SimulateStart()
        {
            _distributorData.StartSimulationData.ResetTimeRemainder();
            
            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    _distributorData.StartSimulationData.DoStep();
                    
                    if (_distributorData.StartSimulationData.TimeRemainder < 0f)
                    {
                        _disposable.Clear();
                    }
                })
                .AddTo(_disposable);
        }
    }
}