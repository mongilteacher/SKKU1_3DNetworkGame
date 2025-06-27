using UnityEngine;

public class Billboard : MonoBehaviour
{
    // 캔버스에 달아주세요.
    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
