using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate.Linq;
using Wcs;
using Wcs.Framework;

namespace Wcs.App.Plugins.Tools
{
    public partial class 任务类型_任务方向设置 : Form
    {
        const string _out = "出库任务类型";
        const string _in = "入库任务类型";
        const string _scal = "盘点任务类型";
        const string _unknown = "Unknown";

        public 任务类型_任务方向设置()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取所有任务类型
        /// </summary>
        /// <returns></returns>
        public String[] LoadTaskType()
        {
            List<String> _taskTypes;
            using (NHBackupServerUnitOfWork unitOfWork = new NHBackupServerUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                _taskTypes = unitOfWork.session.Query<Task>()
                    .GroupBy(x => x.TaskType)
                    .Select(x => x.Key)
                    .ToList();

                unitOfWork.Commit();
            }
            return _taskTypes.Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
        }

        CheckBox[] _checkBoxArr;
        private void 任务类型_任务方向设置_Load(object sender, EventArgs e)
        {
            cbxTaskTypes.Items.Add("");
            cbxTaskTypes.Items.Add(_out);
            cbxTaskTypes.Items.Add(_in);
            cbxTaskTypes.Items.Add(_scal);

            var _taskTypes = LoadTaskType();
            _checkBoxArr = new CheckBox[_taskTypes.Length];
            if (_checkBoxArr.Any(x => x == null))
            {
                for (int i = 0; i < _taskTypes.Length; i++)
                {
                    CheckBox _cbx = new CheckBox();
                    _cbx.Name = _taskTypes[i];
                    _cbx.Text = _taskTypes[i];
                    _cbx.ForeColor = Color.Green;
                    _cbx.Enabled = false;
                    _cbx.Checked = false;
                    _cbx.CheckedChanged += _cbx_CheckedChanged;
                    _cbx.Tag = _unknown;
                    _cbx.Location = new Point(3, 1 + i * 20);
                    _checkBoxArr[i] = _cbx;
                    splitContainer1.Panel1.Controls.Add(_cbx);
                }
            }

            cbxTaskTypes.TextChanged += cbxTaskTypes_TextChanged;
        }

        void _cbx_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chbx = (CheckBox)sender;
            if (_chbx.Checked == false)
                return;

            switch (cbxTaskTypes.Text)
            {
                case _out:
                    var _outTaskTypeStr = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<String>(_out, "");
                    if (_chbx.Tag.ToString() == _unknown)
                    {
                        if (String.IsNullOrWhiteSpace(_outTaskTypeStr))
                            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<string>(_out, _chbx.Text);
                        else
                            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<String>(_out, _outTaskTypeStr + "," + _chbx.Text);
                    }
                    else
                    {
                        var _outTaskTypes = _outTaskTypeStr.Split(',').ToList().Where(x => x != _chbx.Text);
                        Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<string>(_out, String.Join(",", _outTaskTypes));
                    }
                    break;
                case _in:
                    var _inTaskTypeStr = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<String>(_in, "");
                    if (_chbx.Tag.ToString() == _unknown)
                    {
                        if (String.IsNullOrWhiteSpace(_inTaskTypeStr))
                            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<string>(_in, _chbx.Text);
                        else
                            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<String>(_in, _inTaskTypeStr + "," + _chbx.Text);
                    }
                    else
                    {
                        var _inTaskTypes = _inTaskTypeStr.Split(',').ToList().Where(x => x != _chbx.Text);
                        Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<string>(_in, String.Join(",", _inTaskTypes));
                    }
                    break;
                case _scal:
                    var _scalTaskTypeStr = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<String>(_scal, "");
                    if (_chbx.Tag.ToString() == _unknown)
                    {
                        if (String.IsNullOrWhiteSpace(_scalTaskTypeStr))
                            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<string>(_scal, _chbx.Text);
                        else
                            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<String>(_scal, _scalTaskTypeStr + "," + _chbx.Text);
                    }
                    else
                    {
                        var _outTaskTypes = _scalTaskTypeStr.Split(',').ToList().Where(x => x != _chbx.Text);
                        Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<string>(_scal, String.Join(",", _outTaskTypes));
                    }
                    break;
                default:
                    break;
            }
            cbxTaskTypes_TextChanged(null, null);
        }

