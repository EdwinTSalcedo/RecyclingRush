using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RenderQueue
{
    Background = 1000,
    Geometry = 2000,
    AlphaTest = 2450, // we want it to be in the end of geometry queue
    GeometryLast = 2500, // last queue that is considered "opaque" by Unity
    Transparent = 3000,
    Overlay = 4000,
}
