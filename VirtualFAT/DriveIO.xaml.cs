using System;
using System.Windows;

namespace VirtualFAT
{
    /// <summary>
    /// Interaction logic for DriveIO.xaml
    /// </summary>
    public partial class DriveIO : Window
    {
        public bool IsCanCancel = true;

        public DriveIO(bool isCanCancel)
        {
            InitializeComponent();
            IsCanCancel = isCanCancel;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(VolumeLabel.Text))
            {
                Drive.Format(Int32.Parse(Capacity.Text), Int32.Parse(AllocationUnitSize.Text), VolumeLabel.Text);
                DialogResult = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(VolumeLabel.Text))
            {
                e.Cancel = true;
                MessageBox.Show(this, $"Enter volume name.",
                           "Confirmation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Drive.Format(Int32.Parse(Capacity.Text), Int32.Parse(AllocationUnitSize.Text), VolumeLabel.Text);
                DialogResult = true;
            }
        }
    }
}
