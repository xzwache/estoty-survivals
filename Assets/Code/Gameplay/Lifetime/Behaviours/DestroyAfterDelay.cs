using UnityEngine;

namespace Code.Gameplay.Lifetime.Behaviours
{
	public class DestroyAfterDelay : MonoBehaviour
	{
		[SerializeField] private float _delay = 5f;

		private void Start()
		{
			Destroy(gameObject, _delay);
		}
	}
} 