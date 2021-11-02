public interface IOnPlayerPress
{
    void OnPressDown();
    void OnPressUp();
    void OnPressDownUpdate(float duration);
    void OnPressUpUpdate(float duration);
}

public static class IOnPlayerPressHelper
{
    static public System.Collections.Generic.List<IOnPlayerPress> instances = new System.Collections.Generic.List<IOnPlayerPress>();
     
    public static void Initialize(this IOnPlayerPress onPlayerPress) {
        instances.Add(onPlayerPress);
    }

    static public void ForeachInstance(System.Action<IOnPlayerPress> function) => instances.ForEach(function);
}