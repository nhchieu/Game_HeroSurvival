using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("# Enemy Info")]
    public float speed;
    public float health;
    public float maxHealth;

    [Header("# Enemy control")]
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    [SerializeField] public bool islive;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator animator;
    WaitForSeconds wait = new WaitForSeconds(1);
    Collider2D coll;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        Vector2 dirVec = target.position - rigid.position;//vector chỉ hướng từ enemy đến player

        Vector2 nextVec = dirVec.normalized * speed * Time.deltaTime;

        rigid.MovePosition(rigid.position + nextVec);

        if (!islive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }

    }
    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        islive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        animator.SetBool("Dead", false);
        health = maxHealth;

    }

    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !islive)
        {
            return;
        }
        StartCoroutine(KnockBack());

        health -= collision.GetComponent<Bullet>().damage;
        //HUD
        GameManager.instance.Spawner.Hp-= collision.GetComponent<Bullet>().damage;

        if (health > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            islive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            animator.SetBool("Dead", true);
            StartCoroutine(Dead());
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            AudioManager.instance.sfx(1);
        }
    }
    IEnumerator KnockBack()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        transform.Translate(dirVec.normalized * 0.2f);
        yield return wait;
    }
    IEnumerator Dead()
    {
        yield return wait;
        gameObject.SetActive(false);

    }
}

