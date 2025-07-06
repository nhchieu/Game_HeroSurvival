using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("# Player Info")]
    public Vector2 inputVec;
    public float speed = 7f;
    public float maxHealth = 100;
    public float Health;
    public float damegeTaken = 10f;

    [Header("# Player Control")]
    private float horizontal_input;
    private float vertical_input;
    public Scanner scanner;
    private SpriteRenderer spriter;
    private Rigidbody2D rigid;
    private Animator animator;
    
    private bool isRolling;
    private float rollSpeed = 30f;
    private float rollDuration = 0.55f;
    private float rollCooldown = 3f;

    [SerializeField] 
    private bool isAlive => GameManager.instance.isLive;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        Health = maxHealth;
    }

    private void Update()
    {
        
        if (!isAlive || isRolling)
            return;
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("run", inputVec.magnitude);
        

        HandleRoll();
    }
    private void FixedUpdate()
    {
        if (!isAlive || isRolling)
            return;

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    private void LateUpdate()
    {
        if (!isAlive)
            return;

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
    public void HandleRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("roll");
            StartCoroutine(Roll());
        }
    }
    private IEnumerator Roll()
    {
        Vector2 moveDirection = new Vector2(inputVec.x, inputVec.y).normalized;

     
        if (moveDirection == Vector2.zero)
            yield break;

        
        isRolling = true;


        rigid.linearVelocity = moveDirection * rollSpeed;

        yield return new WaitForSeconds(rollDuration);

        rigid.linearVelocity = Vector2.zero;
        isRolling = false;
        yield return new WaitForSeconds(rollCooldown);
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isAlive)
            return;
        if (isRolling)
        {
            Health -= Time.deltaTime * damegeTaken * 0.3f;
        }
        else if (Health > 0)
        {
            Health -= Time.deltaTime * damegeTaken;
        }
        if (Health <= 0){
            for (int i = 2; i < transform.childCount; i++) 
            { 
                transform.GetChild(i).gameObject.SetActive(false);
            }

            animator.SetTrigger("dead");
            GameManager.instance.GameOver();
        }
    }
}
