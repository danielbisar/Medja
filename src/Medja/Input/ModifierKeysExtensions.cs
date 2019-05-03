namespace Medja.Input
{
    public static class ModifierKeysExtensions
    {
        public static bool HasShift(this ModifierKeys modifierKeys)
        {
            return modifierKeys.HasFlag(ModifierKeys.LeftShift) || modifierKeys.HasFlag(ModifierKeys.RightShift);
        }

        public static bool HasControl(this ModifierKeys modifierKeys)
        {
            return modifierKeys.HasFlag(ModifierKeys.LeftCtrl) || modifierKeys.HasFlag(ModifierKeys.RightCtrl);
        }
    }
}