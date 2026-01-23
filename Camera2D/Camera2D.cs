using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HLG
{
    public class Camera2D
    {
        protected float zoom; // Zoom
        private Matrix transform; // Matrix Transform
        private Vector2 pos; // Position
        protected float rotation; // Rotation
        private GraphicsDevice graphicsDevice;

        public Camera2D(GraphicsDevice graphicsDevice)
        {
            zoom = 1f;
            rotation = 0.0f;
            pos = Vector2.Zero;
            this.graphicsDevice = graphicsDevice;
        }
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
        }

        public float Rotation
        {
            get {return rotation; }
            set { rotation = value; }
        }

        public void Move(Vector2 amount)
        {
            pos += amount;
        }
        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        public Matrix Get_transformation()
        {
            transform =
                Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) * 
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            return transform;
        }
    }
}