using UnityEngine;

namespace ScrollingEnvironment
{
    public class MBDistributor : MonoBehaviour
    {
        [SerializeField] private SODistributor _distributor = null;
        [SerializeField] private PLSpawn _pool = null;
        
        private ETDistributor _entity = null;

        private void Start()
        {
            _entity = new ETDistributor(_distributor, _pool);
        }

        private void OnDisable()
        {
            _entity.Dispose();
        }
    }
}