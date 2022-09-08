using System.Collections.Generic;
using UnityEngine;

namespace ScrollingEnvironment
{
    public class PLSpawn : MonoBehaviour
    {
        [SerializeField] private int _stepExpanding = 1;
        
        private readonly Dictionary<GameObject, MBSpawn[]> _spawnedObjectsDictionary = 
            new Dictionary<GameObject, MBSpawn[]>();

        public MBSpawn GetSpawn(MBSpawn spawnObject)
        {
            if (_spawnedObjectsDictionary.TryGetValue(spawnObject.gameObject, out var spawnedObject))
            {
                var index = System.Array.FindIndex(spawnedObject, value => value.IsFree);
                
                if (index == -1)
                {
                    var expandedLength = spawnedObject.Length + _stepExpanding;

                    System.Array.Resize(ref spawnedObject, expandedLength);

                    index = expandedLength - 1;

                    for (var i = expandedLength - _stepExpanding; i < expandedLength; ++i)
                    {
                        spawnedObject[i] = Instantiate(spawnObject);
                        spawnedObject[i].Initialize();
                    }
                    
                    _spawnedObjectsDictionary[spawnObject.gameObject] = spawnedObject;
                }

                return spawnedObject[index];
            }

            _spawnedObjectsDictionary.Add(spawnObject.gameObject, new MBSpawn[_stepExpanding]);
            
            spawnedObject = _spawnedObjectsDictionary[spawnObject.gameObject];
            for (var i = 0; i < _stepExpanding; ++i)
            {
                spawnedObject[i] = Instantiate(spawnObject);
                spawnedObject[i].Initialize();
            }

            return spawnedObject[0];
        }
    }
}