using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;
using Microsoft.AspNetCore.Components.Web;

namespace Ambacht.Common.Blazor.Components
{


	/// <summary>
	/// Describes the state (position, buttons, etc...) of the mouse. If you want to use it in your application,
	/// you need to add a MouseEventLayer component to your application to catch the events and a MouseStateProvider
	/// to retrieve the latest MouseState.
	/// </summary>
	public record class MouseState
	{

		/// <summary>
		/// Mouse position
		/// </summary>
		public MousePosition Position { get; init; }

		/// <summary>
		/// Is the left button currently pressed?
		/// </summary>
		public bool IsLeftButtonPressed { get; init; }

		/// <summary>
		/// Is the middle button currently pressed?
		/// </summary>
		public bool IsMiddleButtonPressed { get; init; }

		/// <summary>
		/// Is the right button currently pressed?
		/// </summary>
		public bool IsRightButtonPressed { get; init; }

		/// <summary>
		/// Is the cursor currently hovering over the layer intercepting the events
		/// </summary>
		public bool Over { get; init; }

		/// <summary>
		/// The component that last updated the mouse state
		/// </summary>
		public object Sender { get; set; }

	}

	public record struct MousePosition
	{
		public MousePosition(Vector2 screen, Vector2 page, Vector2 client, Vector2 offset, Vector2 movement)
		{
			this.Screen = screen;
			this.Page = page;
			this.Client = client;
			this.Offset = offset;
			this.Movement = movement;
		}

		/// <summary>
		/// Coordinates of the mouse pointer in global (screen) coordinates
		/// </summary>
		public Vector2 Screen { get; }

		/// <summary>
		/// Coordinates of the mouse pointer relative to the whole document
		/// </summary>
		public Vector2 Page { get; }

		/// <summary>
		/// Coordinates of the mouse pointer in local (DOM content) coordinates
		/// </summary>
		public Vector2 Client { get; }

		/// <summary>
		/// Coordinates of the mouse pointer in relative (target element) coordinates
		/// </summary>
		public Vector2 Offset { get; }

		/// <summary>
		/// Coordinates of the mouse pointer relative to the previous mouse position
		/// </summary>
		public Vector2 Movement { get; }

	}

	public enum MouseButton : long
	{
		Left = 0,
		Middle = 1,
		Right = 2,
	}
}
