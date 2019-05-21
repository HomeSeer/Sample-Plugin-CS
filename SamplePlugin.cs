using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using HomeSeerAPI;

namespace HomeSeerSamplePlugin {

    public class SamplePlugin : HomeSeerAPI.IPluginApi2 {
        
        
        #region Properties
        
        public bool IsShutdown { get; private set; }

        private List<Page> _settingsPages;
        private Dictionary<string, int> _settingsPageIndexes;
        //TODO feature pages
        
        public bool   HSCOMPort          { get; } = false;
        public string ID                 { get; } = "homeseer-sample-plugin";
        public string Name               { get; } = "Sample Plugin";
        public bool   ActionAdvancedMode { get; set; }
        //TODO HasTriggers
        public bool   HasTriggers        { get; } = false;
        //TODO TriggerCount
        public int    TriggerCount       { get; } = 0;
        
        public IHSApplication HomeSeerSystem { get; set; }

        #endregion
        
        #region Constructor
        
        public SamplePlugin() {
            _settingsPages = new List<Page>();
            _settingsPageIndexes = new Dictionary<string, int>();
            //TODO load from INI config file
            //Initialize settings pages
            
            //Build Settings Page 1
            var pageId = "settings-page1";
            var settingsPage1 = Page.Factory.CreateSettingPage(pageId, "Page 1");
            var sampleToggle1 = new ToggleView(new StringBuilder(pageId).Append(".sampletoggle1").ToString(),
                                              "Sample Toggle 1");
            var sampleLabel1 = new LabelView(new StringBuilder(pageId).Append(".samplelabel1").ToString(),
                                             "Sample Label 1",
                                             "This is a sample label with a title");
            var sampleSelectListOptions = new List<string> {"Option 1", "Option 2", "Option 3"};
            var sampleSelectList1 =
                new SelectListView(new StringBuilder(pageId).Append(".sampleselectlist1").ToString(),
                                   "Sample Select List 1",
                                   sampleSelectListOptions);
            var sampleButton1 = new ButtonView(new StringBuilder(pageId).Append(".samplebutton1").ToString(),
                                               "Sample Button 1",
                                               "samplebutton1");
            
            settingsPage1.AddView(sampleLabel1);
            settingsPage1.AddView(sampleToggle1);
            settingsPage1.AddView(sampleSelectList1);
            settingsPage1.AddView(sampleButton1);
            _settingsPageIndexes.Add(settingsPage1.Id, _settingsPages.Count);
            _settingsPages.Add(settingsPage1);
            //Build Settings Page 2
            pageId = "settings-page2";
            var settingsPage2 = Page.Factory.CreateSettingPage(pageId, "Page 2");
            var sampleToggle2 = new ToggleView(new StringBuilder(pageId).Append(".sampletoggle2").ToString(),
                                               "Sample Toggle 2");
            var sampleToggle3 = new ToggleView(new StringBuilder(pageId).Append(".sampletoggle3").ToString(),
                                               "Sample Toggle 3");
            var sampleToggle4 = new ToggleView(new StringBuilder(pageId).Append(".sampletoggle4").ToString(),
                                               "Sample Toggle 4");
            var sampleViewGroup1 = new ViewGroup(new StringBuilder(pageId).Append(".sampleviewgroup1").ToString(),
                                                 "Sample View Group 1");
            sampleViewGroup1.AddView(sampleToggle2);
            sampleViewGroup1.AddView(sampleToggle3);
            sampleViewGroup1.AddView(sampleToggle4);
            var sampleLabel2 = new LabelView(new StringBuilder(pageId).Append(".samplelabel2").ToString(),
                                             null,
                                             "This is a sample label without a title");
            var sampleSelectList2 =
                new SelectListView(new StringBuilder(pageId).Append(".sampleselectlist2").ToString(),
                                   "Sample Select List 2",
                                   sampleSelectListOptions, ESelectListType.RadioList, 0);
            
            settingsPage2.AddView(sampleViewGroup1);
            settingsPage2.AddView(sampleLabel2);
            settingsPage2.AddView(sampleSelectList2);
            _settingsPageIndexes.Add(settingsPage2.Id, _settingsPages.Count);
            _settingsPages.Add(settingsPage2);
            //Build Settings Page 3
            pageId = "settings-page3";
            var settingsPage3 = Page.Factory.CreateSettingPage(pageId, "Page 3");
            var sampleInput1 = new InputView(new StringBuilder(pageId).Append(".sampleinput1").ToString(),
                                             "Sample Input 1");
            var sampleInput2 = new InputView(new StringBuilder(pageId).Append(".sampleinput2").ToString(),
                                             "Sample Input 2", EInputType.Number);
            var sampleInput3 = new InputView(new StringBuilder(pageId).Append(".sampleinput3").ToString(),
                                             "Sample Input 3", EInputType.Email);
            var sampleInput4 = new InputView(new StringBuilder(pageId).Append(".sampleinput4").ToString(),
                                             "Sample Input 4", EInputType.Url);
            var sampleInput5 = new InputView(new StringBuilder(pageId).Append(".sampleinput5").ToString(),
                                             "Sample Input 5", EInputType.Password);
            var sampleInput6 = new InputView(new StringBuilder(pageId).Append(".sampleinput6").ToString(),
                                             "Sample Input 6", EInputType.Decimal);
            
            settingsPage3.AddView(sampleInput1);
            settingsPage3.AddView(sampleInput2);
            settingsPage3.AddView(sampleInput3);
            settingsPage3.AddView(sampleInput4);
            settingsPage3.AddView(sampleInput5);
            settingsPage3.AddView(sampleInput6);
            _settingsPageIndexes.Add(settingsPage3.Id, _settingsPages.Count);
            _settingsPages.Add(settingsPage3);
            //Initialize feature pages
        }
        
