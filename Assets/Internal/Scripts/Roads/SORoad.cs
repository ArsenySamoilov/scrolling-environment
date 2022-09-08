using UnityEngine;

namespace ScrollingEnvironment
{
    [CreateAssetMenu(menuName = "Scrolling Environment/Road",
        fileName = "Road")]
    public class SORoad : ScriptableObject
    {
        public Vector2 Center => _center;
        public float Length => _length;
        public float Speed => _speed;
        public Vector2 Size => _size;
        public string LayerName => _layerName;
        public int OrderInLayer => _orderInLayer;
        public Vector2 StartPoint => _startPoint;
        public Vector2 FinishPoint => _finishPoint;

        [SerializeField] private Vector2 _center = Vector2.zero;
        [SerializeField] private float _length = 0f;
        [SerializeField] private float _speed = 0f;
        [SerializeField] private Vector2 _size = Vector2.one;
        [SerializeField] private string _layerName = "Default";
        [SerializeField] private int _orderInLayer = 0;
        [SerializeField, HideInInspector] private Vector2 _startPoint = Vector2.zero;
        [SerializeField, HideInInspector] private Vector2 _finishPoint = Vector2.zero;

        private void OnValidate()
        {
            _startPoint = CalculateStartPoint();
            _finishPoint = CalculateFinishPoint();
        }

        private Vector2 CalculateStartPoint()
            => _center + new Vector2(_length / 2f, 0f);

        private Vector2 CalculateFinishPoint()
            => _center + new Vector2(-_length / 2f, 0f);
    }
}