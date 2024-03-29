using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class BanditTaskAttack : MyNode
{

    private LayerMask _playerLayerMask;

    private Animator anim;
    private AnimatorStateInfo info;
    private Transform _transform;
    private Transform _lightAttackPoint;

    EnemyCombat enemyCombat;

    private float lightAttackRadius = 0.2f;

    private int attackDamage = 15;

    private float attackCooldown = 1f;
    private float attackTimer = 0;
    
    public BanditTaskAttack(Transform transform, Transform lightAttackPoint, LayerMask playerLayerMask)
    {
        anim = transform.GetComponent<Animator>();
        _transform = transform;
        _lightAttackPoint = lightAttackPoint;
        _playerLayerMask = playerLayerMask;

        enemyCombat = _transform.GetComponent<EnemyCombat>();
    }

    public override NodeState Evaluate()
    {

        info = anim.GetCurrentAnimatorStateInfo(0);

        /*
        if (!info.IsName("Bandit_Block"))
        {
            Debug.Log("True");
            enemyCombat.canBeAttacked = true;
        }
        */

        if (!info.IsName("Bandit_Light_Attack") && !info.IsName("Bandit_Block"))
        {
            anim.SetTrigger("attack");
            //anim.SetBool("isBlocking", false);
            anim.SetBool("isRunning", false);
        }

        Transform target = (Transform)GetData("target");

        attackTimer += Time.deltaTime;

        //if(attackTimer >= attackCooldown)
        if(info.IsName("Bandit_Light_Attack") && info.normalizedTime % 1 > 0.9)
        {
            attackTimer = 0;
            //Debug.Log("Attacking Player");

            Collider2D player = Physics2D.OverlapCircle(_lightAttackPoint.position, lightAttackRadius, _playerLayerMask);

            //Debug.Log(player.name);

            if(player != null)
            {
                player.GetComponent<Player>().TakeDamage(attackDamage);
            }

            /*

            RaycastHit2D player = Physics2D.Raycast(_transform.position, _transform.right * BanditBT.direction, 0.5f, _playerLayerMask);

            if(player.collider != null)
            {
                player.collider.GetComponent<PlayerMovement>().TakeDamage(25);
            }
            */
        }

        state = NodeState.RUNNING;
        return state;
    }

}
