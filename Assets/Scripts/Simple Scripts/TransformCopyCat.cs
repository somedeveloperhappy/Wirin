using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCopyCat : MonoBehaviour
{
    public Transform target;
    
    [System.Serializable]
    public class CopyState {
        public bool enabled, x, y, z;
    }
    public CopyState position;
    public CopyState rotation;
    public CopyState scale;
    
    public enum ApplyTime { 
        Awake, Start, Update, LateUpdate, FIxedUpdate
    };
    
    public ApplyTime applyTime;
    
    private void Awake() 
    {
        if(applyTime == ApplyTime.Awake) ApplyTrabsformCopyCat();
    }
    
    private void Start() 
    {
        if(applyTime == ApplyTime.Start) ApplyTrabsformCopyCat();
    }
    
    private void Update() 
    {
        if(applyTime == ApplyTime.Update) ApplyTrabsformCopyCat();
    }
    
    private void LateUpdate() 
    {
        if(applyTime == ApplyTime.LateUpdate) ApplyTrabsformCopyCat();
    }
    
    private void FixedUpdate() 
    {
        if(applyTime == ApplyTime.FIxedUpdate) ApplyTrabsformCopyCat();
    }
    
    
    
    void ApplyTrabsformCopyCat()
    {
        if(target == null)
        {
            Debug.LogError("Target must not be null!");
            return;
        }
        
        // position
        if(position.enabled) 
        {
            Vector3 pos = new Vector3(
                position.x ? target.position.x : transform.position.x,
                position.y ? target.position.y : transform.position.y,
                position.z ? target.position.z : transform.position.z
            );
            
            transform.position = pos;
        }
        
        // rotation
        if(rotation.enabled)
        {
            Vector3 this_rot = transform.rotation.eulerAngles;
            Vector3 target_rot = target.rotation.eulerAngles;
            
            Vector3 rot = new Vector3(
                rotation.x ? target_rot.x : this_rot.x,
                rotation.y ? target_rot.y : this_rot.y,
                rotation.z ? target_rot.z : this_rot.z
            );
            
            transform.rotation = Quaternion.Euler(rot);
        }
        
        // scale
        if(scale.enabled)
        {
            Vector3 _scale = new Vector3(
                scale.x ? transform.localScale.x : target.localScale.x,
                scale.y ? transform.localScale.y : target.localScale.y,
                scale.z ? transform.localScale.z : target.localScale.z
            );
            
            transform.localScale = _scale;
        }
    } 
}
