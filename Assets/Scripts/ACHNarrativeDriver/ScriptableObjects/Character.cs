using System.Collections.Generic;
using UnityEngine;

namespace ACHNarrativeDriver.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewNarrativeSequence", menuName = "ACH Narrative Driver / Character")]
    public class Character : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private List<Sprite> _poses;
        [SerializeField] private Sprite _nameplateSprite;
        [SerializeField] private bool _isFirstPerson;

        public string Name => _name;
        public IReadOnlyList<Sprite> Poses => _poses.AsReadOnly();
        public Sprite NameplateSprite => _nameplateSprite;
        public bool IsFirstPerson => _isFirstPerson;
    }
}
