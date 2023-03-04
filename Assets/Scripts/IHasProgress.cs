using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress {

    public event EventHandler<OnprogressEventChangedArgs> OnProgressChanged;
    public class OnprogressEventChangedArgs : EventArgs
    {
        public float progressNormalized;
    }
}
