using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk;
using HomeSeer.PluginSdk.Logging;
using Newtonsoft.Json;

namespace HSPI_HomeSeerSamplePlugin {

    /// <inheritdoc />
    /// <summary>
    /// The plugin class for HomeSeer Sample Plugin that implements the <see cref="AbstractPlugin"/> base class.
    /// </summary>
    /// <remarks>
    /// This class is accessed by HomeSeer and requires that its name be "HSPI" and be located in a namespace
    ///  that corresponds to the name of the executable. For this plugin, "HomeSeerSamplePlugin" the executable
    ///  file is "HSPI_HomeSeerSamplePlugin.exe" and this class is HSPI_HomeSeerSamplePlugin.HSPI
    /// <para>
    /// If HomeSeer is unable to find this class, the plugin will not start.
    /// </para>
    /// </remarks>
    // ReSharper disable once InconsistentNaming
    public class HSPI : AbstractPlugin, WriteLogSampleActionType.IWriteLogActionListener {


        #region Properties

        /// <inheritdoc />
        /// <remarks>
        /// This ID is used to identify the plugin and should be unique across all plugins
        /// <para>
        /// This must match the MSBuild property $(PluginId) as this will be used to copy
        ///  all of the HTML feature pages located in .\html\ to a relative directory
        ///  within the HomeSeer html folder.
        /// </para>
        /// <para>
        /// The relative address for all of the HTML pages will end up looking like this:
        ///  ..\Homeseer\Homeseer\html\HomeSeerSamplePlugin\
        /// </para>
        /// </remarks>
        public override string Id { get; } = "HomeSeerSamplePlugin";
        
        /// <inheritdoc />
        /// <remarks>
        /// This is the readable name for the plugin that is displayed throughout HomeSeer
        /// </remarks>
        public override string Name { get; } = "Sample Plugin";
        
        protected override string SettingsFileName { get; } = "HomeSeerSamplePlugin.ini";

        #endregion

        public HSPI() {
            //Initialize the plugin 

            //Enable internal debug logging to console
            LogDebug = true;
            //Setup anything that needs to be configured before a connection to HomeSeer is established
            // like initializing the starting state of anything needed for the operation of the plugin
            
            //Such as initializing the settings pages presented to the user (currently saved state is loaded later)
            InitializeSettingsPages();
            
            //Or adding an event action or trigger type definition to the list of types supported by your plugin
            ActionTypes.AddActionType(typeof(WriteLogSampleActionType));
            //TODO sample trigger
        }

