using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.MessageBoard
{
    [WcsPluginInfo(typeof(MessageBoardPlugin), "消息推送", "Sineva", "2022年6月", "业务消息推送", false, "日志管理", "实时日志", 5, 0, 0)]
    public class MessageBoardPlugin:Wcs.WcsPlugin
    {
        Wcs.WcsContext _context;
        Panel _p;
        public override int Priority
        {
            get
            {
                return 50;
            }
        }
        public override bool Initialization(Wcs.WcsContext context)
        {
            _context = context;

            Panel p = new Panel();
            p.Padding = new Padding(2);
            p.Height = 150;
            p.BorderStyle = BorderStyle.None;
            p.Dock = DockStyle.Bottom;

            ucMessageBoard umb = new ucMessageBoard();
            umb.Dock = DockStyle.Fill;
            p.Controls.Add(umb);

            _p = p;

            p.MouseDown += p_MouseDown;
            p.MouseMove += p_MouseMove;
            p.MouseUp += p_MouseUp;

            //_p.Visible = false;

            _context.Application.MainForm.ControlAdd(_p);
            _p.Visible = false;

            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "业务消息";
            barButtonItem.Id = 0;
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += mi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

        void mi_Click(object sender, EventArgs e)
        {
            _p.Visible = !_p.Visible;
        }

        void p_MouseUp(object sender, MouseEventArgs e)
        {
            //_context.Application.MainForm.Cursor = Cursors.Default;
            _oPointClicked = default(Point);
        }

        void p_MouseMove(object sender, MouseEventArgs e)
        {
            var currentPoint = new Point(_p.Left + e.X, _p.Top + e.Y);
            if (_oPointClicked != default(Point) && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _p.Top = _p.Top + (currentPoint.Y - _oPointClicked.Y);

                _p.Height -= (currentPoint.Y - _oPointClicked.Y);

                _oPointClicked = new Point(currentPoint.X, currentPoint.Y);
            }
        }

        Point _oPointClicked;
        void p_MouseDown(object sender, MouseEventArgs e)
        {
            var currentPoint = new Point(_p.Left + e.X, _p.Top + e.Y);
            if (currentPoint.X < _p.Left || currentPoint.X > _p.Left + _p.Width)
            {
                //_context.Application.MainForm.Cursor = Cursors.Default;
                return;
            }
            if (Math.Abs(currentPoint.Y - _p.Top) > 5)
            {
                //_context.Application.MainForm.Cursor = Cursors.Default;
                return;
            }

            //_context.Application.MainForm.Cursor = Cursors.SizeNS;
            _oPointClicked = new Point(currentPoint.X, currentPoint.Y);
        }
    }
}
