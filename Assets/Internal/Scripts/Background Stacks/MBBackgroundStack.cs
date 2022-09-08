using System;
using UnityEngine;

namespace ScrollingEnvironment
{
    public class MBBackgroundStack : MonoBehaviour
    {
        [SerializeField] private SOBackgroundStack _backgroundStack = null;

        private ETBackground[] _backgrounds = Array.Empty<ETBackground>();
        
        private void Start()
        {
            var backgroundsAmount = _backgroundStack.Backgrounds.Length;
            _backgrounds = new ETBackground[backgroundsAmount];
            
            for (var i = 0; i < backgroundsAmount; ++i)
            {
                _backgrounds[i] = new ETBackground(_backgroundStack.Backgrounds[i]);
            }
        }
    }
}