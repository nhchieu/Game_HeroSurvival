using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class FinalBoss : MonoBehaviour
{
    public float health = 5000f;
    public float speed = 15f;
    bool isLive = true;
    bool isfalling = false;
    bool isfire = false;
    public Rigidbody2D target;
    SpriteRenderer spriter;
    Animator animator;
    Rigidbody2D rigid;
    Collider2D coll;
    Scanner scanner;
    Transform nearestTarget;
   
    public GameObject HealthBar;
    public GameObject FallObject;
    public Rigidbody2D SlimeBall;
    public Rigidbody2D RigidFallObject;
    Vector2 playerPos;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        scanner = GetComponent<Scanner>();
        


    }
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        
    }
    private void Start()
    {
        UpdateNearestTarget();
        if (nearestTarget != null)
            playerPos = nearestTarget.position;
    }
    void Update()
    {
        if (!GameManager.instance.isLive || !isLive)
            return;
        

    }
    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive || !isLive)
            return;
        UpdateNearestTarget();
        if (nearestTarget != null)
            playerPos = nearestTarget.position;

        Vector2 dirVec = playerPos - rigid.position;

        Vector2 nextVec = dirVec.normalized * speed * Time.deltaTime;

        rigid.MovePosition(rigid.position + nextVec);

        if (health <= 2500f)
        {
            speed = 7f;
            animator.SetFloat("bossJump", dirVec.magnitude);
            if (isfalling)
                return;
            isfalling = true;
            FallObject.SetActive(true);
            Vector2 spawnPosFalling = playerPos + Vector2.up * 10f;
            Debug.Log("Spawn Position: " + spawnPosFalling);
            FallObject.transform.position = spawnPosFalling;
            StartCoroutine(Falling());

        }

        if (health <= 1500f)
        {
            animator.SetFloat("bossJump", 1);
            if (isfire)
                return;
            isfire = true;

            transform.position = new Vector2(9983, 10020);
            rigid.linearVelocity = Vector2.zero;
            Vector2 playerDir = playerPos - rigid.position;


        }


    }
    private void UpdateNearestTarget()
    {
        if (scanner != null && scanner.nearestTarget != null)
            nearestTarget = scanner.nearestTarget;
    }
    IEnumerator Falling()
    {
        yield return new WaitForSeconds(1f);
        RigidFallObject.gravityScale = 2f;
        yield return new WaitForSeconds(2f);
        FallObject.SetActive(false);
        isfalling = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
        {
            return;
        }


        health -= collision.GetComponent<Bullet>().damage;
        //HUD
        GameManager.instance.Spawner.Hp -= collision.GetComponent<Bullet>().damage;

        if (health > 0)
        {
            animator.SetTrigger("bosshit");
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            animator.SetBool("bossdead", true);
            StartCoroutine(Dead());
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            AudioManager.instance.sfx(1);
        }
    }
    IEnumerator Dead()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        GameManager.instance.GameWin();

    }
}
