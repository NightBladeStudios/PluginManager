using System;
using System.Collections.Generic;

namespace NightBladeStudios.PluginManager
{
    [Serializable]
    public class PluginRepository
    {
        public List<PluginRepositoryElement> Plugins = new List<PluginRepositoryElement>();
    }
}