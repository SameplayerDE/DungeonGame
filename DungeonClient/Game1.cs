using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace DungeonClient
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TextureManager _textureManager;
        private SpriteFont _font;

        private Guid _playerGuid;

        public KeyboardState CurrKeyboardState;
        public KeyboardState PrevKeyboardState;
        public MouseState CurrMouseState;
        public MouseState PrevMouseState;

        int retroWidth = 640;  // Retro Breite
        int retroHeight = 480; // Retro Höhe
        RenderTarget2D _renderTarget;

        private World _world;
        private bool _isInventoryOpen = false;
        private bool _combineMode = false;
        private int _selectedItemIndex = 0;
        private int _inventoryIndex = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _world = new World();
            _playerGuid = Guid.NewGuid();

            _renderTarget = new RenderTarget2D(GraphicsDevice, retroWidth, retroHeight);

            Entity.Pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Entity.Pixel.SetData(new[] { Color.White });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _textureManager = new TextureManager(Content);

            _font = Content.Load<SpriteFont>("font");

            _textureManager.LoadTexture("slot");
            _textureManager.LoadTexture("allowed");
            _textureManager.LoadTexture("no");
            _textureManager.LoadTexture("info");
            _textureManager.LoadTexture("alert");
            _textureManager.LoadTexture("arrow_down");
            _textureManager.LoadTexture("arrow_up");
            _textureManager.LoadTexture("player");
            _textureManager.LoadTexture("monster");
            _textureManager.LoadTexture("sword");
            _textureManager.LoadTexture("helmet");
            _textureManager.LoadTexture("helmet_icon");
            _textureManager.LoadTexture("sword_icon");
            _textureManager.LoadTexture("bundle");
            _textureManager.LoadTexture("bundle_icon");
            _textureManager.LoadTexture("money_bg");

            // Laden des Spieler-Entities
            var player = new Entity(EntityType.Player)
            {
                Guid = _playerGuid,
                Texture = _textureManager.GetTexture("player")
            };
            _world.Add(player);

            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                var monster = new Entity(EntityType.Monster)
                {
                    Guid = Guid.NewGuid(),
                    Texture = _textureManager.GetTexture("monster"),
                    Position = new Vector2(random.Next(0, _graphics.PreferredBackBufferWidth), random.Next(0, _graphics.PreferredBackBufferHeight)),
                    Direction = (float)(random.NextDouble() * Math.PI * 2), // Zufällige Richtung
                    Inventory = GenerateRandomDrops() // Generiere zufällige Drops für das Monster
                };

                _world.Add(monster);
            }
        }

        private List<Entity> GenerateRandomDrops()
        {
            Random random = new Random();
            List<Entity> drops = new List<Entity>();

            // Zufällige Anzahl von Geld-Entities hinzufügen
            int moneyCount = random.Next(1, 5);
            for (int i = 0; i < moneyCount; i++)
            {
                drops.Add(new Entity(EntityType.Item, ItemType.Money)
                {
                    Amount = random.Next(1, 50),
                    Texture = _textureManager.GetTexture("bundle")
                });
            }

            // Zufällige Schwerter und Helme hinzufügen
            if (random.NextDouble() < 0.1) // 50% Chance, ein Schwert hinzuzufügen
            {
                drops.Add(new Entity(EntityType.Item, ItemType.Sword)
                {
                    Texture = _textureManager.GetTexture("sword")
                });
            }

            if (random.NextDouble() < 0.1) // 50% Chance, einen Helm hinzuzufügen
            {
                drops.Add(new Entity(EntityType.Item, ItemType.Helmet)
                {
                    Texture = _textureManager.GetTexture("helmet")
                });
            }

            return drops;
        }

        protected override void Update(GameTime gameTime)
        {
            PrevKeyboardState = CurrKeyboardState;
            PrevMouseState = CurrMouseState;
            CurrKeyboardState = Keyboard.GetState();
            CurrMouseState = Mouse.GetState();

            if (!_isInventoryOpen)
            {
                if (PrevKeyboardState.IsKeyUp(Keys.I) && CurrKeyboardState.IsKeyDown(Keys.I))
                {
                    _isInventoryOpen = true;
                }
                _world.Update(gameTime);
            }
            else
            {
                if (PrevKeyboardState.IsKeyUp(Keys.Escape) && CurrKeyboardState.IsKeyDown(Keys.Escape))
                {
                    _isInventoryOpen = false;
                }
                var inventorySize = _world.GetPlayer().Inventory.Count;
                if (PrevKeyboardState.IsKeyUp(Keys.Up) && CurrKeyboardState.IsKeyDown(Keys.Up))
                {
                    _inventoryIndex = Math.Max(_inventoryIndex - 1, 0);
                }
                if (PrevKeyboardState.IsKeyUp(Keys.Down) && CurrKeyboardState.IsKeyDown(Keys.Down))
                {
                    _inventoryIndex = Math.Min(_inventoryIndex + 1, inventorySize - 1);
                }
                if (CurrKeyboardState.IsKeyDown(Keys.C) && PrevKeyboardState.IsKeyUp(Keys.C))
                {
                    _combineMode = !_combineMode; // Wechsle in den Kombinationsmodus
                    _selectedItemIndex = _inventoryIndex; // Wähle das aktuell ausgewählte Item für die Kombination aus
                }
                if (CurrKeyboardState.IsKeyDown(Keys.E) && PrevKeyboardState.IsKeyUp(Keys.E))
                {
                    _world.GetPlayer().EquipItem(_world.GetPlayer().Inventory[_inventoryIndex]);
                }

                if (_combineMode)
                {
                    // Logik für die Auswahl von Items zum Kombinieren
                    if (CurrKeyboardState.IsKeyDown(Keys.Enter) && PrevKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        var playerInventory = _world.GetPlayer().Inventory;
                        var firstItem = playerInventory[_selectedItemIndex];
                        var secondItem = playerInventory[_inventoryIndex];

                        // Überprüfe, ob die Items kombinierbar sind und führe die Kombination durch
                        if (firstItem.CanCombineWith(secondItem))
                        {
                            secondItem.Combine(firstItem);
                            playerInventory.Remove(firstItem);
                            if (_selectedItemIndex < _inventoryIndex)
                            {
                                _inventoryIndex--;
                            }
                            _combineMode = false; // Verlasse den Kombinationsmodus
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            if (_isInventoryOpen)
            {

                var inventoryItems = _world.GetPlayer().Inventory;
                int inventorySize = inventoryItems.Count;

                var inventoryPosition = new Vector2(32, 0); // Startposition für das Inventar

                if (_inventoryIndex > 0)
                {
                    _spriteBatch.Draw(_textureManager.GetTexture("arrow_up"), new Vector2(8, 8), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(_textureManager.GetTexture("arrow_up"), new Vector2(8, 8), Color.Gray);
                }
                if (_inventoryIndex < inventorySize - 1)
                {
                    _spriteBatch.Draw(_textureManager.GetTexture("arrow_down"), new Vector2(8, 8 + 16 + 8), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(_textureManager.GetTexture("arrow_down"), new Vector2(8, 8 + 16 + 8), Color.Gray);
                }

                for (int i = _inventoryIndex; i < inventorySize; i++)
                {
                    var item = inventoryItems[i];
                    string itemInfo = GetItemInfo(item);
                    Texture2D iconTexture = GetIconTexture(item);

                    _spriteBatch.Draw(iconTexture, new Rectangle((int)inventoryPosition.X, (int)inventoryPosition.Y, 32, 32), Color.White);

                    Color textColor = Color.Gray;

                    if (_combineMode)
                    {
                        if (i == _selectedItemIndex)
                        {
                            textColor = Color.Red; // Roter Text für das für die Kombination ausgewählte Item
                            _spriteBatch.Draw(_textureManager.GetTexture("alert"), new Rectangle((int)inventoryPosition.X + 16, (int)inventoryPosition.Y + 16, 16, 16), Color.White);

                        }
                        else
                        {
                            if (item.CanCombineWith(inventoryItems[_selectedItemIndex]))
                            {
                                textColor = Color.White;
                                _spriteBatch.Draw(_textureManager.GetTexture("allowed"), new Rectangle((int)inventoryPosition.X + 16, (int)inventoryPosition.Y + 16, 16, 16), Color.White);

                            }
                            else
                            {
                                _spriteBatch.Draw(_textureManager.GetTexture("no"), new Rectangle((int)inventoryPosition.X + 16, (int)inventoryPosition.Y + 16, 16, 16), Color.White);

                            }
                        }
                    }
                    else
                    {
                        textColor = Color.White;
                    }
                    if (i == _inventoryIndex)
                    {
                        textColor = Color.Yellow;
                    }

                    _spriteBatch.DrawString(_font, itemInfo, new Vector2(inventoryPosition.X + 40, inventoryPosition.Y), textColor);
                    inventoryPosition.Y += 40; // Verschiebe die Position für das nächste Item

                    Point slotPosition = new Point(retroWidth / 2, retroHeight / 2);

                    _spriteBatch.Draw(_textureManager.GetTexture("slot"), new Rectangle(slotPosition.X, slotPosition.Y, 64, 64), Color.White);
                    if (_world.GetPlayer().EquippedHelmet != null)
                    {
                        _spriteBatch.Draw(GetIconTexture(_world.GetPlayer().EquippedHelmet), new Rectangle(slotPosition.X, slotPosition.Y, 64, 64), Color.White);
                    }
                    slotPosition.X += 64;
                    _spriteBatch.Draw(_textureManager.GetTexture("slot"), new Rectangle(slotPosition.X, slotPosition.Y, 64, 64), Color.White);
                    if (_world.GetPlayer().EquippedSword != null)
                    {
                        _spriteBatch.Draw(GetIconTexture(_world.GetPlayer().EquippedSword), new Rectangle(slotPosition.X, slotPosition.Y, 64, 64), Color.White);
                    }
                }
            }
            else
            {
                _world.Draw(_spriteBatch, gameTime);

            }

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp);
            _spriteBatch.Draw(_renderTarget, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private float GetAlphaForItem(int itemIndex, int selectedIndex, int totalItems)
        {
            int maxDistance = totalItems / 2; // Maximale Distanz vom zentralen Item
            int distance = Math.Abs(itemIndex - selectedIndex);
            return MathHelper.Clamp(1 - (float)distance / maxDistance, 0, 1);
        }

        private string GetItemInfo(Entity item)
        {
            // Diese Methode kann erweitert werden, um detailliertere Informationen zurückzugeben
            switch (item.ItemType)
            {
                case ItemType.Money:
                    return "Geld: " + item.Amount;
                case ItemType.Helmet:
                    return "Helm Stf." + item.Level;
                case ItemType.Sword:
                    return "Schwert Stf." + item.Level;
                default:
                    return "Unbekanntes Item";
            }
        }

        private Texture2D GetIconTexture(Entity item)
        {
            switch (item.ItemType)
            {
                case ItemType.Money:
                    return _textureManager.GetTexture("bundle_icon");
                case ItemType.Helmet:
                    return _textureManager.GetTexture("helmet_icon");
                case ItemType.Sword:
                    return _textureManager.GetTexture("sword_icon");
                default:
                    return null;
            }
        }

        protected void DrawItemListItem(int x, int y, Entity entity)
        {
            _spriteBatch.Draw(Entity.Pixel, new Rectangle(x, y, 100, 100), Color.Gold);
            _spriteBatch.Draw(Entity.Pixel, new Rectangle(x, y, 100, 100), Color.Gold);
        }
    }
}
