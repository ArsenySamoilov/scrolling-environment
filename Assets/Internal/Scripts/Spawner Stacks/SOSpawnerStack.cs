using System;
using UnityEngine;

namespace ScrollingEnvironment
{
    [CreateAssetMenu(menuName = "Scrolling Environment/Spawner Stack",
        fileName = "Spawner Stack")]
    public class SOSpawnerStack : ScriptableObject
    {
        public SOSpawner[] SpawnersData => _spawners;
        public int SpawnersAmount => _spawnersAmount;
        
        [SerializeField] private SOSpawner[] _spawners = Array.Empty<SOSpawner>();
        [SerializeField, HideInInspector] private int _spawnersAmount = 0;

        private void OnValidate()
        {
            _spawnersAmount = _spawners.Length;
        }
    }
}