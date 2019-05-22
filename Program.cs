using HomeSeer.PluginSdk;

namespace HomeSeerSamplePlugin {

	internal class Program {

		private static SamplePlugin Plugin;
		private static InstanceComManager ComManager;

		public static void Main(string[] args) {
			
			Plugin = new SamplePlugin();
			ComManager = new InstanceComManager(Plugin, args);

			ComManager.Connect();
		}

	}

}