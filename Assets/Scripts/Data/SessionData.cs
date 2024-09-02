using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SessionData",menuName = "Data/Session")]
public class SessionData : ScriptableObject
{
    public string nameSession;
    public List<LevelData> levelDatas;
}
