using System;

namespace Common.Settings
{
    public class APISettings
    {
        public string APIName { get; set; }
        public string APIDescription { get; set; }
        /// <summary>
        /// DC: Distirbuted Cache
        /// </summary>
        public bool DCEnabled { get; set; }
        public bool ProfilingEnabled { get; set; }
        public bool EventBusEnabled { get; set; }
        public bool JWTEnabled { get; set; }
    }
}
