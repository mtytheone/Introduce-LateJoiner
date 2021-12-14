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
public class ViaOwnerRequest : UdonSharpBehaviour
{
    private const string DEFAULT_STATE_NAME = "Init";
    private const string ANIMATION_STATE_NAME = "PingPong";


    [SerializeField] private Text _boolLabel;
    // [SerializeField] private Text _intLabel;
    [SerializeField] private Animator _animator;

    private bool _boolData;
    // private int _intData;
    private bool _animData;

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        // Late-Joiner対応箇所
        if (Networking.LocalPlayer.IsOwner(this.gameObject))
        {
            // Ownerがtrueの状態になっていたら、一度全員にtrue処理を渡す
            if (_boolData)
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(BoolTrueProcess));
            }
            if (_animData)
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(AnimationTrueProcess));
            }
        }
    }



    public void ToggleBool()
    {
        // _boolData = !_boolDataと同じ処理内容
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, _boolData ? nameof(BoolFalseProcess) : nameof(BoolTrueProcess));
    }

    public void BoolTrueProcess()
    {
        // _boolDataがfalseの人にだけ処理させる（Late-Joiner対応にも使うため）
        if (!_boolData)
        {
            _boolData = true;

            // 実際の処理
            UpdateBoolLabel();
        }
    }

    public void BoolFalseProcess()
    {
        _boolData = false;

        // 実際の処理
        UpdateBoolLabel();
    }


    public void AddInt()
    {
        // _intData++;
        // SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, ReceivedIntProcess(_intData));
    }

    public void ToggleAnimation()
    {
        // _animData = !_animDataaと同じ処理内容
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, _animData ? nameof(AnimationFalseProcess) : nameof(AnimationTrueProcess));
    }

    public void AnimationTrueProcess()
    {
        // _animDataがfalseの人にだけ処理させる（Late-Joiner対応にも使うため）
        if (!_animData)
        {
            _animData = true;

            // 実際の処理
            UpdateAnimation();
        }
    }

    public void AnimationFalseProcess()
    {
        _animData = false;

        // 実際の処理
        UpdateAnimation();
    }




    private void UpdateBoolLabel()
    {
        _boolLabel.text = $"bool: {_boolData}";
    }

    private void UpdateAnimation()
    {
        _animator.Play(_animData ? ANIMATION_STATE_NAME : DEFAULT_STATE_NAME);
    }
}