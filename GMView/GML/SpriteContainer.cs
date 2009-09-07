using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    /// <summary>
    /// Sprite container organizes sprites into arrays divided for levels
    /// Each level then is drawn on graphics from lowest to highest
    /// </summary>
    public class SpriteContainer: ISprite
    {
        private const int MaxLevels = 4;
        private List<ISprite>[] levels = new List<ISprite>[MaxLevels];
        private int drawLevel = 0;

        public SpriteContainer()
        {
            for (int i = 0; i < levels.Length; i++)
                levels[i] = new List<ISprite>();
        }

        /// <summary>
        /// Adding new sprite to the container
        /// </summary>
        /// <param name="newSprite"></param>
        public virtual void addSub(ISprite newSprite)
        {
            if (newSprite != null && newSprite.dLevel < MaxLevels)
            {
                levels[newSprite.dLevel].Add(newSprite);
            }
        }

        /// <summary>
        /// Deletes sprite from the containers list
        /// </summary>
        /// <param name="sprite"></param>
        public virtual void delSub(ISprite sprite)
        {
            if (sprite != null && sprite.dLevel < MaxLevels)
            {
                levels[sprite.dLevel].Remove(sprite);
            }
        }

        #region ISprite Members

        /// <summary>
        /// Draw sprites from lists by levels
        /// </summary>
        /// <param name="gr"></param>
        public virtual void draw(Graphics gr)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                foreach (ISprite sprite in levels[i])
                {
                    sprite.draw(gr);
                }
            }
        }

        public virtual void draw(Graphics gr, int x, int y)
        {
            throw new NotImplementedException();
        }

        public int dLevel
        {
            get
            {
                return drawLevel;
            }
            set
            {
                drawLevel = value;
            }
        }

        public void show()
        {
            for (int i = 0; i < levels.Length; i++)
            {
                foreach (ISprite sprite in levels[i])
                {
                    sprite.show();
                }
            }
        }

        public void hide()
        {
            for (int i = 0; i < levels.Length; i++)
            {
                foreach (ISprite sprite in levels[i])
                {
                    sprite.hide();
                }
            }
        }

        public virtual void glDraw(int centerx, int centery)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                foreach (ISprite sprite in levels[i])
                {
                    sprite.glDraw(centerx, centery);
                }
            }
        }

        #endregion
    }
}
