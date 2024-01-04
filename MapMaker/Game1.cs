using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UEyeFrame;

namespace MapMaker
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private KeyboardState _currKeyboardState;
        private KeyboardState _prevKeyboardState;

        private MouseState _currMouseState;
        private MouseState _prevMouseState;

        private Texture2D _texture;
        private Texture2D _tile;
        private SpriteFont _font;

        private Stack<Menu> _menuStack = new();

        //world settings
        private int _worldWidth;
        private int _worldHeight;

        //editor settings
        private int _tileId = -1;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var toolbar = new Menu();

            var createFile = new MenuItem();
            createFile.Text = "New World";
            createFile.Action = () =>
            {
                var newWorldPopUp = new Menu();
                newWorldPopUp.Add(new NumberField(0, 1024, 128, (value) =>
                {
                    _worldWidth = value;
                })); //W
                newWorldPopUp.Add(new NumberField(0, 1024, 128, (value) =>
                {
                    _worldHeight = value;
                })); //H

                var create = new MenuItem();
                create.Text = "Create";
                create.Action = () =>
                {
                    _menuStack.Pop();
                };

                newWorldPopUp.Add(create);
                _menuStack.Push(newWorldPopUp);
            };

            var openTiles = new MenuItem();
            openTiles.Text = "Tiles";
            openTiles.Action = () =>
            {
                var tilesPopUp = new Menu();
                
                for (int i = 0; i < 10; i++)
                {
                    var tile = new ImageButton();
                    tile.ImagePath = "image";
                    tile.Action = () =>
                    {
                        _tileId = i;
                        _menuStack.Pop();
                    };
                    tilesPopUp.Add(tile);
                }
               
                _menuStack.Push(tilesPopUp);
            };


            var closeApp = new MenuItem();
            closeApp.Text = "Exit";
            closeApp.Action = Exit;

            toolbar.Add(createFile);
            toolbar.Add(openTiles);
            toolbar.Add(closeApp);

            _menuStack.Push(toolbar);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("font");
            _texture = Content.Load<Texture2D>("dirt");
            _tile = Content.Load<Texture2D>("fass");
        }

        protected override void Update(GameTime gameTime)
        {
            _prevKeyboardState = _currKeyboardState;
            _currKeyboardState = Keyboard.GetState();

            _prevMouseState = _currMouseState;
            _currMouseState = Mouse.GetState();

            var menu = _menuStack.Peek();
            var selected = menu.Items[menu.Index];

            if (_prevKeyboardState.IsKeyUp(Keys.Left) && _currKeyboardState.IsKeyDown(Keys.Left))
            {
                menu.Index--;
            }
            if (_prevKeyboardState.IsKeyUp(Keys.Right) && _currKeyboardState.IsKeyDown(Keys.Right))
            {
                menu.Index++;
            }
            if (_prevKeyboardState.IsKeyUp(Keys.Enter) && _currKeyboardState.IsKeyDown(Keys.Enter))
            {
                if (selected is MenuItem menuItem)
                {
                    menuItem.Action?.Invoke();
                }
                else if (selected is ImageButton imageButton)
                {
                    imageButton.Action?.Invoke();
                }
            }
            if (selected is NumberField numberField)
            {
                for (Keys key = Keys.D0; key <= Keys.D9; key++)
                {
                    if (IsKeyPressed(key, _prevKeyboardState, _currKeyboardState))
                    {
                        int digit = key - Keys.D0;
                        numberField.Value = numberField.Value * 10 + digit;
                        numberField.Value = Math.Clamp(numberField.Value, numberField.Min, numberField.Max);
                    }
                }
                if (IsKeyPressed(Keys.Back, _prevKeyboardState, _currKeyboardState))
                {
                    numberField.Value /= 10;
                }
            }

            if (IsKeyPressed(Keys.Escape, _prevKeyboardState, _currKeyboardState))
            {
                if (_menuStack.Count > 1)
                {
                    _menuStack.Pop();
                }
            }

            base.Update(gameTime);
        }

        private bool IsKeyPressed(Keys key, KeyboardState prevKeyboardState, KeyboardState currKeyboardState)
        {
            return prevKeyboardState.IsKeyUp(key) && currKeyboardState.IsKeyDown(key);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //render scene

            if (_tileId >= 0)
            {
                _spriteBatch.Draw(_tile, _currMouseState.Position.ToVector2(), Color.White);
            }
            //render ui

            //_spriteBatch.Draw(_texture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, _font.LineSpacing), Color.White);

            float x = 0;
            float y = 0;
            float padding = 15;

            //var menu = _menuStack.Peek();

            foreach (var menu in _menuStack.Reverse())
            {
                _spriteBatch.Draw(_texture, new Rectangle(0, (int)y, GraphicsDevice.Viewport.Width, _font.LineSpacing), Color.White);

                foreach (var item in menu.Items)
                {
                    x += padding;

                    if (item is MenuItem menuItem)
                    {
                        _spriteBatch.DrawString(_font, menuItem.Text, new Vector2(x, y), item.IsSelected ? Color.Yellow : Color.White);
                        x += _font.MeasureString(menuItem.Text).X;
                    }

                    if (item is NumberField numberField)
                    {
                        string value = numberField.Value + "";
                        _spriteBatch.DrawString(_font, value, new Vector2(x, y), item.IsSelected ? Color.Yellow : Color.White);
                        x += _font.MeasureString(value).X;
                    }

                    if (item is ImageButton image)
                    {
                        _spriteBatch.Draw(_tile, new Vector2(x, y), item.IsSelected ? Color.Yellow : Color.White);
                        x += _tile.Width;
                    }

                    x += padding;
                }
                y += _font.LineSpacing;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
