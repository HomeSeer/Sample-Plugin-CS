using System.Collections.Generic;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk.Events;

namespace HSPI_HomeSeerSamplePlugin {

    public class SampleActionType : AbstractActionType {

        private const string ActionName = "Sample Plugin Sample Action";
        private string SelectListId => $"{PageId}-selectlist1";
        private string InputId => $"{PageId}-input";
        private List<string> SelectListOptions = new List<string>
                                                 {
                                                     "Option1",
                                                     "Option2",
                                                     "Input"
                                                 };

        public SampleActionType(int id, int eventRef, byte[] dataIn) : base(id, eventRef, dataIn) { }
        public SampleActionType() { }

        public override bool IsFullyConfigured() {
            switch (ConfigPage.ViewCount) {
                case 1: {
                    var selectList = ConfigPage.GetViewById(SelectListId) as SelectListView;
                    return selectList?.GetSelectedOption() != "Input";
                }
                case 2: {
                    var inputView = ConfigPage.GetViewById(InputId) as InputView;
                    return (inputView?.Value.Length ?? 0) > 0;
                }
                default:
                    return false;
            }
        }

        public override string GetPrettyString() {
            //TODO create a pretty string builder
            var selectList = ConfigPage.GetViewById(SelectListId) as SelectListView;
            return $"run the action with {selectList?.GetSelectedOption()} selection";
        }

        public override bool OnRunAction() {
            //TODO move to HSPI (AbstractPlugin)
            return true;
        }

        public override bool ReferencesDeviceOrFeature(int devOrFeatRef) {
            return false;
        }

        protected override void OnEditAction(Page viewChanges) {
            foreach (var changedView in viewChanges.Views) {

                if (!ConfigPage.ContainsViewWithId(changedView.Id)) {
                    continue;
                }
                
                ConfigPage.UpdateViewById(changedView);
                if (changedView.Id == SelectListId) {
                    var selectList = ConfigPage.GetViewById(changedView.Id) as SelectListView;
                    var selection = selectList?.Selection ?? 0;
                    switch (selectList?.GetSelectedOption()) {
                        case "Input":
                            if (ConfigPage.ViewCount == 2) {
                                break;
                            }
                            var inputview = new InputView(InputId, "Sample Input");
                            ConfigPage.AddView(inputview);
                            break;
                        default:
                            if (ConfigPage.ViewCount != 2) {
                                break;
                            }
                            InitializePage();
                            InitActionPage(selection);
                            break;
                    }
                }
            }
        }

        protected override string GetName() {
            return ActionName;
        }

        protected override void OnNewAction() {
            InitActionPage();
        }

        private void InitActionPage(int selection = 0) {
            var selectList1 = new SelectListView(SelectListId, "Action Option", SelectListOptions);
            selectList1.Selection = selection;
            ConfigPage.AddView(selectList1);
        }

    }

}