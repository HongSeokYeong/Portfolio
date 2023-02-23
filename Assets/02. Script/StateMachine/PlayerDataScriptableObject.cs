using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Custom/Player")]
public class PlayerDataScriptableObject : ScriptableObject
{
    public PlayerGroundedData groundedData;

    public PlayerAirborneData airborneData;
}
