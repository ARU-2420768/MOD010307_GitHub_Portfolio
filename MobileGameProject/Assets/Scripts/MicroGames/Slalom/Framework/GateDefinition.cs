using UnityEngine;

[CreateAssetMenu(fileName = "GateType", menuName = "Framework/Slalom/GateDefinition")]
public class GateDefinition : ScriptableObject
{
    public GameObject prefab;
    public GateConnection allowedLocations;
    [Range(0,2)]
    public int nextOffset;
}
