﻿using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Direction : IComponentData
{
    public int2 Value;
}
