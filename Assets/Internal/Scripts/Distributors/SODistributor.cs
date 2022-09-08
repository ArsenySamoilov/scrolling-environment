using System;
using Array2DEditor;
using UnityEngine;

namespace ScrollingEnvironment
{
    [CreateAssetMenu(menuName = "Scrolling Environment/Distributor",
        fileName = "Distributor")]
    public class SODistributor : ScriptableObject
    {
        public SOSpawnerStack[] SpawnerStacksData => _spawnerStacks;
        public SORoadStack[] RoadStacksData => _roadStacks;
        public float[,] DistributionChances => _distributionChances.GetCells();
        public SOStartSimulation StartSimulationData => _startSimulation;
        public float[] DistributionChancesSums => _distributionChancesSums;
        
        [SerializeField] private SOSpawnerStack[] _spawnerStacks = Array.Empty<SOSpawnerStack>();
        [SerializeField] private SORoadStack[] _roadStacks = Array.Empty<SORoadStack>();
        [SerializeField] private Array2DFloat _distributionChances = new Array2DFloat();
        [SerializeField] private SOStartSimulation _startSimulation = null;
        [SerializeField, HideInInspector] private float[] _distributionChancesSums = Array.Empty<float>();

        private void OnValidate()
        {
            var height = _distributionChances.GridSize.y;
            var width = _distributionChances.GridSize.x;
            
            if (_distributionChancesSums.Length != height)
            {
                Array.Resize(ref _distributionChancesSums, height);
            }

            for (var i = 0; i < height; ++i)
            {
                _distributionChancesSums[i] = CalculateDistributionChancesSumRow(i, width);
            }
        }
        
        public int SelectRandomRoadStackIndex(int spawnerStackIndex)
        {
            var i = -1;
            var random = UnityEngine.Random.Range(0.001f, 
                _distributionChancesSums[spawnerStackIndex]);
            
            do
            {
                ++i;
                random -= _distributionChances.GetCell(spawnerStackIndex, i);
            } while (random > 0.00001f);

            return i;
        }

        private float CalculateDistributionChancesSumRow(int row, int width)
        {
            var result = 0f;
            for (var i = 0; i < width; ++i)
            {
                result += _distributionChances.GetCells()[row, i];
            }
            return result;
        }
    }
}