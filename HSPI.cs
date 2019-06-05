using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk;

namespace HSPI_HomeSeerSamplePlugin {

    public class HSPI : AbstractPlugin {


        #region Properties
        //TODO feature pages

        public override string ID { get; } = "homeseer-sample-plugin";
        public override string Name { get; } = "Sample Plugin";
        protected override string SettingsFileName { get; } = "HomeSeerSamplePlugin.ini";

        public override bool HasSettings { get; } = true;

        #endregion
        
        public HSPI() : base() {
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
            SettingsPageIndexes.Add(settingsPage1.Id, SettingsPages.Count);
            SettingsPages.Add(settingsPage1);
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
            SettingsPageIndexes.Add(settingsPage2.Id, SettingsPages.Count);
            SettingsPages.Add(settingsPage2);
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
            SettingsPageIndexes.Add(settingsPage3.Id, SettingsPages.Count);
            SettingsPages.Add(settingsPage3);
            //Initialize feature pages
        }

        protected override void Initialize() {
            //Default behavior is sufficient
            Console.WriteLine("Initialized");
        }

        public override List<string> GetJuiSettingsPages() {
            var jsonSettingsPages = SettingsPages.Select(p => p.ToJsonString()).ToList();
            return jsonSettingsPages;
        }

        protected override bool OnSettingsChange(List<Page> pages) {
            foreach (var pageDelta in pages) {
                var page = SettingsPages[SettingsPageIndexes[pageDelta.Id]];
                foreach (var settingDelta in pageDelta.Views) {
                    //process settings changes
                    page.UpdateViewById(settingDelta);
                    try {
                        var newValue = settingDelta.GetStringValue();
                        if (newValue == null) {
                            continue;
                        }

                        //TODO revise INI setting saving
                        HomeSeerSystem.SaveINISetting(SettingsSectionName, settingDelta.Id, newValue, SettingsFileName);
                    }
                    catch (InvalidOperationException exception) {
                        Console.WriteLine(exception);
                        //TODO Process ViewGroup?
                    }
                }

                //Make sure the new state is stored
                SettingsPages[SettingsPageIndexes[pageDelta.Id]] = page;
            }

            //Return the new state of the settings pages
            //var jsonSettingsPages = SettingsPages.Select(p => p.ToJsonString()).ToList();
            return true;
        }

        //TODO clean up the documentation here to better indicate how it should be used
        public override InterfaceStatus InterfaceStatus() {
            //TODO handle status
            var intStat = new InterfaceStatus { intStatus = Constants.enumInterfaceStatus.OK };
            return intStat;
        }

        // sample functions and properties that can be called from HS, and a HTML page


        // sample function with one parameter
        // on the HTML page call this with:
        // {{plugin_function 'homeseer-sample-plugin' 'MyCustomFunction' ['1']}}
        public string MyCustomFunction(string sParm)
        {
            return "1234";
        }

        // sample property that returns a string
        // on the HTML page call this with:
        public string MyCustomProperty { get; } = "Sample property";



    }

}