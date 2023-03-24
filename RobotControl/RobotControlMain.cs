using RobotControl.ClassLibrary;
using RobotControl.ClassLibrary.ImageRecognition;
using System.Text.RegularExpressions;

namespace RobotControl
{
    public partial class RobotControlMain : Form
    {
        IImageRecognitionFromCamera imageRecognition;
        protected Thread cameraWorkerThread;
        public RobotControlMain()
        {
            InitializeComponent();
            lstLabelsToFind.Items.AddRange(YOLORecognitionLabels);
            cameraWorkerThread = new Thread(CameraWorkerProc) { Priority = ThreadPriority.AboveNormal };
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            imageRecognition = ClassFactory.CreateImageRecognitionFromCamera(int.Parse(txtGPUDeviceId.Text));

            imageRecognition.Open(txtCameraRSTLURL.Text);
            cameraWorkerThread.Start(this);
            btnStart.Enabled = false;
        }

        private void CameraWorkerProc(object? obj)
        {
            var thisWindow = obj as RobotControlMain;
            if (thisWindow != null)
            {
                IList<string> itemsToRecognize = GetItemsToRecognize(thisWindow);

                while (true)
                {
                    var imageData = thisWindow?.imageRecognition.Get(itemsToRecognize);
                    thisWindow.Invoke(() =>
                    {
                        thisWindow.pctImage.Image = imageData?.Bitmap;
                    });
                }
            }
        }

        private static IList<string> GetItemsToRecognize(RobotControlMain? thisWindow)
        { 
            var items = new List<string>();
            if (thisWindow != null)
            {
                thisWindow.Invoke(() =>
                {
                    foreach (var selectedItem in thisWindow.lstLabelsToFind.SelectedItems)
                    {
                        if (selectedItem != null)
                        {
                            items.Add(selectedItem.ToString());
                        }
                    }
                });
            }

            return items;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkHasGPU_CheckedChanged(object sender, EventArgs e) =>
            txtGPUDeviceId.Enabled = chkHasGPU.Checked;
    }
}