using System;

namespace HostApp
{
    internal class PluginInfo
    {
        public PluginInfo(string name, string description, Type type, string dllPath)
        {
            Name = name;
            Description = description;
            Type = type;
            DllPath = dllPath;
        }

        public string Name { get; }
        public string Description { get; }
        public Type Type { get; }
        public string DllPath { get; }
    }
}