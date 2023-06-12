using UnityEditor;
using UnityEngine;

namespace Game.AI
{
    [CustomEditor(typeof(StateData), true)]
    public class StateDataEditor : Editor
    {
/*        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            StateData script = (StateData)target;
            if (GUILayout.Button("Balance Probabilities"))
            {
                script.BalanceProbabilities();
            }

            if (script.GetProbabilitiesSum() != 1f)
            {
                EditorGUILayout.HelpBox("Probabilities sum is different than 1!", MessageType.Error);
            }

            if (script.HasRepeatedTransition())
            {
                EditorGUILayout.HelpBox("More than one transition leads to the same state!", MessageType.Error);
            }
        }*/
    } 
}
