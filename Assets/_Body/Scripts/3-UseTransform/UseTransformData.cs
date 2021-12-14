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
public class UseTransformData : UdonSharpBehaviour
{
    private const string DEFAULT_STATE_NAME = "Init";
    private const string ANIMATION_STATE_NAME = "PingPong";


    [SerializeField] private Text _boolLabel;
    [SerializeField] private Text _intLabel;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transformForData;



    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        // Late-Joiner対応箇所
        if (player.isLocal)
        {
            // Late-Joinerにだけ、ObjectSyncのTransform値を再取得させる
            UpdateBoolLabel();
            UpdateIntLabel();
            UpdateAnimation();
        }
    }




    public void ToggleBool()
    {
        SetOwner();

        // x座標が四捨五入で1ならtrue、0ならfalseという扱い
        // 処理内容はbool値を反転させているだけ
        var position = _transformForData.position;
        position.x = Mathf.RoundToInt(position.x) == 1 ? 0 : 1;
        _transformForData.position = position;

        // 全員にテキストの更新をさせる
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(UpdateBoolLabelBody));
    }

    public void UpdateBoolLabel()
    {
        // すぐ実行すると、Owner以外は同期に時間がかかるため、誤った結果にならないように遅延させている
        SendCustomEventDelayedSeconds(nameof(UpdateBoolLabelBody), 0.1f, VRC.Udon.Common.Enums.EventTiming.Update);
    }

    public void UpdateBoolLabelBody()
    {
        // 実際の処理
        _boolLabel.text = $"bool: {Mathf.RoundToInt(_transformForData.position.x) == 1}";
    }

    public void AddInt()
    {
        SetOwner();

        // y座標の値をそのままintの値とする
        // 処理内容はintをインクリメントしているだけ
        var position = _transformForData.position;
        position.y = Mathf.RoundToInt(position.y) + 1;
        _transformForData.position = position;

        // 全員にテキストの更新をさせる
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(UpdateIntLabel));
    }

    public void UpdateIntLabel()
    {
        // 実際の処理
        _intLabel.text = $"int: {Mathf.RoundToInt(_transformForData.position.y)}";
    }

    public void ToggleAnimation()
    {
        SetOwner();

        // z座標が四捨五入で1ならtrue、0ならfalseという扱い
        // 処理内容はbool値を反転させているだけ
        var position = _transformForData.position;
        position.z = Mathf.RoundToInt(position.z) == 1 ? 0 : 1;
        _transformForData.position = position;

        // 全員にテキストの更新をさせる
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(UpdateAnimation));
    }

    public void UpdateAnimation()
    {
        // すぐ実行すると、Owner以外は同期に時間がかかるため、誤った結果にならないように遅延させている
        SendCustomEventDelayedSeconds(nameof(UpdateAnimationBody), 0.1f, VRC.Udon.Common.Enums.EventTiming.Update);
    }

    public void UpdateAnimationBody()
    {
        // 実際の処理
        _animator.Play(Mathf.RoundToInt(_transformForData.position.z) == 1 ? ANIMATION_STATE_NAME : DEFAULT_STATE_NAME);
    }



    private void SetOwner()
    {
        // UdonとObjectSync両方のOwnerになる必要がある
        var localPlayer = Networking.LocalPlayer;
        Networking.SetOwner(localPlayer, this.gameObject);
        Networking.SetOwner(localPlayer, _transformForData.gameObject);
    }
}