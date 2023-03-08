using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

/// <summary>
/// D&D対応のUI
/// </summary>
public class ClipingUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IDropHandler, IPointerUpHandler
{
    private GameObject _ui;
    [SerializeField] private DropSlotSize _slotSize = DropSlotSize.OneSlot;
    [SerializeField] private UiElementsType _uiType = UiElementsType.TestA;

    /// <summary> アタッチ通知 </summary>
    public IObservable<(ClipArea clipArea, UiElementsType uiType)> OnAttatched => _onAttatched;
    private readonly Subject<(ClipArea clipArea, UiElementsType uiType)> _onAttatched = new();

    /// <summary> UIタイプ </summary>
    public UiElementsType UiType => _uiType;

    private Vector3 _basePos;
    private bool _isDrag = false;

    /// <summary> 座標を移動させます </summary>
    public void MovePosision(Vector3 pos)
        => _ui.transform.position = pos;

    /// <summary> 位置を基準位置に戻します </summary>
    public void ResetPosition()
        => _ui.transform.position = _basePos;

    /// <summary> 基準位置を設定します </summary>
    public void SetBasePosition(Vector3 pos)
        => _basePos = pos;

    /// <summary> 現在の位置を基準位置として設定します </summary>
    public void SetBasePosition()
        => _basePos = _ui.transform.position;

    /// <summary> ヒットしたオブジェクトを取得します </summary>
    private GameObject GetHitObject(PointerEventData eventData)
    {
        GameObject obj = null;
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var hit in raycastResults)
        {
            if (hit.gameObject.tag == _slotSize.ToString())
            {
                obj = hit.gameObject;
                break;
            }
        }

        return obj;
    }

    /// <summary> ドラッグを開始時の処理をします </summary>
    private void DragBeginAction(PointerEventData eventData)
    {
        if(_isDrag) return;
        
        // SetBasePosition(eventData.position);
        _isDrag = true;
    }

    /// <summary> ドロップ時の処理をします </summary>
    private void DropAction(PointerEventData eventData)
    {
        if (_isDrag == false) return;

        // Debug.Log("Drop");
        var hitObject = GetHitObject(eventData);

        if (hitObject != null)
        {
            MovePosision(hitObject.transform.position);
            SetBasePosition();
            _onAttatched.OnNext((hitObject.gameObject.GetComponent<ClipArea>(), _uiType));
        }
        else
        {
            ResetPosition();
        }
        _isDrag = false;
    }

    #region Unity Pointer Event Systems

    public void OnPointerDown(PointerEventData eventData)
        => DragBeginAction(eventData);

    public void OnBeginDrag(PointerEventData eventData)
        => DragBeginAction(eventData);

    public void OnDrag(PointerEventData eventData)
    {
        MovePosision(eventData.position);
    }

    public void OnDrop(PointerEventData eventData)
        => DropAction(eventData);

    public void OnPointerUp(PointerEventData eventData)
        => DropAction(eventData);

    #endregion

    private void Awake()
    {
        _ui = gameObject;

        _basePos = _ui.transform.position;
    }
}

/// <summary>
/// ドラッグアンドドロップをする枠のサイズ
/// </summary>
public enum DropSlotSize
{
    OneSlot,
    TwoSlot,
    ThreeSlot,
}

public enum UiElementsType
{
    TestA,
    TestB,
}