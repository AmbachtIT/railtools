using Ambacht.Common.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railtools.Interop.TrainPlayer;
using Railtools.Tracks.Layouts;

namespace Railtools.Test.Interop.TrainPlayer
{
	[TestFixture()]
	public class TrainPlayerTests
	{

		[Test()]
		public void ReadFile()
		{
			var result = ReadSampleLayout();
			Assert.That(result.Parts.Count, Is.EqualTo(291));

			var part = result.Parts[1];
			Assert.That(part.Type, Is.EqualTo("DoubleSlipswitch"));
			Assert.That(part.Id, Is.EqualTo(639));
			Assert.That(part.EndpointNrs.Count, Is.EqualTo(4));
			Assert.That(part.EndpointNrs[0], Is.EqualTo(3));
			Assert.That(part.EndpointNrs[1], Is.EqualTo(4));
			Assert.That(part.Drawings.Count, Is.EqualTo(4));
			Assert.That(part.Drawings[0], Is.TypeOf<TrainPlayerLine>());
			Assert.That(part.Drawings[2], Is.TypeOf<TrainPlayerArc>());

			Assert.That(result.Connections.Count, Is.EqualTo(299));

			Assert.That(result.Endpoints.Count, Is.EqualTo(623));
		}

		[Test()]
		public void ConvertFile()
		{
			var result = ConvertSampleLayout();
			Assert.That(result.Sections.Count, Is.EqualTo(291));
		}


		public static Layout ConvertSampleLayout()
		{
			return new TrainPlayerLayoutConverter(ReadSampleLayout()).Convert();
		}


		public static TrainPlayerLayout ReadSampleLayout()
		{
			using (var stream = File.OpenRead(SamplePath))
			{
				return new TrainPlayerReader().Read(stream);
			}
		}

		public static string SamplePath => SourceHelper.GetSourceRoot("test", "Railtools.Test", "Interop", "TrainPlayer", "TrainPlayerSample.xml");

	}
}
