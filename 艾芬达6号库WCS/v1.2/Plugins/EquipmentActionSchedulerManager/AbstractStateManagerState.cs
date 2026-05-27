using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.EquipmentActionSchedulerManager
{
    public partial class AbstractStateManagerState : Form
    {
        Wcs.Framework.AbstractStateManager _context;
        public AbstractStateManagerState(Wcs.Framework.AbstractStateManager context)
        {
            InitializeComponent();
            _context = context;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_context != null && !_context._IsDisposing)
                {
                    label2.Text = _context.CurrentState.Name;
                    label5.Text = _context.ContextIsCompelted.CreateAt.ToString("yyyy-mm-dd HH:MM:ss.ffff") + " " + _context.ContextIsCompelted.Result.ToString() + " " + _context.ContextIsCompelted.Information;
                    label6.Text = _context.ContextCanPerform.CreateAt.ToString("yyyy-mm-dd HH:MM:ss.ffff") + " " + _context.ContextCanPerform.Result.ToString() + " " + _context.ContextCanPerform.Information;
                }
                else
                    this.Close();
            }
            catch
            {
            }
        }

        private void AbstractStateManagerState_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
