using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
   public float MoveSpeed     = 7f;
   public float JumpPower     = 2.5f;
   public float RotationSpeed = 200f;
   public float AttackSpeed   = 1.2f;    // 초당 1.2번 공격할 수 있다.
}
