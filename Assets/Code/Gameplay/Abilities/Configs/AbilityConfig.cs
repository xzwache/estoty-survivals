using UnityEngine;

namespace Code.Gameplay.Abilities.Configs
{
    [CreateAssetMenu(fileName = "AbilityConfig", menuName = Constants.GameName + "/Configs/Ability")]
    public class AbilityConfig : ScriptableObject
    {
        [Header("Basic Info")]
        public AbilityId Id;

        public string Name;

        [TextArea(3, 5)]
        public string Description;

        public Sprite Icon;

        [Header("Mechanics")]
        public AbilityStackType StackType = AbilityStackType.Single;

        public float Value = 1f;

        [Header("UI Presentation")]
        public Color CardBackgroundColor = Color.white;
    }
}
