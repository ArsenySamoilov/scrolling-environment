using UniRx;
using UnityEngine;

namespace ScrollingEnvironment
{
    public class MBSpawn : MonoBehaviour
    {
        public ETSpawner SpawnerEntity { get; set; } = null;
        public ETRoad RoadEntity { get; set; } = null;
        
        public bool IsFree => !_gameObject.activeSelf;
        public Transform Transform => _transform;
        public float LeftBorder => GetLeftBorder();
        public float RightBorder => GetRightBorder();

        [SerializeField] protected GameObject _gameObject = null;
        [SerializeField] protected Transform _transform = null;
        [SerializeField] protected SOStartSimulation _startSimulation = null;
        
        protected readonly Vector2 _leftDirection = Vector2.left;
        
        protected readonly CompositeDisposable _disposable = new CompositeDisposable();

        private void Reset()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Disable();
        }

        public virtual void Initialize()
        {
            if (!_gameObject)
            {
                _gameObject = gameObject;
            }
            
            if (!_transform)
            {
                _transform = GetComponent<Transform>();
            }
            
            _gameObject.SetActive(false);
        }
        
        public virtual void StartCall()
        {
            Prepare();
            Wait();
        }

        public virtual bool HasCrossedStartPoint()
            => _transform.position.x + GetRightBorder() * RoadEntity.RoadData.Size.x 
               <= RoadEntity.RoadData.StartPoint.x;

        public virtual void BeginMovement()
        {
            if (_startSimulation.TimeRemainder > 0f)
            {
                SimulateMovementStart();
            }
            else
            {
                SimulateMovementUpdate();
            }
        }

        protected virtual float GetLeftBorder() => 0f;

        protected virtual float GetRightBorder() => 0f;

        protected virtual void Prepare()
        {
            _gameObject.SetActive(true);
            
            _transform.position =
                (Vector3) (RoadEntity.RoadData.StartPoint 
                - GetLeftBorder() * RoadEntity.RoadData.Size.x * _leftDirection);
            _transform.localScale = (Vector3) RoadEntity.RoadData.Size;
        }

        protected virtual void Wait()
            => RoadEntity.Enqueue(this);

        protected virtual void SimulateMovementStart()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    Move(_startSimulation.Step);
                    
                    if (_startSimulation.TimeRemainder < 0f)
                    {
                        _disposable.Clear();
                        SimulateMovementUpdate();
                    }
                })
                .AddTo(_disposable);
        }
        
        protected virtual void SimulateMovementUpdate()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    Move(Time.deltaTime);
                })
                .AddTo(_disposable);
        }

        protected virtual void Move(float deltaTime)
        {
            TranslatePosition(deltaTime);

            if (IsOutsideBorder())
            {
                Disable();
                _gameObject.SetActive(false);
            }
        }
        
        protected virtual void TranslatePosition(float deltaTime)
            => _transform.position 
                += RoadEntity.RoadData.Speed * deltaTime * (Vector3) _leftDirection;

        protected virtual bool IsOutsideBorder()
            => _transform.position.x + GetRightBorder() * RoadEntity.RoadData.Size.x 
               < RoadEntity.RoadData.FinishPoint.x;

        protected virtual void Disable()
        {
            _disposable.Clear();
        }
    }
}