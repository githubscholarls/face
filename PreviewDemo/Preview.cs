using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.IO;

using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Data.SqlClient;
using AForge.Video.DirectShow;
using System.Configuration;
using Baidu.Aip.Face;
using System.Drawing.Imaging;

namespace PreviewDemo
{
    /// <summary>
    /// Form1 ��ժҪ˵����
    /// </summary>
    public class Preview : System.Windows.Forms.Form
    {
        private uint iLastErr = 0;
        private Int32 m_lUserID = -1;
        private bool m_bInitSDK = false;
        private bool m_bRecord = false;
        private bool m_bTalk = false;
        private Int32 m_lRealHandle = -1;
        private int lVoiceComHandle = -1;
        private string str;

        CHCNetSDK.REALDATACALLBACK RealData = null;
        CHCNetSDK.LOGINRESULTCALLBACK LoginCallBack = null;
        public CHCNetSDK.NET_DVR_PTZPOS m_struPtzCfg;
        public CHCNetSDK.NET_DVR_USER_LOGIN_INFO struLogInfo;
        public CHCNetSDK.NET_DVR_DEVICEINFO_V40 DeviceInfo;

        public delegate void UpdateTextStatusCallback(string strLogStatus, IntPtr lpDeviceInfo);

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.PictureBox RealPlayWnd;
        private TextBox textBoxIP;
        private TextBox textBoxPort;
        private TextBox textBoxUserName;
        private TextBox textBoxPassword;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Button btnBMP;
        private Button btnJPEG;
        private Label label11;
        private Label label12;
        private Label label13;
        private TextBox textBoxChannel;
        private Button btnRecord;
        private Label label14;
        private Button btn_Exit;
        private Button btnVioceTalk;
        private Label label16;
        private Label label17;
        private TextBox textBoxID;
        /*private Button PtzGet;
        private Button PtzSet;*/
        private Label label19;
        /*private ComboBox comboBox1;
        private TextBox textBoxPanPos;
        private TextBox textBoxTiltPos;
        private TextBox textBoxZoomPos;*/
        private Label label20;
        private Label label21;
        private Label label22;
        private Button PreSet;
        private Label label23;
        private Label labelLogin;
        private Panel panel1;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer1;
        private Button btnEnd;
        private Button btnUpLoadPhone;
        private Button btnManualIdentifiction;

        //private GroupBox groupBox1;

        /// <summary>
        /// ����������������
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Preview()
        {

            StaticStringBuilder();//WinformFace

            //
            // Windows ���������֧���������
            //
            InitializeComponent();

            m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            if (m_bInitSDK == false)
            {
                MessageBox.Show("NET_DVR_Init error!");
                return;
            }
            else
            {
                //����SDK��־ To save the SDK log
                CHCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", true);
            }
            //
            // TODO: �� InitializeComponent ���ú�����κι��캯������
            //
        }

