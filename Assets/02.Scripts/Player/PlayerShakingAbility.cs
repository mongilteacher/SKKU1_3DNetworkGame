using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShakingAbility : PlayerAbility
{
    // 무엇을 어떤 힘으로 몇초동안 흔들것인가
    public Transform Target;
    public float Strength;
    public float Duration;

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(Shake_Coroutine());
    }

    private IEnumerator Shake_Coroutine()
    {
        float elapsedTime = 0f;

        // 원위치 저장
        Vector3 startPosition = Target.localPosition;

        while (elapsedTime <= Duration)
        {
            elapsedTime += Time.deltaTime;
            
            // 흔들어 재낀다음
            Vector3 randomPosition = Random.insideUnitSphere.normalized * Strength; 
            randomPosition.y = startPosition.y;
            Target.localPosition = randomPosition;
            
            yield return null;
        }
        
        // 원위치로
        Target.localPosition = startPosition;
    }

}
