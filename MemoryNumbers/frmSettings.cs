﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace MemoryNumbers
{
    public partial class frmSettings : Form
    {
        // The return value
        public ProgramSettings<string, string> settings;
        private ProgramSettings<string, string> _defaultSettings;

        public frmSettings()
        {
            InitializeComponent();

            // Set form icons and images
            var path = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (System.IO.File.Exists(path + @"\images\settings.ico")) this.Icon = new Icon(path + @"\images\settings.ico");
        }

        public frmSettings(ProgramSettings<string, string> _settings, ProgramSettings<string, string> _default)
            :this()
        {
            
            try
            {
                PlayMode play = (PlayMode)Convert.ToInt32(_settings.ContainsKey("PlayMode") ? _settings["PlayMode"] : _default["PlayMode"]);
                this.radProgressive.Checked = ((play & PlayMode.SequenceProgressive) == PlayMode.SequenceProgressive);
                this.radRandom.Checked = ((play & PlayMode.SequenceRandom) == PlayMode.SequenceRandom);
                this.radFixed.Checked = ((play & PlayMode.TimeFixed) == PlayMode.TimeFixed);
                this.radIncremental.Checked = ((play & PlayMode.TimeIncremental) == PlayMode.TimeIncremental);
                this.numTimeIncrement.Enabled = this.radIncremental.Checked;
                this.trackTimeIncrement.Enabled = this.radIncremental.Checked;

                this.numTime.Value = Convert.ToInt32(_settings.ContainsKey("Time") ? _settings["Time"] : _default["Time"]);
                this.numTimeIncrement.Value = Convert.ToInt32(_settings.ContainsKey("TimeIncrement") ? _settings["TimeIncrement"] : _default["TimeIncrement"]);
                this.numMaxDigit.Value = Convert.ToInt32(_settings.ContainsKey("MaximumDigit") ? _settings["MaximumDigit"] : _default["MaximumDigit"]);
                this.numMinDigit.Value = Convert.ToInt32(_settings.ContainsKey("MinimumDigit") ? _settings["MinimumDigit"] : _default["MinimumDigit"]);
                this.numMaxAttempts.Value = Convert.ToInt32(_settings.ContainsKey("MaximumAttempts") ? _settings["MaximumAttempts"] : _default["MaximumAttempts"]);

                this.numCountRatio.Value = Convert.ToDecimal(_settings.ContainsKey("CountDownRatio") ? _settings["CountDownRatio"]: _default["CountDownRatio"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                this.numNumbersRatio.Value = Convert.ToDecimal(_settings.ContainsKey("NumbersRatio") ? _settings["NumbersRatio"] : _default["NumbersRatio"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                this.numBorderRatio.Value = Convert.ToDecimal(_settings.ContainsKey("BorderRatio") ? _settings["BorderRatio"] : _default["BorderRatio"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                this.numFontRatio.Value = Convert.ToDecimal(_settings.ContainsKey("FontRatio") ? _settings["FontRatio"] : _default["FontRatio"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                this.numResultsRatio.Value = Convert.ToDecimal(_settings.ContainsKey("ResultsRatio") ? _settings["ResultsRatio"] : _default["ResultsRatio"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                this.checkBox1.Checked = Convert.ToInt32(_settings.ContainsKey("WindowPosition") ? _settings["WindowPosition"] : _default["WindowPosition"]) == 1 ? true : false;

            }
            catch (KeyNotFoundException e)
            {
                MessageBox.Show(this, "Unexpected error while applying settings.\nPlease report the error to the engineer.", "Settings error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            //ApplySettings(_settings);
            settings = _settings;
            _defaultSettings = _default;
            //_programSettings.ContainsKey("Sound") ?

        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            settings["Time"] = this.numTime.Value.ToString();
            settings["TimeIncrement"] = this.numTimeIncrement.Value.ToString();
            settings["MaximumDigit"] = this.numMaxDigit.Value.ToString();
            settings["MinimumDigit"] = this.numMinDigit.Value.ToString();
            settings["MaximumAttempts"] = this.numMaxAttempts.Value.ToString();

            settings["CountDownRatio"] = this.numCountRatio.Value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            settings["NumbersRatio"] = this.numNumbersRatio.Value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            settings["BorderRatio"] = this.numBorderRatio.Value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            settings["FontRatio"] = this.numFontRatio.Value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            settings["ResultsRatio"] = this.numResultsRatio.Value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            settings["WindowPosition"] = (this.checkBox1.Checked ? 1 : 0).ToString();

            settings["PlayMode"] = (
                                    (this.radFixed.Checked ? 1 : 0) * 1 +
                                    (this.radIncremental.Checked ? 1 : 0) * 2 +
                                    (this.radProgressive.Checked ? 1 : 0) * 4 +
                                    (this.radRandom.Checked ? 1 : 0) * 8
                                    ).ToString();

            Close(); 
        }

        private void ApplySettings(ProgramSettings<string, string> _settings)
        {
            try
            {
                PlayMode play = (PlayMode)Convert.ToInt32(_settings["PlayMode"]);
                this.radProgressive.Checked = ((play & PlayMode.SequenceProgressive) == PlayMode.SequenceProgressive);
                this.radRandom.Checked = ((play & PlayMode.SequenceRandom) == PlayMode.SequenceRandom);
                this.radFixed.Checked = ((play & PlayMode.TimeFixed) == PlayMode.TimeFixed);
                this.radIncremental.Checked = ((play & PlayMode.TimeIncremental) == PlayMode.TimeIncremental);
                this.numTimeIncrement.Enabled = this.radIncremental.Checked;
                this.trackTimeIncrement.Enabled = this.radIncremental.Checked;

                this.numTime.Value = Convert.ToInt32(_settings["Time"]);
                this.numTimeIncrement.Value = Convert.ToInt32(_settings["TimeIncrement"]);
                this.numMaxDigit.Value = Convert.ToInt32(_settings["MaximumDigit"]);
                this.numMinDigit.Value = Convert.ToInt32(_settings["MinimumDigit"]);
                this.numMaxAttempts.Value = Convert.ToInt32(_settings["MaximumAttempts"]);

                this.numCountRatio.Value = Convert.ToDecimal(_settings["CountDownRatio"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                this.numNumbersRatio.Value = Convert.ToDecimal(_settings["NumbersRatio"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                this.numBorderRatio.Value = Convert.ToDecimal(_settings["BorderRatio"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                this.numFontRatio.Value = Convert.ToDecimal(_settings["FontRatio"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                this.numResultsRatio.Value = Convert.ToDecimal(_settings["ResultsRatio"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                this.checkBox1.Checked = Convert.ToInt32(_settings["WindowPosition"]) == 1 ? true : false;

            }
            catch (KeyNotFoundException e)
            {
                using (new CenterWinDialog(this))
                {
                    MessageBox.Show(this,
                        "Unexpected error while applying settings.\nPlease report the error to the engineer.",
                        "Settings error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Ask for overriding confirmation
            DialogResult result;
            using (new CenterWinDialog(this))
            {
                result = MessageBox.Show(this, "You are about to override the actual settings\n" +
                                                "with the default values.\n\n" +
                                                "Are you sure you want to continue?",
                                                "Override settings",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }

            // If "Yes", then reset values to default
            if (result == DialogResult.Yes)
            {
                ApplySettings(_defaultSettings);    
            }
        }

        private void trackTime_ValueChanged(object sender, EventArgs e)
        {
            if (numTime.Value != trackTime.Value) numTime.Value = trackTime.Value;
        }

        private void numTime_ValueChanged(object sender, EventArgs e)
        {
            if (trackTime.Value != (int)numTime.Value) trackTime.Value = Convert.ToInt32(numTime.Value);
        }

        private void trackTimeIncrement_ValueChanged(object sender, EventArgs e)
        {
            if (numTimeIncrement.Value != trackTimeIncrement.Value) numTimeIncrement.Value = trackTimeIncrement.Value;
        }

        private void numTimeIncrement_ValueChanged(object sender, EventArgs e)
        {
            if (trackTimeIncrement.Value != (int)numTimeIncrement.Value) trackTimeIncrement.Value = Convert.ToInt32(numTimeIncrement.Value);
        }

        private void trackCountRatio_ValueChanged(object sender, EventArgs e)
        {
            decimal ratio = Decimal.Round((decimal)trackCountRatio.Value / 100, 2, MidpointRounding.AwayFromZero);
            if (numCountRatio.Value != ratio) numCountRatio.Value = ratio;
        }

        private void numCountRatio_ValueChanged(object sender, EventArgs e)
        {
            int ratio = Convert.ToInt32(100 * numCountRatio.Value);
            if (trackCountRatio.Value != ratio) trackCountRatio.Value = ratio;
        }

        private void trackNumbersRatio_ValueChanged(object sender, EventArgs e)
        {
            decimal ratio = Decimal.Round((decimal)trackNumbersRatio.Value / 100, 2, MidpointRounding.AwayFromZero);
            if (numNumbersRatio.Value != ratio) numNumbersRatio.Value = ratio;
        }

        private void numNumbersRatio_ValueChanged(object sender, EventArgs e)
        {
            int ratio = Convert.ToInt32(100 * numNumbersRatio.Value);
            if (trackNumbersRatio.Value != ratio) trackNumbersRatio.Value = ratio;
        }

        private void trackBorderRatio_ValueChanged(object sender, EventArgs e)
        {
            decimal ratio = Decimal.Round((decimal)trackBorderRatio.Value / 100, 2, MidpointRounding.AwayFromZero);
            if (numBorderRatio.Value != ratio) numBorderRatio.Value = ratio;
        }

        private void numBorderRatio_ValueChanged(object sender, EventArgs e)
        {
            int ratio = Convert.ToInt32(100 * numBorderRatio.Value);
            if (trackBorderRatio.Value != ratio) trackBorderRatio.Value = ratio;
        }

        private void trackFontRatio_ValueChanged(object sender, EventArgs e)
        {
            decimal ratio = Decimal.Round((decimal)trackFontRatio.Value / 100, 2, MidpointRounding.AwayFromZero);
            if (numFontRatio.Value != ratio) numFontRatio.Value = ratio;
        }

        private void numFontRatio_ValueChanged(object sender, EventArgs e)
        {
            int ratio = Convert.ToInt32(100 * numFontRatio.Value);
            if (trackFontRatio.Value != ratio) trackFontRatio.Value = ratio;
        }

        private void trackResultsRatio_ValueChanged(object sender, EventArgs e)
        {
            decimal ratio = Decimal.Round((decimal)trackResultsRatio.Value / 100, 2, MidpointRounding.AwayFromZero);
            if (numResultsRatio.Value != ratio) numResultsRatio.Value = ratio;
        }

        private void numResultsRatio_ValueChanged(object sender, EventArgs e)
        {
            int ratio = Convert.ToInt32(100 * numResultsRatio.Value);
            if (trackResultsRatio.Value != ratio) trackResultsRatio.Value = ratio;
        }

        private void radIncremental_CheckedChanged(object sender, EventArgs e)
        {
            if (radIncremental.Checked==true)
            {
                numTimeIncrement.Enabled = true;
                trackTimeIncrement.Enabled = true;
            }
            else
            {
                numTimeIncrement.Enabled = false;
                trackTimeIncrement.Enabled = false;
            }
        }


        // https://stackoverrun.com/es/q/1915473
    }
}
