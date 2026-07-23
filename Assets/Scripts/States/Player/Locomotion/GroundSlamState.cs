using UnityEngine;

public class GroundSlamState : PlayerLocomState
{
    public GroundSlamState(LocomotionController locomotionController, Rigidbody rb) : base(locomotionController, rb) { }

    public override void Update()
    {
        base.Update();

        if (player.GroundCheck.IsGrounded)
        {
            // OnSlamImpact();
            player.SwitchState(new GroundedState(player, Rb));
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Rb.linearVelocity = new Vector3(0f, -player.slamSpeed, 0f);
    }
    
    /*private void OnSlamImpact()
     {
        // Add necessary effects here
     }*/
}
