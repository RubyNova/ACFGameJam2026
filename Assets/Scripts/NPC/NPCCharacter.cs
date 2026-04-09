using System;
using System.Linq;
using ACHNarrativeDriver.ScriptableObjects;
using CraftingAPI;
using UnityEngine;

namespace NPC 
{
    [CreateAssetMenu(fileName = "NPCCharacter", menuName = "NPCs/Create New NPC")]
    public class NPCCharacter : ScriptableObject
    {
        public class NPCAppearanceCondition
        {
            public NPCAppearanceConditionType Condition = NPCAppearanceConditionType.None;
            public NPCAppearanceComparisonType ComparisonType = NPCAppearanceComparisonType.EqualTo;
            public float ComparisonValue = 0.0f;
        }


        [Header("Character Configuration")]
        [SerializeField]
        private string _name;

        [SerializeField]
        private Sprite _idleSprite;

        [SerializeField]
        private ItemConfig _desiredItem;

        [Header("Narrative Configuration")]
        [SerializeField]
        private float _delayBeforeStartingDialogue;

        [SerializeField]
        private NarrativeSequence _arrivalSequence;

        [SerializeField]
        private NarrativeSequence _departingSequence;

        [SerializeField]
        private NarrativeSequence[] _negativeSequences = Array.Empty<NarrativeSequence>();

        [Header("Animation Controls")]
        [SerializeField]
        private RuntimeAnimatorController _animator;

        [SerializeField]
        private AnimationClip _spawnInAnimation;

        [SerializeField]
        private NPCAppearanceCondition _conditionForAppearing;

        [SerializeField]
        private int _negativeInteractionsBeforeLeaving = 5;


        public string Name => _name;
        public Sprite IdleSprite => _idleSprite;
        public NarrativeSequence ArrivalSequence => _arrivalSequence;
        public NarrativeSequence DepartingSequence => _departingSequence;
        public NarrativeSequence[] NegativeSequences => _negativeSequences;
        public int NegativeSequenceCount => _negativeSequences.Count();
        public float DelayBeforeStartingDialogue => _delayBeforeStartingDialogue;
        public RuntimeAnimatorController AnimController => _animator;
        public AnimationClip SpawnInAnimationClip => _spawnInAnimation;
        public ItemConfig DesiredItem => _desiredItem;
        public NPCAppearanceCondition ConditionForAppearing => _conditionForAppearing;
        public int NegativeInteractionsBeforeLeavingUnhappy => _negativeInteractionsBeforeLeaving;
    }

}