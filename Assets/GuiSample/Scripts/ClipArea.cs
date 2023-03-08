using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// D&DUIのドロップ先のエリア
/// </summary>
public class ClipArea : MonoBehaviour
{
    [SerializeField] private DropSlotSize _slotSize = DropSlotSize.OneSlot;
    public DropSlotSize SlotSize => _slotSize;

    private void Awake()
    {
        Extension.SetTag(gameObject, _slotSize.ToString());
    }
}