using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas
{
    public class SpritePrimitive
    {
        public int mNumRow, mNumColumn, mPadding;
        public int mSpriteWidth, mSpriteHeight;

        public int mUserSpecififiedTicks;
        private int mCurrentTick;
        public int mCurrentRow, mCurrentColumn;
        private int mBeginRow, mEndRow;
        private int mBeginCol, mEndCol;
        private Vector2 mPosition;
        public Texture2D image;

        public SpritePrimitive(string imageName, Vector2 position, Vector2 size, int rowCounts, int columnCount, int padding)
        {
            image = Game1.sContent.Load<Texture2D>(imageName);
            mNumRow = rowCounts;
            mNumColumn = columnCount;
            mPadding = padding;
            mSpriteWidth =  image.Width/ mNumRow;
            mSpriteHeight = image.Height / mNumColumn;

            mUserSpecififiedTicks = 1;
            mCurrentTick = 0;
            mCurrentRow = 0;
            mCurrentColumn = 0;
            mBeginRow = mBeginCol = mEndRow = mEndCol = 0;

            mPosition = position;
        }

        public int SpriteBeginRow
        {
            get { return mBeginRow; }
            set { mBeginRow = value; mCurrentRow = value; }
        }
        public int SpriteEndRow
        {
            get { return mEndRow; }
            set { mEndRow = value; }
        }
        public int SpriteBeginColumn
        {
            get { return mEndCol; }
            set { mBeginCol = value; mCurrentColumn = value; }
        }
        public int SpriteEndColumn
        {
            get { return mEndCol; }
            set { mEndCol = value; }
        }
        public int SpriteAnimationTicks
        {
            get { return mUserSpecififiedTicks; }
            set { mUserSpecififiedTicks = value; }
        }
        public Vector2 MPosition
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        public virtual void SetSpriteAnimation(int beginRow, int beginCol, int endRow, int endCol, int tickInterval)
        {
            mUserSpecififiedTicks = tickInterval;
            mBeginRow = beginRow;
            mBeginCol = beginCol;
            mEndRow = endRow;
            mEndCol = endCol;

            mCurrentRow = mBeginRow;
            //mCurrentColumn = mBeginCol;
            //mCurrentTick = 0;
        }

        public virtual void Update()
        {
            mCurrentTick++;
            if (mCurrentTick > mUserSpecififiedTicks)
            {
                mCurrentTick = 0;
                mCurrentColumn++;
                if (mCurrentColumn > mEndCol)
                {
                    mCurrentColumn = mBeginCol;
                    mCurrentRow++;
                    if (mCurrentRow > mEndRow) mCurrentRow = mBeginRow;
                }
            }
        }

        public void Draw(Rectangle centro)
        {
            int imageTop = mCurrentRow * mSpriteWidth;
            int imageLeft = mCurrentColumn * mSpriteHeight;

            Rectangle srcRect = new Rectangle(imageLeft + mPadding,
                imageTop + mPadding,mSpriteWidth,mSpriteHeight);

            Vector2 org = new Vector2(mSpriteWidth / 2, mSpriteHeight / 2);

            //DRAWTEXTURE
            Game1.spriteBatch.Draw(image, centro, srcRect, Color.Pink, 0f, org, SpriteEffects.None, 0f);
        }
      
    }
}