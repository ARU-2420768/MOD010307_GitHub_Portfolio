using UnityEngine;

[CreateAssetMenu(fileName = "GateType", menuName = "Framework/Slalom/GateDefinition")]
public class GateDefinition : ScriptableObject
{
    public GameObject prefab;
    public GateConnection allowedLocations;
    [Range(-1,1)]
    public int preOffset;
    [Range(0,1)]
    public int nextOffset;
}
