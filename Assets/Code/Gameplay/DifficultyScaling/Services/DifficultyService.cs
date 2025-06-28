using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Code.Infrastructure.ConfigsManagement;
using UnityEngine;

namespace Code.Gameplay.DifficultyScaling.Services
{
	public class DifficultyService : IDifficultyService, IDisposable
	{
		private readonly IConfigsService _configsService;
		private DifficultyConfig _config;
		private CancellationTokenSource _cancellationTokenSource;
		
		private float _updateInterval;
		
		public float CurrentHealthMultiplier { get; private set; } = 1f;
		public float CurrentDamageMultiplier { get; private set; } = 1f;
		
		public event Action<float, float> OnDifficultyChanged;

		public DifficultyService(IConfigsService configsService)
		{
			_configsService = configsService;
		}

		public void StartScaling()
		{
			_config = _configsService.GetDifficultyConfig();
			_cancellationTokenSource = new CancellationTokenSource();
			
			ScalingLoop(_cancellationTokenSource.Token).Forget();
		}

		public void StopScaling()
		{
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = null;
		}

		private async UniTaskVoid ScalingLoop(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				var timeSpawn = TimeSpan.FromSeconds(_config.UpdateInterval);
				await UniTask.Delay(timeSpawn, cancellationToken: cancellationToken);
				
				if (cancellationToken.IsCancellationRequested)
					break;
					
				_updateInterval += _config.UpdateInterval;
				UpdateDifficultyMultipliers();
			}
		}

		private void UpdateDifficultyMultipliers()
		{
			CurrentHealthMultiplier = Mathf.Clamp(
				_config.MinStatMultiplier + (_updateInterval * _config.HealthScalingPerSecond),
				_config.MinStatMultiplier,
				_config.MaxStatMultiplier);
				
			CurrentDamageMultiplier = Mathf.Clamp(
				_config.MinStatMultiplier + (_updateInterval * _config.DamageScalingPerSecond),
				_config.MinStatMultiplier,
				_config.MaxStatMultiplier);
				
			OnDifficultyChanged?.Invoke(CurrentHealthMultiplier, CurrentDamageMultiplier);
		}

		public void Dispose()
		{
			StopScaling();
		}
	}
} 