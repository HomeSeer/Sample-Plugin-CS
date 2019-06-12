using System;
using System.Collections.Generic;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk;
using Newtonsoft.Json;

// ReSharper disable IdentifierTypo

namespace HSPI_HomeSeerSamplePlugin {

    public class HSPI : AbstractPlugin {


        #region Properties
        //TODO feature pages

        public override string ID { get; } = "homeseer-sample-plugin";
        public override string Name { get; } = "Sample Plugin";
        protected override string SettingsFileName { get; } = "HomeSeerSamplePlugin.ini";

        public override bool HasSettings => (Settings?.Count ?? 0) > 0;
        
        #endregion
        
        public HSPI() {
            //Initialize settings pages
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
                                               "Red");
            var orangeToggle = new ToggleView(new StringBuilder(pageId).Append(".orange").ToString(),
                                               "Orange");
            var yellowToggle = new ToggleView(new StringBuilder(pageId).Append(".yellow").ToString(),
                                               "Yellow");
            var greenToggle = new ToggleView(new StringBuilder(pageId).Append(".green").ToString(),
                                              "Green");
            var blueToggle = new ToggleView(new StringBuilder(pageId).Append(".blue").ToString(),
                                              "Blue");
            var indigoToggle = new ToggleView(new StringBuilder(pageId).Append(".indigo").ToString(),
                                              "Indigo");
            var violetToggle = new ToggleView(new StringBuilder(pageId).Append(".violet").ToString(),
                                              "Violet");
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
            HomeSeerSystem.RegisterFeaturePage(ID, "sample-plugin-feature.html", "Sample Feature Page 1");
            HomeSeerSystem.RegisterFeaturePage(ID, "sample-guided-process.html", "Sample Guided Process");
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
            return PluginStatus.OK();
        }

        
        // sample functions and properties that can be called from HS, and a HTML page

        /// <summary>
        /// sample function with one parameter
        /// on the HTML page call this with:
        /// {{plugin_function 'homeseer-sample-plugin' 'MyCustomFunction' ['1']}}
        /// </summary>
        public string MyCustomFunction(string param) {
            return "1234";
        }
        
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

                    if (!colorView.IsEnabled) {
                        colorList.Add("");
                    }
                    
                    colorList.Add(colorView.Name);
                }
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                colorList = new List<string> {
                                                 "Red",
                                                 "Orange",
                                                 "Yellow",
                                                 "Green",
                                                 "Blue",
                                                 "Indigo",
                                                 "Violet"
                                             };
            }
           
            for (var i = 0; i < colorList.Count; i++) {
                var color = colorList[i];
                if (string.IsNullOrWhiteSpace(color)) {
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
            var response = "";

            switch(page) {
                case "sample-plugin-feature.html":
                    // use default ajax handler to update 3 divs
                    return "['timediv', '" + "Saved at " + DateTime.Now.ToString() + "','div2','Save OK','main_content','']";
                    
                case "sample-guided-process.html":
                    try {
                        var postData = JsonConvert.DeserializeObject<PostData>(data);

                        Console.WriteLine("Post back from sample-guided-process page");
                
                        postData.InternalData = JsonConvert.DeserializeObject<PostData.SampleInternalData>(postData.Data);
                        var colorList = new List<string> {
                                                             "Red",
                                                             "Orange",
                                                             "Yellow",
                                                             "Green",
                                                             "Blue",
                                                             "Indigo",
                                                             "Violet"
                                                         };
                        var color = colorList[postData.InternalData.ColorIndex];
                
                        response = postData.InternalData.TextValue.ToLower().Contains("mushroom") ? "It's a snake!" : "You said " + postData.InternalData.TextValue + " and selected " + color;

                    }
                    catch (JsonSerializationException exception) {
                        response = "error";
                    }

                    return response;
            }
            return response;
        }

    }

}