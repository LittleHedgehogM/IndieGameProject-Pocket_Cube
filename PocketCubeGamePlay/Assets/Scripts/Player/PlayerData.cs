using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int level;
    public int tutorials;
    public string version;
    public string preVersion;

    public PlayerData(int level, int tutorials, string version, string preVersion)
    {
        this.level = level;
        this.tutorials = tutorials;
        this.version = version;
        this.preVersion = preVersion;
    }

    public override string ToString()
    {
        return $"Reached level {level}";
    }


}
