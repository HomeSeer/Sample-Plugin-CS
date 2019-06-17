using System;
using System.Collections.Generic;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk;
using Newtonsoft.Json;

// ReSharper disable IdentifierTypo

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
    public class HSPI : AbstractPlugin {


        #region Properties
        
        /// <summary>
        /// The list of colors used throughout the plugin
        /// </summary>
        public static List<string> ColorList { get; } = new List<string> {
                                                                             "Red",
                                                                             "Orange",
                                                                             "Yellow",
                                                                             "Green",
                                                                             "Blue",
                                                                             "Indigo",
                                                                             "Violet"
                                                                         };

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
            //Setup anything that needs to be configured before a connection to HomeSeer is established
            // like building the settings pages
            
            //Build Settings Page 1
            var pageId = "settings-page1";
            var settingsPage1 = Page.Factory.CreateSettingPage(pageId, "Page 1");
            var sampleToggle1 = new ToggleView(new StringBuilder(pageId).Append(".sampletoggle1").ToString(),
                                              "Sample Toggle 1");
            var sampleLabel1 = new LabelView(new StringBuilder(pageId).Append(".samplelabel1").ToString(),
                                             "Sample Label 1",
                                             "This is a sample label with a title");
            var sampleSelectListOptions = new List<string> { "Option 1", "Option 2", "Option 3" };
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
            Settings.Add(settingsPage1);
            //Build Settings Page 2
            pageId = "settings-page2";
            var settingsPage2 = Page.Factory.CreateSettingPage(pageId, "Page 2");
            var sampleLabel2 = new LabelView(new StringBuilder(pageId).Append(".colorlabel").ToString(),
                                             null,
                                             "These control the list of colors presented for selection in the Sample Guided Process feature page.");
            var redToggle = new ToggleView(new StringBuilder(pageId).Append(".red").ToString(),
                                               "Red", true);
            var orangeToggle = new ToggleView(new StringBuilder(pageId).Append(".orange").ToString(),
                                               "Orange", true);
            var yellowToggle = new ToggleView(new StringBuilder(pageId).Append(".yellow").ToString(),
                                               "Yellow", true);
            var greenToggle = new ToggleView(new StringBuilder(pageId).Append(".green").ToString(),
                                              "Green", true);
            var blueToggle = new ToggleView(new StringBuilder(pageId).Append(".blue").ToString(),
                                              "Blue", true);
            var indigoToggle = new ToggleView(new StringBuilder(pageId).Append(".indigo").ToString(),
                                              "Indigo", true);
            var violetToggle = new ToggleView(new StringBuilder(pageId).Append(".violet").ToString(),
                                              "Violet", true);
            var sampleViewGroup1 = new ViewGroup(new StringBuilder(pageId).Append(".colorgroup").ToString(),
                                                 "Available colors");
            sampleViewGroup1.AddView(redToggle);
            sampleViewGroup1.AddView(orangeToggle);
            sampleViewGroup1.AddView(yellowToggle);
            sampleViewGroup1.AddView(greenToggle);
            sampleViewGroup1.AddView(blueToggle);
            sampleViewGroup1.AddView(indigoToggle);
            sampleViewGroup1.AddView(violetToggle);
            var sampleLabel3 = new LabelView(new StringBuilder(pageId).Append(".samplelabel3").ToString(),
                                             null,
                                             "This is a sample label without a title");
            var sampleSelectList2 =
                new SelectListView(new StringBuilder(pageId).Append(".sampleselectlist2").ToString(),
                                   "Sample Select List 2",
                                   sampleSelectListOptions, ESelectListType.RadioList);

            settingsPage2.AddView(sampleViewGroup1);
            settingsPage2.AddView(sampleLabel3);
            settingsPage2.AddView(sampleSelectList2);
            Settings.Add(settingsPage2);
            //Build Settings Page 3
            pageId = "settings-page3";
            var settingsPage3 = Page.Factory.CreateSettingPage(pageId, "Page 3");
            var sampleInput1 = new InputView(new StringBuilder(pageId).Append(".sampleinput1").ToString(),
                                             "Sample Text Input");
            var sampleInput2 = new InputView(new StringBuilder(pageId).Append(".sampleinput2").ToString(),
                                             "Sample Number Input", EInputType.Number);
            var sampleInput3 = new InputView(new StringBuilder(pageId).Append(".sampleinput3").ToString(),
                                             "Sample Email Input", EInputType.Email);
            var sampleInput4 = new InputView(new StringBuilder(pageId).Append(".sampleinput4").ToString(),
                                             "Sample URL Input", EInputType.Url);
            var sampleInput5 = new InputView(new StringBuilder(pageId).Append(".sampleinput5").ToString(),
                                             "Sample Password Input", EInputType.Password);
            var sampleInput6 = new InputView(new StringBuilder(pageId).Append(".sampleinput6").ToString(),
                                             "Sample Decimal Input", EInputType.Decimal);

            settingsPage3.AddView(sampleInput1);
            settingsPage3.AddView(sampleInput2);
            settingsPage3.AddView(sampleInput3);
            settingsPage3.AddView(sampleInput4);
            settingsPage3.AddView(sampleInput5);
            settingsPage3.AddView(sampleInput6);
            Settings.Add(settingsPage3);
        }

        protected override void Initialize() {
            Console.WriteLine("Registering feature pages");
            //Initialize feature pages            
            HomeSeerSystem.RegisterFeaturePage(Id, "sample-guided-process.html", "Sample Guided Process");
            HomeSeerSystem.RegisterFeaturePage(Id, "sample-blank.html", "Sample Blank Page");
            HomeSeerSystem.RegisterFeaturePage(Id, "sample-plugin-feature.html", "Sample Feature Page 1");
            Console.WriteLine("Initialized");
        }

        protected override bool OnSettingsChange(List<Page> pages) {
            foreach (var pageDelta in pages) {
                var page = Settings[pageDelta.Id];
                foreach (var settingDelta in pageDelta.Views) {
                    //process settings changes
                    page.UpdateViewById(settingDelta);
                    try {
                        var newValue = settingDelta.GetStringValue();
                        if (newValue == null) {
                            continue;
                        }

                        //TODO react to settings change
                        
                        
                        
                        //TODO revise INI setting saving
                        HomeSeerSystem.SaveINISetting(SettingsSectionName, settingDelta.Id, newValue, SettingsFileName);
                    }
                    catch (InvalidOperationException exception) {
                        Console.WriteLine(exception);
                        //TODO Process ViewGroup?
                    }
                }

                //Make sure the new state is stored
                Settings[pageDelta.Id] = page;
            }

            return true;
        }

        //TODO clean up the documentation here to better indicate how it should be used
        public override PluginStatus OnStatusCheck() {
            //Determine the state of your plugin
            return PluginStatus.OK();
        }


        // sample functions and properties that can be called from HS, and a HTML page

        /// <summary>
        /// sample function with one parameter
        /// on the HTML page call this with:
        /// {{plugin_function 'HomeSeerSamplePlugin' 'MyCustomFunction' ['1']}}
        /// </summary>
        public string MyCustomFunction(string param) {
            return "1234";
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
                colorList = ColorList;
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

        /// <summary>
        /// sample property that returns a string
        /// on the HTML page call this with:
        /// </summary>
        public string MyCustomProperty { get; } = "Sample property";

        public override string PostBackProc(string page, string data, string user, int userRights) {
            Console.WriteLine("PostBack");
            //TODO expand on the response content
            var response = "";

            switch(page) {
                case "sample-plugin-feature.html":
                    // use default ajax handler to update 3 divs
                    response = "['timediv', '" + "Saved at " + DateTime.Now.ToString() + "','div2','Save OK','main_content','']";
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

    }

}