using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private PlayerEquipment equip;
    private float movementSpeed = 12;
    private float jumpPower = 20;
    private float maxFallSpeed = 20;
    private bool grounded;
    private bool usedDoubleJump;

    public float dashForce = 10f;
    private float currentDashTime;
    private float dashDirection;
    private bool isDashing;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        equip = GetComponent<PlayerEquipment>();
    }
    void Update() {
        // Grounding
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1, LayerMask.GetMask("Geometry"));
        Debug.DrawRay(transform.position, -Vector2.up, Color.red);

        if (hit.collider != null) {
            grounded = true;
            usedDoubleJump = false;
        } else {
            grounded = false;
        }


        float targetHorizontalV = Input.GetAxis("Horizontal") * movementSpeed;
        // Digital input doesn't have any smoothing, so this lerp smooths out the horizontal acceleration based on the target velocity
        float horizontalV = Mathf.Lerp(rb.velocity.x, targetHorizontalV, Mathf.Abs(rb.velocity.x) < Mathf.Abs(targetHorizontalV) ? 0.05f : 0.4f);
        float verticalV = rb.velocity.y;

        // Jump Handling
        if(Input.GetKeyDown(KeyCode.Space) && (grounded || (!usedDoubleJump && equip.GetPowerupState(PowerUps.DoubleJump)))) {
            verticalV = jumpPower;
            if (!grounded && !usedDoubleJump) { usedDoubleJump = true; }
        }
        
        //code for dashing
        if (Input.GetKeyDown(KeyCode.Z) && horizontalV != 0 && equip.GetPowerupState(PowerUps.Dash)){
            isDashing = true;
            rb.velocity = Vector2.zero;
            dashDirection = (int)horizontalV;

            if (isDashing)
            {
                horizontalV = dashDirection * dashForce;
                currentDashTime -= Time.deltaTime;

                if (currentDashTime <= 0)
                {
                    isDashing = false;
                }
            }
        }

        rb.velocity = new Vector3(horizontalV, Mathf.Clamp(verticalV, -maxFallSpeed, float.MaxValue), 0);
    }
}
