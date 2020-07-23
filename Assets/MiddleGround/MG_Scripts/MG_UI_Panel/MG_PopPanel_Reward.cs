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
            switch (RewardPanelType)
            {
                case MG_RewardPanelType.AdClaim:
                case MG_RewardPanelType.AdDouble:
                case MG_RewardPanelType.AdRandom:
                    clickAdTime++;
                    MG_Manager.ShowRV(GetReward, clickAdTime, "Get " + RewardType + " Reward in " + RewardPanelType + " RewardPanel");
                    break;
                case MG_RewardPanelType.MutipleClaim:
                    GetReward();
                    break;
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
                case MG_RewardType.SSS:
                    MG_Manager.Instance.Add_Save_777(finalRewardNum);
                    MG_UIManager.Instance.FlyEffectTo_MenuTarget(flyStartPos, MG_MenuFlyTarget.SSS, finalRewardNum);
                    break;
            }
            MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.RewardPanel);
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
            }
            MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.RewardPanel);
        }
        public override IEnumerator OnEnter()
        {
            RewardType = MG_Manager.Instance.RewardType;
            RewardPanelType = MG_Manager.Instance.RewardPanelType;
            RewardNum = MG_Manager.Instance.RewardNum;
            RewardMutiple = MG_Manager.Instance.RewardMutiple;

            img_rewardIcon.sprite = MG_Manager.Instance.Get_RewardSprite(RewardType);
            text_rewardNum.text = RewardNum.ToString();
            switch (RewardPanelType)
            {
                case MG_RewardPanelType.AdClaim:
                    text_buttonText.text = "Claim";
                    text_claim.text = "Give up";
                    text_claimUnderline.text = "─────";
                    go_claim.SetActive(true);
                    go_adIcon.SetActive(true);
                    go_mutiple.SetActive(false);
                    break;
                case MG_RewardPanelType.AdDouble:
                    text_buttonText.text = "Claim x2";
                    text_claim.text = "Claim Reward!";
                    text_claimUnderline.text = "─────────";
                    go_claim.SetActive(true);
                    go_adIcon.SetActive(true);
                    go_mutiple.SetActive(false);
                    break;
                case MG_RewardPanelType.AdRandom:
                    text_buttonText.text = "Random x1~5";
                    text_claim.text = "Claim Reward!";
                    text_claimUnderline.text = "─────────";
                    go_claim.SetActive(true);
                    go_adIcon.SetActive(true);
                    go_mutiple.SetActive(false);
                    break;
                case MG_RewardPanelType.MutipleClaim:
                    text_buttonText.text = "Claim";
                    text_rewardMutiple.text = "x" + RewardMutiple;
                    go_claim.SetActive(false);
                    go_adIcon.SetActive(false);
                    go_mutiple.SetActive(true);
                    break;
            }

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

        public override IEnumerator OnExit()
        {
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
