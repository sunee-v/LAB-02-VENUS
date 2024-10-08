using UnityEngine;

[System.Serializable]
public class Waypoint
{
    [SerializeField] public Vector3 pos;

    public void Setpos(Vector3 newPos) { pos = newPos; }
    public Vector3 Getpos() { return pos;}
    public Waypoint() { pos = Vector3.zero; }
}
