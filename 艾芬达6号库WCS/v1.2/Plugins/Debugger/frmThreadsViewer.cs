using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.Debugger
{
    public partial class frmThreadsViewer : Form
    {
        public frmThreadsViewer()
        {
            InitializeComponent();

            //ProcessCpu.CallNewProcess += ProcessCpu_CallNewProcess;
            //ProcessCpu.CallProcessUpdate += ProcessCpu_CallProcessUpdate;
            //ProcessCpu.CallProcessClose += ProcessCpu_CallProcessClose;
            //ThreadCpu.CallNewThread += ThreadCpu_CallNewThread;
            //ThreadCpu.CallThreadUpdate += ThreadCpu_CallThreadUpdate;
            //ThreadCpu.CallThreadClose += ThreadCpu_CallThreadClose;
        }

        //void ThreadCpu_CallThreadClose(ThreadInfo TempProcess)
        //{
            
        //}

        //void ThreadCpu_CallThreadUpdate(ThreadInfo TempProcess)
        //{
            
        //}

        //void ThreadCpu_CallNewThread(ThreadInfo TempProcess)
        //{
            
        //}

        //void ProcessCpu_CallProcessClose(ProcessInfo TempProcess)
        //{
           
        //}

        //void ProcessCpu_CallProcessUpdate(ProcessInfo TempProcess)
        //{
            
        //}

        //void ProcessCpu_CallNewProcess(ProcessInfo TempProcess)
        //{
            
        //}

        List<threadInfo> getThreads()
        {
            List<threadInfo> result = new List<threadInfo>();

            var mi = typeof(System.Threading.ThreadPool).GetMethod("GetGloballyQueuedWorkItemsForDebugger", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            var threadPoolAllItems = (object[])mi.Invoke(null, null);

            foreach (object item in threadPoolAllItems)
            {
                threadInfo ti = new threadInfo();
                var state_fi = item.GetType().GetField("state", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var callback_fi = item.GetType().GetField("callback", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                //ti.id = item.context.Id;
                ti.state = Convert.ToString(state_fi.GetValue(item));
                ti.callback = Convert.ToString(((System.Delegate)callback_fi.GetValue(item)).Method);
                ti.target = Convert.ToString(((System.Delegate)callback_fi.GetValue(item)).Target);

                result.Add(ti);
            }

            var managedThreads = Wcs.ThreadExtentions.Threads.Select(x=>new 
            {
                Id=x.GetThreadId(),
                x=x 
            });
            
            foreach (System.Diagnostics.ProcessThread item in System.Diagnostics.Process.GetCurrentProcess().Threads)
            {
                threadInfo ti = new threadInfo();
                ti.id = item.Id;
                ti.state = item.ThreadState.ToString();
                var t=managedThreads.FirstOrDefault(x => x.Id == item.Id);
                if (t!=null)
                {
                    ti.target = t.x.Name;
                }

                //var tc = ThreadCpu.GetThreadInfo(item);
                try
                {

                    ti.time = item.UserProcessorTime;
                }
                catch (Exception)
                {

                }
                //if(tc!=null){
                //    ti.cpuUsage = tc.Value.CpuUsage;
                //}
                //ti.callback = item..Method.ToString();
                //ti.target = item.callback.Target.ToString();

                result.Add(ti);
            }

            return result;
        }

        class threadInfo
        {
            public Int32? id { get; set; }
            public String state { get; set; }
            public String callback { get; set; }
            public String target { get; set; }
            public TimeSpan? time { get; set; }
            public String cpuUsage { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int32 index = 0;
            if (dgvGrid.CurrentRow != null)
            {
                index = dgvGrid.CurrentRow.Index;
            }

            //ProcessCpu.UpdateProcessList();
            //ThreadCpu.UpdateThreadList();

            var data=getThreads();
            
            //ProcessCpu.UpdateProcessList();
            //ThreadCpu.UpdateThreadList();
            //data=getThreads();
            data = data.OrderBy(x => x.id == null ? 999999 : x.id.Value)
                .ToList();

            dgvGrid.DataSource = data;

            lblNum.Text = data.Count.ToString();

            if (dgvGrid.Rows.Count > index)
            {
                dgvGrid.Rows[index].Selected = true;
                dgvGrid.FirstDisplayedScrollingRowIndex = index;
            }
        }
    }
}
