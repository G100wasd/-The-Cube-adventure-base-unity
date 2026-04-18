using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using Unity.Mathematics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.XR;
#endif

public class CharterController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public CharterView charterView;
    [SerializeField] public CharterModel charterModel;
    [SerializeField] public GameObject Bullet; 

    public bool isDashed = false;
    public bool isGetDamge = false;
    public bool isHit = false;
    public bool isShoot = false;
    private float dir_H = 0;
    Vector2 velocity = Vector2.zero;

    Rigidbody2D rb;
    Phy physices;
    GameObject bullets;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        physices = GetComponent<Phy>();

        charterModel.onHpChanged += charterView.UpdateHpView;

        bullets =  GameObject.FindWithTag("Bullets");
        //BagManager.Instance.Add(0, 16);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {Jump();}
        if (Input.GetKeyDown(KeyCode.Alpha1)) { Skill_Dash(); }
        else if(Input.GetKeyDown(KeyCode.Alpha2)) { Skill_Restore(); }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { Skill_Jump(); }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { Skill_Hit(); }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) { Skill_Shoot(); }

            dir_H = Input.GetAxis("Horizontal");
    }
    private void FixedUpdate()
    {
        Move_h();
    }

    private void Move_h()
    {
        charterView.FaceMove(dir_H);
        if (!isDashed && !isGetDamge)
        { rb.velocity = new Vector2(dir_H * 5, rb.velocity.y);}
        if (physices.IsOnFloor() == 0 && ((physices.isOnWall() * dir_H) > 0 || physices.isOnWall() ==2)) { rb.velocity = new Vector2(0, rb.velocity.y); }
    }
    private void Jump()
    {
        if (physices.IsOnFloor() != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * 25, ForceMode2D.Impulse);
        }
    }
    private void Skill_Dash()
    {
        
        int index = BagManager.Instance.GetIndex(0);
        if (isME() || index != -1) 
        {
            if (!isME()) { BagManager.Instance.Decrase(index); }
            BagView.Instance.BagUpdate();
            if (physices.isOnWall() * dir_H <= 0 && dir_H != 0)
            {
                rb.AddForce(new Vector2(dir_H, 0) * 75, ForceMode2D.Impulse);
                StartCoroutine(DashCollDown());
            }
        }

    }
    private void Skill_Restore()
    {
        int index = BagManager.Instance.GetIndex(1);
        if (isME() || index != -1)
        {
            if (charterModel.hp < charterModel._maxHp)
            {
                if (!isME()) { BagManager.Instance.Decrase(index); }
                charterModel.hp += 1;
                charterView.UpdateHpView(charterModel.hp);
            }
            BagView.Instance.BagUpdate();
        }
    }
    private void Skill_Jump()
    {
        int index = BagManager.Instance.GetIndex(2);
        if ((isME() || index != -1) && physices.IsOnFloor()!= 0)
        {
            if (!isME()) { BagManager.Instance.Decrase(index); }
            rb.AddForce(Vector2.up * 35, ForceMode2D.Impulse);
            BagView.Instance.BagUpdate();
        }
    }
    private void Skill_Hit()
    {
        int index = BagManager.Instance.GetIndex(3);
        if((isME() || index != -1) && physices.IsOnFloor() != 0 && !isHit)
        {
            if(!isME()) { BagManager.Instance.Decrase(index); }
            StartCoroutine(HitCoolDown());
            BagView.Instance.BagUpdate();
            IEnumerator HitCoolDown()
            {
                charterModel.hit = 5;
                isHit = true;
                Debug.Log($"hit{charterModel.hit} ");
                yield return new WaitForSeconds(5.0f);
                charterModel.hit = 2;
                isHit = false;
            }
        }
    }
    private void Skill_Shoot()
    {
        int index = BagManager.Instance.GetIndex(4);
        if((isME() || index != -1) && !isShoot)
        {
            if (!isME()) { BagManager.Instance.Decrase(index);}
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (mousePos - this.transform.position).normalized;
            GameObject obj = GameObject.Instantiate(Bullet, bullets.transform);
            Bullet busp = obj.GetComponent<Bullet>();
            obj.transform.position = this.transform.position;
            busp.Move(dir);
            BagView.Instance.BagUpdate();

        }
    }
    private bool isME()
    {
        int index = BagManager.Instance.GetIndex(5);
        return (index != -1);
    }
    private IEnumerator DashCollDown()
    {
        isDashed = true;
        yield return new WaitForSeconds(0.30f);
        isDashed = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "enemy")
        {
            Rigidbody2D rbEn = collision.gameObject.GetComponent<Rigidbody2D>();
            float LR = rb.position.x - rbEn.position.x;
            LR = LR / Mathf.Abs(LR);
            if (isDashed)
            {
                EnemyController ec = collision.gameObject.GetComponent<EnemyController>();
                ec.GetDamge(charterModel.hit, LR);
            }
            else if(!isDashed)
            {
                getDamge(1, LR);
            }
        }
    }

    public void getDamge(int damHp, float LR)
    {
        charterModel.ChangeHp(damHp, false);
        rb.AddForce(new Vector2(45, 10) * LR, ForceMode2D.Impulse);
        Debug.Log($"HP : {charterModel.hp}");
        StartCoroutine(DamgeCoolDown());
    }

    public IEnumerator DamgeCoolDown()
    {
        isGetDamge = true;
        yield return new WaitForSeconds(0.2f);
        isGetDamge = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "skillCollider")
        {
            Debug.Log("Unlock");
            int index = Convert.ToInt16(collision.name);
            charterModel.SkillList[index] = true;
            GameObject.Destroy(collision.gameObject);
        }
    }
}