        /// <summary>
        /// Initialize the starting state of the settings pages for the HomeSeerSamplePlugin.
        ///  This constructs the framework that the user configurable settings for the plugin live in.
        ///  Any saved configuration options are loaded later in <see cref="Initialize"/> using
        ///  <see cref="AbstractPlugin.LoadSettingsFromIni"/>
        /// </summary>
        /// <remarks>
        /// For ease of use throughout the plugin, all of the view IDs, names, and values (non-volatile data)
        ///  are stored in the <see cref="HSPI_HomeSeerSamplePlugin.Constants.Settings"/> static class.
        /// </remarks>
        private void InitializeSettingsPages() {
            //Initialize the first settings page
            // This page is used to manipulate the behavior of the sample plugin
            
            //Start a PageFactory to construct the Page
            var settingsPage1 = PageFactory.CreateSettingsPage(Constants.Settings.SettingsPage1Id, 
                                                               Constants.Settings.SettingsPage1Name);
            //Add a LabelView to the page
            settingsPage1.WithLabel(Constants.Settings.Sp1ColorLabelId, null, 
                                    Constants.Settings.Sp1ColorLabelValue);
            //Create a list of ToggleViews using the keys and values stored in Constants.Settings.ColorMap as
            // the IDs and Names respectively
            var colorToggles = Constants.Settings.ColorMap
                                        .Select(kvp => new ToggleView(kvp.Key, kvp.Value, true)
                                                       {ToggleType = EToggleType.Checkbox})
                                        .ToList();
            //Add a ViewGroup containing all of the ToggleViews to the page
            settingsPage1.WithGroup(Constants.Settings.Sp1ColorGroupId,
                                    Constants.Settings.Sp1ColorGroupName,
                                    colorToggles);
            //Create 2 ToggleViews for controlling the visibility of the other two settings pages
            var pageToggles = new List<ToggleView> {
                                  new ToggleView(Constants.Settings.Sp1PageVisToggle1Id, Constants.Settings.Sp1PageVisToggle1Name, true),
                                  new ToggleView(Constants.Settings.Sp1PageVisToggle2Id, Constants.Settings.Sp1PageVisToggle2Name, true),
                              };
            //Add a ViewGroup containing all of the ToggleViews to the page
            settingsPage1.WithGroup(Constants.Settings.Sp1PageToggleGroupId,
                                    Constants.Settings.Sp1PageToggleGroupName,
                                    pageToggles);
            //Add the first page to the list of plugin settings pages
            Settings.Add(settingsPage1.Page);
            
            //Initialize the second settings page
            // This page is used to visually demonstrate all of the available JUI views except for InputViews.
            // None of these views interact with the plugin and are merely for show.
            
            //Start a PageFactory to construct the Page
            var settingsPage2 = PageFactory.CreateSettingsPage(Constants.Settings.SettingsPage2Id, 
                                                               Constants.Settings.SettingsPage2Name);
            //Add a LabelView with a title to the page
            settingsPage2.WithLabel(Constants.Settings.Sp2LabelWTitleId, 
                                    Constants.Settings.Sp2LabelWTitleName, 
                                    Constants.Settings.Sp2LabelWTitleValue);
            //Add a LabelView without a title to the page
            settingsPage2.WithLabel(Constants.Settings.Sp2LabelWoTitleId,
                                    null,
                                    Constants.Settings.Sp2LabelWoTitleValue);
            //Add a toggle switch to the page
            settingsPage2.WithToggle(Constants.Settings.Sp2SampleToggleId, Constants.Settings.Sp2SampleToggleName);
            //Add a checkbox to the page
            settingsPage2.WithCheckBox(Constants.Settings.Sp2SampleCheckBoxId, Constants.Settings.Sp2SampleCheckBoxName);
            //Add a drop down select list to the page
            settingsPage2.WithDropDownSelectList(Constants.Settings.Sp2SelectListId,
                                         Constants.Settings.Sp2SelectListName,
                                         Constants.Settings.Sp2SelectListOptions);
            //Add a radio select list to the page
            settingsPage2.WithRadioSelectList(Constants.Settings.Sp2RadioSlId,
                                         Constants.Settings.Sp2RadioSlName,
                                         Constants.Settings.Sp2SelectListOptions);
            //Add the second page to the list of plugin settings pages
            Settings.Add(settingsPage2.Page);
            
            //Initialize the third settings page
            // This page is used to visually demonstrate the different types of JUI InputViews.
            
            //Start a PageFactory to construct the Page
            var settingsPage3 = PageFactory.CreateSettingsPage(Constants.Settings.SettingsPage3Id, Constants.Settings.SettingsPage3Name);
            //Add a text InputView to the page
            settingsPage3.WithInput(Constants.Settings.Sp3SampleInput1Id,
                                    Constants.Settings.Sp3SampleInput1Name);
            //Add a number InputView to the page
            settingsPage3.WithInput(Constants.Settings.Sp3SampleInput2Id,
                                    Constants.Settings.Sp3SampleInput2Name,
                                    EInputType.Number);
            //Add an email InputView to the page
            settingsPage3.WithInput(Constants.Settings.Sp3SampleInput3Id,
                                    Constants.Settings.Sp3SampleInput3Name,
                                    EInputType.Email);
            //Add a URL InputView to the page
            settingsPage3.WithInput(Constants.Settings.Sp3SampleInput4Id,
                                    Constants.Settings.Sp3SampleInput4Name,
                                    EInputType.Url);
            //Add a password InputView to the page
            settingsPage3.WithInput(Constants.Settings.Sp3SampleInput5Id,
                                    Constants.Settings.Sp3SampleInput5Name,
                                    EInputType.Password);
            //Add a decimal InputView to the page
            settingsPage3.WithInput(Constants.Settings.Sp3SampleInput6Id,
                                    Constants.Settings.Sp3SampleInput6Name,
                                    EInputType.Decimal);
            //Add the third page to the list of plugin settings pages
            Settings.Add(settingsPage3.Page);
        }

        protected override void Initialize() {
            //Load the state of Settings saved to INI if there are any.
            LoadSettingsFromIni();
            Console.WriteLine("Registering feature pages");
            //Initialize feature pages            
            HomeSeerSystem.RegisterFeaturePage(Id, "sample-guided-process.html", "Sample Guided Process");
            HomeSeerSystem.RegisterFeaturePage(Id, "sample-blank.html", "Sample Blank Page");
            //TODO convert this to a couple of buttons that interact with triggers
            HomeSeerSystem.RegisterFeaturePage(Id, "sample-plugin-feature.html", "Sample Feature Page 1");
            Console.WriteLine("Initialized");
            Status = PluginStatus.Ok();
        }

