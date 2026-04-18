using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    float dir_h = 0;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        dir_h = Input.GetAxis("Horizontal");
        HorizontalMove();
        Debug.Log(rb.velocity);
    }
    private void Jump()
    {
        rb.AddForce(Vector2.up*300);
    }
    private void HorizontalMove()
    {
        if(math.abs(rb.velocity.x) < 5 && dir_h!=0)
        {
            rb.velocity += (new Vector2(1, 0)*dir_h*2);
        }
    }
}
