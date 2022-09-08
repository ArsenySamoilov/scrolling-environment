using UniRx;
using UnityEngine;

namespace ScrollingEnvironment
{
    public class ETSpawner : System.IDisposable
    {
        private float _currentTime = 0f;

        private readonly SOSpawner _spawnerData = null;
        private readonly SOStartSimulation _startSimulationData = null;
        
        private readonly ETSpawnerStack _spawnerStackEntity = null;
        
        private readonly PLSpawn _spawnPool = null;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public ETSpawner(SOSpawner spawnerData, ETSpawnerStack spawnerStackEntity, PLSpawn spawnPool)
        {
            _spawnerData = spawnerData;
            _startSimulationData = spawnerStackEntity.StartSimulationData;
            _spawnerStackEntity = spawnerStackEntity;
            _spawnPool = spawnPool;
            
            _currentTime = spawnerData.GetRandomTime();
            
            BeginSimulation();
        }

        public void Dispose()
        {
            _disposable.Clear();
        }

        private void BeginSimulation()
        {
            if (_startSimulationData.TimeRemainder > 0f)
            {
                SimulateStart();
            }
            else
            {
                SimulateUpdate();
            }
        }

        private void SimulateStart()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    UpdateCall(_startSimulationData.Step);

                    if (_startSimulationData.TimeRemainder < 0f)
                    {
                        _disposable.Clear();
                        SimulateUpdate();
                    }
                })
                .AddTo(_disposable);
        }

        private void SimulateUpdate()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    UpdateCall(Time.deltaTime);
                })
                .AddTo(_disposable);
        }

        private void UpdateCall(float deltaTime)
        {
            _currentTime -= deltaTime;

            if (_currentTime <= 0f)
            {
                _currentTime = _spawnerData.GetRandomTime();
                SpawnSpriteObject();
            }
        }

        private void SpawnSpriteObject()
        {
            var road = _spawnerStackEntity.Distribute();
            var spawn = _spawnPool.GetSpawn(_spawnerData.GetRandomSpawnObject());

            spawn.SpawnerEntity = this;
            spawn.RoadEntity = road;
            
            spawn.StartCall();
        }
    }
}