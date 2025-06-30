using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
   [Header("이동")]
   public float MoveSpeed     = 7f;
   public float RunSpeed      = 12f;
   public float JumpPower     = 2.5f;
   public float RotationSpeed = 200f;
   
   [Header("공격")]
   public float AttackSpeed   = 1.2f;    // 초당 1.2번 공격할 수 있다.

   [Header("스태미너")] 
   public float MaxStamina        = 100;
   public float Stamina           = 100;
   public float StaminaRecovery    = 20;
   public float StaminaJumpCost   = 20;
   public float StaminaAttackCost = 10;
   public float StaminaRunCost    = 10;

}
