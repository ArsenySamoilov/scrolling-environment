using System;
using UnityEngine;

namespace ScrollingEnvironment
{
    [CreateAssetMenu(menuName = "Scrolling Environment/Background Stack", 
        fileName = "Background Stack")]
    public class SOBackgroundStack : ScriptableObject
    {
        public SOBackground[] Backgrounds => _backgrounds;
        
        [SerializeField] private SOBackground[] _backgrounds = Array.Empty<SOBackground>();
    }
}