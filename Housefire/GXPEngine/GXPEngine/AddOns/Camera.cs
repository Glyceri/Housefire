using System;

namespace GXPEngine
{
	/// <summary>
	/// A Camera gameobject, that owns a rectangular render window, and determines the focal point, rotation and scale
	/// of what's rendered in that window.
	/// (Don't forget to add this as child somewhere in the hierarchy.)
	/// </summary>
	public class Camera : GameObject
	{
		Window renderTarget;
		public static Camera main;

		public Vector2 CameraPos()
		{
			return (parent != null ? TransformPoint(x - (renderTarget.width / 2), y - (renderTarget.height / 2)) : TransformPoint(x, y));
		}

		public Vector2 ScreenToWorldPos(Vector2 screenPoint)
		{

			return CameraPos() + screenPoint;
		}

		/// <summary>
		/// Creates a camera game object and a sub window to render to.
		/// Add this camera as child to the object you want to follow, or 
		/// update its coordinates directly in an update method.
		/// The scale of the camera determines the "zoom factor" (High scale = zoom out)
		/// </summary>
		/// <param name="windowX">Left x coordinate of the render window.</param>
		/// <param name="windowY">Top y coordinate of the render window.</param>
		/// <param name="windowWidth">Width of the render window.</param>
		/// <param name="windowHeight">Height of the render window.</param>
		public Camera(int windowX, int windowY, int windowWidth, int windowHeight)
		{
			main = this;
			renderTarget = new Window(windowX, windowY, windowWidth, windowHeight, this);
			Gizmos.Instance = null;
			game.OnAfterRender += renderTarget.RenderWindow;



		}

		protected override void OnDestroy()
		{

			game.OnAfterRender -= renderTarget.RenderWindow;
			renderTarget = null;
			main = null;
		}

	}
}
