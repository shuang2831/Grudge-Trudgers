using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreBehavior
{
    public static int[] PlayerScores = new int[4] { 50, 50, 50, 50 };

    private static System.Random rng = new System.Random();

    public static List<string> Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list as List<string>;
    }

    public static List<string> levels = (new List<string>() {
        "PickSides Level",
        "Paint Level",
        "Backstab Level",
        "Box Level",
        "Bridge Level",
        "Button Level",
        "Huddle Level",
        "Ice Level",
        "Island Level",
        "Maze Level",
        "Prisoner Level",
        "Soccer Level",
        "Reward Level",
        "Blob Level"
    }).Shuffle();

}
