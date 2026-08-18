using System.Diagnostics;

namespace ListaDeTarefa.Application.Commom
{
	public static class TelemetryHelper
	{
		private static readonly ActivitySource _activitySource = new("ListaDeTarefa.Telemetria");

		public static string ActivitySourceName { get => _activitySource.Name; }

		public static Activity? IniciaRequisicao(string nomeOperacao, IDictionary<string, object?>? tags = null)
		{
			var activity = _activitySource.StartActivity(nomeOperacao, ActivityKind.Server);
			activity.AdicionaTags(tags);
			return activity;
		}

		public static void AdicionaTag(this Activity? activity, string chave, object? valor)
		{
			activity?.SetTag(chave, valor);
		}

		public static void AdicionaTags(this Activity? activity, IDictionary<string, object?>? tags)
		{
			if (activity is null || tags is null)
				return;

			foreach (var (chave, valor) in tags)
				activity.SetTag(chave, valor);
		}

		public static void RegistraSucesso(this Activity? activity, string? mensagem = null)
		{
			activity?.SetStatus(ActivityStatusCode.Ok, mensagem);
		}

		public static void RegistraErro(this Activity? activity, Exception excecao)
		{
			if (activity is null)
				return;

			activity.SetStatus(ActivityStatusCode.Error, excecao.Message);
			activity.AdicionaTags(new Dictionary<string, object?>
			{
				["error"] = true,
				["error.type"] = excecao.GetType().FullName,
				["error.stacktrace"] = excecao.StackTrace
			});
		}

	}
}
