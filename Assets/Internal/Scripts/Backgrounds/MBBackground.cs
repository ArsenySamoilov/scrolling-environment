using UniRx;
using UnityEngine;

namespace ScrollingEnvironment
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MBBackground : MonoBehaviour
    {
        public ETBackground BackgroundEntity { get; set; } = null;
        public MBBackground PreviousBackgroundObject { get; set; } = null;
        public MBBackground NextBackgroundObject { get; set; } = null;
        public SORoad RoadData { get; set; } = null;

        [SerializeField] private Transform _transform = null;
        [SerializeField] private SpriteRenderer _spriteRenderer = null;

        private SOSprite _spriteData = null;
        
        private float _distanceFromPreviousElement = 0f;

        private readonly Vector2 _leftDirection = Vector2.left;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private void Reset()
        {
            Initialize();
        }

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            PrepareRoadParameters();
            
            if (BackgroundEntity.HeadBackgroundObject == this)
            {
                PrepareSpriteAlongChain();
                PrepareDistanceFromPreviousElementAlongChain();

                _transform.position =
                    (Vector3) (RoadData.FinishPoint 
                               - _spriteData.LeftBorder * RoadData.Size.x * _leftDirection);
                BeginSimulation();
            }
        }

        private void OnDisable()
        {
            _disposable.Clear();
        }

        private void Initialize()
        {
            if (!_transform)
            {
                _transform = GetComponent<Transform>();
            }

            if (!_spriteRenderer)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void PrepareRoadParameters()
        {
            _transform.localScale = (Vector3) RoadData.Size;
            _spriteRenderer.sortingOrder = RoadData.OrderInLayer;
            _spriteRenderer.sortingLayerName = RoadData.LayerName;
        }

        private void PrepareSpriteAlongChain()
        {
            ChangeSprite();
            
            if (BackgroundEntity.HeadBackgroundObject != NextBackgroundObject)
            {
                NextBackgroundObject.PrepareSpriteAlongChain();
            }
        }

        private void PrepareDistanceFromPreviousElementAlongChain()
        {
            _distanceFromPreviousElement = CalculateDistanceFromPreviousElement();

            if (BackgroundEntity.HeadBackgroundObject != NextBackgroundObject)
            {
                NextBackgroundObject.PrepareDistanceFromPreviousElementAlongChain();
            }
        }

        private float CalculateDistanceFromPreviousElement()
            => (_spriteData.LeftBorder + PreviousBackgroundObject._spriteData.RightBorder) 
               * RoadData.Size.x;

        private void BeginSimulation()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    Move();
                    NextBackgroundObject.PlaceAfterPreviousElementAlongChain();

                    if (IsOutsideBorder())
                    {
                        _disposable.Clear();
                        
                        ChangeSprite();
                        
                        _distanceFromPreviousElement = CalculateDistanceFromPreviousElement();
                        
                        TransmitHeadToNext();
                    }
                })
                .AddTo(_disposable);
        }

        private void Move()
            => _transform.position += RoadData.Speed * Time.deltaTime * Vector3.left;

        private void PlaceAfterPreviousElementAlongChain()
        {
            _transform.position = PreviousBackgroundObject._transform.position 
                                  - _distanceFromPreviousElement * (Vector3) _leftDirection;
            
            if (BackgroundEntity.HeadBackgroundObject != NextBackgroundObject)
            {
                NextBackgroundObject.PlaceAfterPreviousElementAlongChain();
            }
        }

        private bool IsOutsideBorder()
            => _transform.position.x + _spriteData.RightBorder * RoadData.Size.x < RoadData.FinishPoint.x;

        private void ChangeSprite()
        {
            _spriteData = BackgroundEntity.GetNextSprite();
            _spriteRenderer.sprite = _spriteData.Sprite;
        }
        
        private void TransmitHeadToNext()
        {
            BackgroundEntity.HeadBackgroundObject = NextBackgroundObject;
            NextBackgroundObject.BeginSimulation();
        }
    }
}