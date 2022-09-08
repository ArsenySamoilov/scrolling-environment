using UnityEngine;

namespace ScrollingEnvironment
{
    [RequireComponent(typeof(MBSpawn))]
    public class DBSpawn : MonoBehaviour
    {
        [SerializeField] private MBSpawn _spawn = null;
        
        [SerializeField] private Color _displayColor = Color.magenta;

        [SerializeField] private bool _isDrawingAlways = false;

        private void Reset()
        {
            if (IsEverythingAssigned())
            {
                _spawn = GetComponent<MBSpawn>();
            }
        }

        private void OnDrawGizmos()
        {
            if (_isDrawingAlways)
            {
                Draw();
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            if (!_isDrawingAlways)
            {
                Draw();
            }
        }

        private void Draw()
        {
            if (!IsEverythingAssigned())
            {
                return;
            }

            var previousColor = Gizmos.color;
            Gizmos.color = _displayColor;

            var leftPoint = CalculateLeftPoint();
            var rightPoint = CalculateRightPoint();
            
            Gizmos.DrawLine(leftPoint, rightPoint);
            
            Gizmos.color = previousColor;
        }

        private bool IsEverythingAssigned()
            => _spawn;

        private Vector3 CalculateLeftPoint()
            => _spawn.Transform.position
               - new Vector3(_spawn.LeftBorder
                             * (_spawn.RoadEntity?.RoadData.Size.x ?? 1f), 0f);

        private Vector3 CalculateRightPoint()
            => _spawn.Transform.position
               + new Vector3(_spawn.RightBorder
                             * (_spawn.RoadEntity?.RoadData.Size.x ?? 1f), 0f);
    }
}