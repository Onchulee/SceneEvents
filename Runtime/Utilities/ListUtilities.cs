using System.Collections;
using UnityEngine;

namespace com.dgn.SceneEvent
{
    internal static class ListUtilities
    {

        public static bool IsValidIndex(this IList list, int index)
        {
            return list.Count > 0 && index == Mathf.Clamp(index, 0, list.Count - 1);
        }
    }
}