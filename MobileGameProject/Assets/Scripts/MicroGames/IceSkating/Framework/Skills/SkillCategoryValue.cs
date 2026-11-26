using System;
using UnityEngine;

[Serializable]
public class SkillCategoryValue
{
    public SkillCategory category;

    [Range(0f,1f)]
    public float value;
}