        #endregion
        
        #region Startup and Shutdown
        
        public string InitIO(string port) {
            var result = "";
            try {
                //TODO Load config/saved state from file
                //register settings pages
                HomeSeerSystem.RegisterJuiSettingsPages(_settingsPages.ToDictionary(p => p.Id, p => p.Name), ID);
                //TODO register feature pages
                //TODO Events
                //TODO Sample Devices
                //TODO Listen for Device Changes
                //TODO Images
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                IsShutdown = true;
                result     = "Error on InitIO: " + exception.Message;
            }
            
            return result;
        }
        
        public void ShutdownIO() {
            
            IsShutdown = true;
        }
        
        #endregion
        
        #region Settings
        
        public List<string> GetJuiSettingsPages() {
            var jsonSettingsPages = _settingsPages.Select(p => p.ToJsonString()).ToList();
            return jsonSettingsPages;
        }
        
        public List<string> SaveJuiSettingsPages(List<string> pages) {
            foreach (var jsonPageDelta in pages) {
                var pageDelta = Page.Factory.FromJsonString(jsonPageDelta);
                var page      = _settingsPages[_settingsPageIndexes[pageDelta.Id]];
                foreach (var settingDelta in pageDelta.Views) {
                    //TODO process settings changes
                    page.UpdateViewById(settingDelta);
                }

                //Make sure the new state is stored
                _settingsPages[_settingsPageIndexes[pageDelta.Id]] = page;
                //TODO save to disk
            }
            
            //Return the new state of the settings pages
            var jsonSettingsPages = _settingsPages.Select(p => p.ToJsonString()).ToList();
            return jsonSettingsPages;
        }
        
        #endregion
        
        #region Configuration and Status Info

        public int AccessLevel() => 1;

        public int Capabilities() => (int) Enums.eCapabilities.CA_IO;
        
        public string InstanceFriendlyName() => Name;
        
        public bool RaisesGenericCallbacks() {
            //TODO assess this method
            return false;
        }

        public bool SupportsAddDevice() {
            //TODO device add screen
            return false;
        }

        public bool SupportsConfigDevice() {
            //TODO device config screen
            return false;
        }

        public bool SupportsConfigDeviceAll() => false;

        public bool SupportsMultipleInstances() => false;

        public bool SupportsMultipleInstancesSingleEXE() => false;

        public int ActionCount() {
            //TODO ActionCount
            return 0;
        }
        
        public bool SupportsConfigDeviceJui() => true;
        
        public IPlugInAPI.strInterfaceStatus InterfaceStatus() {
            //TODO handle status
            var intStat = new IPlugInAPI.strInterfaceStatus {intStatus = IPlugInAPI.enumInterfaceStatus.OK};
            return intStat;
        }
        
        #endregion
        
        #region Devices

        public string ConfigDevice(int @ref, string user, int userRights, bool newDevice) {
            //TODO ConfigDevice
            return "Intentionally blank";
        }

        public Enums.ConfigDevicePostReturn ConfigDevicePost(int @ref, string data, string user, int userRights) {
            //TODO ConfigDevicePost
            return Enums.ConfigDevicePostReturn.DoneAndCancelAndStay;
        }
        
        public IPlugInAPI.PollResultInfo PollDevice(int dvref) {
            //TODO PollDevice
            var pollResult = new IPlugInAPI.PollResultInfo {Result = IPlugInAPI.enumPollResult.OK};
            return pollResult;
        }

        public void SetIOMulti(List<CAPI.CAPIControl> colSend) {

            foreach (var control in colSend) {
                //TODO process the device update
            }
        }
        
        public string GetJuiDeviceConfigPage(string deviceRef) {
            throw new System.NotImplementedException();
        }
        
        public string SaveJuiDeviceConfigPage(string pageContent, int deviceRef) {
            throw new System.NotImplementedException();
        }
        
        #endregion
        
        #region Features

