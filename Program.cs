using System;
using System.Threading;
using HomeSeerAPI;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.EndPoints.Tcp;
using HSCF.Communication.ScsServices.Client;

namespace HomeSeerSamplePlugin {

	internal class Program {

		private static SamplePlugin Plugin;
		
		private static IScsServiceClient<IHSApplication> Client;
		private static IScsServiceClient<IAppCallbackAPI> ClientCallback;

		private static IHSApplication Host;
		private static IAppCallbackAPI Callback;

		private const int HomeSeerPort = 10400;
		

		public static void Main(string[] args) {
			
			Plugin = new SamplePlugin();

			var ip = "127.0.0.1";
			foreach (var arg in args) {
				var parts = arg.Split('=');
				switch (parts[0].ToLower()) {
					case "server":
						ip = parts[1];
						break;
					case "instance":
						//TODO no more instances
						break;
				}
			}

			Client = ScsServiceClientBuilder.CreateClient<IHSApplication>(new ScsTcpEndPoint(ip, HomeSeerPort), Plugin);
			ClientCallback = ScsServiceClientBuilder.CreateClient<IAppCallbackAPI>(new ScsTcpEndPoint(ip, HomeSeerPort), Plugin);
			
			Connect(1);
		}

		private static void Connect(int attempts) {

			try {
				Client.Connect();
				ClientCallback.Connect();
				var apiVersion = 0D;

				try {
					Host       = Client.ServiceProxy;
					apiVersion = Host.APIVersion;
					Console.WriteLine("Host API Version: " + apiVersion);
				}
				catch (Exception exception) {
					Console.WriteLine(exception);
				}

				try {
					Callback   = ClientCallback.ServiceProxy;
					apiVersion = Callback.APIVersion;
				}
				catch (Exception exception) {
					Console.WriteLine(exception);
					return;
				}


			}
			catch (Exception exception) {
				Console.WriteLine(exception);
				Console.WriteLine("Cannot connect attempt " + attempts);
				if (exception.Message.ToLower().Contains("timeout occurred.") && attempts < 6) {
					Connect(attempts + 1);
					if (Client != null) {
						Client.Dispose();
						Client = null;
					}

					if (ClientCallback != null) {
						ClientCallback.Dispose();
						ClientCallback = null;
					}
					return;
				}
			}
			
			Thread.Sleep(4000); //?

			try {
				Plugin.HomeSeerSystem = Host;
				Host.Connect(Plugin.Name, "primary");
				do {
					Thread.Sleep(10);
				} while (Client.CommunicationState == CommunicationStates.Connected && !Plugin.IsShutdown);
				
				Client.Disconnect();
				ClientCallback.Disconnect();
				
				Thread.Sleep(2000); //?
			}
			catch (Exception exception) {
				Console.WriteLine(exception);
				throw;
			}
		}

	}

}