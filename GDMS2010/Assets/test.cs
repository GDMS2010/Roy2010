using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : StateMachineBehaviour
{

    RaycastHit hit;
    RaycastHit[] hits;
    public LayerMask ignoreLayer;
    Transform t;
    BoxCollider c;
    Script_Health h;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //It's a prototype class I can hack stuff
        if (!t)
         t = animator.transform.parent.transform;
        if (!c)
         c = t.gameObject.GetComponent<BoxCollider>();

        //if (Physics.SphereCast(t.position, 3f, t.forward, out hit))
        //{
        //
        //}
        //if (Physics.Raycast(t.position, t.forward, out hit, 3, ~ignoreLayer))
        //{
        //    if (hit.transform.tag == "Player")
        //    {
        //        Debug.Log("Enemy has hit");
        //        if (!h)
        //            h = hit.transform.GetComponent<Script_Health>();
        //        h.takeDamage(30);
        //    }
        //}

        hits = Physics.SphereCastAll(t.position, 3f, t.forward, 2.1f);
        foreach (RaycastHit ishit in hits)
        {
            if (ishit.transform.tag == "Player")
            {
                Debug.Log("Enemy has hit");
                if (!h)
                    h = ishit.transform.GetComponent<Script_Health>();
                h.takeDamage(5);
            }
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
