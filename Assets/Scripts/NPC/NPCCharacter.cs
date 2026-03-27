using ACHNarrativeDriver.ScriptableObjects;
using CraftingAPI;
using UnityEngine;

namespace NPC 
{
    [CreateAssetMenu(fileName = "NPCCharacter", menuName = "NPCs/Create New NPC")]
    public class NPCCharacter : ScriptableObject
    {
        [SerializeField]
        private string _name;

        [SerializeField]
        private Sprite _idleSprite;

        [SerializeField]
        private NarrativeSequence _arrivalSequence;

        [SerializeField]
        private NarrativeSequence _departingSequence;

        [SerializeField]
        private float _delayBeforeStartingDialogue;

        [SerializeField]
        private RuntimeAnimatorController _animator;

        [SerializeField]
        private AnimationClip _spawnInAnimation;

        [SerializeField]
        private ItemConfig _desiredItem;

        public string Name => _name;
        public Sprite IdleSprite => _idleSprite;
        public NarrativeSequence ArrivalSequence => _arrivalSequence;
        public NarrativeSequence DepartingSequence => _departingSequence;
        public float DelayBeforeStartingDialogue => _delayBeforeStartingDialogue;
        public RuntimeAnimatorController AnimController => _animator;
        public AnimationClip SpawnInAnimationClip => _spawnInAnimation;

        public ItemConfig DesiredItem => _desiredItem;
    }

}