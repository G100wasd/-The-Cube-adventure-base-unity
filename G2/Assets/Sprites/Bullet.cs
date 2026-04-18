using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public void Move(Vector2 dir)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = dir.normalized * 50;
        StartCoroutine(BulletDead());
        IEnumerator BulletDead()
        {
            yield return new WaitForSeconds(2);
            GameObject.Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            EnemyController ec = collision.gameObject.GetComponent<EnemyController>();
            Rigidbody2D rbEn = collision.gameObject.GetComponent<Rigidbody2D>();
            float LR = rb.position.x - rbEn.position.x;
            LR = LR / Mathf.Abs(LR);
            ec.GetDamge(1, LR);
            GameObject.Destroy(this.gameObject);
        }
        else if(collision.gameObject.tag == "Map")
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
