using System.Collections.Generic;

namespace HSPI_HomeSeerSamplePlugin.Constants {

    public static class Devices {
        
        public static List<string> SampleDeviceTypeList => new List<string> {
                                                                    "Line-powered switch",
                                                                    "Line-powered sensor"
                                                                };
        
        public static List<string[]> SampleDeviceTypeFeatures => new List<string[]>
                                                                     {
                                                                         LinePoweredSwitchFeatures,
                                                                         LinePoweredSensorFeatures
                                                                     };
        
        public static string[] LinePoweredSwitchFeatures => new []{ "On-Off control feature" };
        public static string[] LinePoweredSensorFeatures => new []{ "Open-Close status feature" };

    }

}