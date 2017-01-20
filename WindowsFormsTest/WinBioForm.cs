using System;
using System.Threading;
using System.Windows.Forms;
using WinBioNET;
using WinBioNET.Enums;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;

namespace WindowsFormsTest
{
    public partial class WinBioForm
        : Form
    {
        //private static readonly Guid DatabaseId = Guid.Parse("BC7263C3-A7CE-49F3-8EBF-D47D74863CC6");
        private WinBioSessionHandle _session;
        private WinBioIdentity _identity;
        private int _unitId;
        private String _name;

        public WinBioForm()
        {
            InitializeComponent();
        }

        protected void Log(string message)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new Action<string>(Log), message);
                return;
            }
            richTextBox.AppendText(message + "\n");
            // scroll it automatically
            richTextBox.ScrollToCaret();
        }

        protected void setComboxSelectedIndex(ComboBox cbb, int index)
        {
            if (cbb.InvokeRequired)
            {
                cbb.Invoke(new Action<ComboBox, int>(setComboxSelectedIndex), cbb, index);
                return;
            }
            cbb.SelectedIndex = index;
        }

        protected void Log(WinBioException exception)
        {
            Log(exception.Message);
        }

        protected override void OnLoad(EventArgs e)
        {
            _identity = null;
            comboUnitId.Items.Clear();
            comboUnitId.ResetText();

            var units = WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint);
            Log(string.Format("Found {0} units", units.Length));
            if (units.Length == 0) return;
            for (int i = 0; i < units.Length; i++)
            {
                Log(string.Format("- Unit id: {0}", units[i].UnitId));
                comboUnitId.Items.Add(units[i].UnitId);

                string strDeviceInstanceId = units[i].DeviceInstanceId;
                Log(string.Format("     Unit id: {0}", strDeviceInstanceId));
                string strFirmwareVersion = getFirmwareVersion(strDeviceInstanceId);
                if(strFirmwareVersion.Equals(""))
                    Log(string.Format("     Firmware: {0}", units[i].FirmwareVersion.ToString()));
                else
                    Log(string.Format("     Firmware: {0}", strFirmwareVersion));

                int lastInvSlash = units[i].DeviceInstanceId.LastIndexOf("\\");
                Log(string.Format("     sn: {0}",
                    strDeviceInstanceId.Substring(lastInvSlash+1, strDeviceInstanceId.Length - lastInvSlash - 1)));
            }

            _unitId = units[0].UnitId;
            setComboxSelectedIndex(comboUnitId, 0); // use 1st one by default.
            Log(string.Format("Using unit id: {0}", _unitId));

            _session = WinBio.OpenSession(WinBioBiometricType.Fingerprint, WinBioPoolType.System, WinBioSessionFlag.Default, null, 0);
            Log("Session opened: " + _session.Value);

            comboBoxFp.Items.Clear();
            comboBoxFp.ResetText();
            String[] myArr = new String[] {
                "Unknown",//0
                "RhThumb",
                "RhIndexFinger",
                "RhMiddleFinger",
                "RhRingFinger",
                "RhLittleFinger",
                "LhThumb",
                "LhIndexFinger",
                "LhMiddleFinger",
                "LhRingFinger",
                "LhLittleFinger" }; //10
            comboBoxFp.Items.AddRange(myArr);
            setComboxSelectedIndex(comboBoxFp, 1);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_session.IsValid) return;
            WinBio.CloseSession(_session);
            _session.Invalidate();
            Log("Session closed");
        }

        private void buttonLocateSensor_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                Log("Locating sensor...");
                try
                {
                    _unitId = WinBio.LocateSensor(_session);
                    Log(string.Format("Sensor located: unit id {0}", _unitId));
                    setComboxSelectedIndex(comboUnitId, comboUnitId.Items.IndexOf(_unitId));
                }
                catch (WinBioException ex)
                {
                    Log(ex);
                }
            });
        }

        private void buttonIdentify_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                Log("Identifying user...");
                try
                {
                    WinBioBiometricSubType subFactor;
                    WinBioRejectDetail rejectDetail;
                    _unitId = WinBio.Identify(_session, out _identity, out subFactor, out rejectDetail);
                    setComboxSelectedIndex(comboUnitId, comboUnitId.Items.IndexOf(_unitId));
                    Log(string.Format("Sensor used: unit id {0}", _unitId));
                    Log(string.Format("Identity = {0}", _identity));
                    updateCurNameFromIdentity();
                    Log(string.Format("Identity Name = {0}", _name));
                    Log(string.Format("SubFactor= {0}", subFactor));
                }
                catch (WinBioException ex)
                {
                    Log(ex);
                }
            });
        }

        private void updateCurNameFromIdentity()
        {
            StringBuilder name = new StringBuilder();
            uint cchName = (uint)name.Capacity;
            StringBuilder referencedDomainName = new StringBuilder();
            uint cchReferencedDomainName = (uint)referencedDomainName.Capacity;
            SetupAPI.SID_NAME_USE sidUse;
            // Sid for BUILTIN\Administrators
            byte[] Sid = new byte[_identity.AccountSidSize];
            _identity.AccountSid.GetBinaryForm(Sid, 0);
            if (!SetupAPI.LookupAccountSid(null, Sid, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse))
                _name = "";
            else
                _name = name.ToString();
        }


        private void buttonEnroll_Click(object sender, EventArgs e)
        {
            WinBioBiometricSubType subType = (WinBioBiometricSubType)comboBoxFp.SelectedIndex;
            if (subType == WinBioBiometricSubType.Unknown)
            {
                Log("Please select a finger index, not Unknown.");
                return;
            }
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    Log("Put your finger on sensor...");
                    WinBioBiometricSubType subFactor;
                    WinBioRejectDetail rejectDetail;
                    while (true)
                    {
                        _unitId = WinBio.Identify(_session, out _identity, out subFactor, out rejectDetail);
                        Log("No. This finger has been enrolled before. Change another one...");
                    }
                }
                catch (WinBioException ex)
                {
                    Log("OK. This finger not yet be enrolled before.");
                }


                try
                {
                    _identity = AddEnrollment(_session, _unitId, subType);
                    Log(string.Format("Identity: {0}", _identity));
                }
                catch (WinBioException ex)
                {
                    Log(ex);
                    if (ex.ErrorCode == WinBioErrorCode.DuplicateEnrollment)
                        Log(string.Format("Please select another finger index or just delete it for a new one."));
                }
            });
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            WinBio.Cancel(_session);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (_identity == null)
            {
                Log(string.Format("Please do Identity first."));
            }
            else
            {
                try
                {
                    WinBio.DeleteTemplate(_session, _unitId, _identity, (WinBioBiometricSubType)comboBoxFp.SelectedIndex);
                    Log(string.Format("Delete Done."));
                }
                catch (WinBioException ex)
                {
                    Log(ex);
                }
            }
        }

        private WinBioIdentity AddEnrollment(WinBioSessionHandle session, int unitId, WinBioBiometricSubType subType)
        {
            Log(string.Format("Beginning enrollment of {0}:", subType));
            WinBio.EnrollBegin(session, subType, unitId);
            var code = WinBioErrorCode.MoreData;
            for (var swipes = 1; code != WinBioErrorCode.Ok; swipes++)
            {
                WinBioRejectDetail rejectDetail;
                code = WinBio.EnrollCapture(session, out rejectDetail);
                switch (code)
                {
                    case WinBioErrorCode.MoreData:
                        Log(string.Format("Swipe {0} was good", swipes));
                        break;
                    case WinBioErrorCode.BadCapture:
                        Log(string.Format("Swipe {0} was bad: {1}", swipes, rejectDetail));
                        break;
                    case WinBioErrorCode.Ok:
                        Log(string.Format("Enrollment complete with {0} swipes", swipes));
                        break;
                    default:
                        // Force to discard in-progress Enroll operation.
                        WinBio.EnrollDiscard(session);
                        throw new WinBioException(code, "WinBioEnrollCapture failed");
                }
            }
            Log(string.Format("Committing enrollment.."));
            WinBioIdentity identity;
            var isNewTemplate = WinBio.EnrollCommit(session, out identity);
            Log(string.Format(isNewTemplate ? "New template committed." : "Template already existing."));
            return identity;
        }

        private void comboUnitId_SelectionChanged(object sender, EventArgs e)
        {
            _unitId = int.Parse(comboUnitId.SelectedItem.ToString());
        }

        private string getFirmwareVersion(string deviceInstanceId)
        {
            var biometricClassGuid = "53D29EF7-377C-4D14-864B-EB3A85769359";
            Guid FPguid = new Guid(biometricClassGuid);
            IntPtr h = SetupAPI.SetupDiGetClassDevs(
                ref FPguid,
                IntPtr.Zero,
                IntPtr.Zero,
                SetupAPI.DiGetClassFlags.DIGCF_PRESENT
            );
            if (h == (IntPtr)0)
                return "";
            SetupAPI.SP_DEVINFO_DATA dia = new SetupAPI.SP_DEVINFO_DATA();
            dia.cbSize = Marshal.SizeOf(dia);
            for (int i = 0; SetupAPI.SetupDiEnumDeviceInfo(h, i, ref dia); ++i)
            {
                UInt32 RequiredSize = 0;
                StringBuilder sb = new StringBuilder(1024);
                if (!SetupAPI.SetupDiGetDeviceInstanceId(h, ref dia, sb, 1024, out RequiredSize))
                    continue;

                if(!deviceInstanceId.Equals(sb.ToString()))
                    continue;

                UInt32 RegType;
                if (!SetupAPI.SetupDiGetDeviceRegistryProperty(h, ref dia, SetupAPI.RegPropertyType.SPDRP_HARDWAREID, out RegType, sb, 1024, out RequiredSize))
                    continue;

                int pos = sb.ToString().IndexOf("REV_");
                if (pos <= 0)
                    break;
                pos += "REV_".Length;
                string s_toReturn = sb.ToString().Substring(pos, 4);

                SetupAPI.SetupDiDestroyDeviceInfoList(h);
                return s_toReturn;
            }
            return "";
        }

        private void IdentifyTheKeyLED_Click(object sender, EventArgs e)
        {
            try
            {
                WinBioRejectDetail rejectDetail;
                Log(string.Format("Please touch session: unit id {0} in flashing", _unitId));
                WinBio.EnrollBegin(_session, WinBioBiometricSubType.LhThumb, _unitId);
                WinBio.EnrollCapture(_session, out rejectDetail);
                WinBio.EnrollDiscard(_session);
                Log(string.Format("Done"));
            }
            catch (WinBioException ex)
            {
               //ignore
            }

        }
    }
}
