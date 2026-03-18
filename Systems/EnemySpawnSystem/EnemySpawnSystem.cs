using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ____.Camera;
using ____.Entities;
using ____.Entities.Enemies;
using ____.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ____.Systems.EnemySpawnSystem
{
    public class EnemySpawnSystem
    {
        private readonly List<(Func<BaseEnemyEntity> Factory, int Weight)> spawnTypes = new();
        private readonly Random rng = new();
        private float spawnTimer;

        public float SpawnIntervalSeconds { get; set; } = 2.5f;

        public int MaxActiveEnemies { get; set; } = 20;

        public void AddEnemyType(Func<BaseEnemyEntity> factory, int weight)
        {
            if (weight <= 0) throw new ArgumentOutOfRangeException(nameof(weight));
            spawnTypes.Add((factory, weight));
        }

        public void Update(GameTime gameTime, List<BaseEnemyEntity> activeEnemies, Camera2D camera, Map map)
        {
            if (activeEnemies.Count >= MaxActiveEnemies || spawnTypes.Count == 0)
                return;

            spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (spawnTimer < SpawnIntervalSeconds)
                return;

            spawnTimer = 0f;

            var chosen = ChooseWeightedSpawn();
            if (chosen == null)
                return;

            var spawnPos = GetSpawnPosition(map, camera, chosen.Value);
            if (spawnPos == null)
                return;

            var enemy = chosen.Value.Factory();
            enemy.SetPosition(spawnPos.Value);
            activeEnemies.Add(enemy);
            enemy.LoadContent(Game1.Instance.Content);
        }

        private (Func<BaseEnemyEntity> Factory, int Weight)? ChooseWeightedSpawn()
        {
            int total = 0;
            foreach (var (_, w) in spawnTypes)
                total += w;
            if (total <= 0)
                return null;

            int roll = rng.Next(total);
            foreach (var entry in spawnTypes)
            {
                if (roll < entry.Weight)
                    return entry;
                roll -= entry.Weight;
            }

            return null;
        }

        private Vector2? GetSpawnPosition(Map map, Camera2D camera, (Func<BaseEnemyEntity> Factory, int Weight) spawnEntry)
        {
            var mapRect = map.Rec;
            var viewRect = camera.GetViewBounds();

            var sampleEnemy = spawnEntry.Factory();
            var size = new Point(sampleEnemy.Hitbox.Width, sampleEnemy.Hitbox.Height);

            const int maxAttempts = 100;
            for (int i = 0; i < maxAttempts; i++)
            {
                float x = rng.Next(mapRect.Left, mapRect.Right - size.X);
                float y = rng.Next(mapRect.Top, mapRect.Bottom - size.Y);

                var candidate = new Rectangle((int)x, (int)y, size.X, size.Y);
                if (viewRect.Intersects(candidate))
                    continue;

                if (!map.IsWalkable(candidate))
                    continue;

                return new Vector2(x, y);
            }

            return new Vector2(
                rng.Next(mapRect.Left, mapRect.Right - size.X),
                rng.Next(mapRect.Top, mapRect.Bottom - size.Y)
            );
        }

        public void ClearSpawnTypes()
        {
            spawnTypes.Clear();
        }

        public void ClearActiveEnemies(List<BaseEnemyEntity> activeEnemies)
        {
            activeEnemies.Clear();
        }
    }
}
