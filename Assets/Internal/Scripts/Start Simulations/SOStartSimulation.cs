using UnityEngine;

namespace ScrollingEnvironment
{
    [CreateAssetMenu(menuName = "Scrolling Environment/Start Simulation",
        fileName = "Start Simulation")]
    public class SOStartSimulation : ScriptableObject
    {
        public float Time => _time;
        public float Step => _step;
        public float TimeRemainder => _timeRemainder;
        
        [SerializeField] private float _time = 0f;
        [SerializeField] private float _step = 0f;
        [SerializeField, HideInInspector] private float _timeRemainder = 0f;
        
        public void ResetTimeRemainder()
            => _timeRemainder = _time;

        public void DoStep()
            => _timeRemainder -= _step;
    }
}