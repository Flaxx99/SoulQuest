using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(PersonajeStats))]
public class PersonajeStatsEditor : Editor
{
   public PersonajeStats StatsTarget => target as PersonajeStats;
    
   public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Resetear Valores"))
        {
            StatsTarget.ResetearValores();
        }
    }
}
