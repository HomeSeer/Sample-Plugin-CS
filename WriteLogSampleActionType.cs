using System.Collections.Generic;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk.Events;
using HomeSeer.PluginSdk.Logging;

namespace HSPI_HomeSeerSamplePlugin {

    public class WriteLogSampleActionType : AbstractActionType {

        private const string ActionName = "Sample Plugin Sample Action - Write to Log";

        private string InstructionsLabelId => $"{PageId}-instructlabel";
        private const string InstructionsLabelValue = "Write a message to the log with a type of...";
        private string LogTypeSelectListId => $"{PageId}-logtypesl";
        private string LogMessageInputId => $"{PageId}-messageinput";
        private List<string> LogTypeOptions = new List<string>
                                                 {
                                                     "Trace",
                                                     "Debug",
                                                     "Info",
                                                     "Warning",
                                                     "Error"
                                                 };

        private IWriteLogActionListener _listener => ActionListener as IWriteLogActionListener;

        public WriteLogSampleActionType(int id, int eventRef, byte[] dataIn) : base(id, eventRef, dataIn) { }
        public WriteLogSampleActionType() { }

        public override bool IsFullyConfigured() {
            switch (ConfigPage.ViewCount) {
                case 3: 
                    var inputView = ConfigPage.GetViewById(LogMessageInputId) as InputView;
                    return (inputView?.Value?.Length ?? 0) > 0;
                default:
                    return false;
            }
        }

        public override string GetPrettyString() {
            //TODO create a pretty string builder
            var selectList = ConfigPage.GetViewById(LogTypeSelectListId) as SelectListView;
            var message = ConfigPage?.GetViewById(LogMessageInputId)?.GetStringValue() ?? "Error retrieving log message";
            return $"write the message \"{message}\" to the log with the type of {selectList?.GetSelectedOption() ?? "Unknown Selection"}";
        }

        public override bool OnRunAction() {
            var iLogType = (ConfigPage?.GetViewById(LogTypeSelectListId) as SelectListView)?.Selection ?? 0;
            var logType = ELogType.Info;
            switch (iLogType) {
                case 0:
                    logType = ELogType.Trace;
                    break;
                case 1:
                    logType = ELogType.Debug;
                    break;
                case 2:
                    logType = ELogType.Info;
                    break;
                case 3:
                    logType = ELogType.Warning;
                    break;
                case 4:
                    logType = ELogType.Error;
                    break;
                default:
                    logType = ELogType.Info;
                    break;
                    
            }

            var message = ConfigPage?.GetViewById(LogMessageInputId)?.GetStringValue() ?? "Error retrieving log message";
            _listener?.WriteLog(logType, message);
            return true;
        }

        public override bool ReferencesDeviceOrFeature(int devOrFeatRef) {
            return false;
        }

        protected override bool OnConfigItemUpdate(AbstractView configViewChange) {
            if (configViewChange.Id != LogTypeSelectListId) {
                return true;
            }
            
            //Log Type selection change
            if (!(configViewChange is SelectListView changedLogTypeSl)) {
                return false;
            }

            if (!(ConfigPage.GetViewById(LogTypeSelectListId) is SelectListView currentLogTypeSl)) {
                return false;
            }
                
            if (currentLogTypeSl.Selection == changedLogTypeSl.Selection) {
                return false;
            }

            var newConfPage = InitConfigPageWithInput();
            ConfigPage = newConfPage.Page;

            return true;
        }

        protected override string GetName() {
            return ActionName;
        }

        protected override void OnNewAction() {
            var confPage = InitNewConfigPage();
            ConfigPage = confPage.Page;
        }

        private PageFactory InitNewConfigPage() {
            var confPage = PageFactory.CreateEventActionPage(PageId, ActionName);
            confPage.WithLabel(InstructionsLabelId, null, InstructionsLabelValue);
            confPage.WithDropDownSelectList(LogTypeSelectListId, "Log Type", LogTypeOptions);
            return confPage;
        }
        
        private PageFactory InitConfigPageWithInput() {
            var confPage = InitNewConfigPage();
            confPage.WithInput(LogMessageInputId, "Message");
            return confPage;
        }
        
        public interface IWriteLogActionListener : ActionTypeCollection.IActionTypeListener {

            void WriteLog(ELogType logType, string message);

        }

    }

}