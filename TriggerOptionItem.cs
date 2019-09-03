using System;

namespace HSPI_HomeSeerSamplePlugin {

    [Serializable]
    public class TriggerOptionItem {
        
        public int Id { get; set; }
        public string Name { get; set; }

        public TriggerOptionItem(int id, string name) {
            Id = id;
            Name = name;
        }

        public TriggerOptionItem() { }

    }

}