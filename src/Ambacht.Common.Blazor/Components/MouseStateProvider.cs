using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Blazor.Components
{


	/// <summary>
	/// The MouseStateProvider can be passed on as a cascading value. It is observable, so it can be monitored for mouse state changes.
	/// </summary>
	public class MouseStateProvider : IObservable<MouseState>
	{

		public MouseState Current
		{
			get => _state.Value;
			private set => _state.OnNext(value);
		}
		private readonly BehaviorSubject<MouseState> _state = new (new MouseState());

		public IDisposable Subscribe(IObserver<MouseState> observer) => _state.Subscribe(observer);

		#region Mouse event notifications

		public void NotifyMouseOver(MouseEventArgs args, object sender)
		{
			Current = Current with
			{
				Over = true
			};
		}

		public void NotifyMouseMove(MouseEventArgs args, object sender)
		{
			Current = Current with
			{
				Position = new MousePosition(
					new Vector2((float)args.ScreenX, (float)args.ScreenY),
					new Vector2((float)args.PageX, (float)args.PageY),
					new Vector2((float)args.ClientX, (float)args.ClientY),
					new Vector2((float)args.OffsetX, (float)args.OffsetY),
					new Vector2((float)args.MovementX, (float)args.MovementY)
				)
			};
		}

		public void NotifyMouseDown(MouseEventArgs args, object sender) => SetButtonState((MouseButton)args.Button, true);

		public void NotifyMouseUp(MouseEventArgs args, object sender) => SetButtonState((MouseButton)args.Button, false);

		private void SetButtonState(MouseButton button, bool value)
		{
			switch (button)
			{
				case MouseButton.Left:
					Current = Current with
					{
						IsLeftButtonPressed = value
					};
					break;
				case MouseButton.Middle:
					Current = Current with
					{
						IsMiddleButtonPressed = value
					};
					break;
				case MouseButton.Right:
					Current = Current with
					{
						IsRightButtonPressed = value
					};
					break;
			}
		}

		private Expression<Func<MouseState, bool>> GetButtonStateExpression(MouseButton button) => button switch
		{
			MouseButton.Left => state => state.IsLeftButtonPressed,
			MouseButton.Middle => state => state.IsMiddleButtonPressed,
			MouseButton.Right => state => state.IsRightButtonPressed,
			_ => throw new ArgumentException()
		};

		public void NotifyMouseOut(MouseEventArgs args, object sender)
		{
			Current = Current with
			{
				Over = false
			};
		}

		#endregion

		#region Event observables

		public IObservable<MousePosition> OnMove() => this.Select(s => s.Position).DistinctUntilChanged();

		public IObservable<Unit> OnLeave() => 
			this
				.Select(s => s.Over)
				.DistinctUntilChanged()
				.Where(v => v == false)
				.Select(v => Unit.Default);


		public IObservable<bool> OnButtonChange(MouseButton button) =>
			this
				.Select(GetButtonStateExpression(button).Compile())
				.DistinctUntilChanged();

		public IObservable<Unit> OnButtonUp(MouseButton button) =>
			OnButtonChange(button)
				.Where(v => v == false)
				.Select(_ => Unit.Default);

		public IObservable<Unit> OnButtonDown(MouseButton button) =>
			OnButtonChange(button)
				.Where(v => v == true)
				.Select(_ => Unit.Default);


		#endregion

	}
}
