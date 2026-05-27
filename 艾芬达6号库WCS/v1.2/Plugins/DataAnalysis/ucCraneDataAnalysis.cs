using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.IO;

namespace Wcs.App.Plugins.DataAnalysis
{
    public partial class ucCraneDataAnalysis : UserControl
    {
        public ucCraneDataAnalysis()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            CraneDeviceDataAnalysis.CraneDeviceChartDataAdapter adp = new CraneDeviceDataAnalysis.CraneDeviceChartDataAdapter();

            var datas = adp.FindStateExecutionTimes(cbxDevices.Text, dtpStartDate.Checked ? dtpStartDate.Value : (DateTime?)null, dtpEndDate.Checked ? dtpEndDate.Value : (DateTime?)null);

            loadLineChat(datas,adp);
            loadPieChat(datas, adp);

            var errors=adp.FindErrorExecutionTimes(cbxDevices.Text, dtpStartDate.Checked ? dtpStartDate.Value : (DateTime?)null, dtpEndDate.Checked ? dtpEndDate.Value : (DateTime?)null);
            loadErrorLineChat(errors, adp);
            loadErrorPieChat(errors, adp);
        }

        void loadLineChat(IEnumerable<StateExecutionTime> datas, IChartDataAdapter adp)
        {
            chart1.Series.Clear();

            var series1 = chart1.Series.Add("series1");
            //设置 x 轴
            chart1.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms
                .DataVisualization.Charting
                .DateTimeIntervalType.Hours; //间隔类型为 小时
            chart1.ChartAreas[0].AxisX.Interval = 0.5; //间隔值为 0.5
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss"; //标签输出格式

            //设置 y 轴
            chart1.ChartAreas[0].AxisY.Interval = 1; //间隔为 1
            chart1.ChartAreas[0].AxisY.LabelStyle.Angle = 0; //标签输出旋转角度为 0

            //禁用 图例
            chart1.Legends[0].Enabled = false;

            //设置图表类型为 线性图
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            //向 Series 添加 数据点
            foreach (var item in datas)
            {
                var index = series1.Points.AddXY(item.FromState.StartTime, item.FromState.State);
            }

            chart1.ChartAreas[0].AxisY.CustomLabels.Clear();
            //将所有状态添加到 y 轴
            foreach (var item in datas.GroupBy(x => new
            {
                x.FromState.State,
                x.FromState.StateDisplayName
            }).OrderBy(x => Convert.ToInt32(x.Key.State)))
            {
                chart1.ChartAreas[0].AxisY.CustomLabels.Add(Convert.ToInt32(item.Key.State) - 1, Convert.ToInt32(item.Key.State) + 1, item.Key.StateDisplayName);
            }
        }

        void loadPieChat(IEnumerable<StateExecutionTime> datas, IChartDataAdapter adp)
        {
            chart2.Series.Clear();

            var series1 = chart2.Series.Add("series1");
            //设置 x 轴
            chart2.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms
                .DataVisualization.Charting
                .DateTimeIntervalType.Hours; //间隔类型为 小时
            chart2.ChartAreas[0].AxisX.Interval = 0.5; //间隔值为 0.5
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss"; //标签输出格式

            //设置 y 轴
            chart2.ChartAreas[0].AxisY.Interval = 1; //间隔为 1
            chart2.ChartAreas[0].AxisY.LabelStyle.Angle = 0; //标签输出旋转角度为 0

            //启用 图例
            chart2.Legends[0].Enabled = true;
            chart2.Legends[0].LegendStyle = LegendStyle.Table;
            chart2.Legends[0].CellColumns.Clear();


            //设置图表类型为 线性图
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1["PieLabelStyle"] = "Outside";
            series1["PieLineColor"] = "Black";

            chart2.ChartAreas[0].Area3DStyle.Enable3D = true;

            //向 Series 添加 数据点
            foreach (var grouping in datas.GroupBy(x => new
            {
                x.FromState.State,
                x.FromState.StateDisplayName
            }))
            {
                var index = series1.Points.AddXY(grouping.Key.State, grouping.Sum(x => x.TotalSeconds));
                series1.Points[index].LegendText = grouping.Key.StateDisplayName + ":#PERCENT";
                series1.Points[index].Label = grouping.Key.StateDisplayName + ":#PERCENT";
            }

            chart2.ChartAreas[0].AxisY.CustomLabels.Clear();
            //将所有状态添加到 y 轴
            foreach (var item in datas.GroupBy(x => new
            {
                x.FromState.State,
                x.FromState.StateDisplayName
            }).OrderBy(x=>Convert.ToInt32(x.Key.State)))
            {
                chart2.ChartAreas[0].AxisY.CustomLabels.Add(Convert.ToInt32(item.Key.State) - 1, Convert.ToInt32(item.Key.State) + 1, item.Key.StateDisplayName);
            }
        }


