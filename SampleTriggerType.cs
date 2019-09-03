using System;
using System.Collections.Generic;
using System.Linq;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk.Events;

namespace HSPI_HomeSeerSamplePlugin {

    public class SampleTriggerType : AbstractTriggerType {

        public const int TriggerNumber = 1;

        private const string TriggerName = "Sample Trigger";
        private string OptionCountSlId => $"{PageId}-optioncountsl";
        private const string OptionCountSlName = "Number of Options Checked";
        private string OptionNumSlId => $"{PageId}-optionnumsl";
        private const string OptionNumSlName = "Required Option";

        public bool ShouldTriggerFire(params bool[] triggerOptions) {
            switch (SelectedSubTriggerIndex) {
                case 0:
                    var numRequiredOptions = GetSelectedOptionCount() + 1;
                    return numRequiredOptions != 0 && triggerOptions.Any(triggerOption => triggerOption);
                case 1:
                    var specificRequiredOption = GetSelectedSpecificOptionNum();
                    if (triggerOptions.Length < specificRequiredOption + 1) {
                        return false;
                    }

                    return triggerOptions[specificRequiredOption];
                case 2:
                    return !triggerOptions.Any(triggerOption => triggerOption);
                case 3:
                    return triggerOptions.Any(triggerOption => triggerOption);
                default:
                    return false;
            }
        }
        
        public SampleTriggerType(int id, int eventRef, byte[] dataIn) : base(id, eventRef, dataIn) { }
        public SampleTriggerType() { }

        protected override List<string> SubTriggerTypeNames { get; set; } = new List<string>
                                                                            {
                                                                                "Button click with X options checked",
                                                                                "Button click with specific option checked",
                                                                                "Button click with no options checked",
                                                                                "Button click with any options checked"
                                                                                
                                                                            };

        protected override string GetName() => TriggerName;

        protected override void OnNewTrigger() {
            switch (SelectedSubTriggerIndex) {
                case 0:
                    ConfigPage = InitializeXOptionsPage().Page;
                    break;
                case 1:
                    ConfigPage = InitializeSpecificOptionPage().Page;
                    break;
                default:
                    ConfigPage = InitializeDefaultPage().Page;
                    break;
            }
        }

        public override bool IsFullyConfigured() {
            switch (SelectedSubTriggerIndex) {
                case 0:
                    //Check to see if the input for the number of options is valid
                    return GetSelectedSpecificOptionNum() >= 0;
                case 1:
                    //Check to see if the input for the required option is valid
                    return GetSelectedOptionCount() >= 0;
                default:
                    //The last two sub trigger types do not require any additional configuration
                    return true;
            }
        }

        protected override bool OnConfigItemUpdate(AbstractView configViewChange) {
            throw new System.NotImplementedException();
        }

        public override string GetPrettyString() {
            switch (SelectedSubTriggerIndex) {
                case 0:
                    try {
                        var optionCountSl = ConfigPage?.GetViewById(OptionCountSlId) as SelectListView;
                        return $"the button on the Sample Plugin Trigger Feature page is clicked and {(optionCountSl?.GetSelectedOption() ?? "???")} options are checked";
                    }
                    catch (Exception exception) {
                        Console.WriteLine(exception);
                        return $"the button on the Sample Plugin Trigger Feature page is clicked and ??? options are checked";
                    }
                case 1:
                    try {
                        var optionNumSl = ConfigPage?.GetViewById(OptionNumSlId) as SelectListView;
                        return $"the button on the Sample Plugin Trigger Feature page is clicked and option number {(optionNumSl?.GetSelectedOption() ?? "???")} is checked";
                    }
                    catch (Exception exception) {
                        Console.WriteLine(exception);
                        return $"the button on the Sample Plugin Trigger Feature page is clicked and option number ??? is checked";
                    }
                case 2:
                    return $"the button the Sample Plugin Trigger Feature page is clicked and no options are checked";
                default:
                    return $"the button the Sample Plugin Trigger Feature page is clicked";
            }
        }

        public override bool IsTriggerTrue(bool isCondition) => true;

        public override bool ReferencesDeviceOrFeature(int devOrFeatRef) => false;

        private PageFactory InitializeXOptionsPage() {
            var cpf = InitializeDefaultPage();
            cpf.WithDropDownSelectList(OptionCountSlId, OptionCountSlName, new[] {"1", "2", "3", "4"}.ToList());
            return cpf;
        }
        
        private PageFactory InitializeSpecificOptionPage() {
            var cpf = InitializeDefaultPage();
            cpf.WithDropDownSelectList(OptionNumSlId, OptionNumSlName, new[] {"1", "2", "3", "4"}.ToList());
            return cpf;
        }
        
        private PageFactory InitializeDefaultPage() {
            var cpf = PageFactory.CreateEventTriggerPage(PageId, TriggerName);
            return cpf;
        }

        private int GetSelectedSpecificOptionNum() {
            try {
                var optionNumSl = ConfigPage?.GetViewById(OptionNumSlId) as SelectListView;
                return optionNumSl?.Selection ?? -1;
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                return -1;
            }
        }

        private int GetSelectedOptionCount() {
            try {
                var optionCountSl = ConfigPage?.GetViewById(OptionCountSlId) as SelectListView;
                return (optionCountSl?.Selection ?? -1);
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                return -1;
            }
        }

    }

}