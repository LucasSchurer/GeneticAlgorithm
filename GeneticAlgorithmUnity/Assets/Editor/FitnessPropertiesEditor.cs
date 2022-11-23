using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game.GA;

[CustomEditor(typeof(FitnessProperties))]
public class FitnessPropertiesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FitnessProperties script = (FitnessProperties)target;
        if (GUILayout.Button("Balance Properties Weights"))
        {
            script.BalancePropertiesWeights();
        }

        if (GUILayout.Button("Set Everything to 0"))
        {
            script.SetWeights(0);
        }

        if (GUILayout.Button("Same Weights"))
        {
            script.SetWeights(1f/script.Properties.Length);
        }
    }
}
