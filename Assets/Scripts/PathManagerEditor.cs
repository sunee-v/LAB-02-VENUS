using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : MonoBehaviour
{
    [SerializeField] private PathManager pathManager;
    [SerializeField] List<Waypoint> ThePath;
    [SerializeField] List<int> toDelete;

    Waypoint selectedPoint = null;
    bool doRepaint = true;
    private PathManager target;

    public object serializedObject { get; private set; }

    private void OnSceneGUI(PathManager pathManager)
    {
        ThePath = pathManager.GetPath();
        DrawPath(ThePath);
    }

    private void OnEnable()
    {
        pathManager  = target;
        toDelete= new List<int>();
    }

    public void OnInspectorGUI()
    {
        this.serializedObject.Update();
        ThePath = pathManager.GetPath();

        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path");

        DrawGUIForPoints();

        if (GUILayout.Button("Add point to path"))
        {
            pathManager.CreateAddPoint();
        }
        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();
    }

    private void DrawGUIForPoints()
    {
        if (ThePath != null && ThePath.Count > 0)
        {
            for (int i = 0; i < ThePath.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                Waypoint p = ThePath[i];

                Vector3 oldPos = p.Getpos();
                Vector3 newPos = EditorGUILayout.Vector3Field("", oldPos);

                if (EditorGUI.EndChangeCheck())
                {
                    p.Setpos(newPos);
                }

                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    toDelete.Add(i);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        if (toDelete.Count > 0)
        {
            foreach (int i in toDelete)
            {
                ThePath.RemoveAt(i);
            }
            toDelete.Clear();
        }
    }

    private void DrawPath(List<Waypoint> path)
    {
        if (path != null)
        {
            int current = 0;
            foreach (Waypoint wp in path)
            {
                doRepaint = DrawPath(wp);
                int next = (current + 1) % path.Count;
                Waypoint wpnext = path[next];

                DrawPathLine(wp, wpnext);
                current += 1;

            }
            if (doRepaint) { Repaint(); }
        }
    }
    public void DrawPathLine(Waypoint p1, Waypoint p2)
    {
        Color c = Handles.color;
        Handles.color = Color.gray;
        Handles.DrawLine(p1.Getpos(), p2.Getpos());
        Handles.color = c;
    }

    public bool DrawPoint(Waypoint p)
    {
        bool isChanged = false;

        Vector3 currPos = p.Getpos();
        float handleSize = HandleUtility.GetHandleSize(currPos);
        if(Handles.Button(currPos, Quaternion.identity, 0.25f *handleSize, 0.25f * handleSize, Handles.SphereHandleCap))
        {
            isChanged = true;
            selectedPoint= p;
        }
        return isChanged;
    }
}
