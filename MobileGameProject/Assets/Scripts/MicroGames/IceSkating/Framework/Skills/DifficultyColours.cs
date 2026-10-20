using System.Collections.Generic;
using UnityEngine;

public static class DifficultyColours
{
    public static readonly Dictionary<Difficulty, Color32> Colours = new()
    {
        { Difficulty.Easy,         new Color32(0,255,0,255) },
        { Difficulty.Intermediate, new Color32(0,0,255,255) },
        { Difficulty.Advanced,     new Color32(225,0,0,255) },
        { Difficulty.Expert,       new Color32(0,0,0,255) }
    };
}
