using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private int dir_H;
    private float faceDir;
    private bool isChange = false;
    private float hp = 3;
    private bool isGetDamge = false;

    private Rigidbody2D rb;
    private Phy phy;
    private GameObject drops;

    [SerializeField] public CharterView face;
    [SerializeField] public GameObject drop;
    void Start()
    {
        dir_H = Random.Range(0, 2) * 2 - 1;
        faceDir = dir_H;
        face.FaceMove(faceDir);

        rb = GetComponent<Rigidbody2D>();
        phy = GetComponent<Phy>();
        drops = GameObject.FindWithTag("Drops");
    }

    private void FixedUpdate()
    {
        ChangeDir();
        Move();
    }

    private void ChangeDir()
    {
        if (!isChange && (phy.IsOnFloor() != 2 || phy.isOnWall() != 0))
        {
            dir_H = -1 * dir_H;
            StartCoroutine(onFaceDirChange());
            StartCoroutine(onChange());
        }
        //rb.velocity = new Vector2(dir_H * 2, rb.velocity.y);
    }

    private void Move()
    {
        if (phy.IsOnFloor() != 0 && !isGetDamge) { rb.velocity = new Vector2(dir_H * 2, rb.velocity.y); }
        else if(!isGetDamge) { rb.velocity = new Vector2(0, rb.velocity.y); }
    }
    private IEnumerator onChange()
    {
        isChange = true;
        yield return new WaitForSeconds(0.3f);
        isChange = false;
    }
    private IEnumerator onFaceDirChange()
    {
        float rate = 0;
        while (faceDir != dir_H)
        {
            faceDir = Mathf.Lerp(faceDir, dir_H, rate);
            rate += 0.05f;
            
            face.FaceMove(faceDir);
            yield return null;
        }
    }


    public void GetDamge(float damgeHp, float dir)
    {
        hp -= damgeHp;
        StartCoroutine(DamgeCoolDown());
        rb.AddForce(new Vector2(5, 0) * -1 * dir + new Vector2(0, 2), ForceMode2D.Impulse);
        if (hp <= 0)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            GameObject obj = GameObject.Instantiate(drop, drops.transform);
 
            obj.SetActive(false);
            obj.gameObject.GetComponent<DropsController>().id = DropsID();
            obj.gameObject.GetComponent<DropsController>().amount = DropAmount();
            obj.transform.position = this.transform.position;

            IEnumerator activeDelay()
            {
                yield return new WaitForSeconds(0.3f);
                obj.SetActive(true);
                GameObject.Destroy(this.gameObject);
            }

            StartCoroutine(activeDelay());

            
        }
    }
    private IEnumerator DamgeCoolDown()
    {
        isGetDamge = true;
        yield return new WaitForSeconds(0.3f);
        isGetDamge = false;
    }

    private int DropsID() 
    {
        // 获取当前解锁的技能列表（索引及权重）
        List<int> unlockedIds = new List<int>();
        for (int i = 0; i < CharterModel.Instance.SkillList.Count; i++)
        {
            if (CharterModel.Instance.SkillList[i])
                unlockedIds.Add(i);
        }
        int n = unlockedIds.Count; // 已解锁技能数量（2~5）

        // 定义初始权重（n=2时）和最终权重（n=5时）
        float[] initialWeight = new float[5] { 7f, 3f, 0f, 0f, 0f };
        float finalWeight = 1f; // 最终每个技能权重1（归一化后即20%）

        // 计算当前每个技能的权重（仅对解锁技能有效）
        float[] weights = new float[5];
        for (int i = 0; i < 5; i++)
        {
            if (CharterModel.Instance.SkillList[i])
            {
                // 线性插值：t = (n - 2) / (5 - 2)
                float t = (n - 2f) / 3f;
                weights[i] = initialWeight[i] * (1 - t) + finalWeight * t;
            }
            else
            {
                weights[i] = 0;
            }
        }

        // 归一化并随机选择
        float total = 0;
        foreach (float w in weights) total += w;
        float rand = Random.Range(0f, total);
        float accum = 0;
        for (int i = 0; i < 5; i++)
        {
            if (weights[i] > 0)
            {
                accum += weights[i];
                if (rand <= accum)
                    return i;
            }
        }
        return unlockedIds[0]; // fallback
    }
    private int DropAmount()
    {
        return Random.Range(5, 10);
    }
}
