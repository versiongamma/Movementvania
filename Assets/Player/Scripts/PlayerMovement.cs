using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Rigidbody2D rb;
    private float movementSpeed = 12;
    private float jumpPower = 25;
    private float maxFallSpeed = 20;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
                                  
        float targetHorizontalV = Input.GetAxis("Horizontal") * movementSpeed;
                                  // Digital input doesn't have any smoothing, so this lerp smooths out  
                                  // the horizontal acceleration based on the target velocity
        float horizontalV = Mathf.Lerp(rb.velocity.x, targetHorizontalV, Mathf.Abs(rb.velocity.x) < Mathf.Abs(targetHorizontalV) ? 0.05f : 0.2f);
        float verticalV = rb.velocity.y;

        // Needs grounding later
        if(Input.GetKeyDown(KeyCode.Space)) {
            verticalV = jumpPower;
        }

        rb.velocity = new Vector3(horizontalV, Mathf.Clamp(verticalV, -maxFallSpeed, float.MaxValue), 0);
    }
}