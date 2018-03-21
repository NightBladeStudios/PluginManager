namespace NightBladeStudios.PluginManager
{
    public interface IPlugin
    {
        string GetPluginTitle();
        void GUI();
    }
}