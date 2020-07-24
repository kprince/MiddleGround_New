using MiddleGround.Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleGround.UI
{
    public class MG_PopPanel_Reward : MG_UIBase
    {
        public Image img_rewardIcon;
        public Text text_rewardNum;
        public Text text_buttonText;
        public Text text_rewardMutiple;
        public Text text_claim;
        public Text text_claimUnderline;
        public Text text_title;
        public Button btn_;
        public Button btn_claim;
        public GameObject go_mutiple;
        public GameObject go_adIcon;
        public GameObject go_claim;

        MG_RewardType RewardType = MG_RewardType.Gold;
        MG_RewardPanelType RewardPanelType = MG_RewardPanelType.AdClaim;
        int RewardNum = 1;
        float RewardMutiple = 1;
        protected override void Awake()
        {
            base.Awake();
            btn_.onClick.AddListener(OnButtonClick);
            btn_claim.onClick.AddListener(OnClaim);
        }
        int clickAdTime = 0;
        void OnButtonClick()
        {
            if (needAd)
            {
                switch (RewardPanelType)
                {
                    case MG_RewardPanelType.AdDouble:
                        RewardMutiple = 2;
                        clickAdTime++;
                        MG_Manager.ShowRV(GetReward, clickAdTime, "Get " + RewardType + " Reward in " + RewardPanelType + " RewardPanel");
                        break;
                    case MG_RewardPanelType.AdClaim:
                    case MG_RewardPanelType.AdRandom:
                        clickAdTime++;
                        MG_Manager.ShowRV(GetReward, clickAdTime, "Get " + RewardType + " Reward in " + RewardPanelType + " RewardPanel");
                        break;
                }
            }
            else
            {
                switch (RewardPanelType)
                {
                    case MG_RewardPanelType.AdDouble:
                        RewardMutiple = 2;
                        GetReward();
                        break;
                    case MG_RewardPanelType.AdClaim:
                    case MG_RewardPanelType.AdRandom:
                    case MG_RewardPanelType.FreeMutipleClaim:
                        GetReward();
                        break;
                }
            }
        }
        void GetReward()
        {
            clickAdTime = 0;
            int finalRewardNum = (int)(RewardNum * RewardMutiple);
            Vector3 flyStartPos = img_rewardIcon.transform.position;
            switch (RewardType)
            {
                case MG_RewardType.Gold:
                    MG_Manager.Instance.Add_Save_Gold(finalRewardNum);
                    MG_UIManager.Instance.FlyEffectTo_MenuTarget(flyStartPos, MG_MenuFlyTarget.OneGold, finalRewardNum);
                    break;
                case MG_RewardType.Diamond:
                    MG_Manager.Instance.Add_Save_Diamond(finalRewardNum);
                    MG_UIManager.Instance.FlyEffectTo_MenuTarget(flyStartPos, MG_MenuFlyTarget.Diamond, finalRewardNum);
                    break;
                case MG_RewardType.Amazon:
                    MG_Manager.Instance.Add_Save_Amazon(finalRewardNum);
                    MG_UIManager.Instance.FlyEffectTo_MenuTarget(flyStartPos, MG_MenuFlyTarget.Amazon, finalRewardNum);
                    break;
                case MG_RewardType.Cherry:
                    MG_Manager.Instance.Add_Save_Fruits(finalRewardNum);
                    MG_UIManager.Instance.FlyEffectTo_MenuTarget(flyStartPos, MG_MenuFlyTarget.Cherry, finalRewardNum);
                    break;
                case MG_RewardType.Orange:
                    MG_Manager.Instance.Add_Save_Fruits(finalRewardNum);
                    MG_UIManager.Instance.FlyEffectTo_MenuTarget(flyStartPos, MG_MenuFlyTarget.Orange, finalRewardNum);
                    break;
                case MG_RewardType.Watermalen:
                    MG_Manager.Instance.Add_Save_Fruits(finalRewardNum);
                    MG_UIManager.Instance.FlyEffectTo_MenuTarget(flyStartPos, MG_MenuFlyTarget.Watermalen, finalRewardNum);
                    break;
                case MG_RewardType.ScratchTicket:
                    MG_Manager.Instance.Add_Save_ScratchTicket(finalRewardNum);
                    MG_UIManager.Instance.FlyEffectTo_MenuTarget(flyStartPos, MG_MenuFlyTarget.ScratchTicket, finalRewardNum);
                    break;
                case MG_RewardType.WheelTicket:
                    MG_Manager.Instance.Add_Save_WheelTickets(finalRewardNum);
                    MG_UIManager.Instance.FlyEffectTo_MenuTarget(flyStartPos, MG_MenuFlyTarget.WheelTicket, finalRewardNum);
                    break;
                case MG_RewardType.SSS:
                    MG_Manager.Instance.Add_Save_777(finalRewardNum);
                    MG_UIManager.Instance.FlyEffectTo_MenuTarget(flyStartPos, MG_MenuFlyTarget.SSS, finalRewardNum);
                    break;
            }
            MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.MostRewardPanel);
        }
        void OnClaim()
        {
            MG_Manager.ShowIV(GiveupReward, "Givp up " + RewardType + " Reward in " + RewardPanelType + " RewardPanel");
        }
        void GiveupReward()
        {
            switch (RewardPanelType)
            {
                case MG_RewardPanelType.AdDouble:
                case MG_RewardPanelType.AdRandom:
                    RewardMutiple = 1;
                    GetReward();
                    break;
                default:
                    MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.MostRewardPanel);
                    break;
            }
        }
        public override IEnumerator OnEnter()
        {
            RewardType = MG_Manager.Instance.RewardType;
            RewardPanelType = MG_Manager.Instance.RewardPanelType;
            RewardNum = MG_Manager.Instance.RewardNum;
            RewardMutiple = MG_Manager.Instance.RewardMutiple;

            img_rewardIcon.sprite = MG_Manager.Instance.Get_RewardSprite(RewardType);
            text_claim.gameObject.SetActive(true);
            switch (RewardPanelType)
            {
                case MG_RewardPanelType.AdClaim:
                    needAd = true;
                    text_buttonText.text = "    Claim";
                    text_claim.text = "Give up";
                    text_claimUnderline.text = "─────";
                    go_claim.SetActive(true);
                    go_adIcon.SetActive(true);
                    go_mutiple.SetActive(false);
                    SetSpecialTokenInfo();
                    break;
                case MG_RewardPanelType.AdDouble:
                    needAd = true;
                    text_buttonText.text = "    Claim x2";
                    text_claim.text = "Claim Reward!";
                    text_claimUnderline.text = "─────────";
                    go_claim.SetActive(true);
                    go_adIcon.SetActive(true);
                    go_mutiple.SetActive(false);
                    if(RewardType==MG_RewardType.Diamond&& !MG_SaveManager.GuidSlots)
                        MG_Manager.Instance.next_GuidType = MG_Guid_Type.SlotsGuid;
                    break;
                case MG_RewardPanelType.AdRandom:
                    needAd = true;
                    text_buttonText.text = "    Random x1~5";
                    text_claim.text = "Claim Reward!";
                    text_claimUnderline.text = "─────────";
                    go_claim.SetActive(true);
                    go_adIcon.SetActive(true);
                    go_mutiple.SetActive(false);
                    break;
                case MG_RewardPanelType.FreeMutipleClaim:
                    needAd = false;
                    text_buttonText.text = "Claim";
                    text_rewardMutiple.text = "x" + RewardMutiple;
                    go_claim.SetActive(false);
                    go_adIcon.SetActive(false);
                    go_mutiple.SetActive(true);
                    break;
            }
            text_rewardNum.text = RewardNum.ToString();

            Transform transAll = transform.GetChild(1);
            transAll.localScale = new Vector3(0.8f, 0.8f, 1);
            canvasGroup.alpha = 0.8f;
            canvasGroup.blocksRaycasts = true;
            while (transAll.localScale.x < 1)
            {
                yield return null;
                float addValue = Time.deltaTime * 2;
                transAll.localScale += new Vector3(addValue, addValue);
                canvasGroup.alpha += addValue;
            }
            transAll.localScale = Vector3.one;
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
        }
        bool needAd = false;
        void SetSpecialTokenInfo()
        {
            switch (RewardType)
            {
                case MG_RewardType.SSS:
                    SetSpecialShowTextButton(MG_SaveManager.Get777Times);
                    if (!MG_SaveManager.GuidScratch)
                        MG_Manager.Instance.next_GuidType = MG_Guid_Type.ScratchGuid;
                    break;
                case MG_RewardType.Amazon:
                    SetSpecialShowTextButton(MG_SaveManager.GetAmazonTimes);
                    break;
                case MG_RewardType.Cherry:
                case MG_RewardType.Orange:
                case MG_RewardType.Watermalen:
                    SetSpecialShowTextButton(MG_SaveManager.GetFruitsTimes);
                    break;
            }
        }
        void SetSpecialShowTextButton(int times)
        {
            if (times == 0)
            {
                go_adIcon.SetActive(false);
                RewardNum = 3;
                text_buttonText.text = "Claim";
                needAd = false;
                text_claim.gameObject.SetActive(false);
            }
            else if (times == 1)
            {
                go_adIcon.SetActive(false);
                RewardNum = 2;
                text_buttonText.text = "Claim";
                needAd = false;
                text_claim.gameObject.SetActive(false);
            }
            else if (times == 2)
            {
                go_adIcon.SetActive(true);
                RewardNum = 2;
                text_buttonText.text = "    Claim";
                needAd = true;
                text_claim.gameObject.SetActive(true);
            }
            else
            {
                go_adIcon.SetActive(true);
                text_buttonText.text = "    Random x1~2";
                needAd = true;
                text_claim.gameObject.SetActive(true);
            }
        }

        public override IEnumerator OnExit()
        {
            clickAdTime = 0;

            Transform transAll = transform.GetChild(1);
            canvasGroup.interactable = false;
            while (transAll.localScale.x > 0.8f)
            {
                yield return null;
                float addValue = Time.deltaTime * 2;
                transAll.localScale -= new Vector3(addValue, addValue);
                canvasGroup.alpha -= addValue;
            }
            transAll.localScale = new Vector3(0.8f, 0.8f, 1);
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
