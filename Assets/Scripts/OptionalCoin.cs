using UnityEngine;

/// <summary>
/// This script marks a coin GameObject as optional, with a set chance to appear during spawning.
/// Used in procedural generation to randomize which coins get activated.
/// </summary>
public class OptionalCoin : MonoBehaviour
{
    [Range(0f, 1f)]
    [Tooltip("Probability (0-1) that this coin will appear when the segment is spawned.")]
    public float spawnChance = 0.75f; // Default 75% chance to appear
}