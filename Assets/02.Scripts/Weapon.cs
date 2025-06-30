using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerAttackAbility _attackAbility;

    private void Start()
    {
        _attackAbility = GetComponentInParent<PlayerAttackAbility>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // 자기 자신과 부딛혔다면 아무고또 안한다.
        if (other.transform == _attackAbility.transform)
        {
            return;
        }
        
        Debug.Log("음냐냐");
        
        // IDamaged 인터페이스를 구현하고 있는지 확인
        IDamaged damagedObject = other.GetComponent<IDamaged>();
        if (damagedObject != null)
        {
            _attackAbility.Hit(damagedObject);
        }
    }
}
