using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int level;

    public PlayerData(int level)
    {
        this.level = level;
    }

    public override string ToString()
    {
        return $"Reached level {level}";
    }
}
