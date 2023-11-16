using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int level;
    public int tutorials;

    public PlayerData(int level, int tutorials)
    {
        this.level = level;
        this.tutorials = tutorials;
    }

    public override string ToString()
    {
        return $"Reached level {level}";
    }


}
