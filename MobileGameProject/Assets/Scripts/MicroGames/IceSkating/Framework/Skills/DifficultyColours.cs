using System.Collections.Generic;
using UnityEngine;

public static class DifficultyColours
{
    public static readonly Dictionary<Difficulty, Color32> Colours = new()
    {
        { Difficulty.Trivial,      new Color32(0,255,0,255) },
        { Difficulty.VeryEasy,     new Color32(28,227,0,255) },
        { Difficulty.Easy,         new Color32(57,198,0,255) },
        { Difficulty.Simple,       new Color32(85,170,0,255) },
        { Difficulty.Moderate,     new Color32(113,142,0,255) },
        { Difficulty.Challenging,  new Color32(142,113,0,255) },
        { Difficulty.Hard,         new Color32(170,85,0,255) },
        { Difficulty.VeryHard,     new Color32(198,57,0,255) },
        { Difficulty.Expert,       new Color32(227,28,0,255) },
        { Difficulty.Master,       new Color32(255,0,0,255) }
    };
}
