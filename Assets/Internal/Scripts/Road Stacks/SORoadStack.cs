using System;
using UnityEngine;

namespace ScrollingEnvironment
{
    [CreateAssetMenu(menuName = "Scrolling Environment/Road Stack",
        fileName = "Road Stack")]
    public class SORoadStack : ScriptableObject
    {
        public SORoad[] RoadsData => _roads;
        public float[] DistributionChances => _distributionChances;
        public int RoadsAmount => _roadsAmount;
        public float DistributionChancesSum => _distributionChancesSum;

        [SerializeField] private SORoad[] _roads = Array.Empty<SORoad>();
        [SerializeField] private float[] _distributionChances = Array.Empty<float>();
        [SerializeField, HideInInspector] private int _roadsAmount = 0;
        [SerializeField, HideInInspector] private float _distributionChancesSum = 0f;

        private void OnValidate()
        {
            _roadsAmount = _roads.Length;
            _distributionChancesSum = CalculateSpawnChancesSum();
        }
        
        public int GetRandomDistributionIndex()
        {
            var i = -1;
            
            var random = UnityEngine.Random.Range(0.001f, _distributionChancesSum);
            do
            {
                ++i;
                random -= _distributionChances[i];
            } while (random > 0.00001f);

            return i;
        }
        
        private float CalculateSpawnChancesSum()
        {
            var result = 0f;
            Array.ForEach(_distributionChances, value => result += value);
            return result;
        }
    }
}