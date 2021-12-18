namespace ThemeSystem
{
    [System.Serializable]
    public class ThemeBased<T>
    {
        public T[] values;

        public static implicit operator T(ThemeBased<T> themeBased)
        {
            return themeBased.values[ThemeManager.instance.enabledIndex];
        }

        public T GetValue() => values[ThemeManager.instance.enabledIndex];
    }
}
