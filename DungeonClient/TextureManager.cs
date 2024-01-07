using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonClient
{
    public class TextureManager
    {
        private readonly ContentManager _content;
        private readonly Dictionary<string, Texture2D> _textures;

        public TextureManager(ContentManager content)
        {
            _content = content;
            _textures = new Dictionary<string, Texture2D>();
        }

        public void LoadTexture(string textureName)
        {
            if (!_textures.ContainsKey(textureName))
            {
                _textures[textureName] = _content.Load<Texture2D>(textureName);
            }
        }

        public Texture2D GetTexture(string textureName)
        {
            if (_textures.TryGetValue(textureName, out Texture2D texture))
            {
                return texture;
            }

            return null;
        }
    }

}
