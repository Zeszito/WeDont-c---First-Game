using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas
{
    public class Camera
    {
        private Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
        }
        public float RotaOri { get; set; }
        public Vector2 centre;
        private Viewport viewport;

        private float zoom = 1;
        private float rotation = 0;

        public float X
        {
            get { return centre.X; }
            set { centre.X = value; }
        }

        public float Y
        {
            get { return centre.Y; }
            set { centre.Y = value; }
        }

        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < 0.001f)
                    zoom = 0.001f;
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        //Aqui eu pus um zoom baixo que é para ficar ja predefinidamente grande
        public Camera(Viewport newViewport)
        {
            viewport = newViewport;
            RotaOri = rotation;
            zoom = 0.51f;
        }

        public void Move(Vector2 movimentacao)
        {
            centre = movimentacao;
        }

        //Cuidado Com isto que eu acho que alterei
        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            return transform;
        }
//        static public Rectangle ComputePixelRectangle(Vector2 position,
//Vector2 size)
//        {
//            float ratio = cameraWindowToPixelRatio();

//            // Convert size from camera window space to pixel space.
//            int width = (int)((size.X * ratio) + 0.5f);
//            int height = (int)((size.Y * ratio) + 0.5f);

//            // Convert the position to pixel space
//            int x, y;
//            ComputePixelPosition(position, out x, out y);

//            // Reference position is the center
//            y -= height / 2;
//            x -= width / 2;


//            return new Rectangle(x, y, width, height);
//        }

    }
}
