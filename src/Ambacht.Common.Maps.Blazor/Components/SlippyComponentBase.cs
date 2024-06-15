using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Blazor;
using Ambacht.Common.Blazor.Components;
using Ambacht.Common.Blazor.Utils;
using Ambacht.Common.Maps.Projections;
using Ambacht.Common.Maps.Tiles;
using Microsoft.AspNetCore.Components;

namespace Ambacht.Common.Maps.Blazor.Components
{
	public class SlippyComponentBase : AmbachtComponentBase
	{
		public SlippyComponentBase()
		{
			_class.OnChangeUntil(_ => OnDirty(), Disposed);
			_enableMouseEvents.OnChangeUntil(_ => OnDirty(), Disposed);
			_view.OnChangeUntil(_ => OnDirty(), Disposed);
			_projection.OnChangeUntil(_ => OnDirty(), Disposed);
		}


		[Parameter()]
		public string Class
		{
			get => _class.Value;
			set => _class.OnNext(value);
		}
		protected BehaviorSubject<string> _class = new(null);



		[Parameter()]
		public EventCallback OnClick { get; set; }



		[Parameter()]
		public bool EnableMouseEvents
		{
			get => _enableMouseEvents.Value;
			set => _enableMouseEvents.OnNext(value);
		}
		protected BehaviorSubject<bool> _enableMouseEvents = new(false);

		protected override void OnAfterRender(bool firstRender)
		{
			base.OnAfterRender(firstRender);
			_hasRendered = true;
		}

		private bool _hasRendered = false;


		protected string Style => new CssStyleBuilder()
			.Add("vector-effect", "non-scaling-stroke")
			.Add("pointer-events", "none", !EnableMouseEvents)
			.Add("cursor", "pointer", EnableMouseEvents)
			.Build();



		/// <summary>
		/// If this is specified, coordinates are interpreted as cartesian and converted to lat/lng using this projection. If not,
		/// the coordinates are assumed to be lat/lng
		/// </summary>
		[CascadingParameter()]
		public Projection Projection
		{
			get => _projection.Value;
			set => _projection.OnNext(value);
		}
		protected BehaviorSubject<Projection> _projection = new(null);



		[CascadingParameter()]
		public SlippyMapView View
		{
			get => _view.Value;
			set => _view.OnNext(value);
		}
		protected BehaviorSubject<SlippyMapView> _view = new(default);

		protected virtual void OnDirty()
		{
			if (_hasRendered)
			{
				_dirty = true;
				StateHasChanged();
			}
		}

		private bool _dirty = true;

		protected override bool ShouldRender()
		{
			if (_dirty)
			{
				_dirty = false;
				return true;
			}

			return false;
		}

	}
}
