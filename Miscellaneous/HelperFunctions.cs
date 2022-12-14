using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    public static bool ContainsLayer(this LayerMask layerMask, int layer) {
        return layerMask == (layerMask | (1 << layer));
    }
}
