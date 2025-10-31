using Quantum;
using UnityEngine;

[CreateAssetMenu(menuName = "Quantum/Map Meta")]
public class MapMeta : AssetObject
{
    [Tooltip("Next Quantum map to load after this one finishes")]
    public AssetRef<Map> NextMap;
}