        void loadErrorLineChat(IEnumerable<ErrorExecutionTime> datas, IChartDataAdapter adp)
        {
            chart3.Series.Clear();
            var allStatus = datas.GroupBy(x => new {
            x.FromState.Code,
            x.FromState.Name
            }).Select(x=>x.Key)
            .ToDictionary((x)=>{
                return x.Code;
            },(x)=>{
                return x.Name;
            });

            if (!allStatus.Any(x => x.Key == "0"))
            {
                allStatus.Add("0","正常");
            }

            var series1 = chart3.Series.Add("series1");
            //设置 x 轴
            chart3.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms
                .DataVisualization.Charting
                .DateTimeIntervalType.Minutes; //间隔类型为 分钟
            chart3.ChartAreas[0].AxisX.Interval = 5; //间隔值为 5
            chart3.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss"; //标签输出格式

            //设置 y 轴
            chart3.ChartAreas[0].AxisY.Interval = 1; //间隔为 1
            chart3.ChartAreas[0].AxisY.LabelStyle.Angle = 0; //标签输出旋转角度为 0
            chart3.ChartAreas[0].AxisY.LabelStyle.IsEndLabelVisible = true;
            //禁用 图例
            chart3.Legends[0].Enabled = false;

            //设置图表类型为 线性图
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            //向 Series 添加 数据点
            foreach (var item in datas)
            {
                var index = series1.Points.AddXY(item.FromState.StartTime, allStatus.ToList().IndexOf(allStatus.Single(x=>x.Key==item.FromState.Code)));
            }


            //chart3.ChartAreas[0].AxisY.IsInterlaced = true;
            //将所有状态添加到 y 轴
            chart3.ChartAreas[0].AxisY.CustomLabels.Clear();
            foreach (var item in allStatus.OrderBy(x=>Convert.ToInt32(x.Key)))
            {
                var index = allStatus.ToList().IndexOf(item);
                chart3.ChartAreas[0].AxisY.CustomLabels.Add(index - 1, index + 1, item.Value);
            }
        }

        void loadErrorPieChat(IEnumerable<ErrorExecutionTime> datas, IChartDataAdapter adp)
        {
            chart4.Series.Clear();

            var series1 = chart4.Series.Add("series1");

            //设置 y 轴
            chart4.ChartAreas[0].AxisY.Interval = 1; //间隔为 1
            chart4.ChartAreas[0].AxisY.LabelStyle.Angle = 0; //标签输出旋转角度为 0

            //启用 图例
            chart4.Legends[0].Enabled = true;
            chart4.Legends[0].LegendStyle = LegendStyle.Table;
            chart4.Legends[0].CellColumns.Clear();
            chart4.Legends[0].IsTextAutoFit = true;


            //设置图表类型为 线性图
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1["PieLabelStyle"] = "Outside";
            series1["PieLineColor"] = "Black";

            chart4.ChartAreas[0].Area3DStyle.Enable3D = true;


            //向 Series 添加 数据点
            foreach (var grouping in datas.GroupBy(x => new
            {
                x.FromState.Code,
                x.FromState.Name
            }))
            {
                var index = series1.Points.AddXY(grouping.Key.Name, grouping.Sum(x => x.TotalSeconds));
                series1.Points[index].LegendText = grouping.Key.Name + ":#PERCENT";
                series1.Points[index].Label = grouping.Key.Name + ":#PERCENT";
            }
        }


    }
}
