using UnityEngine;

public class GroundSlamState : PlayerLocomState
{
    private float _startTime;
    
    public GroundSlamState(LocomotionController locomotionController, Rigidbody rb) : base(locomotionController, rb) { }

    public override void Enter()
    {
        base.Enter();
        AudioManager.Instance.PlayGlobalSFX(player.midairSFX, loop: true);
        _startTime = Time.time;
        Physics.IgnoreLayerCollision(player.gameObject.layer, LayerMask.NameToLayer("Damageable"), true);
    }

    public override void Update()
    {
        base.Update();
        
        if (player.GroundCheck.IsGrounded)
        {
            if (PlayerData.Instance.isGroundSlamAttack)
            {
                OnSlamImpact();
                AudioManager.Instance.PlayGlobalSFX(player.slamSFX);
            }
            else
                AudioManager.Instance.PlayGlobalSFX(player.quietSlamSFX, 0.7f);
            player.SwitchState(new GroundedState(player, Rb));
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Rb.linearVelocity = new Vector3(0f, -player.slamSpeed, 0f);
    }

    public override void Exit()
    {
        AudioManager.Instance.StopGlobalSFX();
        Physics.IgnoreLayerCollision(player.gameObject.layer, LayerMask.NameToLayer("Damageable"), false);
    }
    
    private void OnSlamImpact()
    {
        if (!(Time.time - _startTime > player.slamAttackThreshold))
        {
            return;
        }
        
        // player.PlaySlamImpactEffects();
        PlayerDmgDealer.Instance.DealSlamDamage();
    }
}
