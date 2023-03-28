using RobotControl.ClassLibrary;
using RobotControl.ClassLibrary.ImageRecognition;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace RobotControl
{
    public partial class RobotControlMain : Form
    {
        private IImageRecognitionFromCamera imageRecognition;
        private Thread cameraWorkerThread;
        private string jsonFileName;
        private Regex ipAddressPattern = new Regex(@"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$");
        private Regex cameraRSTLURLPattern = new Regex(@"rtsp://[^:]+:[^@]+@[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}:[0-9]{1,5}.*$");
        private Regex gpuDeviceIdPattern = new Regex(@"^[0-9]{1,2}");
        public RobotControlMain()
        {
            InitializeComponent();
            lstLabelsToFind.Items.AddRange(YOLORecognitionLabels);
            jsonFileName       = Path.Combine(Path.GetTempPath(), nameof(RobotControl) + ".json");
            cameraWorkerThread = new Thread(CameraWorkerProc) { Priority = ThreadPriority.AboveNormal };
            fromJSON();
        }

        private void txtRobotIPAddress_TextChanged(object sender, EventArgs e) =>
            validateText(txtRobotIPAddress, ipAddressPattern);
        private void txtCameraRSTLURL_TextChanged(object sender, EventArgs e) =>
            validateText(txtCameraRSTLURL, cameraRSTLURLPattern);
        private void txtGPUDeviceId_TextChanged(object sender, EventArgs e)
        {
            if (chkHasGPU.Checked)
            {
                validateText(txtGPUDeviceId, gpuDeviceIdPattern);
            }
        }

        public static string[] YOLORecognitionLabels => TinyYolo2Labels.Labels;
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
            if (btnStart.Enabled)
            {
                saveToJSON();
            }
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
            if (thisWindow == null)
            {
                throw new ArgumentException("Window parameter was null, aborting.");
            }
            IList<string> itemsToRecognize = GetItemsToRecognize(thisWindow);

            while (true)
            {
                var imageData = thisWindow?.imageRecognition.Get(itemsToRecognize);
                if (imageData != null && imageData?.Bitmap != null)
                {
                    try
                    {
                        thisWindow.Invoke(() =>
                        {
                            this.lblTime.Text = DateTime.Now.ToString();
                            thisWindow.pctImage.Image = new Bitmap(imageData?.Bitmap, new Size(thisWindow.pctImage.Width, thisWindow.pctImage.Height));
                        });
                    }
                    catch (ObjectDisposedException ex)
                    { 
                        // This happens when the form is closed.
                        System.Diagnostics.Debug.WriteLine(ex);
                        break;
                    }
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

        private void chkHasGPU_CheckedChanged(object sender, EventArgs e) =>
            txtGPUDeviceId.Enabled = chkHasGPU.Checked;

        private void fromJSON()
        {
            if (Path.Exists(jsonFileName)) 
            {
                var config = JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(jsonFileName));
                if (config != null) 
                { 
                    foreach (string controlName in config.Keys) 
                    {
                        Control control = Controls[controlName];
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = config[controlName].ToString();
                        }
                        if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = bool.Parse(config[controlName].ToString());
                        }
                        if (control is ListBox)
                        {
                            ListBox lb = (ListBox)control;
                            IList<string> strings = JsonSerializer.Deserialize<List<string>>(config[controlName].ToString());
                            var indices = strings.Select(s => Array.FindIndex(YOLORecognitionLabels, l => l == s));
                            foreach (var index in indices)
                            {
                                lb.SelectedIndex= index;
                            }
                        }
                    }
                }
            }
        }

        private void saveToJSON()
        {
            var config = new Dictionary<string, object>();
            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                {
                    config[control.Name] = ((TextBox)control).Text;
                }
                if (control is CheckBox)
                {
                    config[control.Name] = ((CheckBox)control).Checked;
                }
                if (control is ListBox)
                {
                    config[control.Name] = ((ListBox)control).SelectedItems.Cast<object>().ToList().Select(item => item.ToString()).ToList();
                }
            }

            File.WriteAllText(jsonFileName, JsonSerializer.Serialize(config));
        }
    }
}