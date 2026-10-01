using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public static class AI
{
    public static bool IsSeeking(List<Node> distanceToPlayer)
    {
        if (distanceToPlayer != null && distanceToPlayer.Count <= 10) return true;
        else return false;
    }

    public static bool IsFleeing(List<Node> distanceToPlayer)
    {
        if (distanceToPlayer != null && distanceToPlayer.Count <= 10) return true;
        else return false;
    }
}
