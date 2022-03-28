using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(GameScenario))]
[CanEditMultipleObjects]
public class GameScenarioEditor : Editor
{
    private bool showMap = false;
    private SerializedProperty cyclesProp;
    private SerializedProperty cycleSpeedUpProp;
    private SerializedProperty wavesProp;
    private SerializedProperty boardSizeProp;

    private void OnEnable()
    {
        cyclesProp = serializedObject.FindProperty("cycles");
        cycleSpeedUpProp = serializedObject.FindProperty("cycleSpeedUp");
        wavesProp = serializedObject.FindProperty("waves");
        boardSizeProp = serializedObject.FindProperty("BoardSize");
    }

    public override void OnInspectorGUI()
    {
        GameScenario gameScenario = (GameScenario) target;

        serializedObject.Update();
        EditorGUILayout.PropertyField(cyclesProp);
        EditorGUILayout.PropertyField(cycleSpeedUpProp);
        EditorGUILayout.PropertyField(wavesProp);
        EditorGUILayout.PropertyField(boardSizeProp);

        showMap = EditorGUILayout.Foldout(showMap, "Map");

        if (showMap)
        {
            var boardSize = gameScenario.BoardSize;
            int size = 20;
            int padding = 0;

            GUILayout.BeginScrollView(Vector2.one);
            for (int x = 0; x <= boardSize.x; x++)
            {
                GUILayout.BeginHorizontal();
                for (int y = 0; y <= boardSize.y; y++)
                {
                    GUILayout.Button(x + "," + y, GUILayout.Width(size), GUILayout.Height(size));
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}