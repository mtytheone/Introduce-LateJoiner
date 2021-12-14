#region License
/*------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/* MIT License                                                                                                                                                                                                                                                                                                                                                                                                                                                                  */
/*                                                                                                                                                                                                                                                                                                                                                                                                                                                                              */
/* Copyright (c) 2021 hatuxes                                                                                                                                                                                                                                                                                                                                                                                                                                                   */
/*                                                                                                                                                                                                                                                                                                                                                                                                                                                                              */
/* Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:                             */
/*                                                                                                                                                                                                                                                                                                                                                                                                                                                                              */
/* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                                                                                                                                                                                                                                                                                                                                               */
/*                                                                                                                                                                                                                                                                                                                                                                                                                                                                              */
/* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
/*------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
#endregion


using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class UseSyncVariable : UdonSharpBehaviour
{
    private const string DEFAULT_STATE_NAME = "Init";
    private const string ANIMATION_STATE_NAME = "PingPong";


    [SerializeField] private Text _boolLabel;
    [SerializeField] private Text _intLabel;
    [SerializeField] private Animator _animator;


    [UdonSynced, FieldChangeCallback(nameof(BoolData))] private bool _boolData;
    [UdonSynced, FieldChangeCallback(nameof(IntData))] private int _intData;
    [UdonSynced, FieldChangeCallback(nameof(AnimData))] private bool _animData;

    public bool BoolData
    {
        get => _boolData;
        set
        {
            _boolData = value;

            // 実際の処理
            _boolLabel.text = $"bool: {_boolData}";
        }
    }

    public int IntData
    {
        get => _intData;
        set
        {
            _intData = value;

            // 実際の処理
            _intLabel.text = $"int: {_intData}";
        }
    }

    public bool AnimData
    {
        get => _animData;
        set
        {
            _animData = value;

            // 実際の処理
            _animator.Play(_animData ? ANIMATION_STATE_NAME : DEFAULT_STATE_NAME);
        }
    }




    public void ToggleBool()
    {
        // 同期変数変更するためのオーナー譲渡
        SetOwner();

        // 値変更
        BoolData = !BoolData;
        RequestSerialization();
    }

    public void AddInt()
    {
        // 同期変数変更するためのオーナー譲渡
        SetOwner();

        // 加算処理
        IntData++;
        RequestSerialization();
    }

    public void ToggleAnimation()
    {
        // 同期変数変更するためのオーナー譲渡
        SetOwner();

        // 値変更
        AnimData = !AnimData;
        RequestSerialization();
    }


    private void SetOwner()
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
    }
}