using Photon.Pun;
using UnityEngine;

public class PlayerAnimationAbility :PlayerAbility
{
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        if (_owner.State == EPlayerState.Death)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _photonView.RPC(nameof(PlayAnimation), RpcTarget.All, "Victory");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _photonView.RPC(nameof(PlayAnimation), RpcTarget.All, "Dance");

        }
    }

    
    
    [PunRPC]
    private void PlayAnimation(string animationName)
    {
        _animator.Play(animationName);
    }
    
}
