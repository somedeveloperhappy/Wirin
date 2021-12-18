using System.Collections;
using UnityEngine;

/// <summary>
/// all resettables should do <code>this.ResettableInit();</code> at the first possible situation ( i.e. on Awake)
/// and should also do <code>this.ResettableDestroy()</code> at the last possible situation ( i.e. OnDestroy )
/// </summary>
public interface IOnGameplayEnd
{
    /// <summary>
    /// it's called on gameplay end. like when player loses and it goes to main menu screen
    /// </summary>
    public void OnGameplayEnd();
}
static public class OnGameplayEnd
{
    static public System.Collections.Generic.List<IOnGameplayEnd> instances = new System.Collections.Generic.List<IOnGameplayEnd>();
    static public void ResettableInit(this IOnGameplayEnd instance)
    {
        instances.Add( instance );
    }
    static public void ResettableDestroy(this IOnGameplayEnd instance)
    {
        instances.Remove( instance );
    }
}
