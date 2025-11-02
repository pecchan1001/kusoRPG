using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleUnit : MonoBehaviour
{
    public Battler Battler { get; set; }

    public　virtual void Setup(Battler battler)
    {
        Battler = battler;
    }
    public virtual void UpdateUI()
    {

    }
}
