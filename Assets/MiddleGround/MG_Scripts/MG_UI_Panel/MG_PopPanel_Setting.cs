using MiddleGround.Audio;
using MiddleGround.Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace MiddleGround.UI
{
    public class MG_PopPanel_Setting : MG_UIBase
    {
        public Button btn_Close;
        public Button btn_Sound;
        public Button btn_Music;
        public Image img_SoundSwitch;
        public Image img_MusicSwitch;
        public Text text_goldNum;
        public Text text_cashNum;
        SpriteAtlas settingSA;
        Sprite sp_SwitchOn;
        Sprite sp_SwitchOff;
        protected override void Awake()
        {
            base.Awake();
            btn_Close.onClick.AddListener(OnCloseButtonClick);
            btn_Sound.onClick.AddListener(OnSoundButtonClick);
            btn_Music.onClick.AddListener(OnMusicButtonClick);
            settingSA = MG_UIManager.Instance.GetSpriteAtlas((int)MG_PopPanelType.SettingPanel);
            sp_SwitchOn = settingSA.GetSprite("MG_Sprite_Setting_SwitchOn");
            sp_SwitchOff = settingSA.GetSprite("MG_Sprite_Setting_SwitchOff");
        }
        void OnCloseButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.SettingPanel);
        }
        void OnSoundButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            bool oldstate = MG_SaveManager.SoundOn;
            MG_SaveManager.SoundOn = !oldstate;
            MG_AudioManager.Instance.SetSoundState(oldstate);
            img_SoundSwitch.sprite = oldstate ? sp_SwitchOff : sp_SwitchOn;
        }
        void OnMusicButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            bool oldstate = MG_SaveManager.MusicOn;
            MG_SaveManager.MusicOn = !oldstate;
            MG_AudioManager.Instance.SetMusicState(oldstate);
            img_MusicSwitch.sprite = oldstate ? sp_SwitchOff : sp_SwitchOn;
        }
        Vector3 startPos = new Vector3(-1395.205f, 0);
        Vector3 endPos = new Vector3(-540f, 0);
        const float offsetAlpha = 0.6f;
        public override IEnumerator OnEnter()
        {
            img_MusicSwitch.sprite = MG_SaveManager.MusicOn ? sp_SwitchOn : sp_SwitchOff;
            img_SoundSwitch.sprite = MG_SaveManager.SoundOn ? sp_SwitchOn : sp_SwitchOff;
            text_cashNum.text = MG_Manager.Get_CashShowText(MG_Manager.Instance.Get_Save_Cash());
            text_goldNum.text = MG_Manager.Instance.Get_Save_Gold().ToString();
            yield return null;
            Transform transAll = transform.GetChild(1);
            Image img_BG = transform.GetChild(0).GetComponent<Image>();
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
            float progress = 0;
            float finalProgress;
            while (progress<1)
            {
                yield return null;
                progress += Time.unscaledDeltaTime * 2;
                finalProgress = -Mathf.Pow(progress - 1, 4f) + 1;
                transAll.localPosition = Vector3.Lerp(startPos, endPos, finalProgress);
                img_BG.color = new Color(0, 0, 0, offsetAlpha * finalProgress);
            }
            yield return null;
            transAll.localPosition = endPos;
            canvasGroup.interactable = true;

        }

        public override IEnumerator OnExit()
        {
            Transform transAll = transform.GetChild(1);
            Image img_BG = transform.GetChild(0).GetComponent<Image>();
            canvasGroup.interactable = false;
            float progress = 0;
            float finalProgress;
            while (progress<1)
            {
                yield return null;
                progress += Time.unscaledDeltaTime * 2;
                finalProgress = -Mathf.Pow(progress - 1, 4f) + 1;
                transAll.localPosition = Vector3.Lerp(endPos, startPos, finalProgress);
                img_BG.color = new Color(0, 0, 0, offsetAlpha *(1- finalProgress));
            }
            yield return null;
            transAll.localPosition = startPos;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

        public override void OnPause()
        {
        }

        public override void OnResume()
        {
        }
    }
}
