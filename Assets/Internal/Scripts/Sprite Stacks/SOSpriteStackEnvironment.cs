using System;
using UnityEngine;

namespace ScrollingEnvironment
{
    [CreateAssetMenu(menuName = "Scrolling Environment/Sprite Stack/Environment",
        fileName = "Sprite Stack")]
    public class SOSpriteStackEnvironment : ScriptableObject
    {
        public SOSprite[] SpritesData => _sprites;
        public float[] SelectingChances => _selectingChances;
        public float SelectingChancesSum => _selectingChancesSum;
        
        [SerializeField] private SOSprite[] _sprites = Array.Empty<SOSprite>();
        [SerializeField] private float[] _selectingChances = Array.Empty<float>();
        [SerializeField, HideInInspector] private float _selectingChancesSum = 0f;

        private void OnValidate()
        {
            _selectingChancesSum = CalculateSelectingChancesSum();
        }
        
        public SOSprite GetRandomSpriteData()
        {
            var i = -1;
            
            var random = UnityEngine.Random.Range(0.001f, _selectingChancesSum);
            do
            {
                ++i;
                random -= _selectingChances[i];
            } while (random > 0.00001f);

            return _sprites[i];
        }
        
        private float CalculateSelectingChancesSum()
        {
            var result = 0f;
            Array.ForEach(_selectingChances, value => result += value);
            return result;
        }
    }
}