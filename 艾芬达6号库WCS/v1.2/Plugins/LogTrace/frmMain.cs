using DevExpress.XtraEditors;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.LogTrace
{
    public partial class frmLogTrace_TraceWindow : Form
    {
        const string TargetName = "Wcs.App.Plugins.LogTrace_frmMain_Target";
        LoggingRule _loggingRule;
        public frmLogTrace_TraceWindow()
        {
            InitializeComponent();
        }

        void ReloadTrace()
        {
            var target = NLog.LogManager.Configuration.AllTargets.SingleOrDefault(x => x.Name == TargetName) as RichTextBoxTarget;
            if (target != null)
            {
                NLog.LogManager.Configuration.RemoveTarget(TargetName);
            }

            if (_loggingRule != null)
            {
                NLog.LogManager.Configuration.LoggingRules.Remove(_loggingRule);
                _loggingRule = null;
            }
            if (!cbxStart.Checked)
            {
                LogManager.Configuration.Reload();
            }
            else
            {
                target = new RichTextBoxTarget();
                target.Name = TargetName;
                target.Layout = tbxOutputFormat.Text;
                target.ControlName = "richTextBox1";
                target.FormName = this.Name;
                if (cbxAutoScroll.Checked)
                {
                    target.AutoScroll = true;
                }
                else
                {
                    target.AutoScroll = false;
                }
                target.MaxLines = Convert.ToInt32(nudMaxLines.Value);
                target.UseDefaultRowColoringRules = true;
                NLog.LogManager.Configuration.AddTarget(TargetName, target);

                String loggerNamePattern = "*";
                LogLevel level = LogLevel.Trace;
                if (rbTrace.Checked)
                {
                    level = LogLevel.Trace;
                }
                else if (rbDebug.Checked)
                {
                    level = LogLevel.Debug;
                }
                else if (rbInfo.Checked)
                {
                    level = LogLevel.Info;
                }
                else if (rbWarn.Checked)
                {
                    level = LogLevel.Warn;
                }
                else if (rbError.Checked)
                {
                    level = LogLevel.Error;
                }
                else
                {
                    level = LogLevel.Fatal;
                }

                if (!string.IsNullOrWhiteSpace(tbxLogNameMatch.Text))
                {
                    loggerNamePattern = tbxLogNameMatch.Text;
                }
                _loggingRule = new LoggingRule(loggerNamePattern, level, target);
                NLog.LogManager.Configuration.LoggingRules.Add(_loggingRule);

                LogManager.Configuration.Reload();
            }
        }

        private void cbxStart_CheckedChanged(object sender, EventArgs e)
        {
            ReloadTrace();

            foreach (Control item in groupBox1.Controls)
            {
                if (item != cbxStart)
                {
                    item.Enabled = !cbxStart.Checked;
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var logFactoryFieldInfo = typeof(NLog.LogManager).GetField("globalFactory", System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (logFactoryFieldInfo != null)
            {
                var logFactory = (NLog.LogFactory)logFactoryFieldInfo.GetValue(null);
                var loggerCacheFieldInfo = logFactory.GetType().GetField("loggerCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (loggerCacheFieldInfo != null)
                {
                    try
                    {
                        var xx = (dynamic)loggerCacheFieldInfo.GetValue(logFactory);
                        foreach (var item in xx)
                        {
                            var itemObj = item.GetType()
                                .GetProperty("Key", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                                .GetValue(item, null);
                            Type type = itemObj.GetType();
                            var namePropertyInfo = type.GetProperty("Name", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.NonPublic);
                            if (namePropertyInfo != null)
                            {
                                var name = (String)namePropertyInfo.GetValue(itemObj, null);
                                tbxLogNameMatch.Items.Add(name);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            cbxStart_CheckedChanged(null, null);
        }

        private void frmLogTrace_TraceWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            cbxStart.Checked = false;
            cbxStart_CheckedChanged(null, null);
        }

        private void frmLogTrace_TraceWindow_SizeChanged(object sender, EventArgs e)
        {
            richTextBox1.Width = this.Width - richTextBox1.Left;
            richTextBox1.Height = this.Height - richTextBox1.Top - 8;
            groupBox1.Width = this.Width - groupBox1.Left;
            tbxOutputFormat.Width = groupBox1.Width - tbxOutputFormat.Left - 8;
        }

        //private void richTextBox1_TextChanged(object sender, EventArgs e)
        //{
        //    //让文本框获取焦点 
        //    this.richTextBox1.Focus();
        //    //设置光标的位置到文本尾 
        //    this.richTextBox1.Select(this.richTextBox1.TextLength, 0);
        //    //滚动到控件光标处 
        //    this.richTextBox1.ScrollToCaret();
        //}
    }
}
