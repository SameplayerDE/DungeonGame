using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonClient
{
    public class World
    {

        public List<Entity> Entities { get; set; } = new();
        private Queue<Entity> _newEntities = new();
        private Queue<Entity> _removedEntities = new();

        public void Remove(Entity entity)
        {
            _removedEntities.Enqueue(entity);
        }

        public void Add(Entity entity)
        {
            _newEntities.Enqueue(entity);
        }

        public Entity GetPlayer()
        {
            return Entities.FirstOrDefault(e => e.EntityType == EntityType.Player);
        }

        public Entity Get(Guid guid)
        {
            foreach (var entity in Entities)
            {
                if (entity.Guid.Equals(guid))
                {
                    return entity;
                }
            }
            return null;
        }

        public List<Entity> Get(Vector2 center, float radius)
        {
            var result = new List<Entity>();
            foreach (var entity in Entities)
            {
                if (Vector2.Distance(center, entity.Position) <= radius)
                {
                    result.Add(entity);
                }
            }
            return result;
        }

        public void Update(GameTime gameTime)
        {
            while (_newEntities.Count > 0)
            {
                Entity newEntity = _newEntities.Dequeue();
                newEntity.World = this;
                Entities.Add(newEntity);
            }
            foreach (var entity in Entities.ToList())
            {
                entity.Update(gameTime);
            }
            while (_removedEntities.Count > 0)
            {
                Entity removedEntity = _removedEntities.Dequeue();
                Entities.Remove(removedEntity);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var entity in Entities.ToList())
            {
                entity.Draw(spriteBatch, gameTime);
            }
        }

    }
}
