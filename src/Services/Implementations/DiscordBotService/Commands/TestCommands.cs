using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Trees.Metadata;
using DSharpPlus.Entities;

namespace Gjallarhorn.Services.Commands {
	[Command("testing")]
	[Description("Test Commands")]
	public class TestCommands {
		[Command("test1"), DefaultGroupCommand]
		[Description("Tests if Gjallarhorn is online and running correctly.")]
		public static async Task TestAsync(CommandContext ctx) {
			await ctx.RespondAsync("Hello World!");
		}
		[Command("test2")]
		[Description("Just Because.")]
		public static async Task Testing(CommandContext ctx, [Description("Really, just anything.")] string Anything) {
			await ctx.DeferResponseAsync();
			await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Hello, {Anything}."));
		}
	}
}
