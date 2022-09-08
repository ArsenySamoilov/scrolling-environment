using System;
using UnityEngine;

namespace ScrollingEnvironment
{
    [CreateAssetMenu(menuName = "Scrolling Environment/Spawner",
        fileName = "Spawner")]
    public class SOSpawner : ScriptableObject
    {
        public MBSpawn[] SpawnObjects => _spawns;
        public float[] SpawnChances => _spawnChances;
        public Vector2 TimerMinMax => _timerMinMax;
        public float SpawnChancesSum => _spawnChancesSum;
        
        [SerializeField] private MBSpawn[] _spawns = Array.Empty<MBSpawn>();
        [SerializeField] private float[] _spawnChances = Array.Empty<float>();
        [SerializeField] private Vector2 _timerMinMax = Vector2.zero;
        [SerializeField, HideInInspector] private float _spawnChancesSum = 0f;

        private void OnValidate()
        {
            _spawnChancesSum = CalculateSpawnChancesSum();
        }
        
        public float GetRandomTime()
            => UnityEngine.Random.Range(_timerMinMax[0], _timerMinMax[1]);
        
        public MBSpawn GetRandomSpawnObject()
        {
            var i = -1;
            
            var random = UnityEngine.Random.Range(0.001f, _spawnChancesSum);
            do
            {
                ++i;
                random -= _spawnChances[i];
            } while (random > 0.00001f);

            return _spawns[i];
        }
        
        private float CalculateSpawnChancesSum()
        {
            var result = 0f;
            Array.ForEach(_spawnChances, value => result += value);
            return result;
        }
    }
}