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
    public partial class uc堆垛机_故障数据_24小时 : UserControl,IChart
    {
        public uc堆垛机_故障数据_24小时()
        {
            InitializeComponent();
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

            load();
        }

        string lastPieChartState = null;
        void load()
        {
            CraneDeviceChartDataAdapter adp = new CraneDeviceChartDataAdapter();
            if (double.IsNaN(chart1.ChartAreas[0].AxisX.Minimum))
            {
                chart1.ChartAreas[0].AxisX.Minimum = 0;
            }

            if (double.IsNaN(chart1.ChartAreas[0].AxisX.Maximum))
            {
                chart1.ChartAreas[0].AxisX.Maximum = 24;
            }

            var datas = adp.FindErrorExecutionTimes(this.DeviceName, this.StartTime, this.EndTime);

            createDeviceFilterControls(datas);
            createStateFilterControls(datas);

            loadLineChart(datas, chart1.ChartAreas[0].AxisX.Minimum, chart1.ChartAreas[0].AxisX.Maximum);
            loadPieChat(datas, lastPieChartState);
            loadGridDatas(datas);

            adp = null;
        }

        void loadLineChart(IEnumerable<ErrorExecutionTime> datas, double fromHours, double toHours)
        {
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
            chart1.ChartAreas[0].AxisX.Minimum = fromHours;
            chart1.ChartAreas[0].AxisX.Maximum = toHours;
            //chart1.ChartAreas[0].AxisY.Interval = 1;
            #endregion
            
            #region 设置样式
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(trackBar1.Value, Color.Black);
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(trackBar1.Value, Color.Black);
            #endregion

            #region 设置标题
            if (toHours - fromHours == 1)
            {
                if (rbCountType_TotalSecnods.Checked)
                {
                    chart1.Titles[0].Text = string.Format("{0:yyyy年MM月dd日} {1} 时至 {2} 时 设备故障持续时长合计（秒）", this.StartTime, fromHours, toHours==24?0:toHours);
                    chart1.Legends[0].Title = "设备故障持续时长合计（单位：秒）";
                }
                else
                {
                    chart1.Titles[0].Text = string.Format("{0:yyyy年MM月dd日} {1} 时至 {2} 时 设备次数合计（次）", this.StartTime, fromHours, toHours == 24 ? 0 : toHours);
                    chart1.Legends[0].Title = "设备故障次数合计（单位：次）";
                }
            }
            else
            {
                if (rbCountType_TotalSecnods.Checked)
                {
                    chart1.Titles[0].Text = string.Format("{0:yyyy年MM月dd日} 设备故障持续时长合计（秒）", this.StartTime);
                    chart1.Legends[0].Title = "设备故障持续时长合计（单位：秒）";
                }
                else
                {
                    chart1.Titles[0].Text = string.Format("{0:yyyy年MM月dd日} 设备次数合计（次）", this.StartTime);
                    chart1.Legends[0].Title = "设备故障次数合计（单位：次）";
                }
            }
            #endregion

            #region 添加 x 轴刻度
            if (toHours - fromHours < 2)
            {
                var interval = 0.0416666666666667;
                for (double i = fromHours; i < toHours; i += interval)
                {
                    var minutes = (i - Math.Floor(i)) * 60;
                    var hours = Math.Floor(i);

                    if (minutes >= 60 - 0.001)
                    {
                        minutes = 0;
                        hours += 1;
                    }
                    var curstomLabel = chart1.ChartAreas[0].AxisX.CustomLabels.Add(i - interval, i + interval, string.Format("{0:00}:{1:00}", hours, minutes));
                    curstomLabel.GridTicks = System.Windows.Forms.DataVisualization.Charting.GridTickTypes.Gridline;
                }

                chart1.ChartAreas[0].AxisX.Interval = interval;
            }
            else
            {
                for (int i = 0; i <= 24; i++)
                {
                    var curstomLabel = chart1.ChartAreas[0].AxisX.CustomLabels.Add(i - 1, i + 1, string.Format("{0:00}:00", i));
                    curstomLabel.GridTicks = System.Windows.Forms.DataVisualization.Charting.GridTickTypes.Gridline;
                }

                chart1.ChartAreas[0].AxisX.Interval = 1;
            }
            #endregion

            #region 查询数据
            IEnumerable<ErrorExecutionTime> times;

            if (toHours - fromHours == 1)
            {
                times = datas.Where(x => x.FromState.StartTime.Hour == fromHours);
            }
            else
            {
                times = datas.Where(x => x.FromState.StartTime.Hour >= fromHours && x.FromState.StartTime.Hour <= toHours);
            }


            var devicesCheckboxs = groupBox1.Controls.Cast<Control>().Where(x => x is CheckBox && Convert.ToString(x.Tag) == "series")
                .Select(x => x as CheckBox);

            foreach (var item in devicesCheckboxs.Where(x => x.Checked == false))
            {
                times = times.Where(x => x.DeviceName != item.Text);
            }
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
            #endregion

            #region 添加自定义图例列

            chart1.Legends[0].TitleSeparator = LegendSeparatorStyle.GradientLine;
            chart1.Legends[0].TitleSeparatorColor = Color.Gray;
            chart1.Legends[0].HeaderSeparator = LegendSeparatorStyle.Line;
            chart1.Legends[0].HeaderSeparatorColor = Color.Gray;
            chart1.Legends[0].LegendStyle = LegendStyle.Column;

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "颜色";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            chart1.Legends[0].CellColumns.Add(firstColumn);

            // Add Legend Text column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "故障";
            secondColumn.Text = "#LEGENDTEXT";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            chart1.Legends[0].CellColumns.Add(secondColumn);

            foreach (var item in times.GroupBy(x => x.DeviceName).OrderBy(x => x.Key))
            {
                LegendCellColumn column = new LegendCellColumn();
                column.ColumnType = LegendCellColumnType.Text;
                column.HeaderText = item.Key;
                column.Text = item.Key;
                column.Tag = item.Key;
                column.HeaderBackColor = Color.WhiteSmoke;
                chart1.Legends[0].CellColumns.Add(column);
            }
            #endregion

            #region 添加 series 和顶点数据
            foreach (var itemGrouping in times.GroupBy(x => x.DisplayName).OrderBy(x => x.Key))
            {
                chart1.Series.Add(itemGrouping.Key);
                chart1.Series[itemGrouping.Key].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart1.Series[itemGrouping.Key].MarkerSize = 15;

                foreach (var item in itemGrouping)
                {
                    if (rbCountType_TotalSecnods.Checked)
                    {
                        //开始状态
                        var hours = item.FromState.StartTime.Subtract(this.StartTime.Date).TotalHours;
                        var v = item.TotalSeconds;
                        var pointIndex = chart1.Series[itemGrouping.Key].Points.AddXY(hours, v);
                        //chart1.Series[itemGrouping.Key].Points[pointIndex].Label = v.ToString("0.##");

                        //结束点
                        hours = item.ToState.StartTime.Subtract(this.StartTime.Date).TotalHours;
                        v = 0;
                        chart1.Series[itemGrouping.Key].Points.AddXY(hours, v);
                    }
                    else
                    {
                        //开始状态
                        var hours = item.FromState.StartTime.Subtract(this.StartTime.Date).TotalHours;
                        var v = 1;
                        var pointIndex = chart1.Series[itemGrouping.Key].Points.AddXY(hours, v);
                        //chart1.Series[itemGrouping.Key].Points[pointIndex].Label = v.ToString("0.##");

                        //结束点
                        hours = item.ToState.StartTime.Subtract(this.StartTime.Date).TotalHours;
                        v = 0;
                        chart1.Series[itemGrouping.Key].Points.AddXY(hours, v);
                    }

                }
            }        
            #endregion

            chart1.Tag = times;
        }

        void loadPieChat(IEnumerable<ErrorExecutionTime> datas, string state)
        {
            #region 清理报表数据
            chart2.Series.Clear();
            chart2.ChartAreas[0].AxisY.CustomLabels.Clear();
            chart2.ChartAreas[0].AxisX.CustomLabels.Clear();
            chart2.Legends[0].CellColumns.Clear();
            chart2.Legends[0].CustomItems.Clear();
            #endregion

            #region 设置数据
            chart2.ChartAreas[0].AxisX.Interval = 1;
            #endregion

            #region 查询数据
            IEnumerable<ErrorExecutionTime> times = datas.ToList();

            if (!string.IsNullOrWhiteSpace(state))
            {
                times = times.Where(x => x.DisplayName == state);
            }

            var devicesCheckboxs = groupBox1.Controls.Cast<Control>().Where(x => x is CheckBox && Convert.ToString(x.Tag) == "series")
                .Select(x => x as CheckBox);

            foreach (var item in devicesCheckboxs.Where(x => x.Checked == false))
            {
                times = times.Where(x => x.DeviceName != item.Text);
            }
            #endregion
            
            #region 设置样式
            chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(trackBar1.Value, Color.Black);
            chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(trackBar1.Value, Color.Black);
            //设置图表类型为 线性图
            chart2.ChartAreas[0].Area3DStyle.Enable3D = false;
            #endregion

            #region 设置标题
            if (rbCountType_TotalSecnods.Checked)
            {
                chart2.Titles[0].Text = string.Format("{0:yyyy年MM月dd日} 设备故障持续时长合计（秒）",this.StartTime);
                chart2.Legends[0].Title = "设备故障持续时长合计（单位：秒）";
            }
            else
            {
                chart2.Titles[0].Text = string.Format("{0:yyyy年MM月dd日} 设备故障次数合计（次）", this.StartTime);

                chart2.Legends[0].Title = "设备故障次数合计（单位：次）";
            }
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
            #endregion

            #region 添加自定义图例列

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
            secondColumn.HeaderText = "故障";
            secondColumn.Text = "#LEGENDTEXT";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            chart2.Legends[0].CellColumns.Add(secondColumn);

            foreach (var item in times.GroupBy(x=>x.DeviceName).OrderBy(x=>x.Key))
            {
                LegendCellColumn column = new LegendCellColumn();
                column.ColumnType = LegendCellColumnType.Text;
                column.HeaderText = item.Key;
                column.Text = item.Key;
                column.Tag = item.Key;
                column.HeaderBackColor = Color.WhiteSmoke;
                chart2.Legends[0].CellColumns.Add(column);
            }
            #endregion

            #region 添加 x 轴刻度
            int xIndex = 0;
            //添加状态到 x 轴
            foreach (var item in times.GroupBy(x => x.DisplayName).OrderBy(x => x.Key))
            {
                if (!String.IsNullOrWhiteSpace(state))
                {
                    chart2.ChartAreas[0].AxisX.CustomLabels.Add(xIndex, xIndex + 2, item.Key);
                }
                else
                {
                    chart2.ChartAreas[0].AxisX.CustomLabels.Add(xIndex - 1, xIndex + 1, item.Key);
                }
                xIndex++;
            }
            #endregion

            #region 添加 series 和顶点数据
            //foreach (var deviceGrouping in times.GroupBy(x => x.DeviceName).OrderBy(x=>x.Key))
            //{
            //    var series = chart2.Series.Add(deviceGrouping.Key);
            //    series.ChartType = SeriesChartType.Column;
            //    foreach (var item in deviceGrouping.GroupBy(x => x.DisplayName).OrderBy(x => x.Key)
            //    )
            //    {
            //        var label=chart2.ChartAreas[0].AxisX.CustomLabels.Single(x=>x.Text==item.Key);
            //        var pointIndex = series.Points.AddXY(label.FromPosition + 1, item.Sum(x => x.TotalSeconds));
            //        var point = series.Points[pointIndex];

            //        if (!String.IsNullOrWhiteSpace(state))
            //        {
            //            point.Label = string.Format("{0} {1:0.###}",deviceGrouping.Key,item.Sum(x => x.TotalSeconds).ToString("00.##"));
            //        }
            //    }
            //}

            var series = chart2.Series.Add("default");
            series.ChartType = SeriesChartType.Pie;

            series["PieLabelStyle"] = "Outside";
            series["PieLineColor"] = "Black";
            chart2.ChartAreas[0].Area3DStyle.Enable3D = true;

            foreach (var item in times.GroupBy(x => x.DisplayName).OrderBy(x => x.Key)
                )
            {
                var label = chart2.ChartAreas[0].AxisX.CustomLabels.Single(x => x.Text == item.Key);
                var pointIndex = series.Points.AddXY(label.FromPosition + 1, item.Sum(x => x.TotalSeconds));
                var point = series.Points[pointIndex];
                point.Label = string.Format("{0}:#PERCENT",item.Key,item.Sum(x=>x.TotalSeconds));
                point.LegendText = item.Key;
            }
            #endregion

            chart2.Tag = times;
        }

        void createDeviceFilterControls(IEnumerable<ErrorExecutionTime> dates)
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

        void createStateFilterControls(IEnumerable<ErrorExecutionTime> datas)
        {
            if (!groupBox2.Controls.Cast<Control>().Any(x => Convert.ToString(x.Tag) == "series"))
            {
                Control prevControl = null;
                foreach (var item in datas.GroupBy(x => x.DisplayName).OrderBy(x => x.Key))
                {
                    CheckBox cbx = new CheckBox();
                    cbx.AutoSize = true;
                    cbx.Text = item.Key;
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

        void clearDeviceFilterControls()
        {
            while(groupBox1.Controls.Cast<Control>().Any(x => Convert.ToString(x.Tag) == "series"))
            {
                Control ctl = groupBox1.Controls.Cast<Control>().First(x => Convert.ToString(x.Tag) == "series");
                groupBox1.Controls.Remove(ctl);
            }

        }

        void clearStateFilterControls()
        {
            while (groupBox2.Controls.Cast<Control>().Any(x => Convert.ToString(x.Tag) == "series"))
            {
                Control ctl = groupBox2.Controls.Cast<Control>().First(x => Convert.ToString(x.Tag) == "series");
                groupBox2.Controls.Remove(ctl);
            }
        }
        
        void loadGridDatas(IEnumerable<ErrorExecutionTime> datas)
        {
            var statusCheckBoxs = groupBox2.Controls.Cast<Control>().Where(x => x is CheckBox && Convert.ToString(x.Tag) == "series")
                .Select(x => x as CheckBox);

            IEnumerable<ErrorExecutionTime> bindingSource = datas.ToList();
            foreach (var item in statusCheckBoxs)
            {
                if (item.Checked == false)
                {
                    bindingSource = bindingSource.Where(x => x.DisplayName != item.Text);
                }
            }
            dgvGrid.AutoGenerateColumns = false;
            dgvGrid.DataSource = bindingSource
                .OrderBy(x => x.DeviceName)
                .ThenBy(x => x.FromState.StartTime).ToList();
        }

        private void chart1_CustomizeLegend(object sender, CustomizeLegendEventArgs e)
        {
            if (e.LegendItems.Count == 0)
            {
                return;
            }

            var datas = (IEnumerable<ErrorExecutionTime>)chart1.Tag;
            foreach (var item in e.LegendItems)
            {
                string stateName = item.Cells[1].Text;
                foreach (var cell in item.Cells.Skip(2))
                {
                    string deviceName = cell.Text;
                    if (rbCountType_TotalSecnods.Checked)
                    {
                        var v = datas.Where(x => x.DeviceName == deviceName && x.DisplayName == stateName)
                            .Sum(x => x.TotalSeconds);
                        cell.Text = v.ToString("0.##");
                    }
                    else
                    {
                        var v = datas.Where(x => x.DeviceName == deviceName && x.DisplayName == stateName)
                            .Count();
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

            var datas =(IEnumerable<ErrorExecutionTime>)chart2.Tag;
            foreach (var item in e.LegendItems)
            {
                string stateName = item.Cells[1].Text;
                foreach (var cell in item.Cells.Skip(2))
                {
                    string deviceName = cell.Text;
                    if (rbCountType_TotalSecnods.Checked)
                    {
                        var v = datas.Where(x => x.DeviceName == deviceName && x.DisplayName == stateName)
                            .Sum(x => x.TotalSeconds);

                        cell.Text = v.ToString("0.##");
                    }
                    else
                    {
                        var v = datas.Where(x => x.DeviceName == deviceName && x.DisplayName == stateName)
                            .Count();

                        cell.Text = v.ToString("0.##");
                    }
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
            load();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        private void chart1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (chart1.ChartAreas[0].AxisX.Maximum - chart1.ChartAreas[0].AxisX.Minimum < 24)
            {
                CraneDeviceChartDataAdapter adp = new CraneDeviceChartDataAdapter();
                loadLineChart(adp.FindErrorExecutionTimes(this.DeviceName,this.StartTime,this.EndTime), 0, 24);
                adp = null;
                return;
            }

            if (!cbxScaleViewEnabled.Checked)
            {
                return;
            }


            {
                var hours = chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                var fromHours = Math.Floor(hours);
                var toHours = fromHours + 1;
                CraneDeviceChartDataAdapter adp = new CraneDeviceChartDataAdapter();
                loadLineChart(adp.FindErrorExecutionTimes(this.DeviceName, this.StartTime, this.EndTime), fromHours, toHours);
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
            DateTime newDate = this.StartTime.Date.AddDays(-1);
            dateTimePicker1.Value = newDate.Date;
        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            DateTime newDate = this.StartTime.Date.AddDays(1);
            dateTimePicker1.Value = newDate.Date;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {


            this.StartTime = dateTimePicker1.Value.Date;
            this.EndTime = StartTime.AddDays(1);

            clearDeviceFilterControls();
            clearStateFilterControls();

            load();
        }

        private void chart2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lastPieChartState))
            {
                lastPieChartState = null;

                CraneDeviceChartDataAdapter adp = new CraneDeviceChartDataAdapter();
                loadPieChat(adp.FindErrorExecutionTimes(this.DeviceName, this.StartTime, this.EndTime), lastPieChartState);
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
                loadPieChat(adp.FindErrorExecutionTimes(this.DeviceName, this.StartTime, this.EndTime), lastPieChartState);
                adp = null;
            }
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
                    saveFileDialog.FileName = string.Format("堆垛机故障数据");
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
