using System;
using UnityEngine;

namespace ScrollingEnvironment
{
    [CreateAssetMenu(menuName = "Scrolling Environment/Sprite Stack/Background",
        fileName = "Sprite Stack")]
    public class SOSpriteStackBackground : ScriptableObject
    {
        public SOSprite[] SpritesData => _sprites;
        public bool CanStartFromAnyPosition => _canStartFromAnyPosition;
        
        [SerializeField] private SOSprite[] _sprites = Array.Empty<SOSprite>();
        [SerializeField] private bool _canStartFromAnyPosition = false;
    }
}