using System.Collections.Generic;
using UniRx;

namespace ScrollingEnvironment
{
    public class ETRoad : System.IDisposable
    {
        public SORoad RoadData { get; } = null;

        private MBSpawn _lastSpawnedObject = null;

        private readonly ETRoadStack _roadStackEntity = null;

        private readonly Queue<MBSpawn> _spawnObjectsQueue = new Queue<MBSpawn>();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public ETRoad(SORoad roadData, ETRoadStack roadStackEntity)
        {
            RoadData = roadData;
            _roadStackEntity = roadStackEntity;
            
            SimulateQueue();
        }

        public void Dispose()
        {
            _disposable.Clear();
        }

        public bool CheckHasCrossedStartPoint(
            bool isForStack = false, bool isForDistributor = false)
            => isForStack
                ? _roadStackEntity.CheckHasAnyRoadCrossedStartPoint(isForDistributor)
                : HasLastSpawnedCrossedStartPoint();


        public void Enqueue(MBSpawn spawnObject)
            => _spawnObjectsQueue.Enqueue(spawnObject);

        private void SimulateQueue()
        {
            Observable
                .EveryUpdate()
                .Where(_ => CanDequeue())
                .Subscribe(_ =>
                {
                    var spawn = _spawnObjectsQueue.Dequeue();
                    _lastSpawnedObject = spawn;
                    spawn.BeginMovement();
                })
                .AddTo(_disposable);
        }

        private bool CanDequeue() 
            => _spawnObjectsQueue.Count > 0 && HasLastSpawnedCrossedStartPoint();
        

        private bool HasLastSpawnedCrossedStartPoint()
            => !_lastSpawnedObject || _lastSpawnedObject.HasCrossedStartPoint();
    }
}