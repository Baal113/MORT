System;
System.Collections.Generic;
System.Windows.Forms;
MORT.LocalizeManager;

 MORT
{
 

UIQuickSetting : Form, ILocalizeForm
    {            

        enum StepType
        {
            None, SetFont, SetOCR, OcrComplete, Final,
        }
       

        StepType currentStepType StepType.None;

        QuickSettingData settingData null;


        UIQuickSetting()
        {
            InitializeComponent();
            LocalizeForm();
        }

        void LocalizeForm()
        {
            LocalizeLabel("Quick Setting");
            rbBlack.LocalizeLabel("Quick Setting Black");
            rbWhite.LocalizeLabel("Quick Setting White");
            rbUnknown.LocalizeLabel("Quick Setting Unknown");
            lbOCR.LocalizeLabel("Quick Setting OCR Information");

            btShowTrnaslate.LocalizeLabel("Quick Setting Link Translate");
            btShowBasic.LocalizeLabel("Quick Setting Link Basic");
            lbEnd.LocalizeLabel("Quick Setting End Information");
            lbOcrArea.LocalizeLabel("Quick Setting OCR Area");

            btNext.LocalizeLabel("Common Next");
        }

        void Show(OcrLanguageType language)
        {
            settingData = QuickSettingData();

            FormManager.Instace.SetTopMostOcrArea(false);
            DisableAll();
            Show();
            settingData.languageType language;

            ShowSetFont();
        }

        /// <summary>
        /// 폰트 색 설정하기
        /// </summary>
        void SetFontColor()
        {
            (rbBlack.Checked)
            {
                settingData.fontColorType  QuickSettingData.FontColorType.Black;
            }
            else if (rbWhite.Checked)
            {
                settingData.fontColorType =QuickSettingData.FontColorType.White;
            }
            else
            {
                settingData.fontColorType QuickSettingData.FontColorType.None;
            }


        }

        void SetTranslatorType()
        {
            SettingManager.TransType transType SettingManager.TransType.google_url;

            (settingData.languageType OcrLanguageType.vi)
            {
                //구글 기본 번역기
                transType SettingManager.TransType.google_url;
            }
            else if(settingData.languageType OcrLanguageType.Japen)
            {
                //1. 이지트랜스 사용여부 체크 -> 되면 이지 트랜스
                //2. 안 되면 구글 기본 번역기
                bool isEzTrans TransManager.Instace.InitEzTrans();

                (isEzTrans)
                {
                    transType SettingManager.TransType.ezTrans;
                }
                else
                {
                    transType SettingManager.TransType.google_url;
                }
            }

            settingData.transType transType;
        }

        void SetOcrType()
        {
            settingData.ocrType SettingManager.OcrType.Tesseract;

            (FormManager.Instace.MyMainForm.isAvailableWinOCR)
            {
                codeList FormManager.Instace.MyMainForm.WinLanguageCodeList;
                string code settingData.LanguageCode;

                (var obj codeList)
                {
                    (Util.GetIsEqualWinCode(code, obj))
                    {
                        settingData.ocrType SettingManager.OcrType.Window;
                    }
                }
            }
        }

        void SetSetting()
        {
            (FormManager.Instace.MySearchOptionForm null)
            {
                FormManager.Instace.MySearchOptionForm.acceptCaptureArea();
            }

            SetOcrType();
            SetTranslatorType();

            bool isUseJpn = false;

            if(settingData.languageType == OcrLanguageType.English)
            {
                isUseJpn = true;
            }

            FormManager.Instace.SetInvisibleOcrArea();
            FormManager.Instace.MyMainForm.ApplyFromQuickSetting(settingData);
            settingData = null;
        }



        private void DisableAll()
        {
            this.pnFinal.Visible = false;
            this.pnOcrComplete.Visible = false;
            this.pnSetFont.Visible = false;
            this.pnSetOcr.Visible = false;         
        
        }

        private void ShowSetFont()
        {
            DisableAll();

            currentStepType = StepType.SetFont;
            this.pnSetFont.Visible = true;
          
            rbUnknown.Checked = true;
            lbTitle.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Quick Setting Font Setting", "");
            btNext.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Common Next", "");
        }

     

        private void ShowSetOCR()
        {
            DisableAll();

            currentStepType = StepType.SetOCR;
            this.pnSetOcr.Visible = true;

            lbTitle.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Quick Setting OCR Area Setting", "");
            btNext.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Quick Setting OCR Area Button", "");
        }

        private void DoOcrArea()
        {
            FormManager.Instace.ResetUseColorGroup();
            FormManager.Instace.DestoryAllOcrAreaForm(true);
            FormManager.Instace.MyMainForm.SetCaptureArea();
            FormManager.Instace.MakeCpatureAreaForm(ShowOcrComplete);
        }

        private void ShowOcrComplete()
        {
        
            FormManager.Instace.MakeSearchOptionForm();
            FormManager.Instace.SetTopMostOcrArea(false);
            DisableAll();

            currentStepType = StepType.OcrComplete;
            this.pnOcrComplete.Visible = true;

            lbTitle.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Quick Setting OCR Area Complete", "");
            btNext.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Common Next", "");

            this.Focus();
        }

        private void ShowFinal()
        {
            DisableAll();

            currentStepType = StepType.Final;
            this.pnFinal.Visible = true;

            lbTitle.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Quick Setting Complete", "");
            btNext.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Quick Setting Close", "");
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            switch(currentStepType)
            {
                case StepType.SetFont:
                    SetFontColor();
                    ShowSetOCR();
                    break;

                case StepType.SetOCR:
                    DoOcrArea();
                    break;

                case StepType.OcrComplete:
                    SetSetting();
                    ShowFinal();
                    break;
                case StepType.Final:

                    this.Close();
                

                    break;
            }
        }

        private void btShowTrnaslate_Click(object sender, EventArgs e)
        {
            try
            {
                Util.OpenURL("https://blog.naver.com/killkimno/221760617100");
            }
            catch { }
      
        }

        private void btShowBasic_Click(object sender, EventArgs e)
        {
            try
            {
                Util.OpenURL("https://blog.naver.com/killkimno/221904769542");
            }
            catch { }
        }

        private void UIQuickSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FormManager.GetIsRemain())
            {
                FormManager.Instace.SetTopMostOcrArea(true);
                FormManager.Instace.DestoryQuickSetting();
            }
        }

    
    }
}
