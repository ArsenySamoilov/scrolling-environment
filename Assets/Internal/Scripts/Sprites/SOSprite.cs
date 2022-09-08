using UnityEngine;

namespace ScrollingEnvironment
{
    [CreateAssetMenu(menuName = "Scrolling Environment/Sprite",
        fileName = "Sprite")]
    public class SOSprite : ScriptableObject
    {
        public Sprite Sprite => _sprite;
        public float LeftBorder => _leftBorder;
        public float RightBorder => _rightBorder;
        
        [SerializeField] private Sprite _sprite = null;
        [SerializeField, HideInInspector] private float _leftBorder = 0f;
        [SerializeField, HideInInspector] private float _rightBorder = 0f;

        private void OnValidate()
        {
            CalculateBorders();
        }

        [ContextMenu("Validate Manually")]
        private void ValidateManually()
        {
            CalculateBorders();
        }

        private void CalculateBorders()
        {
            if (!IsEverythingAssigned())
            {
                _leftBorder = 0f;
                _rightBorder = 0f;
                return;
            }
            
            _leftBorder = CalculateLeftBorder();
            _rightBorder = CalculateRightBorder();
        }

        private bool IsEverythingAssigned()
            => _sprite;

        private float CalculateLeftBorder()
            => (_sprite.pivot.x - _sprite.border.x) 
               / _sprite.pixelsPerUnit;

        private float CalculateRightBorder()
            => (_sprite.rect.width - _sprite.pivot.x - _sprite.border.z)
               / _sprite.pixelsPerUnit;
    }
}