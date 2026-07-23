using UnityEngine;

public class GroundedState : PlayerLocomState
{
    public GroundedState(LocomotionController player, Rigidbody rb) : base(player, rb)
    { }
    
    public override void Update()
    {
        base.Update();
        if (!player.GroundCheck.IsGrounded)
        {
            player.SwitchState(new AirborneState(player, Rb));
            return;
        }
        
        if(player.input.JumpPressed)
        {
            player.Jump();
            return;
        }

        if (player.input.SlidePressed)
        {
            player.SwitchState(new SlideState(player, Rb));
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        var dir = GetCameraRelativeMoveDirection();
        
        if (dir.sqrMagnitude > 0.01f)
        {
            Rb.linearVelocity = new Vector3(dir.x * player.moveSpeed, Rb.linearVelocity.y, dir.z * player.moveSpeed);
        }
        else
        {
            Rb.linearVelocity = new Vector3(0f, Rb.linearVelocity.y, 0f);
        }
    }
}
