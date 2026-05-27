using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using Wcs.Framework;
using Wcs.Framework.EventBus;
using Wcs.Framework.Events;

namespace WarningRecord
{
    [WcsPluginInfo(typeof(WarningRecordPlugin), "报警记录查询", "Sineva", "2023年4月", "", false, "任务管理", "任务管理", 1, 0, 4)]
    public class WarningRecordPlugin : Wcs.WcsPlugin
    {
        static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "报警记录";
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

       
        void tsmi_Click(object sender, EventArgs e)
        {
            try
            {
                this.Context.Application.MainForm.OpenNewForm<frmMain>("报警记录");
            }
            catch (System.Security.SecurityException securityException)
            {
                _logger.Error1(securityException, this);
                XtraMessageBox.Show(securityException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is System.Security.SecurityException)
                {
                    XtraMessageBox.Show(ex.InnerException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                _logger.Error1(ex, this);
            }
        }
    }
}
