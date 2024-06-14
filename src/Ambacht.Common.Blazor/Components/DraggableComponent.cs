using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ambacht.Common.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Ambacht.Common.Blazor.Components
{
    public class DraggableComponent : AmbachtComponentBase
    {

        [CascadingParameter()]
        public DragContainer Container { get; set; }


        [Parameter()]
        public EventCallback<DragArgs> DragStarted { get; set; }

		[Parameter()]
        public EventCallback<DragArgs> Dragged { get; set; }

        [Parameter()]
        public EventCallback<DragArgs> DragEnded { get; set; }

        [Parameter()]
        public EventCallback<MouseEventArgs> MouseDown { get; set; }

		[Inject()]
        public IToastService Toast { get; set; }

        public virtual async Task OnDragStarted(DragArgs v)
        {
	        await DragStarted.InvokeAsync(v);
        }

		public virtual async Task OnDragged(DragArgs args)
        {
            await Dragged.InvokeAsync(args);
        }

		public virtual async Task OnDragEnded(DragArgs v)
		{
			await DragEnded.InvokeAsync(v);
		}

		protected virtual async Task OnMouseDown(MouseEventArgs e)
		{
			await MouseDown.InvokeAsync(e);
			await Container.OnMouseDown(this, e);
		}

		protected virtual async Task OnTouchStart(TouchEventArgs e)
        {
            await Container.OnTouchStart(this, e);
        }

    }

    public record class DragArgs
    {
		/// <summary>
		/// Position of event in pixel coordinates relative to drag container origin
		/// </summary>
	    public Vector2 PixelPosition { get; set; }

		/// <summary>
		/// Delta of event relative in pixel coordinates
		/// </summary>
	    public Vector2 PixelDelta { get; set; }


		/// <summary>
		/// Position of event in local coordinates
		/// </summary>
		public Vector2 LocalPosition { get; set; }

		/// <summary>
		/// Delta in local coordinates
		/// </summary>
		public Vector2 LocalDelta { get; set;}


	}
}
