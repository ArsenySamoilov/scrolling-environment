using UnityEngine;

namespace ScrollingEnvironment
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DBSprite : MonoBehaviour
    {
        [SerializeField] private Transform _transform = null;
        [SerializeField] private SpriteRenderer _spriteRenderer = null;
        
        [SerializeField] private SOSprite _sprite = null;
        
        [SerializeField] private Color _displayColor = Color.cyan;
        
        private void Reset()
        {
            _transform = GetComponent<Transform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnDrawGizmosSelected()
        {
            if (!IsEverythingAssigned())
            {
                return;
            }

            var previousColor = Gizmos.color;
            Gizmos.color = _displayColor;
            
            var leftTopPoint = CalculateLeftTopPoint();
            var rightTopPoint = CalculateRightTopPoint();
            var leftBottomPoint = CalculateLeftBottomPoint();
            var rightBottomPoint = CalculateRightBottomPoint();

            Gizmos.DrawLine(leftTopPoint, leftBottomPoint);
            Gizmos.DrawLine(rightTopPoint, rightBottomPoint);
            
            Gizmos.color = previousColor;
        }

        [ContextMenu("Assign Sprite To Renderer")]
        private void AssignSpriteToRenderer()
        {
            if (_spriteRenderer && _sprite)
            {
                _spriteRenderer.sprite = _sprite.Sprite;
            }
        }

        private bool IsEverythingAssigned()
            => _transform && _spriteRenderer && _sprite && _sprite.Sprite;

        private float CalculateHeight() 
            => (_sprite.Sprite.rect.height - _sprite.Sprite.pivot.y) 
               / _sprite.Sprite.pixelsPerUnit;

        private Vector3 CalculateLeftTopPoint() 
            => _transform.position 
               + new Vector3(-_sprite.LeftBorder, CalculateHeight());
        
        private Vector3 CalculateRightTopPoint() 
            => _transform.position
               + new Vector3(_sprite.RightBorder, CalculateHeight());

        private Vector3 CalculateLeftBottomPoint()
            => _transform.position
               + new Vector3(-_sprite.LeftBorder, 0f);

        private Vector3 CalculateRightBottomPoint() 
            => _transform.position
               + new Vector3(_sprite.RightBorder, 0f);
    }
}