using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    bool isMoving;
    Animator animator;
    [SerializeField] LayerMask solidObjLayer;
    [SerializeField] LayerMask encountLayer;

    [SerializeField] Battler battler;
    public UnityAction<Battler> OnEncounts;

    public Battler Battler { get => battler;}

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        battler.Init();
    }
    // Update is called once per frame
    void Update()
    {
        if(isMoving == false)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            if(x != 0)
            {
                y = 0;
            }

            if (x != 0 || y != 0)
            {
                animator.SetFloat("InputX", x);
                animator.SetFloat("InputY", y);
                StartCoroutine(Move(new Vector2(x, y)));

            }

        }
        animator.SetBool("IsMoving", isMoving);
        
    }

    IEnumerator Move(Vector3 direction)
    {
        isMoving = true;
        Vector3 targetPos = transform.position + direction;

        if(IsWalkable(targetPos) == false)
        {
            isMoving = false;
            yield break;
        }

        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f*Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        //敵に合うか調べる
        CheckForEncounts();
    }

    void CheckForEncounts()
    {
        Collider2D encount = Physics2D.OverlapCircle(transform.position, 0.2f, encountLayer);
        if (encount)
        {
            if (Random.Range(0, 100) < 20)
            {
                Battler enemy = encount.GetComponent<EncountArea>().GetRandomBattler();
                OnEncounts?.Invoke(enemy);
            }
        }
        
    }



    bool IsWalkable(Vector3 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.2f, solidObjLayer) == false;
    }

}
