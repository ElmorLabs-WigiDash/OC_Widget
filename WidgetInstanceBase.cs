using WigiDashWidgetFramework;
using WigiDashWidgetFramework.WidgetUtility;
using System;

namespace OCWidget
{

    public partial class OCWidgetInstance : IWidgetInstance
    {

        // Identity
        private OCWidgetServer parent;
        public IWidgetObject WidgetObject
        {
            get
            {
                return parent;
            }
        }
        public Guid Guid { get; set; }

        // Location
        public WidgetSize WidgetSize { get; set; }

        // Events
        public event WidgetUpdatedEventHandler WidgetUpdated;

    }
}
