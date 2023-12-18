using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace QColonFrame
{
    public partial class QCScene
    {

        public Game Game;
        public Guid Id;
        public string Name;
        protected ContentManager Content;

        public QCScene(string name, Game game)
        {
            Id = Guid.NewGuid();
            Game = game;
            Name = name;
            Content = new ContentManager(game.Services, "Content");
        }

        public override bool Equals(object obj)
        {
            if (obj is QCScene scene)
            {
                return Name.Equals(scene.Name);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public virtual void Activated() { }
        public virtual void Deactivated() { }

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(QCRenderContext context, GameTime gameTime) { }
    }
}
