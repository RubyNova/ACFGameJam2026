using UnityEngine;

namespace NPC
{
    public class NPCDeletionAnimationBehaviour : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        var npc = animator.GetComponent<NPCController>();
        npc?.CharacterGoneEvent.Invoke();
        npc?.DeleteSelf();
        }
    }
}