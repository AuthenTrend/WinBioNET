using System;
using System.Threading;
using System.Windows.Forms;
using WinBioNET;
using WinBioNET.Enums;
using System.Text;

namespace WindowsFormsTest
{
    public partial class WinBioForm
        : Form
    {
        private static readonly Guid DatabaseId = Guid.Parse("BC7263C3-A7CE-49F3-8EBF-D47D74863CC6");
        private WinBioSessionHandle _session;
        private int _unitId;

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
        }

        protected void Log(WinBioException exception)
        {
            Log(exception.Message);
        }

        protected override void OnLoad(EventArgs e)
        {
            var units = WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint);
            Log(string.Format("Found {0} units", units.Length));
            if (units.Length == 0) return;
            for(int i =0;i< units.Length;i++)
            {
                Log(string.Format("- Unit id: {0}", units[i].UnitId));
                Log(string.Format("     Unit id: {0}", units[i].DeviceInstanceId));
            }
            _unitId = units[0].UnitId;

            var databases = WinBio.EnumDatabases(WinBioBiometricType.Fingerprint);
            Console.WriteLine("Found {0} databases", databases.Length);
            for (var i = 0; i < databases.Length; i++)
            {
                Console.WriteLine("DatabaseId {0}: {1}", i, databases[i].DatabaseId);
            }

            Log(string.Format("Using unit id: {0}", _unitId));
            Log(string.Format("Using database: {0}", _databaseId));

            _session = WinBio.OpenSession(WinBioBiometricType.Fingerprint, WinBioPoolType.System, WinBioSessionFlag.Default, null, 0);
            //_session = WinBio.OpenSession(WinBioBiometricType.Fingerprint);
            Log("Session opened: " + _session.Value);
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
                    var unitId = WinBio.LocateSensor(_session);
                    Log(string.Format("Sensor located: unit id {0}", unitId));
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
                    WinBioIdentity identity;
                    WinBioBiometricSubType subFactor;
                    WinBioRejectDetail rejectDetail;
                    WinBio.Identify(_session, out identity, out subFactor, out rejectDetail);

                    StringBuilder name = new StringBuilder();
                    uint cchName = (uint)name.Capacity;
                    StringBuilder referencedDomainName = new StringBuilder();
                    uint cchReferencedDomainName = (uint)referencedDomainName.Capacity;
                    WinBio.SID_NAME_USE sidUse;
                    // Sid for BUILTIN\Administrators
                    byte[] Sid = new byte[identity.AccountSidSize];
                    identity.AccountSid.GetBinaryForm(Sid, 0);
                    if (!WinBio.LookupAccountSid(null, Sid, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse))
                    {
                        Log(string.Format("Identity= {0}", identity));
                    }
                    else
                    {
                        Log(string.Format("Identity Name = {0}", name));
                    }

                    Log(string.Format("SubFactor= {0}", subFactor));
                }
                catch (WinBioException ex)
                {
                    Log(ex);
                }
            });
        }

        private void buttonEnroll_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    var identity = AddEnrollment(_session, _unitId, WinBioBiometricSubType.RhIndexFinger);
                    Log(string.Format("Identity: {0}", identity));
                }
                catch (WinBioException ex)
                {
                    Log(ex);
                }
            });
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            WinBio.Cancel(_session);
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
                        throw new WinBioException(code, "WinBioEnrollCapture failed");
                }
            }
            Log(string.Format("Committing enrollment.."));
            WinBioIdentity identity;
            var isNewTemplate = WinBio.EnrollCommit(session, out identity);
            Log(string.Format(isNewTemplate ? "New template committed." : "Template already existing."));
            return identity;
        }
    }
}