        private void cbxTaskTypes_TextChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel1.Controls.Clear();
            splitContainer1.Panel2.Controls.Clear();

            int j = 0;
            var _outTaskTypes = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<String>(_out, "").Split(',').Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
            var _inTaskTypes = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<String>(_in, "").Split(',').Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
            switch (cbxTaskTypes.Text)
            {      
                case _out:
                    for (int i = 0; i < _outTaskTypes.Length; i++)
                    {
                        var _chx = _checkBoxArr.FirstOrDefault(x => x.Name == _outTaskTypes[i]);
                        if (_chx == null)
                        {
                            _chx = new CheckBox();
                            _chx.Name = _outTaskTypes[i];
                            _chx.Text = _outTaskTypes[i];
                            _chx.ForeColor = Color.Red;
                            _chx.CheckedChanged += _cbx_CheckedChanged;
                        }
                        _chx.Tag = _out;
                        _chx.Enabled = true;
                        _chx.Checked = false;
                        _chx.Location = new Point(3, 1 + i * 20);
                        splitContainer1.Panel2.Controls.Add(_chx);
                    }
                    foreach (var item in _checkBoxArr)
                    {
                        if (_inTaskTypes.Contains(item.Name) || _outTaskTypes.Contains(item.Name))
                            continue;

                        item.Tag = _unknown;
                        item.Enabled = true;
                        item.Checked = false;
                        item.Location = new Point(3, 1 + j * 20);
                        splitContainer1.Panel1.Controls.Add(item);
                        ++j;
                    }
                    break;
                case _in:
                    for (int i = 0; i < _inTaskTypes.Length; i++)
                    {
                        var _chx = _checkBoxArr.FirstOrDefault(x => x.Name == _inTaskTypes[i]);
                        if (_chx == null)
                        {
                            CheckBox _cbx = new CheckBox();
                            _cbx.Name = _inTaskTypes[i];
                            _cbx.Text = _inTaskTypes[i];
                            _chx.ForeColor = Color.Red;
                            _chx.CheckedChanged += _cbx_CheckedChanged;
                        }
                        _chx.Tag = _in;
                        _chx.Enabled = true;
                        _chx.Checked = false;
                        _chx.Location = new Point(3, 1 + i * 20);
                        splitContainer1.Panel2.Controls.Add(_chx);
                    }
                    foreach (var item in _checkBoxArr)
                    {
                        if (_inTaskTypes.Contains(item.Name) || _outTaskTypes.Contains(item.Name))
                            continue;

                        item.Tag = _unknown;
                        item.Enabled = true;
                        item.Checked = false;
                        item.Location = new Point(3, 1 + j * 20);
                        splitContainer1.Panel1.Controls.Add(item);
                        ++j;
                    }
                    break;
                case _scal:
                    var _scalTaskType = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<String>(_scal, "").Split(',').Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
                    for (int i = 0; i < _scalTaskType.Length; i++)
                    {
                        var _chx = _checkBoxArr.FirstOrDefault(x => x.Name == _scalTaskType[i]);
                        if (_chx == null)
                        {
                            CheckBox _cbx = new CheckBox();
                            _cbx.Name = _scalTaskType[i];
                            _cbx.Text = _scalTaskType[i];
                            _chx.ForeColor = Color.Red;
                            _chx.CheckedChanged += _cbx_CheckedChanged;
                        }
                        _chx.Tag = _scal;
                        _chx.Enabled = true;
                        _chx.Checked = false;
                        _chx.Location = new Point(3, 1 + i * 20);
                        splitContainer1.Panel2.Controls.Add(_chx);
                    }
                    foreach (var item in _checkBoxArr)
                    {
                        if (_scalTaskType.Contains(item.Name))
                            continue;

                        item.Tag = _unknown;
                        item.Enabled = true;
                        item.Checked = false;
                        item.Location = new Point(3, 1 + j * 20);
                        splitContainer1.Panel1.Controls.Add(item);
                        ++j;
                    }
                    break;
                default:
                    foreach (var item in _checkBoxArr)
                    {
                        item.Tag = _unknown;
                        item.Enabled = false; ;
                        item.Checked = false;
                        item.Location = new Point(3, 1 + j * 20);
                        splitContainer1.Panel1.Controls.Add(item);
                        ++j;
                    }
                    break;
            }
        }
    }
}
