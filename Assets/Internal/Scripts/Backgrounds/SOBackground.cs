using UnityEngine;

namespace ScrollingEnvironment
{
    [CreateAssetMenu(menuName = "Scrolling Environment/Background",
        fileName = "Background")]
    public class SOBackground : ScriptableObject
    {
        public MBBackground BackgroundObject => _background;
        public SOSpriteStackBackground SpriteStackBackground => _spriteStackBackground;
        public SORoad RoadData => _road;
        public int BackgroundsAmount => _backgroundsAmount;

        [SerializeField] private SOSpriteStackBackground _spriteStackBackground = null;
        [SerializeField] private SORoad _road = null;
        [SerializeField] private MBBackground _background = null;
        [SerializeField, HideInInspector] private int _backgroundsAmount = 0;

        private void OnValidate()
        {
            if (IsEverythingAssigned())
            {
                CalculateBackgroundsAmount();
            }
        }

        private bool IsEverythingAssigned() 
            => _road && _spriteStackBackground;        

        private void CalculateBackgroundsAmount()
        {
            var spritesAmount = _spriteStackBackground.SpritesData.Length;
            
            var sortedSpritesBoundsSizesX = new float[spritesAmount];
            for (var i = 0; i < spritesAmount; ++i)
            {
                sortedSpritesBoundsSizesX[i] = CalculateSpriteBoundsSizeX(i);
            }
            System.Array.Sort(sortedSpritesBoundsSizesX);
            
            var sumLength = 0f;
            var necessaryLength = (RoadData.StartPoint.x - RoadData.FinishPoint.x) / RoadData.Size.x;
            _backgroundsAmount = 0;

            while (sumLength <= necessaryLength)
            {
                sumLength += sortedSpritesBoundsSizesX[
                                 _backgroundsAmount % sortedSpritesBoundsSizesX.Length];
                ++_backgroundsAmount;
            }

            if (sumLength - necessaryLength < sortedSpritesBoundsSizesX[0])
            {
                ++_backgroundsAmount;
            }
        }

        private float CalculateSpriteBoundsSizeX(int index)
            => _spriteStackBackground.SpritesData[index].LeftBorder
               + _spriteStackBackground.SpritesData[index].RightBorder;
    }
}