namespace ____.Entities.Player
{
    public class StatScaling
    {
        // Base values
        public int BaseHealth { get; set; } = 100;
        public int BaseMana { get; set; } = 50;
        public float BaseDamage { get; set; } = 5f;
        public float BaseCritChance { get; set; } = 0.05f; // 5%
        
        // Scaling per stat point
        public int HealthPerStamina { get; set; } = 10;
        public int ManaPerIntelligence { get; set; } = 5;
        public float DamagePerStrength { get; set; } = 2f;
        public float CritChancePerAgility { get; set; } = 0.01f; // 1% per point
        public float DamageReductionPerStamina { get; set; } = 0.005f; // 0.5% per point
        public float MaxDamageReduction { get; set; } = 0.75f; // 75% max
        
        // Regeneration scaling
        public float HealthRegenPerStamina { get; set; } = 0.1f;
        public float ManaRegenPerIntelligence { get; set; } = 0.2f;
        
        // Movement scaling
        public float BaseSpeed { get; set; } = 100f;
        public float SpeedPerAgility { get; set; } = 0.1f; // 10% per point
        public float RunSpeedMultiplier { get; set; } = 1.5f;
        public float DashSpeedMultiplier { get; set; } = 2.5f;
        
        // Attack speed scaling
        public float BaseAttackSpeed { get; set; } = 1.0f;
        public float AttackSpeedPerAgility { get; set; } = 0.05f; // 5% per point
        
        // Defense scaling
        public float BaseDefense { get; set; } = 5f;
        public float DefensePerStamina { get; set; } = 1.5f;
        
        public static StatScaling LoadFromConfig(string configPath)
        {
            // TODO: Implement JSON deserialization
            // For now, return default values
            return new StatScaling();
        }
    }
}