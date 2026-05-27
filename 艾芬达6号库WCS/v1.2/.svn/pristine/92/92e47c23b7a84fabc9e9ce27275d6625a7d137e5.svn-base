using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NLog.Config;
using NLog.Targets;
using NLog;

namespace MatedataServer.App
{
    public partial class ucLogTrace : UserControl
    {
        const string TargetName = "MatedataServer.App.LogTrace_ucLogTrace_Target";

        LoggingRule _loggingRule;

        public ucLogTrace()
        {
            InitializeComponent();
        }


        private void cbxStart_CheckedChanged(object sender, EventArgs e)
        {
            ReloadTrace();

            foreach (Control item in groupBox2.Controls)
            {
                if (item != cbxStart)
                {
                    item.Enabled = !cbxStart.Checked;
                }
            }
        }

        void ParentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            cbxStart.Checked = false;
            cbxStart_CheckedChanged(null, null);
        }
        private void ParentForm_Load(object sender, EventArgs e)
        {
            init();
        }

        void ReloadTrace()
        {
            if (NLog.LogManager.Configuration == null)
            {
                NLog.LogManager.Configuration = new LoggingConfiguration();
                //MessageBox.Show("log 未配置，无法启动日志跟踪", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //return;
            }

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
               target.ControlName = this.richTextBox1.Name; ;
               target.FormName = this.ParentForm.Name;
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

        private void ucLogTrace_Load(object sender, EventArgs e)
        {
            this.ParentForm.FormClosed += ParentForm_FormClosed;
            this.ParentForm.Load += ParentForm_Load;
        }

        void init()
        {
            var logFactoryFieldInfo = typeof(NLog.LogManager).GetField("globalFactory", System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (logFactoryFieldInfo != null)
            {
                var logFactory = (NLog.LogFactory)logFactoryFieldInfo.GetValue(null);
                var loggerCacheFieldInfo = logFactory.GetType().GetField("loggerCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (loggerCacheFieldInfo != null)
                {
                    var xx = (System.Collections.IEnumerable)loggerCacheFieldInfo.GetValue(logFactory);
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
            }
            cbxStart_CheckedChanged(null, null);
        }
    }
}
