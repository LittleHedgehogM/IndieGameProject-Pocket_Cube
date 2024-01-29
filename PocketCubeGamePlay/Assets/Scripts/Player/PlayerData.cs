using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int level;
    public int tutorials;
    public string version;

    public PlayerData(int level, int tutorials, string version)
    {
        this.level = level;
        this.tutorials = tutorials;
        this.version = version;
    }

    public override string ToString()
    {
        return $"Reached level {level}";
    }


}
