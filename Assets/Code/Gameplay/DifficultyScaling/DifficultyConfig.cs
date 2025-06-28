using UnityEngine;

namespace Code.Gameplay.DifficultyScaling
{
	[CreateAssetMenu(fileName = "DifficultyConfig", menuName = Constants.GameName + "/Configs/Difficulty")]
	public class DifficultyConfig : ScriptableObject
	{
		[Header("Difficulty Scaling Settings")]
		[Tooltip("How much enemy HP increases per second")]
		public float HealthScalingPerSecond = 1f;
		
		[Tooltip("How much enemy damage increases per second")]
		public float DamageScalingPerSecond = 0.5f;
		
		[Tooltip("Minimum multiplier for stats")]
		public float MinStatMultiplier = 1f;
		
		[Tooltip("Maximum multiplier for stats")]
		public float MaxStatMultiplier = 10f;
		
		[Tooltip("Time interval for difficulty updates (in seconds)")]
		public float UpdateInterval = 1f;
	}
} 