        public string GetPagePlugin(string page, string user, int userRights, string queryString) {
            //TODO GetPagePlugin
            return "";
        }
        
        public string PostBackProc(string page, string data, string user, int userRights) {
            //TODO PostBackProc
            return "";
        }
        
        public string ExecuteActionById(string actionId, Dictionary<string, string> @params) {
            //TODO ExecuteActionById
            return "";
        }

        public string GetJuiPagePlugin(string pageId) {
            //TODO GetJuiPagePlugin
            return "";
        }
        
        public string SaveJuiPage(string pageContent) {
            //TODO SaveJuiPage
            return "";
        }
        
        #endregion
        
        #region Events

        public void HSEvent(Enums.HSEvent EventType, object[] parms) {
            //TODO process events
        }
        
        public bool ActionReferencesDevice(IPlugInAPI.strTrigActInfo ActInfo, int dvRef) {
            //TODO ActionReferencesDevice
            return false;
        }
        
        public string ActionBuildUI(string sUnique, IPlugInAPI.strTrigActInfo ActInfo) {
            //TODO ActionBuildUI
            return "";
        }

        public bool ActionConfigured(IPlugInAPI.strTrigActInfo ActInfo) {
            //TODO ActionConfigured
            return false;
        }

        public string ActionFormatUI(IPlugInAPI.strTrigActInfo ActInfo) {
            //TODO ActionFormatUI
            return "";
        }

        public IPlugInAPI.strMultiReturn ActionProcessPostUI(NameValueCollection PostData, IPlugInAPI.strTrigActInfo TrigInfoIN) {
            //TODO ActionProcessPostUI
            var result = new IPlugInAPI.strMultiReturn
                         {
                             sResult = "", DataOut = TrigInfoIN.DataIn, TrigActInfo = TrigInfoIN
                         };
            return result;
        }

        public bool HandleAction(IPlugInAPI.strTrigActInfo ActInfo) {
            //TODO HandleAction
            return false;
        }

        public string TriggerBuildUI(string sUnique, IPlugInAPI.strTrigActInfo TrigInfo) {
            //TODO TriggerBuildUI
            return "";
        }

        public string TriggerFormatUI(IPlugInAPI.strTrigActInfo TrigInfo) {
            //TODO TriggerFormatUI
            return "";
        }

        public IPlugInAPI.strMultiReturn TriggerProcessPostUI(NameValueCollection PostData, IPlugInAPI.strTrigActInfo TrigInfoIN) {
            //TODO TriggerProcessPostUI
            var result = new IPlugInAPI.strMultiReturn
                         {
                             sResult = "", DataOut = TrigInfoIN.DataIn, TrigActInfo = TrigInfoIN
                         };
            return result;
        }

        public bool TriggerReferencesDevice(IPlugInAPI.strTrigActInfo TrigInfo, int dvRef) {
            //TODO TriggerReferencesDevice
            return false;
        }

        public bool TriggerTrue(IPlugInAPI.strTrigActInfo TrigInfo) {
            //TODO TriggerTrue
            return true;
        }
        
        public string get_ActionName(int ActionNumber) {
            //TODO get_ActionName
            return null;
        }

        public bool get_Condition(IPlugInAPI.strTrigActInfo TrigInfo) {
            //TODO get_Condition
            return false;
        }

        public void set_Condition(IPlugInAPI.strTrigActInfo TrigInfo, bool Value) {
            //TODO set_Condition
        }

        public bool get_HasConditions(int TriggerNumber) {
            //TODO get_HasConditions
            return false;
        }
        
        public int get_SubTriggerCount(int TriggerNumber) {
            //TODO get_SubTriggerCount
            return 0;
        }

        public string get_SubTriggerName(int TriggerNumber, int SubTriggerNumber) {
            //TODO get_SubTriggerName
            return null;
        }

        public bool get_TriggerConfigured(IPlugInAPI.strTrigActInfo TrigInfo) {
            //TODO get_TriggerConfigured
            return false;
        }
        
        public string get_TriggerName(int TriggerNumber) {
            //TODO get_TriggerName
            return null;
        }
        
        #endregion

        #region Deprecated

        public string PagePut(string data) {
            return null;
        }
        
        public string GenPage(string link) {
            return null;
        }

        #endregion

        #region Dynamic Method/Property Calls

        public object PluginFunction(string procName, object[] parms) {
            //TODO PluginFunction
            return null;
        }

        public object PluginPropertyGet(string procName, object[] parms) {
            //TODO PluginPropertyGet
            return null;
        }

        public void PluginPropertySet(string procName, object value) {
            //TODO PluginPropertySet
            return;
        }

        #endregion

        public SearchReturn[] Search(string SearchString, bool RegEx) {
            //TODO Search
            var searchResults = new List<SearchReturn>();
            return searchResults.ToArray();
        }

        public void SpeakIn(int device, string txt, bool w, string host) {
            //TODO SpeakIn
        }
        
    }

}