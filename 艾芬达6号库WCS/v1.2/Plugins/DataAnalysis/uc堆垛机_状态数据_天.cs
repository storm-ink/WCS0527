using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs.App.Plugins.DataAnalysis.CraneDeviceDataAnalysis;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace Wcs.App.Plugins.DataAnalysis
{
    public partial class uc堆垛机_状态数据_天 : UserControl, IChart
    {
        Dictionary<string, List<string>> types = new Dictionary<string, List<string>>();

        public uc堆垛机_状态数据_天()
        {
            InitializeComponent();


            types.Add("活动", new List<string>() { "1", "4", "5", "6", "7" });
            types.Add("静止", new List<string>() { "0", "2", "3", "11", "12" });
            types.Add("故障", new List<string>() { "8", "9", "10" });

        }
        public String DeviceName { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public void Refresh(string deviceName, DateTime startTime, DateTime endTime)
        {
            this.DeviceName = deviceName;
            this.StartTime = startTime.Date;
            this.EndTime = endTime.Date;

            dateTimePicker1.Value = this.StartTime.Date;
        }

        string lastPieChartState = null;
        void load()
        {
            CraneDeviceChartDataAdapter adp = new CraneDeviceChartDataAdapter();

            if (double.IsNaN(chart1.ChartAreas[0].AxisX.Minimum))
            {
                chart1.ChartAreas[0].AxisX.Minimum = 1;
            }

            if (double.IsNaN(chart1.ChartAreas[0].AxisX.Maximum))
            {
                double totalDays = new DateTime(this.StartTime.AddMonths(1).Year, this.StartTime.AddMonths(1).Month, 1)
                    .Subtract(new DateTime(this.StartTime.Year, this.StartTime.Month, 1))
                    .TotalDays;

                chart1.ChartAreas[0].AxisX.Maximum = totalDays;
            }

            var datas = adp.FindStateExecutionTimes(this.DeviceName, this.StartTime, this.EndTime);

            createDeviceFilterControls(datas);

            createStateFilterControls(datas);

            loadLineChart(datas, chart1.ChartAreas[0].AxisX.Minimum, chart1.ChartAreas[0].AxisX.Maximum);
            loadPieChat(datas, lastPieChartState);
            loadGridDatas(datas);
            adp = null;
        }

        void loadLineChart(IEnumerable<StateExecutionTime> datas, double fromDay, double toDay)
        {
            double totalDays = new DateTime(this.StartTime.AddMonths(1).Year, this.StartTime.AddMonths(1).Month, 1)
                .Subtract(new DateTime(this.StartTime.Year, this.StartTime.Month, 1))
                .TotalDays;

            //强制刷新，防止数据更新后 y 轴不更新出现数据显示不全
            foreach (var item in chart1.Series)
            {
                item.ChartType = SeriesChartType.Area;
            }

            #region 清理报表数据
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisY.CustomLabels.Clear();
            chart1.ChartAreas[0].AxisX.CustomLabels.Clear();
            chart1.Legends[0].CellColumns.Clear();
            chart1.Legends[0].CustomItems.Clear();
            #endregion

            #region 设置数据
            chart1.ChartAreas[0].AxisX.Minimum = fromDay;
            chart1.ChartAreas[0].AxisX.Maximum = toDay;
            #endregion

            #region 设置样式
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(trackBar1.Value, Color.Black);
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(trackBar1.Value, Color.Black);
            #endregion

            #region 设置图例
            chart1.Legends[0].Enabled = true;
            chart1.Legends[0].LegendStyle = LegendStyle.Table;
            chart1.Legends[0].Docking = Docking.Bottom;
            chart1.Legends[0].Alignment = StringAlignment.Center;
            chart1.Legends[0].BorderDashStyle = ChartDashStyle.Solid;
            chart1.Legends[0].BorderColor = Color.Gray;
            chart1.Legends[0].BackColor = Color.Transparent;
            chart1.Legends[0].BackSecondaryColor = Color.Gainsboro;
            chart1.Legends[0].BackGradientStyle = GradientStyle.LeftRight;
            chart1.Legends[0].InterlacedRows = true;
            if (rbCountType_Num.Checked)
            {
                chart1.Legends[0].Title = "设备状态变化次数合计（单位：次）";
            }
            else
            {
                chart1.Legends[0].Title = "设备状态保持时间合计（单位：秒）";
            }
            chart1.Legends[0].TitleSeparator = LegendSeparatorStyle.GradientLine;
            chart1.Legends[0].TitleSeparatorColor = Color.Gray;
            chart1.Legends[0].HeaderSeparator = LegendSeparatorStyle.Line;
            chart1.Legends[0].HeaderSeparatorColor = Color.Gray;
            chart1.Legends[0].LegendStyle = LegendStyle.Column;
            chart1.Legends[0].Alignment = StringAlignment.Center;
            #endregion

            #region 设置标题
            if (rbCountType_Num.Checked)
            {
                if (toDay - fromDay == 1)
                {
                    chart1.Titles[0].Text = string.Format("{0:yyyy年MM月}{1:00}日堆垛机状态变化次数", this.StartTime, fromDay);
                }
                else
                {
                    chart1.Titles[0].Text = string.Format("{0:yyyy年MM月}{1:00}日 至 {2:yyyy年MM月}{3:00}日堆垛机状态变化次数", this.StartTime, fromDay, this.StartTime, toDay);
                }
            }
            else
            {
                if (toDay - fromDay == 1)
                {
                    chart1.Titles[0].Text = string.Format("{0:yyyy年MM月}{1:00}日堆垛机状态持续时间", this.StartTime, fromDay);
                }
                else
                {
                    chart1.Titles[0].Text = string.Format("{0:yyyy年MM月}{1:00}日 至 {2:yyyy年MM月}{3:00}日堆垛机状态持续时间", this.StartTime, fromDay, this.StartTime, toDay);
                }
            }
            #endregion

            #region 添加自定义图例列
            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "颜色";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            chart1.Legends[0].CellColumns.Add(firstColumn);

            // Add Legend Text column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "设备";
            secondColumn.Text = "#LEGENDTEXT";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            chart1.Legends[0].CellColumns.Add(secondColumn);

            for (double i = fromDay; i <= (toDay - fromDay == 1 ? fromDay : toDay); i += 1)
            {
                LegendCellColumn column = new LegendCellColumn();
                column.ColumnType = LegendCellColumnType.Text;
                column.HeaderText = i.ToString("00");
                column.Text = i.ToString("00");
                column.Tag = i.ToString("00");
                column.HeaderBackColor = Color.WhiteSmoke;
                chart1.Legends[0].CellColumns.Add(column);
            }
            #endregion

            #region 添加 x 轴刻度
            double byHoursCountInterval = 0.0416666666666667;
            if (toDay - fromDay < 2)
            {
                for (double i = fromDay; i < toDay; i=i+ byHoursCountInterval)
                {
                    var hours = (i - fromDay) * 24;

                    var curstomLabel = chart1.ChartAreas[0].AxisX.CustomLabels.Add(i, i + byHoursCountInterval, string.Format("{0:00}:00", hours));
                    curstomLabel.GridTicks = System.Windows.Forms.DataVisualization.Charting.GridTickTypes.Gridline;
                }

                chart1.ChartAreas[0].AxisX.Interval = byHoursCountInterval;
            }
            else
            {
                for (int i = 1; i <= totalDays; i++)
                {
                    var curstomLabel = chart1.ChartAreas[0].AxisX.CustomLabels.Add(i, i + 1, string.Format("{0:00}", i));
                    curstomLabel.GridTicks = System.Windows.Forms.DataVisualization.Charting.GridTickTypes.Gridline;
                }

                chart1.ChartAreas[0].AxisX.Interval = 1;
            }
            #endregion
            
            #region 查询数据
            IEnumerable<StateExecutionTime> times;

            if (toDay - fromDay == 1)
            {
                times = datas.Where(x => x.FromState.StartTime.Day == fromDay);
            }
            else
            {
                times = datas.Where(x => x.FromState.StartTime.Day >= fromDay && x.FromState.StartTime.Day <= toDay);
            }
                        
            if (!cbxShowData_Active.Checked)
            {
                times = times.Where(x => !types[cbxShowData_Active.Text].Any(y => y == x.State));
            }

            if (!cbxShowData_stationary.Checked)
            {
                times = times.Where(x => !types[cbxShowData_stationary.Text].Any(y => y == x.State));
            }

            if (!cbxShowData_Warning.Checked)
            {
                times = times.Where(x => !types[cbxShowData_Warning.Text].Any(y => y == x.State));
            }


            var devicesCheckboxs = groupBox1.Controls.Cast<Control>().Where(x => x is CheckBox && Convert.ToString(x.Tag) == "series")
                .Select(x => x as CheckBox);

            foreach (var item in devicesCheckboxs.Where(x => x.Checked == false))
            {
                times = times.Where(x => x.DeviceName != item.Text);
            }
            #endregion

            #region 添加 series 和顶点数据
            foreach (var itemGrouping in times.GroupBy(x => x.DeviceName).OrderBy(x => x.Key))
            {
                chart1.Series.Add(itemGrouping.Key);
                chart1.Series[itemGrouping.Key].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                //按天统计
                for (double day = fromDay; day <= (toDay - fromDay==1?fromDay:toDay); day++)
                {
                    if (toDay - fromDay == 1)
                    {
                        double stepValue = byHoursCountInterval*24;
                        //每小时显示一个点
                        for (double hours = 0; hours < 24; hours += stepValue)
                        {
                            if (rbCountType_Num.Checked)
                            {
                                
                                var yValue = itemGrouping
                                    .Where(x => x.FromState.StartTime.Day == day
                                        && x.FromState.StartTime.Subtract(x.FromState.StartTime.Date).TotalHours >= hours
                                        && x.FromState.StartTime.Subtract(x.FromState.StartTime.Date).TotalHours<hours+stepValue
                                        )
                                    .Count();
                                var xValue = fromDay + (hours+stepValue/2) / 24d;

                                chart1.Series[itemGrouping.Key].Points.AddXY(xValue, yValue);
                            }
                            else
                            {
                                var yValue = itemGrouping
                                     .Where(x => x.FromState.StartTime.Day == day
                                        && x.FromState.StartTime.Subtract(x.FromState.StartTime.Date).TotalHours >= hours
                                        && x.FromState.StartTime.Subtract(x.FromState.StartTime.Date).TotalHours < hours + stepValue
                                        )
                                      .Sum(x => x.TotalSeconds);
                                var xValue = fromDay + (hours + stepValue/2) / 24d;

                                chart1.Series[itemGrouping.Key].Points.AddXY(xValue, yValue);
                            }
                        }

                    }
                    else
                    {
                        if (rbCountType_Num.Checked)
                        {
                            var yValue = itemGrouping
                                       .Where(x => x.FromState.StartTime.Day == day)
                                       .Count();
                            var xValue = day+1/2d;

                            chart1.Series[itemGrouping.Key].Points.AddXY(xValue, yValue);
                        }
                        else
                        {
                            var yValue = itemGrouping
                                    .Where(x => x.FromState.StartTime.Day == day)
                                    .Sum(x => x.TotalSeconds);
                            var xValue = day + 1 / 2d;

                            chart1.Series[itemGrouping.Key].Points.AddXY(xValue, yValue);
                        }

                    }

                }

            }
            #endregion

            chart1.Tag = times;
        }

        void createDeviceFilterControls(IEnumerable<StateExecutionTime> dates)
        {
            if (!groupBox1.Controls.Cast<Control>().Any(x => Convert.ToString(x.Tag) == "series"))
            {
                Control prevControl = btnNextDay;
                foreach (var item in dates.GroupBy(x => x.DeviceName).OrderBy(x => x.Key))
                {
                    CheckBox cbx = new CheckBox();
                    cbx.AutoSize = true;
                    cbx.Text = item.Key;
                    cbx.Checked = true;
                    cbx.Tag = "series";
                    cbx.Top = cbxShowLegends.Top;
                    cbx.Left = prevControl.Left + prevControl.Width;

                    cbx.CheckedChanged += cbx_CheckedChanged;

                    groupBox1.Controls.Add(cbx);

                    prevControl = cbx;
                }
            }

        }

        void createStateFilterControls(IEnumerable<StateExecutionTime> datas)
        {
            if (!groupBox2.Controls.Cast<Control>().Any(x => Convert.ToString(x.Tag) == "series"))
            {
                Control prevControl = null;
                foreach (var item in datas.GroupBy(x => new
                {
                    x.State,
                    x.StateDisplayName
                }).OrderBy(x => x.Key.State))
                {
                    CheckBox cbx = new CheckBox();
                    cbx.AutoSize = true;
                    cbx.Text = item.Key.StateDisplayName;
                    cbx.Checked = true;
                    cbx.Tag = "series";
                    cbx.Top = 16;
                    if (prevControl == null)
                    {
                        cbx.Left = 10;
                    }
                    else
                    {
                        cbx.Left = prevControl.Left + prevControl.Width;
                    }

                    cbx.CheckedChanged += cbx_CheckedChanged;

                    groupBox2.Controls.Add(cbx);

                    prevControl = cbx;
                }
            }
        }

        void loadPieChat(IEnumerable<StateExecutionTime> datas, string state)
        {
            //强制刷新，防止数据更新后 y 轴不更新出现数据显示不全
            foreach (var item in chart2.Series)
            {
                item.ChartType = SeriesChartType.Area;
            }

            #region 查询数据
            IEnumerable<StateExecutionTime> times;
            if (!string.IsNullOrWhiteSpace(state))
            {
                times = datas.Where(x => types[state].Any(y=>y==x.State));
            }
            else
            {
                times = datas.ToList();
            }


            var devicesCheckboxs = groupBox1.Controls.Cast<Control>().Where(x => x is CheckBox && Convert.ToString(x.Tag) == "series")
                .Select(x => x as CheckBox);

            foreach (var item in devicesCheckboxs)
            {
                if (item.Checked == false)
                {
                    times = times.Where(x => x.DeviceName != item.Text);
                }
            }
            #endregion

            #region 清理报表数据
            chart2.Series.Clear();
            chart2.ChartAreas[0].AxisY.CustomLabels.Clear();
            chart2.ChartAreas[0].AxisX.CustomLabels.Clear();
            chart2.Legends[0].CellColumns.Clear();
            chart2.Legends[0].CustomItems.Clear();
            #endregion

            #region 设置数据
            chart2.ChartAreas[0].AxisY.Minimum = 0;
            chart2.ChartAreas[0].AxisX.Interval = 1;
            #endregion

            #region 设置样式
            chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(trackBar1.Value, Color.Black);
            chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(trackBar1.Value, Color.Black);
            chart2.ChartAreas[0].Area3DStyle.Enable3D = false;
            #endregion

            #region 设置图例
            chart2.Legends[0].Enabled = true;
            chart2.Legends[0].LegendStyle = LegendStyle.Table;
            chart2.Legends[0].Docking = Docking.Bottom;
            chart2.Legends[0].Alignment = StringAlignment.Center;
            chart2.Legends[0].BorderDashStyle = ChartDashStyle.Solid;
            chart2.Legends[0].BorderColor = Color.Gray;
            chart2.Legends[0].BackColor = Color.Transparent;
            chart2.Legends[0].BackSecondaryColor = Color.Gainsboro;
            chart2.Legends[0].BackGradientStyle = GradientStyle.LeftRight;
            chart2.Legends[0].InterlacedRows = true;
            if (rbCountType_Num.Checked)
            {
                chart2.Legends[0].Title = "设备状态变化次数合计（单位：次）";
            }
            else
            {
                chart2.Legends[0].Title = "设备状态保持时间合计（单位：秒）";
            }
            chart2.Legends[0].TitleSeparator = LegendSeparatorStyle.GradientLine;
            chart2.Legends[0].TitleSeparatorColor = Color.Gray;
            chart2.Legends[0].HeaderSeparator = LegendSeparatorStyle.Line;
            chart2.Legends[0].HeaderSeparatorColor = Color.Gray;
            chart2.Legends[0].LegendStyle = LegendStyle.Column;
            chart2.Legends[0].Alignment = StringAlignment.Center;
            #endregion

            #region 设置标题
            if (rbCountType_Num.Checked)
            {
                chart2.Titles[0].Text = "设备状态变化次数合计（单位：次）";
            }
            else
            {
                chart2.Titles[0].Text = "设备状态保持时间合计（单位：秒）";
            }

            if (rbCountType_Num.Checked)
            {
                chart2.Legends[0].Title = "设备状态变化次数合计（单位：次）";
            }
            else
            {
                chart2.Legends[0].Title = "设备状态保持时间合计（单位：秒）";
            }
            #endregion

            #region 添加 x 轴刻度
            int xIndex = 0;
            //添加状态到 x 轴
            foreach (var item in types)
            {
                if (!String.IsNullOrWhiteSpace(state))
                {
                    if (item.Key != state)
                    {
                        continue;
                    }

                    chart2.ChartAreas[0].AxisX.CustomLabels.Add(xIndex , xIndex + 2, item.Key);
                    break;
                }
                else
                {
                    chart2.ChartAreas[0].AxisX.CustomLabels.Add(xIndex - 1, xIndex + 1, item.Key);
                } 
                xIndex++;
            }
            #endregion

            #region 添加自定义图例列
            // Add header separator of type line
            chart2.Legends[0].TitleSeparator = LegendSeparatorStyle.GradientLine;
            chart2.Legends[0].TitleSeparatorColor = Color.Gray;
            chart2.Legends[0].HeaderSeparator = LegendSeparatorStyle.Line;
            chart2.Legends[0].HeaderSeparatorColor = Color.Gray;
            chart2.Legends[0].LegendStyle = LegendStyle.Column;

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "颜色";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            chart2.Legends[0].CellColumns.Add(firstColumn);

            // Add Legend Text column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "设备";
            secondColumn.Text = "#LEGENDTEXT";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            chart2.Legends[0].CellColumns.Add(secondColumn);


            foreach (var item in types)
            {
                if (!String.IsNullOrWhiteSpace(state))
                {
                    if (item.Key != state)
                    {
                        continue;
                    }
                }
                LegendCellColumn column = new LegendCellColumn();
                column.ColumnType = LegendCellColumnType.Text;
                column.HeaderText = item.Key;
                column.Text = item.Key;
                column.Tag = item.Key;
                column.HeaderBackColor = Color.WhiteSmoke;
                chart2.Legends[0].CellColumns.Add(column);
            }
            #endregion

            #region 添加 series 和顶点数据
            foreach (var item in types)
            {
                foreach (var deviceGrouping in times
                    .Where(x=>item.Value.Any(t=>t==x.State))
                    .GroupBy(x => x.DeviceName).OrderBy(x => x.Key)
                    )
                {
                    double v;
                    if (rbCountType_Num.Checked)
                    {
                        v = deviceGrouping.Count();
                    }
                    else
                    {
                        v = deviceGrouping.Sum(x => x.TotalSeconds);
                    }
                    Series series;
                    if (chart2.Series.Any(x => x.Name == deviceGrouping.Key))
                    {
                        series = chart2.Series[deviceGrouping.Key];
                    }
                    else
                    {
                        series = chart2.Series.Add(deviceGrouping.Key);
                    }
                    series.ChartType = SeriesChartType.Column;
                    var label = chart2.ChartAreas[0].AxisX.CustomLabels.Single(x => x.Text == item.Key);
                    var pointIndex = series.Points.AddXY(label.FromPosition + 1, v);
                    var point = series.Points[pointIndex];

                    point.Label = string.Format("{0:0.###}", v);
                }
            }
            #endregion

            chart2.Tag = times;
        }

        void loadGridDatas(IEnumerable<StateExecutionTime> datas)
        {
            var statusCheckBoxs = groupBox2.Controls.Cast<Control>().Where(x => x is CheckBox && Convert.ToString(x.Tag) == "series")
                .Select(x => x as CheckBox);

            IEnumerable<StateExecutionTime> bindingSource = datas.ToList();
            foreach (var item in statusCheckBoxs)
            {
                if (item.Checked == false)
                {
                    bindingSource = bindingSource.Where(x => x.StateDisplayName != item.Text);
                }
            }
            dgvGrid.AutoGenerateColumns = false;
            dgvGrid.DataSource = bindingSource
                .OrderBy(x => x.DeviceName)
                .ThenBy(x => x.From).ToList();
        }

        private void chart1_CustomizeLegend(object sender, CustomizeLegendEventArgs e)
        {
            if (e.LegendItems.Count == 0)
            {
                return;
            }

            var datas = (IEnumerable<StateExecutionTime>)chart1.Tag;
            foreach (var item in e.LegendItems)
            {
                string deviceName = item.Cells[1].Text;
                foreach (var cell in item.Cells.Skip(2))
                {
                    int day = int.Parse(cell.Text);
                    if (rbCountType_Num.Checked)
                    {
                        var v = datas.Where(x => x.DeviceName == deviceName && x.FromState.StartTime.Day == day)
                            .Count();
                        cell.Text = v.ToString("0.##");
                    }
                    else
                    {
                        var v = datas.Where(x => x.DeviceName == deviceName && x.FromState.StartTime.Day == day)
                            .Sum(x => x.TotalSeconds);
                        cell.Text = v.ToString("0.##");
                    }

                }
            }

            LegendItem legendItem = new LegendItem();
            legendItem.Cells.Add(new LegendCell
            {
                CellType = LegendCellType.Text,
                Text = "合计",
                Alignment = ContentAlignment.MiddleCenter,
            });

            legendItem.Cells.Add(new LegendCell
            {
                CellType = LegendCellType.Text,
                Text = e.LegendItems.Count.ToString(),
                Alignment = ContentAlignment.MiddleCenter,
            });

            for (int i = 2; i < e.LegendItems[0].Cells.Count; i++)
            {
                var text = e.LegendItems.Sum(x => Convert.ToDouble(x.Cells[i].Text));
                legendItem.Cells.Add(new LegendCell
                {
                    CellType = LegendCellType.Text,
                    Text = text.ToString("0.##"),
                    Alignment = ContentAlignment.MiddleCenter,
                });
            }


            e.LegendItems.Add(legendItem);

        }

        private void chart2_CustomizeLegend(object sender, CustomizeLegendEventArgs e)
        {
            if (e.LegendItems.Count == 0)
            {
                return;
            }

            var datas =(IEnumerable<StateExecutionTime>)chart2.Tag;
            foreach (var item in e.LegendItems)
            {
                string deviceName = item.Cells[1].Text;
                foreach (var cell in item.Cells.Skip(2))
                {
                    string stateName = cell.Text;
                    double v;
                    if (rbCountType_Num.Checked)
                    {
                        v = datas.Where(x => x.DeviceName == deviceName && types[stateName].Any(y => y == x.State))
                        .Count();
                    }
                    else
                    {
                        v= datas.Where(x => x.DeviceName == deviceName && types[stateName].Any(y => y == x.State))
                        .Sum(x => x.TotalSeconds);
                    }

                    cell.Text = v.ToString("0.##");
                }
            }

            LegendItem legendItem = new LegendItem();
            legendItem.Cells.Add(new LegendCell
            {
                CellType=LegendCellType.Text,
                Text="合计",
                Alignment=ContentAlignment.MiddleCenter,
            });

            legendItem.Cells.Add(new LegendCell
            {
                CellType = LegendCellType.Text,
                Text = e.LegendItems.Count.ToString(),
                Alignment = ContentAlignment.MiddleCenter,
            });

            for (int i = 2; i < e.LegendItems[0].Cells.Count; i++)
            {
                var text = e.LegendItems.Sum(x => Convert.ToDouble(x.Cells[i].Text));
                legendItem.Cells.Add(new LegendCell
                {
                    CellType = LegendCellType.Text,
                    Text = text.ToString("0.##"),
                    Alignment = ContentAlignment.MiddleCenter,
                });
            }


            e.LegendItems.Add(legendItem);

        }

        void cbx_CheckedChanged(object sender, EventArgs e)
        {
            //CraneDeviceChartDataAdapter adp = new CraneDeviceChartDataAdapter();
            //loadGridDatas(adp.FindStateExecutionTimes(this.DeviceName,this.StartTime, this.EndTime));
            //adp = null;
            load();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        private void chart1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            double totalDays = new DateTime(this.StartTime.AddMonths(1).Year, this.StartTime.AddMonths(1).Month, 1)
                    .Subtract(new DateTime(this.StartTime.Year, this.StartTime.Month, 1))
                    .TotalDays;

            if (chart1.ChartAreas[0].AxisX.Maximum - chart1.ChartAreas[0].AxisX.Minimum < totalDays-1)
            {
                CraneDeviceChartDataAdapter adp = new CraneDeviceChartDataAdapter();

                loadLineChart(adp.FindStateExecutionTimes(this.DeviceName,this.StartTime,this.EndTime), 1, totalDays);
                adp = null;
                return;
            }

            if (!cbxScaleViewEnabled.Checked)
            {
                return;
            }


            {
                var day = chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                var fromDay = Math.Floor(day);
                var toDay = fromDay + 1;
                CraneDeviceChartDataAdapter adp = new CraneDeviceChartDataAdapter();
                loadLineChart(adp.FindStateExecutionTimes(this.DeviceName, this.StartTime, this.EndTime), fromDay, toDay);
                adp = null;
            }
        }

        private void cbxScaleViewEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxScaleViewEnabled.Checked)
            {
                lastPieChartState = null;
                chart1.ChartAreas[0].AxisX.Minimum = 0;
                chart1.ChartAreas[0].AxisX.Maximum = 24;
                load();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(trackBar1.Value, Color.Black);
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(trackBar1.Value, Color.Black);

            chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(trackBar1.Value, Color.Black);
            chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(trackBar1.Value, Color.Black);
        }

        private void cbxShowLegends_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Legends[0].Enabled = cbxShowLegends.Checked;
        }

        private void btnPrevDay_Click(object sender, EventArgs e)
        {

            dateTimePicker1.Value = this.StartTime.Date.AddMonths(-1);

        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = this.StartTime.Date.AddMonths(1);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            this.StartTime = new DateTime(dateTimePicker1.Value.Date.Year,dateTimePicker1.Value.Date.Month,1);
            this.EndTime = StartTime.AddMonths(1);

            load();
        }

        private void chart2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lastPieChartState))
            {
                lastPieChartState = null;

                CraneDeviceChartDataAdapter adp = new CraneDeviceChartDataAdapter();
                loadPieChat(adp.FindStateExecutionTimes(this.DeviceName,this.StartTime,this.EndTime), lastPieChartState);
                adp = null;
                return;

            }

            if (!cbxScaleViewEnabled.Checked)
            {
                return;
            }

            var v = chart2.ChartAreas[0].AxisX.PixelPositionToValue(e.X);

            v = Math.Round(v);

            var xLabel = chart2.ChartAreas[0].AxisX.CustomLabels.FirstOrDefault(x => x.FromPosition == v-1 && x.ToPosition == v+1);
            if (xLabel == null)
            {
                xLabel = chart2.ChartAreas[0].AxisX.CustomLabels.OrderBy(x => Math.Abs(x.FromPosition - v)).FirstOrDefault();
            }

            if (xLabel == null)
            {
                return;
            }
            {
                lastPieChartState = xLabel.Text;
                CraneDeviceChartDataAdapter adp = new CraneDeviceChartDataAdapter();
                loadPieChat(adp.FindStateExecutionTimes(this.DeviceName, this.StartTime, this.EndTime), lastPieChartState);
                adp = null;
            }
        }

        private void cbxShowData_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Controls.Clear();
            load();
        }

        private void rbCountType_CheckedChanged(object sender, EventArgs e)
        {
            load();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName;
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    saveFileDialog.AddExtension = true;
                    saveFileDialog.CheckFileExists = false;
                    saveFileDialog.CheckPathExists = true;
                    saveFileDialog.DefaultExt = "xml";
                    saveFileDialog.FileName = string.Format("堆垛机状态数据");
                    saveFileDialog.Filter = "XML 电子表格 2003|*.xml";
                    saveFileDialog.ValidateNames = true;
                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                    {
                        return;
                    }

                    fileName = saveFileDialog.FileName;
                }

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return;
                }

                dgvGrid.ExportAsExcel(fileName);

                if (MessageBox.Show("导出成功，是否立即打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
