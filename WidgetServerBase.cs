using WigiDashWidgetFramework;
using WigiDashWidgetFramework.WidgetUtility;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OCWidget
{
    public partial class OCWidgetServer : IWidgetObject
    {

        // Identity
        public Guid Guid => new Guid(GetType().Assembly.GetName().Name);
        public string Name => OCWidget.Properties.Resources.OCWidgetServer_Name;

        public string Description => OCWidget.Properties.Resources.OCWidgetServer_DoOC;

        public string Author => "Peter";

        public string Website => "https://yourmama.com/";

        public Version Version => new Version(1, 0, 2);

        // Capabilities
        public SdkVersion TargetSdk => WidgetUtility.CurrentSdkVersion;

        public List<WidgetSize> SupportedSizes =>
            new List<WidgetSize>() {
                new WidgetSize(5, 4),
            };

        public Bitmap PreviewImage => new Bitmap(ResourcePath + "preview_5x4.png");

        // Functionality
        public IWidgetManager WidgetManager { get; set; }

        // Error handling
        public string LastErrorMessage { get; set; }


    }

}
