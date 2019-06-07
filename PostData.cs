using Newtonsoft.Json;

namespace HSPI_HomeSeerSamplePlugin {

    [JsonObject]
    public class PostData {

        [JsonProperty("pageId")]
        public string PageId { get; set; }
        
        [JsonProperty("data")]
        public string Data { get; set; }
        
        [JsonIgnore]
        public SampleInternalData InternalData { get; set; }

        [JsonObject]
        public class SampleInternalData {

            [JsonProperty("textValue")]
            public string TextValue { get; set; }
            
            [JsonProperty("colorIndex")]
            public int ColorIndex { get; set; }

        }
        
    }

}