using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//　どのレベルでどの技を覚えるのかを対応させる
[System.Serializable]
public class LeanableMove
{
    [SerializeField] MoveBase moveBase;

    public MoveBase MoveBase { get => moveBase;}
    public int Level { get => level;}

    [SerializeField] int level;

}
