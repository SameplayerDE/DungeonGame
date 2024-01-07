using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DungeonClient
{

    public enum EntityType
    {
        Monster,
        Human,
        Item,
        Player
    }

    public enum ItemType
    {
        None,
        Money,
        Helmet,
        Sword
    }

    public class Entity
    {
        public Guid Guid { get; set; }
        public EntityType EntityType { get; set; }
        public World World { get; set; }
        public Vector2 Position { get; set; }
        public static Texture2D Pixel { get; set; }
        public Texture2D? Texture { get; set; }
        public float CollisionRadius { get; set; } = 16;
        public Vector2 Velocity { get; set; } = Vector2.Zero;

        // monster, human, player
        public float Direction { get; set; } = 0; // rad
        public int Health { get; set; } = 100;
        public int Armour { get; set; } = 0;
        public int Damage { get; set; } = 400;
        public bool IsAlive => Health > 0;

        // monster
        public int SeekRadius { get; set; } = 128;
        public float SeekAngle { get; set; } = MathHelper.PiOver4 / 4;

        // item
        public ItemType ItemType { get; set; }
        // money
        public int Amount { get; set; }
        // gadgets and mosnter
        public int Level { get; set; } = 1;

        // player, monster
        // dropped if monster dies
        public Entity EquippedSword { get; set; }
        public Entity EquippedHelmet { get; set; }
        public List<Entity> Inventory;
        public int Money { get; set; }

        // player
        public KeyboardState CurrKeyboardState;
        public KeyboardState PrevKeyboardState;
        public MouseState CurrMouseState;
        public MouseState PrevMouseState;

        public Entity(EntityType entityType, ItemType itemType = ItemType.None)
        {
            EntityType = entityType;
            ItemType = itemType;

            switch (entityType)
            {
                case EntityType.Player:
                    {
                        Inventory = new();
                        Money = 0;
                        break;
                    }
                case EntityType.Monster:
                    {
                        Inventory = new();
                        Money = 0;
                        break;
                    }
                case EntityType.Item:
                    {
                        Armour = 100;
                        break;
                    }
            }
        }

        public void Update(GameTime gameTime)
        {
            
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * delta;
            Velocity *= 0.9f;
            switch (EntityType)
            {
                case EntityType.Monster:
                    {
                        if (!IsAlive)
                        {
                            return;
                        }
                        var inRadius = World.Get(Position, SeekRadius);

                        var target = inRadius
                            .Where(other => (other.EntityType == EntityType.Human || other.EntityType == EntityType.Player) && IsInLineOfSight(other, SeekAngle))
                            .OrderBy(other => Vector2.Distance(Position, other.Position))
                            .FirstOrDefault();

                        if (target != null)
                        {
                            // Ausrichtung in Richtung des Ziels
                            Direction = (float)Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X);

                            // Bewegung in Richtung des Ziels
                            Vector2 direction = Vector2.Normalize(target.Position - Position);
                            Position += direction * delta * 100; // Geschwindigkeit anpassen
                        }
                        break;
                    }
                case EntityType.Human:
                    {
                        if (!IsAlive)
                        {
                            return;
                        }
                        break;
                    }
                case EntityType.Item:
                    {
                        CheckForPickup();
                        break;
                    }
                case EntityType.Player:
                    {
                        PrevKeyboardState = CurrKeyboardState;
                        PrevMouseState = CurrMouseState;
                        CurrKeyboardState = Keyboard.GetState();
                        CurrMouseState = Mouse.GetState();

                        // Bewegung mit WASD
                        Vector2 moveDirection = Vector2.Zero;
                        if (CurrKeyboardState.IsKeyDown(Keys.W))
                            moveDirection.Y -= 1;
                        if (CurrKeyboardState.IsKeyDown(Keys.S))
                            moveDirection.Y += 1;
                        if (CurrKeyboardState.IsKeyDown(Keys.A))
                            moveDirection.X -= 1;
                        if (CurrKeyboardState.IsKeyDown(Keys.D))
                            moveDirection.X += 1;

                        if (moveDirection != Vector2.Zero)
                        {
                            moveDirection.Normalize();
                            Position += moveDirection * delta * 100; // Hier kannst du die Geschwindigkeit anpassen
                        }

                        // Richtung zur Maus
                        Vector2 mousePosition = new Vector2(CurrMouseState.X, CurrMouseState.Y);
                        Direction = (float)Math.Atan2(mousePosition.Y - Position.Y, mousePosition.X - Position.X);
                        
                        if (CurrMouseState.LeftButton == ButtonState.Pressed && PrevMouseState.LeftButton == ButtonState.Released)
                        {
                            Attack();
                        }

                        AttractNearbyItems();

                        break;
                    }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            if (Texture == null)
            {
                return;
            }

            Vector2 origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            spriteBatch.Draw(Texture, Position, null, IsAlive ? Color.White : Color.Gray, Direction, origin, 1.0f, SpriteEffects.None, 0f);
            if (EntityType == EntityType.Monster)
            {
                DrawLine(spriteBatch, Position, Position + Vector2.Transform(new Vector2(100, 0), Matrix.CreateRotationZ(Direction - SeekAngle / 2)), Color.Red);
                DrawLine(spriteBatch, Position, Position + Vector2.Transform(new Vector2(100, 0), Matrix.CreateRotationZ(Direction + SeekAngle / 2)), Color.Red);
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(Pixel, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1), null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void Combine(Entity entity)
        {
            if (EntityType == EntityType.Item && entity.EntityType == EntityType.Item)
            {
                if (ItemType != ItemType.Money && entity.ItemType != ItemType.Money)
                {
                    if (ItemType == entity.ItemType)
                    {
                        if (Level == entity.Level)
                        {
                            Level += 1;
                        }
                    }
                }
            }
        }

        public void EquipItem(Entity item)
        {
            if (item.EntityType != EntityType.Item)
            {
                return;
            }
            if (item.ItemType == ItemType.Money)
            {
                return;
            }
            if (item.ItemType == ItemType.Helmet)
            {
                if (EquippedHelmet != null)
                {
                    AddItem(EquippedHelmet);
                    RemoveItem(item);
                }
                EquippedHelmet = item;
                return;
            }
            if (item.ItemType == ItemType.Sword)
            {
                if (EquippedSword != null)
                {
                    AddItem(EquippedSword);
                    RemoveItem(item);
                }
                EquippedSword = item;
                return;
            }
        }

        public void ReceiveDamage(int damage)
        {
            if (EntityType == EntityType.Item || !IsAlive)
            {
                return;
            }
            if (Armour > 0)
            {
                float damageReductionPercent = Armour / 100f;
                int reducedDamage = (int)(damage * (1 - damageReductionPercent));
                Health -= reducedDamage;
            }
            else
            {
                Health -= damage;
            }
            if (Health <= 0)
            {
                Health = 0;
                Die();
            }
        }

        private void AttractNearbyItems()
        {
            float attractionRadius = 6400; // Radius, in dem Items angezogen werden
            float attractionStrength = 100; // Stärke der Anziehung

            var nearbyItems = World.Get(Position, attractionRadius)
                                   .Where(entity => entity.EntityType == EntityType.Item);

            foreach (var item in nearbyItems)
            {
                Vector2 directionToPlayer = Vector2.Normalize(Position - item.Position);
                item.Velocity += directionToPlayer * attractionStrength;
            }
        }

        public void Die()
        {
            switch (EntityType)
            {
                case EntityType.Monster:
                    var random = new Random();
                    foreach (var item in Inventory)
                    {
                        item.Position = Position;
                        item.Velocity = new Vector2(random.Next(-100, 100), random.Next(-100, 100));

                        World.Add(item);
                    }

                    Inventory.Clear();
                    break;
            }
        }

        public void Knockback(Vector2 sourcePosition, float strength)
        {
            if (EntityType == EntityType.Item)
            {
                return;
            }
            Vector2 knockbackDirection = Vector2.Normalize(Position - sourcePosition);
            Velocity += knockbackDirection * strength; // Füge Knockback zur aktuellen Geschwindigkeit hinzu
        }

        public bool IsInLineOfSight(Entity target, float seekAngle)
        {
            Vector2 directionToTarget = Vector2.Normalize(target.Position - Position);
            float angleToTarget = (float)Math.Atan2(directionToTarget.Y, directionToTarget.X);

            // Normalisiere den Winkel zwischen -π und π
            float normalizedDirection = MathHelper.WrapAngle(Direction);
            float normalizedAngleToTarget = MathHelper.WrapAngle(angleToTarget);

            // Berechne den absoluten Winkelunterschied
            float angleDifference = Math.Abs(normalizedAngleToTarget - normalizedDirection);

            // Überprüfe, ob der Winkelunterschied innerhalb des Sichtwinkels liegt
            return angleDifference <= seekAngle / 2 || angleDifference >= (MathHelper.TwoPi - seekAngle / 2);
        }

        public void AddItem(Entity entity)
        {
            if (entity.EntityType == EntityType.Item)
            {
                if (EntityType == EntityType.Player)
                {
                    if (entity.ItemType == ItemType.Money)
                    {
                        Money += entity.Amount;
                    }
                    else
                    {
                        Inventory.Add(entity);
                    }
                }
            }
        }

        public void RemoveItem(Entity entity)
        {
            if (entity.EntityType == EntityType.Item)
            {
                if (EntityType == EntityType.Player)
                {
                    if (entity.ItemType == ItemType.Money)
                    {
                        Money -= entity.Amount;
                    }
                    else
                    {
                        Inventory.Remove(entity);
                    }
                }
            }
        }

        public void Attack()
        {

            float attackRadius = 500; // Beispielwert für den Angriffsradius
            float knockbackDistance = 50 * 10; // Beispielwert für den Knockback-Abstand

            // Position des Angriffsbereichs vor dem Spieler
            Vector2 attackPosition = Position + Vector2.Transform(new Vector2(attackRadius, 0), Matrix.CreateRotationZ(Direction));

            var entitiesInAttackRange = World.Get(attackPosition, attackRadius);

            foreach (var entity in entitiesInAttackRange)
            {
                if (entity != this && Vector2.Distance(attackPosition, entity.Position) <= attackRadius + entity.CollisionRadius)
                {
                    entity.ReceiveDamage(Damage);
                    entity.Knockback(Position, knockbackDistance);
                }
            }

            //isAttacking = true;
            //attackVisualTimer = attackVisualDuration;
        }

        private void CheckForPickup()
        {
            Entity player = World.GetPlayer();
            if (player != null && Vector2.Distance(Position, player.Position) <= player.CollisionRadius + CollisionRadius)
            {
                Velocity = Vector2.Zero;
            }

            if (player != null && Vector2.Distance(Position, player.Position) <= CollisionRadius + player.CollisionRadius)
            {
                player.AddItem(this);
                World.Remove(this);
            }
        }

        public bool CanCombineWith(Entity secondItem)
        {
            if (this == secondItem)
            {
                return false;
            }
            if (EntityType == EntityType.Item && secondItem.EntityType == EntityType.Item)
            {
                if (ItemType != ItemType.Money && secondItem.ItemType != ItemType.Money)
                {
                    if (ItemType == secondItem.ItemType)
                    {
                        if (Level == secondItem.Level)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}