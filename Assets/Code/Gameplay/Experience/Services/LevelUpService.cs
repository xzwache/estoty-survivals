using System;
using Code.Infrastructure.UIManagement;
using Code.UI;

namespace Code.Gameplay.Experience.Services
{
	public class LevelUpService : IDisposable
	{
		private readonly IExperienceService _experienceService;
		private readonly IUiService _uiService;

		public LevelUpService(IExperienceService experienceService, IUiService uiService)
		{
			_experienceService = experienceService;
			_uiService = uiService;
			
			_experienceService.LevelUp += HandleLevelUp;
		}

		public void Dispose()
		{
			_experienceService.LevelUp -= HandleLevelUp;
		}

		private void HandleLevelUp(int newLevel)
		{
			var levelUpWindow = _uiService.OpenWindow<LevelUpWindow>();
			levelUpWindow.Setup(newLevel);
		}
	}
} 