using Newtonsoft.Json;

namespace HSPI_HomeSeerSamplePlugin {

    [JsonObject]
    public class SampleGuidedProcessData {

        [JsonProperty("textValue")]
        public string TextValue { get; set; }
            
        [JsonProperty("colorIndex")]
        public int ColorIndex { get; set; }
        
    }

}