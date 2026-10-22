using UnityEngine;

[CreateAssetMenu(fileName = "GateType", menuName = "Framework/Slalom/GateDefinition")]
public class GateDefinition : ScriptableObject
{
    public GameObject prefab;
    public GateConnection allowedLocations;
    [Range(-2,2)]
    public int nextOffset;
    public float offsetAmount;
}
