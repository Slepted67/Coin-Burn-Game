// OptionalCoin.cs
using UnityEngine;

public class OptionalCoin : MonoBehaviour
{
    [Range(0f, 1f)]
    public float spawnChance = 0.75f; // Default 75% chance to appear
}
