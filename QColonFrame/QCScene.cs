using Microsoft.Xna.Framework;

namespace QColonFrame
{
    public partial class QCScene
    {

        public Game Game;
        public Guid Id;
        public string Name;

        public QCScene(string name, Game game)
        {
            Id = Guid.NewGuid();
            Game = game;
            Name = name;
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
