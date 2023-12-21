using UnityEngine;

namespace Cinemachine
{
    /// <summary>
    /// A definition of an impulse signal that gets propagated to listeners
    /// </summary>
    [CreateAssetMenu(menuName = "Utils/Cinemachine/Recoil")]
    [DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
    public class CinemachineFixedRotationSignal : CinemachineFixedSignal
    {
        [SerializeField]
        private float _yawNoiseAmplitude;

        /// <summary>Get the raw signal at this time</summary>
        /// <param name="timeSinceSignalStart">The time since in seconds since the start of the signal</param>
        /// <param name="pos">The position impulse signal</param>
        /// <param name="rot">The rotation impulse signal</param>
        public override void GetSignal(float timeSinceSignalStart, out Vector3 pos, out Quaternion rot)
        {
            rot = Quaternion.Euler(new Vector3(
                            -AxisValue(m_XCurve, timeSinceSignalStart),
                            (Mathf.PerlinNoise(Time.time, Time.time) - .5f) * 2 * _yawNoiseAmplitude,
                            AxisValue(m_ZCurve, timeSinceSignalStart)));
            pos = Vector3.zero;
        }

        float AxisValue(AnimationCurve axis, float time)
        {
            if (axis == null || axis.length == 0)
                return 0;
            return axis.Evaluate(time);
        }
    }
}
