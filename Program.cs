using HomeSeer.PluginSdk;

namespace HSPI_HomeSeerSamplePlugin {

	internal class Program {

		private static HSPI Plugin;
		private static InstanceComManager ComManager;

		public static void Main(string[] args) {
			
			Plugin = new HSPI();
			ComManager = new InstanceComManager(Plugin, args);

			ComManager.Connect();
		}

	}

}