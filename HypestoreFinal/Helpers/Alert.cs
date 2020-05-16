using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HypestoreFinal.Helpers
{
    public class Alert
    {
        public const string TempDataKey = "TempDataAlerts";

        public string AlertStyle { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }
    }
    public static class AlertStyles
    {
        public const string Success = "Success";
        public const string Information = "Info";
        public const string Warning = "Warning";
        public const string Danger = "danger";
    }
}