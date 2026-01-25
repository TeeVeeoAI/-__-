using System;

namespace ____.Entities.Player
{
    public class PlayerStats
    {
        private StatScaling scaling;
        
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int Stamina { get; set; }
        
        public int Level { get; set; } = 1;
        public int Experience { get; set; }
        public int ExperienceToNextLevel => 100 * Level; // Simple formula
        
        // Calculated properties based on stats and scaling config
        public int MaxHealth => scaling.BaseHealth + (Stamina * scaling.HealthPerStamina);
        public int MaxMana => scaling.BaseMana + (Intelligence * scaling.ManaPerIntelligence);
        public float BaseDamage => scaling.BaseDamage + (Strength * scaling.DamagePerStrength);
        public float CriticalChance => scaling.BaseCritChance + (Agility * scaling.CritChancePerAgility);
        public float DamageReduction => Math.Min(Stamina * scaling.DamageReductionPerStamina, scaling.MaxDamageReduction);
        public float HealthRegenPerSecond => Stamina * scaling.HealthRegenPerStamina;
        public float ManaRegenPerSecond => Intelligence * scaling.ManaRegenPerIntelligence;
        
        public PlayerStats(StatScaling statScaling)
        {
            scaling = statScaling;
            
            // Default starting stats
            Strength = 10;
            Agility = 10;
            Intelligence = 10;
            Stamina = 10;
        }
        
        public void AddExperience(int amount)
        {
            Experience += amount;
            
            while (Experience >= ExperienceToNextLevel)
            {
                LevelUp();
            }
        }
        
        private void LevelUp()
        {
            Experience -= ExperienceToNextLevel;
            Level++;
            
            // Automatic stat increases on level up
            Strength += 2;
            Agility += 2;
            Intelligence += 2;
            Stamina += 2;
            
            // Could trigger an event here: OnLevelUp?.Invoke();
        }
    }
}