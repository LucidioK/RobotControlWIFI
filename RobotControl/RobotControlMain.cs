using System.Text.RegularExpressions;

namespace RobotControl
{
    public partial class RobotControlMain : Form
    {
        public RobotControlMain()
        {
            InitializeComponent();
            lstLabelsToFind.Items.AddRange(YOLORecognitionLabels);
        }

        private Regex ipAddressPattern = new Regex(@"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$");

        private Regex cameraRSTLURLPattern = new Regex(@"rtsp://[^:]+:[^@]+@[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}:[0-9]{1,5}.*$");
        private void txtRobotIPAddress_TextChanged(object sender, EventArgs e) =>
            validateText(txtRobotIPAddress, ipAddressPattern);
        private void txtCameraRSTLURL_TextChanged(object sender, EventArgs e) =>
            validateText(txtCameraRSTLURL, cameraRSTLURLPattern);

        public static string[] YOLORecognitionLabels =
        {
                "aeroplane"  , "bicycle", "bird" , "boat"     , "bottle"   ,
                "bus"        , "car"    , "cat"  , "chair"    , "cow"      ,
                "diningtable", "dog"    , "horse", "motorbike", "person"   ,
                "pottedplant", "sheep"  , "sofa" , "train"    , "tvmonitor"
        };
        private void validateText(TextBox textBox, Regex regex)
        {
            textBox.ForeColor = regex.IsMatch(textBox.Text) ? Color.Black : Color.Red;
            enableStartIfNeeded();
        }

        private void lstLabelsToFind_SelectedIndexChanged(object sender, EventArgs e) =>
            enableStartIfNeeded();

        private void enableStartIfNeeded()
        {
            btnStart.Enabled =
                lstLabelsToFind.SelectedIndices.Count > 0  &&
                txtRobotIPAddress.Text.Any()               &&
                txtRobotIPAddress.ForeColor == Color.Black &&
                txtCameraRSTLURL.Text.Any()                &&
                txtCameraRSTLURL.ForeColor == Color.Black;
        }
    }
}