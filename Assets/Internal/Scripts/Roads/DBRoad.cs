using UnityEngine;

namespace ScrollingEnvironment
{
    public class DBRoad : MonoBehaviour
    {
        [SerializeField] private SORoad _road = null;
        
        [SerializeField] private Color _displayColor = Color.blue;

        [SerializeField] private bool _isDrawingAlways = false;

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

            Gizmos.DrawLine(_road.StartPoint, _road.FinishPoint);
            
            Gizmos.color = previousColor;
        }

        private bool IsEverythingAssigned()
            => _road;
    }
}