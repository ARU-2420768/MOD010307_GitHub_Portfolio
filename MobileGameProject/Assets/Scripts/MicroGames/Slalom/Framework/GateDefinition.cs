using UnityEngine;

[CreateAssetMenu(fileName = "GateType", menuName = "Framework/Slalom/GateDefinition")]
public class GateDefinition : ScriptableObject
{
    public GameObject prefab;
    public GateConnection allowedLocations;
    [Range(-4,4)]
    public int nextOffset;
    public float offsetAmount;
}