        protected override bool OnSettingChange(string pageId, AbstractView currentView, AbstractView changedView) {

            //React to the toggles that control the visibility of the last 2 settings pages
            if (changedView.Id == Constants.Settings.Sp1PageVisToggle1Id) {
                if (!(changedView is ToggleView tView)) {
                    return false;
                }
                
                if (tView.IsEnabled) {
                    Settings.ShowPageById(Constants.Settings.SettingsPage2Id);
                }
                else {
                    Settings.HidePageById(Constants.Settings.SettingsPage2Id);
                }
            }
            else if (changedView.Id == Constants.Settings.Sp1PageVisToggle2Id) {
                if (!(changedView is ToggleView tView)) {
                    return false;
                }
                
                if (tView.IsEnabled) {
                    Settings.ShowPageById(Constants.Settings.SettingsPage3Id);
                }
                else {
                    Settings.HidePageById(Constants.Settings.SettingsPage3Id);
                }
            }

            return true;
        }

        protected override void BeforeReturnStatus() {}
        
        //TODO document everything regarding PostBackProc better
        public override string PostBackProc(string page, string data, string user, int userRights) {
            Console.WriteLine("PostBack");
            //TODO expand on the response content
            var response = "";

            switch(page) {
                case "sample-plugin-feature.html":
                    // use default ajax handler to update 3 divs
                    response = "['timediv', '" + "Saved at " + DateTime.Now.ToString() + "','div2','Saved OK','main_content','']";
                    break;
                    
                case "sample-guided-process.html":
                    try {
                        var postData = JsonConvert.DeserializeObject<SampleGuidedProcessData>(data);

                        Console.WriteLine("Post back from sample-guided-process page");
                        response = postData.GetResponse();

                    }
                    catch (JsonSerializationException exception) {
                        Console.WriteLine(exception.Message);
                        response = "error";
                    }

                    break;
                default:
                    response = "error";
                    break;
            }
            return response;
        }

        /// <summary>
        /// Called by the sample guided process feature page through a liquid tag to provide the list of available colors
        /// </summary>
        /// <returns>The HTML for the list of select list options</returns>
        public string GetSampleSelectList() {
            Console.WriteLine("Getting sample select list for sample-guided-process page");
            var sb = new StringBuilder("<select class=\"mdb-select md-form\" id=\"step3SampleSelectList\">");
            sb.Append(Environment.NewLine);
            sb.Append("<option value=\"\" disabled selected>Color</option>");
            sb.Append(Environment.NewLine);
            
            var colorList = new List<string>();

            try {
                var colorSettings = Settings["settings-page2"].GetViewById("settings-page2.colorgroup");
                if (!(colorSettings is ViewGroup colorViewGroup)) {
                    throw new ViewTypeMismatchException("No View Group found containing colors");
                }

                foreach (var view in colorViewGroup.Views) {
                    if (!(view is ToggleView colorView)) {
                        continue;
                    }
                    
                    colorList.Add(colorView.IsEnabled ? colorView.Name : "");
                }
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                colorList = Constants.Settings.ColorMap.Values.ToList();
            }
           
            for (var i = 0; i < colorList.Count; i++) {
                var color = colorList[i];
                if (string.IsNullOrEmpty(color)) {
                    continue;
                }
                
                sb.Append("<option value=\"");
                sb.Append(i);
                sb.Append("\">");
                sb.Append(color);
                sb.Append("</option>");
                sb.Append(Environment.NewLine);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        /// <inheritdoc />
        public void WriteLog(ELogType logType, string message) {
            
            HomeSeerSystem.WriteLog(logType, message, Name);
        }

        //TODO clean these up

        /// <summary>
        /// sample property that returns a string
        /// on the HTML page call this with:
        /// </summary>
        public string MyCustomProperty { get; } = "Sample property";
        
        // sample functions and properties that can be called from HS, and a HTML page

        /// <summary>
        /// sample function with one parameter
        /// on the HTML page call this with:
        /// {{plugin_function 'HomeSeerSamplePlugin' 'MyCustomFunction' ['1']}}
        /// </summary>
        public string MyCustomFunction(string param) {
            return "1234";
        }

        public List<string> MyCustomFunctionArray(string param) {
            List<string> list = new List<string>();

            list.Add("item 1");
            list.Add("item 2");

            return list;

        }

        [Serializable]
        public class clsItem {
            public int    intItem;
            public string stringItem;
        }

        public List<clsItem> MyCustomFunctionArrayCustomClass(string param) {
            List<clsItem> list = new List<clsItem>();

            clsItem i1 = new clsItem();
            i1.intItem    = 1;
            i1.stringItem = "string item 1";
            list.Add(i1);

            clsItem i2 = new clsItem();
            i2.intItem    = 2;
            i2.stringItem = "string item 2";
            list.Add(i2);

            return list;

        }

    }

}