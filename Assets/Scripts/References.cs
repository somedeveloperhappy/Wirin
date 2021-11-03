using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    static References instance;
    
    static public Pivot pivot => instance?._pivot;
    [SerializeField] Pivot _pivot;
    
    static public Trinon trinon => instance?._trinon;
    [SerializeField] Trinon _trinon;
    
    static public PostPro postPro => instance?._postPro;
    [SerializeField] PostPro _postPro;
    
    static public PlayManagement.PlayerPressManager playerPressManager => instance?._playerPressManager;
    [SerializeField] PlayManagement.PlayerPressManager _playerPressManager;
    
    static public Camera currentCamera => instance?._currentCamera;
    [SerializeField] Camera _currentCamera;
    
    private void Awake() {
        if(!instance) {
            instance = this;
        } else {
            Destroy(this);
        }
    }
    
    
    
}
