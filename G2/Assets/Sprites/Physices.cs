using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Phy : MonoBehaviour
{
    const float g = -9.8f;
    Rigidbody2D rb;
    Vector2 Pos;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int jumpForce = 1;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0;
    }
    private void FixedUpdate()
    {
        Graivity(Time.fixedDeltaTime);
        Pos = this.transform.position;
    }
    public void Graivity(float t)
    {
        bool isOnFloor = (IsOnFloor() != 0);
        if (!isOnFloor && rb.velocity.y > -100)
        {
            rb.velocity += Vector2.up * g * t;
        }
        else if (isOnFloor && rb.velocity.y < 0)
        { 
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }


    public bool isOnLeftFloor()
    {
        RaycastHit2D hitFloorLeft = Physics2D.Raycast(Pos + (new Vector2(-0.40f, 0)), Vector2.down, 0.50f, groundLayer);
        if(hitFloorLeft.collider != null) { return true; }
        return false;
    }
    public bool isOnRightFloor()
    {
        RaycastHit2D hitFloorRight = Physics2D.Raycast(Pos + (new Vector2(0.40f, 0)), Vector2.down, 0.50f, groundLayer);
        if(hitFloorRight.collider != null) { return true; }
        return false;
    }
    public bool isOnLeftWall()
    {
        RaycastHit2D hitLeftTop = Physics2D.Raycast(Pos + (new Vector2(0, 0.40f)), Vector2.left, 0.50f, groundLayer);
        RaycastHit2D hitLeftBottom = Physics2D.Raycast(Pos + (new Vector2(0, -0.40f)), Vector2.left, 0.50f, groundLayer);
        if (hitLeftTop.collider != null || hitLeftBottom.collider != null ) { return true; }
        else { return false; }
    }

    public bool isOnRightWall()
    {
        RaycastHit2D hitRightTop = Physics2D.Raycast(Pos + (new Vector2(0, 0.40f)), Vector2.right, 0.50f, groundLayer);
        RaycastHit2D hitRightBottom = Physics2D.Raycast(Pos + (new Vector2(0, -0.40f)), Vector2.right, 0.50f, groundLayer);
        if(hitRightTop.collider != null || hitRightBottom.collider) { return true; }
        return false;
    }

    public int IsOnFloor()
    {
        if (isOnLeftFloor() && isOnRightFloor()) { return 2; }
        else if (isOnLeftFloor()) { return -1; }
        else if (isOnRightFloor()) { return 1;} 
        return 0;
    }
    public int isOnWall() {
        if (isOnLeftWall() && isOnRightWall()) { return 2; }
        else if (isOnLeftWall()) { return -1; }
        else if (isOnRightWall()) { return 1; }
        return 0;
    }


}
