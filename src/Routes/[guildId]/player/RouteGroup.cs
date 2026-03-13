using Gjallarhorn.Routes;

namespace Gjallarhorn.Components.Gjallar {
	file class Route() : WithFilePath(), IRouteGroup {
		public string[] ExtraRouteBases { get; } = [];
		public RouteGroupBuilder Configure(RouteGroupBuilder builder)
			=> builder
			.WithName("Player Routes")
			.WithTags("Player Routes")
			// .WithDescription("")
			;
	}
}
