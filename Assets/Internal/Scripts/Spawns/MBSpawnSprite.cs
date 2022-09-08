using UnityEngine;

namespace ScrollingEnvironment
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MBSpawnSprite : MBSpawn
    {
        [SerializeField] private SpriteRenderer _spriteRenderer = null;

        [SerializeField] private SOSpriteStackEnvironment _spriteStackEnvironment = null;

        private SOSprite _spriteData = null;
        
        [ContextMenu("Assign Sprites")]
        private void AssignSprites()
        {
            if (!ReferenceEquals(_spriteStackEnvironment, null))
            {
                _spriteData = _spriteStackEnvironment.GetRandomSpriteData();
            }
            
            if (_spriteRenderer && _spriteData)
            {
                _spriteRenderer.sprite = _spriteData.Sprite;
            }
        }

        protected override float GetLeftBorder()
            => _spriteData.LeftBorder;
        protected override float GetRightBorder()
            => _spriteData.RightBorder;

        public override void Initialize()
        {
            base.Initialize();

            if (!_spriteRenderer)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        protected override void Prepare()
        {
            _spriteData = _spriteStackEnvironment.GetRandomSpriteData();
            
            base.Prepare();

            _spriteRenderer.sprite = _spriteData.Sprite;
            _spriteRenderer.sortingLayerName = RoadEntity.RoadData.LayerName;
            _spriteRenderer.sortingOrder = RoadEntity.RoadData.OrderInLayer;
        }
    }
}