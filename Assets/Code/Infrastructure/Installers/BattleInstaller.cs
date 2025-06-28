using Code.Gameplay.Abilities.Services;
using Code.Gameplay.Cameras.Services;
using Code.Gameplay.Characters.Enemies.Services;
using Code.Gameplay.Characters.Heroes.Services;
using Code.Gameplay.DifficultyScaling.Services;
using Code.Gameplay.Experience.Services;
using Code.Gameplay.PickUps.Services;
using Code.Gameplay.Projectiles.Services;
using Zenject;

namespace Code.Infrastructure.Installers
{
	public class BattleInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindHeroServices();
			BindEnemyServices();
			BindCameraServices();
			BindCombatServices();
			BindPickupServices();
			BindExperienceServices();
			BindAbilityServices();
			BindDifficultyServices();
		}

		private void BindPickupServices()
		{
			Container.BindInterfacesTo<PickUpFactory>().AsSingle();
		}

		private void BindCombatServices()
		{
			Container.BindInterfacesTo<ProjectileFactory>().AsSingle();
			Container.BindInterfacesTo<OrbitingProjectileFactory>().AsSingle();
		}

		private void BindCameraServices()
		{
			Container.BindInterfacesTo<CameraProvider>().AsSingle();
		}

		private void BindEnemyServices()
		{
			Container.BindInterfacesTo<EnemyFactory>().AsSingle();
			Container.BindInterfacesTo<EnemyProvider>().AsSingle();
			Container.BindInterfacesTo<EnemyDeathTracker>().AsSingle();
		}

		private void BindHeroServices()
		{
			Container.BindInterfacesTo<HeroFactory>().AsSingle();
			Container.BindInterfacesTo<HeroProvider>().AsSingle();
		}

		private void BindExperienceServices()
		{
			Container.BindInterfacesTo<ExperienceService>().AsSingle();
			Container.Bind<LevelUpService>().AsSingle().NonLazy();
		}

		private void BindAbilityServices()
		{
			Container.BindInterfacesTo<AbilityService>().AsSingle();
		}

		private void BindDifficultyServices()
		{
			Container.BindInterfacesTo<DifficultyService>().AsSingle();
		}
	}
}