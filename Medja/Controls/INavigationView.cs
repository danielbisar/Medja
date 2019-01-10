namespace Medja.Controls
{
    /// <summary>
    /// Interface can be implemented by classes that will be used via ViewNavigator to inform when the view gets
    /// shown or not.
    /// </summary>
    public interface INavigationView
    {
        void OnShowView();
        void OnHideView();
    }
}