        /// <summary>
        /// ������������ʹ�õ���Դ��
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
            }
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
            }
            if (m_bInitSDK == true)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows ������������ɵĴ���
        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.RealPlayWnd = new System.Windows.Forms.PictureBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnBMP = new System.Windows.Forms.Button();
            this.btnJPEG = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxChannel = new System.Windows.Forms.TextBox();
            this.btnRecord = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.btnVioceTalk = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.PreSet = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.labelLogin = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.videoSourcePlayer1 = new AForge.Controls.VideoSourcePlayer();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnUpLoadPhone = new System.Windows.Forms.Button();
            this.btnManualIdentifiction = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Device IP";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 24);
            this.label2.TabIndex = 0;
            this.label2.Text = "User Name";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(385, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "Password";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(385, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Device Port";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(725, 57);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(130, 75);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(50, 656);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(127, 52);
            this.btnPreview.TabIndex = 7;
            this.btnPreview.Text = "Live View";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // RealPlayWnd
            // 
            this.RealPlayWnd.BackColor = System.Drawing.SystemColors.WindowText;
            this.RealPlayWnd.Location = new System.Drawing.Point(30, 196);
            this.RealPlayWnd.Name = "RealPlayWnd";
            this.RealPlayWnd.Size = new System.Drawing.Size(825, 361);
            this.RealPlayWnd.TabIndex = 4;
            this.RealPlayWnd.TabStop = false;
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(125, 33);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(190, 28);
            this.textBoxIP.TabIndex = 2;
            this.textBoxIP.Text = "192.168.1.64";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(508, 33);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(187, 28);
            this.textBoxPort.TabIndex = 3;
            this.textBoxPort.Text = "8000";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(125, 102);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(190, 28);
            this.textBoxUserName.TabIndex = 4;
            this.textBoxUserName.Text = "admin";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxPassword.Location = new System.Drawing.Point(508, 102);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(187, 28);
            this.textBoxPassword.TabIndex = 5;
            this.textBoxPassword.Text = "123456789.qwe";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 18);
            this.label5.TabIndex = 9;
            this.label5.Text = "�豸IP";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(385, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 18);
            this.label6.TabIndex = 10;
            this.label6.Text = "�豸�˿�";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 18);
            this.label7.TabIndex = 11;
            this.label7.Text = "�û���";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(388, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 18);
            this.label8.TabIndex = 12;
            this.label8.Text = "����";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(52, 625);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 18);
            this.label9.TabIndex = 13;
            this.label9.Text = "Ԥ��";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(737, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 18);
            this.label10.TabIndex = 14;
            this.label10.Text = "��¼";
            // 
            // btnBMP
            // 
            this.btnBMP.Location = new System.Drawing.Point(205, 658);
            this.btnBMP.Name = "btnBMP";
            this.btnBMP.Size = new System.Drawing.Size(132, 51);
            this.btnBMP.TabIndex = 8;
            this.btnBMP.Text = "Capture BMP ";
            this.btnBMP.UseVisualStyleBackColor = true;
            this.btnBMP.Click += new System.EventHandler(this.btnBMP_Click);
            // 
            // btnJPEG
            // 
            this.btnJPEG.Location = new System.Drawing.Point(369, 656);
            this.btnJPEG.Name = "btnJPEG";
            this.btnJPEG.Size = new System.Drawing.Size(161, 52);
            this.btnJPEG.TabIndex = 9;
            this.btnJPEG.Text = "Capture JPEG";
            this.btnJPEG.UseVisualStyleBackColor = true;
            this.btnJPEG.Click += new System.EventHandler(this.btnJPEG_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(210, 625);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 18);
            this.label11.TabIndex = 17;
            this.label11.Text = "BMPץͼ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(379, 625);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 18);
            this.label12.TabIndex = 18;
            this.label12.Text = "JPEGץͼ";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(50, 582);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(125, 18);
            this.label13.TabIndex = 19;
            this.label13.Text = "Ԥ��/ץͼͨ��";
            // 
            // textBoxChannel
            // 
            this.textBoxChannel.Location = new System.Drawing.Point(200, 576);
            this.textBoxChannel.Name = "textBoxChannel";
            this.textBoxChannel.Size = new System.Drawing.Size(167, 28);
            this.textBoxChannel.TabIndex = 6;
            this.textBoxChannel.Text = "1";
            // 
            // btnRecord
            // 
            this.btnRecord.Location = new System.Drawing.Point(554, 656);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(166, 52);
            this.btnRecord.TabIndex = 10;
            this.btnRecord.Text = "Start Record";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(555, 625);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(98, 18);
            this.label14.TabIndex = 22;
            this.label14.Text = "�ͻ���¼��";
            // 
            // btn_Exit
            // 
            this.btn_Exit.Location = new System.Drawing.Point(730, 922);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(125, 48);
            this.btn_Exit.TabIndex = 11;
            this.btn_Exit.Tag = "";
            this.btn_Exit.Text = "�˳� Exit";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // btnVioceTalk
            // 
            this.btnVioceTalk.Location = new System.Drawing.Point(30, 962);
            this.btnVioceTalk.Name = "btnVioceTalk";
            this.btnVioceTalk.Size = new System.Drawing.Size(125, 50);
            this.btnVioceTalk.TabIndex = 25;
            this.btnVioceTalk.Text = "Start Talk";
            this.btnVioceTalk.UseVisualStyleBackColor = true;
            this.btnVioceTalk.Click += new System.EventHandler(this.btnVioceTalk_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(30, 932);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 18);
            this.label16.TabIndex = 26;
            this.label16.Text = "�����Խ�";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(419, 580);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(44, 18);
            this.label17.TabIndex = 27;
            this.label17.Text = "��ID";
            // 
            // textBoxID
            // 
            this.textBoxID.Location = new System.Drawing.Point(484, 574);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.Size = new System.Drawing.Size(375, 28);
            this.textBoxID.TabIndex = 28;
            // 
            // PreSet
            // 
            this.PreSet.Location = new System.Drawing.Point(192, 962);
            this.PreSet.Name = "PreSet";
            this.PreSet.Size = new System.Drawing.Size(161, 49);
            this.PreSet.TabIndex = 31;
            this.PreSet.Text = "PTZ Control";
            this.PreSet.UseVisualStyleBackColor = true;
            this.PreSet.Click += new System.EventHandler(this.PreSet_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(198, 932);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(80, 18);
            this.label23.TabIndex = 32;
            this.label23.Text = "��̨����";
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.Location = new System.Drawing.Point(187, 175);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(170, 18);
            this.labelLogin.TabIndex = 33;
            this.labelLogin.Text = "��¼״̬���첽����";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxIP);
            this.panel1.Controls.Add(this.textBoxPort);
            this.panel1.Controls.Add(this.textBoxUserName);
            this.panel1.Controls.Add(this.textBoxPassword);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(42, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 20);
            this.panel1.TabIndex = 34;
            // 
            // videoSourcePlayer1
            // 
            this.videoSourcePlayer1.Location = new System.Drawing.Point(892, 152);
            this.videoSourcePlayer1.Name = "videoSourcePlayer1";
            this.videoSourcePlayer1.Size = new System.Drawing.Size(312, 167);
            this.videoSourcePlayer1.TabIndex = 35;
            this.videoSourcePlayer1.Text = "videoSourcePlayer2";
            this.videoSourcePlayer1.VideoSource = null;
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(906, 476);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(125, 34);
            this.btnEnd.TabIndex = 37;
            this.btnEnd.Text = "���ղ���";
            this.btnEnd.UseVisualStyleBackColor = true;
            // 
            // btnUpLoadPhone
            // 
            this.btnUpLoadPhone.Location = new System.Drawing.Point(1066, 390);
            this.btnUpLoadPhone.Name = "btnUpLoadPhone";
            this.btnUpLoadPhone.Size = new System.Drawing.Size(144, 35);
            this.btnUpLoadPhone.TabIndex = 38;
            this.btnUpLoadPhone.Text = "�ɸ�Ϊ�ϴ���Ƭ";
            this.btnUpLoadPhone.UseVisualStyleBackColor = true;
            this.btnUpLoadPhone.Click += new System.EventHandler(this.btnUpLoadPhone_Click);
            // 
            // btnManualIdentifiction
            // 
            this.btnManualIdentifiction.Location = new System.Drawing.Point(1066, 482);
            this.btnManualIdentifiction.Name = "btnManualIdentifiction";
            this.btnManualIdentifiction.Size = new System.Drawing.Size(144, 28);
            this.btnManualIdentifiction.TabIndex = 39;
            this.btnManualIdentifiction.Text = "�ֶ�ʶ��";
            this.btnManualIdentifiction.UseVisualStyleBackColor = true;
            this.btnManualIdentifiction.Click += new System.EventHandler(this.btnManualIdentifiction_Click);
            // 
            // Preview
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(10, 21);
            this.ClientSize = new System.Drawing.Size(1347, 743);
            this.Controls.Add(this.btnManualIdentifiction);
            this.Controls.Add(this.btnUpLoadPhone);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.videoSourcePlayer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelLogin);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.PreSet);
            this.Controls.Add(this.textBoxID);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btnVioceTalk);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.RealPlayWnd);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.textBoxChannel);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.btnBMP);
            this.Controls.Add(this.btnJPEG);
            this.Controls.Add(this.btnRecord);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Name = "Preview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preview";
            this.Load += new System.EventHandler(this.Preview_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// Ӧ�ó��������ڵ㡣
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new Preview());
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {

        }

        public void UpdateClientList(string strLogStatus, IntPtr lpDeviceInfo)
        {
            //�б�����������Ϣ
            labelLogin.Text = "��¼״̬���첽����" + strLogStatus;
        }

        public void cbLoginCallBack(int lUserID, int dwResult, IntPtr lpDeviceInfo, IntPtr pUser)
        {
            string strLoginCallBack = "��¼�豸��lUserID��" + lUserID + "��dwResult��" + dwResult;

            if (dwResult == 0)
            {
                uint iErrCode = CHCNetSDK.NET_DVR_GetLastError();
                strLoginCallBack = strLoginCallBack + "�������:" + iErrCode;
            }

            //�������ע�͵�Ҳ�����
            if (InvokeRequired)
            {
                object[] paras = new object[2];
                paras[0] = strLoginCallBack;
                paras[1] = lpDeviceInfo;
                labelLogin.BeginInvoke(new UpdateTextStatusCallback(UpdateClientList), paras);
            }
            else
            {
                //�����ÿؼ������߳�ֱ�Ӹ�����Ϣ�б� 
                UpdateClientList(strLoginCallBack, lpDeviceInfo);
            }

        }

        private async void btnLogin_Click(object sender, System.EventArgs e)
        {
            if (textBoxIP.Text == "" || textBoxPort.Text == "" ||
                textBoxUserName.Text == "" || textBoxPassword.Text == "")
            {
                MessageBox.Show("Please input IP, Port, User name and Password!");
                return;
            }
            if (m_lUserID < 0)
            {

                struLogInfo = new CHCNetSDK.NET_DVR_USER_LOGIN_INFO();

                //�豸IP��ַ��������
                byte[] byIP = System.Text.Encoding.Default.GetBytes(textBoxIP.Text);
                struLogInfo.sDeviceAddress = new byte[129];
                byIP.CopyTo(struLogInfo.sDeviceAddress, 0);

                //�豸�û���
                byte[] byUserName = System.Text.Encoding.Default.GetBytes(textBoxUserName.Text);
                struLogInfo.sUserName = new byte[64];
                byUserName.CopyTo(struLogInfo.sUserName, 0);

                //�豸����
                byte[] byPassword = System.Text.Encoding.Default.GetBytes(textBoxPassword.Text);
                struLogInfo.sPassword = new byte[64];
                byPassword.CopyTo(struLogInfo.sPassword, 0);

                struLogInfo.wPort = ushort.Parse(textBoxPort.Text);//�豸����˿ں�

                if (LoginCallBack == null)
                {
                    LoginCallBack = new CHCNetSDK.LOGINRESULTCALLBACK(cbLoginCallBack);//ע��ص�����                    
                }
                struLogInfo.cbLoginResult = LoginCallBack;
                struLogInfo.bUseAsynLogin = false; //�Ƿ��첽��¼��0- ��1- �� 

                DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V40();

                //��¼�豸 Login the device
                m_lUserID = CHCNetSDK.NET_DVR_Login_V40(ref struLogInfo, ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Login_V40 failed, error code= " + iLastErr; //��¼ʧ�ܣ���������
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    //��¼�ɹ�
                    MessageBox.Show("Login Success!");
                    btnLogin.Text = "Logout";

                    btnPreview_Click(null, null);

                    //WinformFace
                    demo = true;//�ʼǱ�����ͷ����
                    if (demo)
                    {
                        SXT();
                    }
                    await Task.Run(() => Demo());
                }

            }
            else
            {
                //ע����¼ Logout the device
                if (m_lRealHandle >= 0)
                {
                    MessageBox.Show("Please stop live view firstly");
                    return;
                }

                if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Logout failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;
                }
                m_lUserID = -1;
                btnLogin.Text = "Login";
            }
            return;
        }

        private void btnPreview_Click(object sender, System.EventArgs e)
        {
            if (m_lUserID < 0)
            {
                MessageBox.Show("Please login the device firstly");
                return;
            }

            if (m_lRealHandle < 0)
            {
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;//Ԥ������
                lpPreviewInfo.lChannel = Int16.Parse(textBoxChannel.Text);//Ԥte�����豸ͨ��
                lpPreviewInfo.dwStreamType = 0;//�������ͣ�0-��������1-��������2-����3��3-����4���Դ�����
                lpPreviewInfo.dwLinkMode = 0;//���ӷ�ʽ��0- TCP��ʽ��1- UDP��ʽ��2- �ಥ��ʽ��3- RTP��ʽ��4-RTP/RTSP��5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = true; //0- ������ȡ����1- ����ȡ��
                lpPreviewInfo.dwDisplayBufNum = 1; //���ſⲥ�Ż�������󻺳�֡��
                lpPreviewInfo.byProtoType = 0;
                lpPreviewInfo.byPreviewMode = 0;

                if (textBoxID.Text != "")
                {
                    lpPreviewInfo.lChannel = -1;
                    byte[] byStreamID = System.Text.Encoding.Default.GetBytes(textBoxID.Text);
                    lpPreviewInfo.byStreamID = new byte[32];
                    byStreamID.CopyTo(lpPreviewInfo.byStreamID, 0);
                }


                if (RealData == null)
                {
                    RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//Ԥ��ʵʱ���ص�����
                }

                IntPtr pUser = new IntPtr();//�û�����

                //��Ԥ�� Start live view 
                // m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);  2020.11.23ע��
                m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, RealData, pUser);
                if (m_lRealHandle < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //Ԥ��ʧ�ܣ���������
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    //Ԥ���ɹ�
                    btnPreview.Text = "Stop Live View";
                }
            }
            else
            {
                //ֹͣԤ�� Stop live view 
                if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;
                }
                m_lRealHandle = -1;
                btnPreview.Text = "Live View";

            }
            return;
        }

        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
            if (dwBufSize > 0)
            {
                byte[] sData = new byte[dwBufSize];
                Marshal.Copy(pBuffer, sData, 0, (Int32)dwBufSize);

                string str = "ʵʱ������.ps";
                FileStream fs = new FileStream(str, FileMode.Create);

                int iLen = (int)dwBufSize;
                fs.Write(sData, 0, iLen);

                fs.Close();
            }
        }

        private void btnBMP_Click(object sender, EventArgs e)
        {
            string sBmpPicFileName;
            //ͼƬ����·�����ļ��� the path and file name to save
            sBmpPicFileName = "F:\\����\\capture picture\\BMP_test.bmp"; //����ͼƬ��·��

            //BMPץͼ Capture a BMP picture
            if (!CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle, sBmpPicFileName))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_CapturePicture failed, error code= " + iLastErr;
                MessageBox.Show(str);
                return;
            }
            else
            {
                str = "Successful to capture the BMP file and the saved file is " + sBmpPicFileName;
                MessageBox.Show(str);
            }
            return;
        }

        private void btnJPEG_Click(object sender, EventArgs e)
        {
            string sJpegPicFileName;
            //ͼƬ����·�����ļ��� the path and file name to save
            sJpegPicFileName = "F:\\����\\capture picture\\JPEG_test.jpg";

            int lChannel = Int16.Parse(textBoxChannel.Text); //ͨ���� Channel number

            CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA();
            lpJpegPara.wPicQuality = 0; //ͼ������ Image quality
            lpJpegPara.wPicSize = 0xff; //ץͼ�ֱ��� Picture size: 2- 4CIF��0xff- Auto(ʹ�õ�ǰ�����ֱ���)��ץͼ�ֱ�����Ҫ�豸֧�֣�����ȡֵ��ο�SDK�ĵ�

            //JPEGץͼ Capture a JPEG picture
            if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, lChannel, ref lpJpegPara, sJpegPicFileName))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_CaptureJPEGPicture failed, error code= " + iLastErr;
                MessageBox.Show(str);
                return;
            }
            else
            {
                str = "Successful to capture the JPEG file and the saved file is " + sJpegPicFileName;
                MessageBox.Show(str);
            }
            return;
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            //¼�񱣴�·�����ļ��� the path and file name to save
            string sVideoFileName;
            sVideoFileName = "Record_test.mp4";

            if (m_bRecord == false)
            {
                //ǿ��I֡ Make a I frame
                int lChannel = Int16.Parse(textBoxChannel.Text); //ͨ���� Channel number
                CHCNetSDK.NET_DVR_MakeKeyFrame(m_lUserID, lChannel);

                //��ʼ¼�� Start recording
                if (!CHCNetSDK.NET_DVR_SaveRealData(m_lRealHandle, sVideoFileName))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_SaveRealData failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    btnRecord.Text = "Stop Record";
                    m_bRecord = true;
                }
            }
            else
            {
                //ֹͣ¼�� Stop recording
                if (!CHCNetSDK.NET_DVR_StopSaveRealData(m_lRealHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_StopSaveRealData failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    str = "Successful to stop recording and the saved file is " + sVideoFileName;
                    MessageBox.Show(str);
                    btnRecord.Text = "Start Record";
                    m_bRecord = false;
                }
            }

            return;
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            //ֹͣԤ�� Stop live view 
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
                m_lRealHandle = -1;
            }

            //ע����¼ Logout the device
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
                m_lUserID = -1;
            }

            CHCNetSDK.NET_DVR_Cleanup();

            Application.Exit();
        }

        private void btnPTZ_Click(object sender, EventArgs e)
        {

        }

        public void VoiceDataCallBack(int lVoiceComHandle, IntPtr pRecvDataBuffer, uint dwBufSize, byte byAudioFlag, System.IntPtr pUser)
        {
            byte[] sString = new byte[dwBufSize];
            Marshal.Copy(pRecvDataBuffer, sString, 0, (Int32)dwBufSize);

            if (byAudioFlag == 0)
            {
                //�������������Ƶ����д���ļ� save the data into a file
                string str = "PC�ɼ���Ƶ�ļ�.pcm";
                FileStream fs = new FileStream(str, FileMode.Create);
                int iLen = (int)dwBufSize;
                fs.Write(sString, 0, iLen);
                fs.Close();
            }
            if (byAudioFlag == 1)
            {
                //�������������Ƶ����д���ļ� save the data into a file
                string str = "�豸��Ƶ�ļ�.pcm";
                FileStream fs = new FileStream(str, FileMode.Create);
                int iLen = (int)dwBufSize;
                fs.Write(sString, 0, iLen);
                fs.Close();
            }

        }

        private void btnVioceTalk_Click(object sender, EventArgs e)
        {
            if (m_bTalk == false)
            {
                //��ʼ�����Խ� Start two-way talk
                CHCNetSDK.VOICEDATACALLBACKV30 VoiceData = new CHCNetSDK.VOICEDATACALLBACKV30(VoiceDataCallBack);//Ԥ��ʵʱ���ص�����

                lVoiceComHandle = CHCNetSDK.NET_DVR_StartVoiceCom_V30(m_lUserID, 1, true, VoiceData, IntPtr.Zero);
                //bNeedCBNoEncData [in]��Ҫ�ص��������������ͣ�0- �������������ݣ�1- ����ǰ��PCMԭʼ����

                if (lVoiceComHandle < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_StartVoiceCom_V30 failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    btnVioceTalk.Text = "Stop Talk";
                    m_bTalk = true;
                }
            }
            else
            {
                //ֹͣ�����Խ� Stop two-way talk
                if (!CHCNetSDK.NET_DVR_StopVoiceCom(lVoiceComHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_StopVoiceCom failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    btnVioceTalk.Text = "Start Talk";
                    m_bTalk = false;
                }
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void Preview_Load(object sender, EventArgs e)
        {

        }

        private void Ptz_Set_Click(object sender, EventArgs e)
        {

        }

        private void PreSet_Click(object sender, EventArgs e)
        {
            PreSet dlg = new PreSet();
            dlg.m_lUserID = m_lUserID;
            dlg.m_lChannel = 1;
            dlg.m_lRealHandle = m_lRealHandle;
            dlg.ShowDialog();

        }
        #region WinformFace  �ֶ�
        private static string API_KEY;
        private static string SECRET_KEY;
        private static string constr;
        public Bitmap bitmap = null;
        public string tuPianStr;
        public int selectedDeviceIndex = 0;

        private bool demo = default;

        private static bool isNoRegister = true;//Ĭ����Ҫע��
        private User_list curUser;
        private UserInfo userInfo = new UserInfo();
        private int user_id_list_count;
        //������ݿ�user_id  ��������id
        List<string> userSearch = new List<string>();

        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private PictureBox picHt;
        #endregion

        /// <summary>
        /// ���þ�̬�ֶ�
        /// </summary>
        private void StaticStringBuilder()
        {
            constr = ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString;
            API_KEY = ConfigurationManager.ConnectionStrings["API_KEY"].ConnectionString;
            SECRET_KEY = ConfigurationManager.ConnectionStrings["SECRET_KEY"].ConnectionString;
        }
        private async Task Demo()
        {
            tuPianStr = default;
            if (!demo)
            {
                if (!await pzToBase64())
                    await Demo();
            }
            int faceCount = await DetectDemo();
            if (faceCount == 0)
                await Demo();
            else if (faceCount == 2)
            {
                if (!await multiSearchDemo())//qps==5
                {
                    if (await Register())
                    {
                        Login(userInfo.userId);
                        Console.WriteLine("���û���¼һ��");
                    }
                    Console.WriteLine("ע��ʧ��");
                }
                Login(userInfo.userId);
            }
            else
            {
                bool isExist = await IsExistFaceDbByPhone();
                if (!isExist)
                {
                   if( await Register())
                    //Login(userInfo.userId);
                    MessageBox.Show("���û���¼���״��ⵥ!");
                    else MessageBox.Show("���û�ע��ʧ��");
                }
                Login(userInfo.userId);
                Console.WriteLine("���û���¼");

            }
            await Demo();

        }
        /// <summary>
        /// �ٶ�������M:N������
        /// 1.����˶�δע�ᣬȫ��ע��  
        /// 2.�����˻�ȫ��ע�ᣬȡ��ֵ�����˵�¼
        /// </summary>
        /// <returns>flase:�������δע��   true:�����Ѵ���</returns>
        public async Task<bool> multiSearchDemo()
        {
            return await Task.Run(() =>
            {
                bool flag = default;
                var image = tuPianStr;

                var imageType = "BASE64";

                var groupIdList = "1";
                var client = new Face(API_KEY, SECRET_KEY);
                FaceMultiSeach seach;
                try
                {
                    // ������������ M:N ʶ�𣬿��ܻ��׳�������쳣����ʹ��try/catch����
                    var result = client.MultiSearch(image, imageType, groupIdList);
                    Console.WriteLine(result);
                    // ����п�ѡ����
                    var options = new Dictionary<string, object>{
                    {"max_face_num", 3},
                    {"match_threshold", 70},
                    {"quality_control", "NORMAL"},
                    {"liveness_control", "LOW"},
                    {"max_user_num", 1}//�������ƶ���ߵ�һ���û�
                };
                    // ������������������ M:N ʶ��
                    result = client.MultiSearch(image, imageType, groupIdList, options);
                    Console.WriteLine(result);
                    seach = JsonConvert.DeserializeObject<FaceMultiSeach>(result.ToString());
                    if (seach.result != null)
                    {
                        flag = true;
                        //ȡ��һ��������Ϣ�б�
                        multiUser_listItem it = seach.result.face_list[0].user_list[0];
                        foreach (var item in seach.result.face_list)
                        {
                            if (item.user_list[0].score > it.score)
                                it = item.user_list[0];
                        }
                        curUser = JsonConvert.DeserializeObject<User_list>(JsonConvert.SerializeObject(it));
                        userInfo.tuPianStr = tuPianStr;
                        userInfo.userId = curUser.user_id;
                    }
                    return flag;

                }
                catch (Exception)
                {
                    MessageBox.Show("����M:N����ʧ��!");
                    return flag;
                }
            });


        }
        /// <summary>
        /// ����ǰ����ת��base64����
        /// </summary>
        public async Task<string> PzToStr()
        {
            if (videoSource == null)
                return default;
            bitmap = videoSourcePlayer1.GetCurrentVideoFrame();

            if (bitmap == null)
                return default;

            MemoryStream ms = new MemoryStream();

            //����ʱ��jpeg��ʽ����
            bitmap.Save(ms, ImageFormat.Jpeg);
            byte[] arr = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(arr, 0, (int)ms.Length);
            ms.Close();
            //ͼƬת�����ַ���
            tuPianStr = Convert.ToBase64String(arr);
            return tuPianStr;
        }

        /// <summary>
        /// ������ͷ��ΪPictureBox��������Դ
        /// </summary>
        public void SXT()
        {
            try
            {
                // ö��������Ƶ�����豸
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count == 0)
                    throw new ApplicationException();
                selectedDeviceIndex = 0;
                videoSource = new VideoCaptureDevice(videoDevices[selectedDeviceIndex].MonikerString);//��������ͷ��
                videoSource.VideoResolution = videoSource.VideoCapabilities[selectedDeviceIndex];
                videoSourcePlayer1.VideoSource = videoSource;
                // set NewFrame event handler
                videoSourcePlayer1.Start();

            }
            catch (ApplicationException)
            {
                MessageBox.Show("No local capture devices");
                videoDevices = null;
            }
        }



        /// <summary>
        /// ʵʱ�������
        /// </summary>
        /// <returns>0��������  1��1������  2���������</returns>
        public async Task<int> DetectDemo()
        {
            if (demo)
                tuPianStr = await PzToStr();
            var image = tuPianStr;
            var imageType = "BASE64";
            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            // ����������⣬���ܻ��׳�������쳣����ʹ��try/catch����
            // ����п�ѡ����
            var options = new Dictionary<string, object>{
                {"face_field", "age"},
                {"max_face_num", 10},
                {"face_type", "LIVE"},//������
                {"liveness_control", "HIGH"}//�ߵĻ�����

                ///�ʼǱ���������   byte.length==18000~31000    ���᷵��quality
                ///
            };
            // �����������������
            var result = client.Detect(image, imageType, options);
            FaceidentifyInfo detect = null;
            detect = JsonConvert.DeserializeObject<FaceidentifyInfo>(result.ToString());
            if (detect.result == null)
                return 0;
            //�ж���Ƭ��������������ȫ����Ҳ��ͨ��
            if (detect.result.face_num == 1)
            {
                if (detect.result.face_list[0].face_probability == 0)
                    return 0;
                return 1;
                //if (detect.result.face_list[0].face_probability == 1 && detect.result.face_list[0].quality.completeness == 1)
                //    return 1;
            }
            return detect.result.face_num;
        }


        /// <summary>
        /// ������������ָ��uidƥ��
        /// </summary>
        /// <param name="uid">������ָ��uid</param>
        /// <returns>���������Ƿ�Ϊͬһ��</returns>
        public bool FaceOneToOne(string uid)
        {
            var image = tuPianStr;
            var imageType = "BASE64";
            var groupIdList = "group1";
            try
            {
                var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
                var options = new Dictionary<string, object>{
                {"max_face_num", 3},
                {"match_threshold", 80},
                {"quality_control", "NORMAL"},
                {"liveness_control", "NORMAL"},
                {"user_id", uid},
                { "max_user_num", 1}
    };
                var result = client.Search(image, imageType, groupIdList, options);
                Console.WriteLine(result);
                FaceSearch seach;
                try
                {
                    JsonSerializerSettings jsSetting = new JsonSerializerSettings();
                    jsSetting.NullValueHandling = NullValueHandling.Ignore;
                    seach = JsonConvert.DeserializeObject<FaceSearch>(result.ToString(), jsSetting);
                    if (seach.result != null && seach.result.user_list[0].score > 90)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Jsonתsearchʧ��");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("FaceToFace��������ʧ�ܣ�  ���������Ƿ��������!");

            }
            return false;
        }

        /// <summary>
        /// ��ǰ�����Ƿ�������ݿ⣬��������ע�ᵽ����������ݿ⣬�������¼��ǰ�û�
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public async Task<bool> IsExistDbByPhone()
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select user_id from UserInfo", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //ͳ�����ڰٶ�����������û����������û��Զ�����uid
                    userSearch.Add(reader["user_id"].ToString());
                    //�������user_id�Ƿ�ƥ����Ƭ
                    if (this.FaceOneToOne(reader["user_id"].ToString()))
                    {
                        isNoRegister = false;
                        curUser.user_id = reader["user_id"].ToString();
                        continue;
                    }
                }
                if (isNoRegister == true)
                {
                    string id = $"user{userSearch.Count + 1}";
                    if (RegisterBaiduServer(id))
                    {
                        MessageBox.Show("�������û���ӳɹ�");
                        if (RegisterSqlServer(id))
                        {
                            MessageBox.Show("���ݿ��û���ӳɹ�");
                        }
                    }
                }
            }
            //�ָ�����Search
            userSearch.Clear();
            return true;
        }

        /// <summary>
        /// �����Ƿ�����ڰٶ�������
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public async Task<bool> IsExistFaceDbByPhone()
        {
            FaceSearch seach;
            bool flag = false;

            var image = tuPianStr;
            var imageType = "BASE64";
            var groupIdList = "group1";
            try
            {
                var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
                var options = new Dictionary<string, object>{
                {"max_face_num", 1},
                {"match_threshold", 80},
                {"quality_control", "NORMAL"},
                {"liveness_control", "NORMAL"},
                { "max_user_num", 3}//��Ϊ1����Ĭ��ȡ��ֵ��ߵ�
    };
                var result = client.Search(image, imageType, groupIdList, options);
                Console.WriteLine(result);

                try
                {
                    JsonSerializerSettings jsSetting = new JsonSerializerSettings();
                    jsSetting.NullValueHandling = NullValueHandling.Ignore;
                    seach = JsonConvert.DeserializeObject<FaceSearch>(result.ToString(), jsSetting);
                    if (seach.result != null && seach.result.user_list[0].score > 90)
                    {
                        //���û�
                        flag = true;
                        curUser = seach.result.user_list[0];
                        foreach (var item in seach.result.user_list)
                        {
                            if (item.score > curUser.score)
                            {
                                curUser = item;
                            }

                        }
                        Console.WriteLine("���û�idΪ" + curUser.user_id);
                        userInfo.tuPianStr = tuPianStr;
                        userInfo.userId = curUser.user_id;
                    }
                    else
                    {
                        userInfo.tuPianStr = tuPianStr;
                        userInfo.userId = "user" + (user_id_list_count + 1);

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Jsonתsearchʧ��");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("FaceToFace��������ʧ�ܣ�  ���������Ƿ��������!");
            }
            return flag;
        }

        public async Task<bool> Register()
        {
            //return await Task.Run(() =>
            //{
                int id = user_id_list_count + 1;
                if (id == 0)
                {
                    MessageBox.Show("��ĵ��ô���������");
                    return false;
                }
            //RegisterBaiduServer("user" + id);
            //RegisterSqlServer("user" + id);
            //Task.Run(() =>
            //{
            //user_id_list_count = SelectBaiDuGroupRootListCount("group1");
            //});
            //return true;


            user_id_list_count = SelectBaiDuGroupRootListCount("group1");
            return RegisterBaiduServer("user" + id) && RegisterSqlServer("user" + id);
            //});
        }

        /// <summary>
        /// �������������û�����
        /// </summary>
        /// <returns></returns>
        public int SelectBaiDuGroupRootListCount(string groupId)
        {
            try
            {
                // ���û�ȡ�û��б����ܻ��׳�������쳣����ʹ��try/catch����
                var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
                var result = client.GroupGetusers(groupId);
                Console.WriteLine(result);
                // ����п�ѡ����
                var options = new Dictionary<string, object>{
                         {"start", 0},
                         {"length", 1000}
                            };
                // ���������û�ȡ�û��б�
                result = client.GroupGetusers(groupId, options);
                FaceGetGroupUsers faceGetGroupUsers;
                faceGetGroupUsers = JsonConvert.DeserializeObject<FaceGetGroupUsers>(result.ToString());
                Console.WriteLine(result);
                if (faceGetGroupUsers.result != null)
                    return faceGetGroupUsers.result.user_id_list.Count;

            }
            catch (Exception)
            {
                MessageBox.Show("��ȡ�û��б�ʧ�ܣ�");
            }
            return -1;

        }

        /// <summary>
        /// ������Ϣ��������idע�ᵽ���ݿ�
        /// </summary>
        /// <param name="userInfo">������Ϣ</param>
        /// <param name="userId">������id</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        private bool RegisterSqlServer(string userId)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand($"insert into UserInfo (user_id,age) values('{userId}',{userInfo.age}) ", con);
                try
                {
                    if (cmd.ExecuteNonQuery() > 0)
                        return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return false;
        }

        /// <summary>
        /// ����ע�ᵽ�ٶ�������
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>�����Ƿ�ɹ�</returns>
        private bool RegisterBaiduServer(string userId)
        {
            var imageType = "BASE64";
            var groupId = "group1";
            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);

            // ��������ע�ᣬ���ܻ��׳�������쳣����ʹ��try/catch����
            // ����п�ѡ����
            var options = new Dictionary<string, object>{
                        {"user_info", "user's info"},
                        {"quality_control", "NORMAL"},
                        {"liveness_control", "LOW"},
                        {"action_type", "APPEND"}
                    };
            FaceAddUser faceAddUser;
            try
            {
                var result = client.UserAdd(tuPianStr, imageType, groupId, userId, options);
                faceAddUser = JsonConvert.DeserializeObject<FaceAddUser>(result.ToString());
                if (faceAddUser.error_msg == "SUCCESS")
                {
                    //MessageBox.Show("�ɹ�ע��");
                    return true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ע��ʧ�ܣ����������Ƿ�����");
            }
            return false;
        }

        /// <summary>
        /// CurrentUser��¼����+1��ģ���ò�һ��
        /// </summary>
        /// <returns></returns>
        public async void Login(string currentUserId)
        {
            //MessageBox.Show($"��ӭ�ؼң�ף��{currentUser}�ò����");
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand($"update Userinfo set loginTimes+=1 where user_id='{currentUserId}'", con);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("֧���ɹ�");
                    Console.WriteLine(System.DateTime.Now);
                }
                else
                {
                    MessageBox.Show("֧��ʧ��");
                }
            }
        }






        /// <summary>
        /// ��֡���ݲ�����ƬתBase64�Ƿ�ɹ�
        /// </summary>
        /// <returns>true �ɹ�   false  ʧ��</returns>
        public async Task<bool> pzToBase64()
        {
            //return await Task.Run(() =>
            //{
                tuPianStr = default;
                int lChannel = Int16.Parse("1"); //ͨ���� Channel number

                CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA();
                lpJpegPara.wPicQuality = 2; //ͼ������ Image quality
                lpJpegPara.wPicSize = 0xff; //ץͼ�ֱ��� Picture size: 2- 4CIF��0xff- Auto(ʹ�õ�ǰ�����ֱ���)��ץͼ�ֱ�����Ҫ�豸֧�֣�����ȡֵ��ο�SDK�ĵ�

                //JPEGץͼ �����ڴ�
                uint size = 0;
                byte[] jpg = new byte[100000];
                bool isJPEG;
                isJPEG = CHCNetSDK.NET_DVR_CaptureJPEGPicture_NEW(m_lUserID, lChannel, ref lpJpegPara, jpg, 100000, ref size);
                if (isJPEG)
                {
                    jpg.Take((int)size).ToArray();
                    tuPianStr = Convert.ToBase64String(jpg);
                    return true;
                }
                else
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Logout failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return false;
                }
            //});
        }

        /// <summary>
        /// ��֡���ݲ��񱣴浽����jpg�ļ�  �Ƿ�ɹ�
        /// </summary>
        /// <returns>true �ɹ�   false  ʧ��</returns>
        public async Task<bool> pzToJPEG()
        {
            return await Task.Run(() =>
            {
                string sJpegPicFileName;
                //ͼƬ����·�����ļ��� the path and file name to save
                sJpegPicFileName = "C:\\Users\\ؼscholarؼ\\Desktop\\JPEG_test.jpg";

                int lChannel = Int16.Parse("1"); //ͨ���� Channel number

                CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA();
                lpJpegPara.wPicQuality = 0; //ͼ������ Image quality
                lpJpegPara.wPicSize = 0xff; //ץͼ�ֱ��� Picture size: 2- 4CIF��0xff- Auto(ʹ�õ�ǰ�����ֱ���)��ץͼ�ֱ�����Ҫ�豸֧�֣�����ȡֵ��ο�SDK�ĵ�

                //JPEGץͼ Capture a JPEG picture  ���汾��
                if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, lChannel, ref lpJpegPara, sJpegPicFileName))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_CaptureJPEGPicture failed, error code= " + iLastErr;
                    //MessageBox.Show(str);
                    return false;
                }
                else
                {
                    str = "Successful to capture the JPEG file and the saved file is " + sJpegPicFileName;
                    //MessageBox.Show(str);
                    return true;
                }
            });
        }




        private void btnUpLoadPhone_Click(object sender, EventArgs e)
        {

        }

        private async void btnManualIdentifiction_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                user_id_list_count = SelectBaiDuGroupRootListCount("group1");
                tuPianStr = await PzToStr();
                int faceCount = await DetectDemo();
                if (faceCount == 0)
                    MessageBox.Show("��ǰ������!");
                else if (faceCount == 2)
                {
                    if (!await multiSearchDemo())//qps==5
                    {
                        if (await Register())
                        {
                            Login(userInfo.userId);
                            Console.WriteLine("���û���¼һ��");
                        }
                        Console.WriteLine("ע��ʧ��");
                    }
                    Login(userInfo.userId);
                }
                else
                {
                    bool isExist = await IsExistFaceDbByPhone();
                    if (!isExist)
                    {
                        await Register();
                        Login(userInfo.userId);
                    }
                    //��¼һ��
                    Login(userInfo.userId);
                    Console.WriteLine("���û���¼");
                }
            });
        }
    }
}
