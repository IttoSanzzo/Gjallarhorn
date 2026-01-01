using Gjallarhorn.Components.MusicComponent;

namespace Gjallarhorn.HttpServer {
	public class PlayerGenericCommand {
		public string		Command		{get;set;} = "";
		public string		TargetBot	{get; set;} = "";
		public string		UserId		{get;set;} = "";
		public string		GuildId		{get; set;} = "";
		public string?	TrackUrl	{get;set;} = null;
		public string?	ChannelId	{get;set;} = null;
		public long?		Position	{get;set;} = null;
		public bool			Priority	{get;set;} = true;
	}
	public class GjallarhornPostBody {
		public string?	TrackUrl				{get;set;} = null;
		public string		Command					{get;set;} = "";
		public string		UserId					{get;set;} = "";
		public string		Color						{get;set;} = "";
		public string?	Message					{get;set;} = "";
		public string?	ChannelId				{get;set;} = null;
	}
	public static class GjallarhornHttpServer {
		private static readonly HttpClient _httpClient	= new();
		private static readonly string ServerPortString	= Environment.GetEnvironmentVariable("HTTP_SERVER_PORT") ?? throw new InvalidOperationException("HTTP_SERVER_PORT not set");
		private static readonly int		 ServerPort				= int.Parse(ServerPortString);
		
		static GjallarhornHttpServer() {}

		public static Thread? OpenGjallarhornHttpServer() {
			Program.WriteLine("Opening http server...");
			var thread2 = new Thread(new ThreadStart(HttpServerThread));
			thread2.Start();
			return (thread2);
		}
		private static async void	HttpServerThread() {
			var app = await Task.Run(() => {
				var builder = WebApplication.CreateBuilder();
				builder.WebHost.ConfigureKestrel(options => {
					options.ListenAnyIP(ServerPort);
				});
				builder.Services.AddHttpClient();
				return builder.Build();
			});

			app.MapRoutes();

			Program.WriteLine("Http Server Ready...");
			app.Run();
		}
		private static void MapRoutes(this WebApplication app) {
			app.MapPost("/{guildId}/player", (Delegate)PostGenericCommandAsync);
		}

		static public async Task<IResult>	PostGenericCommandAsync(string guildId, HttpContext context) {
			var payload = await context.Request.ReadFromJsonAsync<PlayerGenericCommand>();
			if (payload is null)
				return Results.BadRequest("Payload was null.");
			try {
				payload.GuildId = guildId;
				payload.TargetBot = "Gjallarhorn";
				GjallarhornHttpServer.FunctionsSwitch(payload);
				return Results.Ok();
			} catch (Exception ex) {
				return Results.BadRequest($"Exception: {ex.Message}");
			}
		}
		private static async void					FunctionsSwitch(PlayerGenericCommand genericCommand) {
			var gCtx = new GjallarhornContext();
			await gCtx.GjallarhornContextAsync(genericCommand);
			gCtx.Data.WithResponse = false;
			await GjallarhornMusicCalls.TryCallAsync(gCtx);
		}
	}
}