namespace ThemeSystem
{
    [System.Serializable]
    public class ThemeBased<T>
    {
        [System.Serializable]
        public class Values
        {
            public string name;
            public T val;
        }
        public Values[] values;

        public static implicit operator T(ThemeBased<T> themeBased)
        {
            return themeBased.values[ThemeManager.instance.enabledIndex].val;
        }

        public T GetValue() => values[ThemeManager.instance.enabledIndex].val;
    }
}
