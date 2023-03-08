using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class TestPresenter : MonoBehaviour
{
    [SerializeField] private List<ClipingUI> _uiList = new();

    private void Start() {
        foreach(var ui in _uiList)
        {
            ui.OnAttatched
                .Subscribe(x => Debug.Log($"area: {x.clipArea.SlotSize}, ui: {x.uiType}"))
                .AddTo(this);
        }
    }
}

