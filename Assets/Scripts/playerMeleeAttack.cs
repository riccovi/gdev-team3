using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMeleeAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRadius;
    private float timeBetweenAttack;
    public float startTimeBtwAttack;
    public LayerMask MaskDestractable;
    public int damage;

    public SpriteRenderer weapon;

    public Wrench checkWrenchStatus;

    public playerMovement player;




    // Start is called before the first frame update
    void Start()
    {
        player = transform.GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.currentState==GameManager.gameStatus.Run)
        {
            AttackControllers();
        }       

    }

    private void AttackControllers()
    {

        //You can attack when you are on air, to evada bugs

        if(player.isGrounded)
        {
            if (!checkWrenchStatus.isClicked && !checkWrenchStatus.CanCallBack)
            {
                if (timeBetweenAttack <= 0)
                {
                    weapon.color = Color.white;
                    //Debug.Log("Can Attack");
                    if (Input.GetMouseButtonDown(0))
                    {
                        player.Attack();

                        //animation delay
                        Invoke("DoDamage",0.2f);

                    }
                }
                else
                {
                    timeBetweenAttack -= Time.deltaTime;
                }

            }

        }
        
    }

    private void DoDamage()
    {
        weapon.color = Color.red;
        //Debug.Log("Press Attack");
        Collider2D[] destractable = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, MaskDestractable);

        for (int i = 0; i < destractable.Length; i++)
        {
            Debug.Log(destractable[i].name);
            destractable[i].GetComponent<Destractable>().doDamage(damage);

        }
        player.attackAnim=false;
        timeBetweenAttack = startTimeBtwAttack;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(attackPoint.position,attackRadius);
    }
}
