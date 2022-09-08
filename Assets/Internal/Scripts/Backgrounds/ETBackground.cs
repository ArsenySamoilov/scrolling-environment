using UnityEngine;

namespace ScrollingEnvironment
{
    public class ETBackground
    {
        public MBBackground HeadBackgroundObject { get; set; } = null;
        
        private int _previousSpriteIndex = -1;

        private readonly SOBackground _backgroundData = null;
        
        private readonly int _spritesAmount = 0;

        public ETBackground(SOBackground backgroundData)
        {
            _backgroundData = backgroundData;
            _spritesAmount = _backgroundData.SpriteStackBackground.SpritesData.Length;
            
            if (_backgroundData.SpriteStackBackground.CanStartFromAnyPosition)
            {
                _previousSpriteIndex = Random.Range(-1, _spritesAmount - 1);
            }

            SpawnAllBackgrounds();
        }

        public SOSprite GetNextSprite()
            => _backgroundData.SpriteStackBackground.SpritesData[
                _previousSpriteIndex = (_previousSpriteIndex + 1) % _spritesAmount];

        private void SpawnAllBackgrounds()
        {
            var first = SpawnBackgroundObject(null);
            var last = first;
            
            for (var i = 0; i < _backgroundData.BackgroundsAmount - 1; ++i)
            {
                var spawned = SpawnBackgroundObject(last);
                last.NextBackgroundObject = spawned;
                last = spawned;
            }

            last.NextBackgroundObject = first;
            first.PreviousBackgroundObject = last;
            HeadBackgroundObject = first;
        }
        
        private MBBackground SpawnBackgroundObject(MBBackground previousObject)
        {
            var backgroundObject = Object.Instantiate(_backgroundData.BackgroundObject);
            backgroundObject.BackgroundEntity = this;
            backgroundObject.RoadData = _backgroundData.RoadData;
            backgroundObject.PreviousBackgroundObject = previousObject;
            
            return backgroundObject;
        }
    }
}