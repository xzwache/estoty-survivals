using System;
using Code.Gameplay.Abilities.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
	public class AbilityCard : MonoBehaviour
	{
		[SerializeField] private Text _nameText;
		[SerializeField] private Text _descriptionText;
		[SerializeField] private Image _iconImage;
		[SerializeField] private Image _backgroundImage;
		[SerializeField] private Button _selectButton;
		
		private AbilityConfig _config;

		public event Action<AbilityConfig> OnSelected;

		private void Awake()
		{
			_selectButton.onClick.AddListener(OnSelectClicked);
		}

		private void OnDestroy()
		{
			_selectButton.onClick.RemoveListener(OnSelectClicked);
		}

		public void Setup(AbilityConfig config)
		{
			_config = config;
			
			_nameText.text = config.Name;
			_descriptionText.text = config.Description;
			_iconImage.sprite = config.Icon;
			_backgroundImage.color = config.CardBackgroundColor;
		}

		private void OnSelectClicked()
		{
			OnSelected?.Invoke(_config);
		}
	}
} 