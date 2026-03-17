using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ____.Camera
{
    public class Camera2D
    {
        protected float zoom; // Zoom
        private Matrix transform; // Matrix Transform
        private Vector2 pos; // Position
        protected float rotation; // Rotation
        private GraphicsDevice graphicsDevice;
        private Rectangle mapBounds; // Map boundaries to clamp camera
        private bool boundsSet = false;

        public Camera2D(GraphicsDevice graphicsDevice)
        {
            zoom = 1f;
            rotation = 0.0f;
            pos = Vector2.Zero;
            this.graphicsDevice = graphicsDevice;
            mapBounds = Rectangle.Empty;
        }

        public void SetMapBounds(Rectangle bounds)
        {
            mapBounds = bounds;
            boundsSet = true;
            ClampPosition();
        }

        public void LoadContent(ContentManager contentManager, Rectangle mapBounds)
        {
            SetMapBounds(mapBounds);
        }

        public float Zoom
        {
            get { return zoom; }
            set 
            { 
                zoom = value;
                if (zoom < 0.1f) zoom = 0.1f;
                ClampPosition();
            }
        }

        public float Rotation
        {
            get {return rotation; }
            set { rotation = value; }
        }

        public void Move(Vector2 amount)
        {
            pos += amount;
            ClampPosition();
        }

        private void ClampPosition()
        {
            if (!boundsSet) return;

            // Calculate the visible area based on viewport size and zoom
            float viewportWidth = graphicsDevice.Viewport.Width / zoom;
            float viewportHeight = graphicsDevice.Viewport.Height / zoom;

            // Clamp camera position so it doesn't go outside map bounds
            pos.X = Math.Clamp(pos.X, mapBounds.Left + viewportWidth * 0.5f, mapBounds.Right - viewportWidth * 0.5f);
            pos.Y = Math.Clamp(pos.Y, mapBounds.Top + viewportHeight * 0.5f, mapBounds.Bottom - viewportHeight * 0.5f);
        }
        public Vector2 Pos
        {
            get { return pos; }
            set 
            { 
                pos = value;
                ClampPosition();
            }
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