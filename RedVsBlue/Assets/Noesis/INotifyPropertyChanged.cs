using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INotifyPropertyChanged
{
    void OnPropertyChanged(string name);
}

