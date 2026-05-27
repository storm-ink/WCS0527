using System;
using System.Diagnostics;

namespace CpuUsageManaged
{
    // events..
	public delegate void NewThreadEvent(ThreadInfo TempProcess);
	public delegate void ThreadCloseEvent(ThreadInfo TempProcess);
	public delegate void ThreadUpdateEvent(ThreadInfo TempProcess);

	public class ThreadCpu
	{
		public const ProcessThread CLOSED_THREAD = null;

		public static event NewThreadEvent CallNewThread;
		public static event ThreadCloseEvent CallThreadClose;
		public static event ThreadUpdateEvent CallThreadUpdate;

		public static ThreadInfo[] ThreadList;
		private static int ThreadIndex;
		private static int ProcessID = Process.GetCurrentProcess().Id;

		public static void UpdateThreadList(int SelectedProcessID)
		{
			ProcessID = SelectedProcessID;
			ThreadList = null;
			UpdateThreadList();
		}

		public static void UpdateThreadList()
		{
            // updates the thread list
			ProcessThreadCollection NewThreadList = Process.GetProcessById(ProcessID).Threads;

			ThreadIndex = 0;
			UpdateExistingThreads(NewThreadList);
			AddNewThreads(NewThreadList);
		}

		private static void UpdateExistingThreads(ProcessThreadCollection NewThreadList)
		{
            // update threads that are already loaded
			if (ThreadList == null)
			{
				ThreadList = new ThreadInfo[NewThreadList.Count];
				return;
			}

			ThreadInfo[] TempThreadList = new ThreadInfo[NewThreadList.Count];

			foreach (ThreadInfo TempThread in ThreadList)
			{
				ProcessThread CurrentThread = ThreadExists(NewThreadList,TempThread.ID);
				
				if (CurrentThread == CLOSED_THREAD)
					CallThreadClose(TempThread);
				else
				{
					TempThreadList[ThreadIndex++] = GetThreadInfo(TempThread,CurrentThread);
					CallThreadUpdate(TempThread);
				}
			}

			ThreadList = TempThreadList;
		}

		private static ProcessThread ThreadExists(ProcessThreadCollection NewThreadList,string ID)
		{
            // checks if a thread is already loaded
			foreach (ProcessThread TempThread in NewThreadList)
				if (TempThread.Id.ToString() == ID)
					return TempThread;

			return CLOSED_THREAD;
		}

		private static ThreadInfo GetThreadInfo(ThreadInfo TempThread,ProcessThread CurrentThread)
		{
            // gets the thread info
			long NewCpuUsage = (long) CurrentThread.TotalProcessorTime.TotalMilliseconds; 

			TempThread.CpuUsage = ((NewCpuUsage - TempThread.OldCpuUsage) / 
									ProcessCpu.CpuUsagePercent).ToString("F",ProcessCpu.ValueFormat);
			TempThread.OldCpuUsage = NewCpuUsage;
			
			return TempThread;
		}

        public static ThreadInfo? GetThreadInfo(ProcessThread CurrentThread)
        {
            ThreadInfo? TempThread = GetThreadInfo(CurrentThread.Id);
            if (TempThread ==null)
            {
                return null;
            }

            var xxx = TempThread.Value;
            // gets the thread info
            long NewCpuUsage = (long)CurrentThread.TotalProcessorTime.TotalMilliseconds;

            xxx.CpuUsage = (Math.Abs((NewCpuUsage - xxx.OldCpuUsage)) /
                                    ProcessCpu.CpuUsagePercent).ToString("F", ProcessCpu.ValueFormat);
            xxx.OldCpuUsage = NewCpuUsage;

            return TempThread;
        }

		private static void AddNewThreads(ProcessThreadCollection NewThreadList)
		{
            // adds the new threads
			foreach (ProcessThread TempThread in NewThreadList)
				if (!ThreadInfoExists(TempThread.Id))
					AddNewThread(TempThread);
		}

		private static bool ThreadInfoExists(int ThreadID)
		{
            // checks if the thread info is already loaded
			foreach (ThreadInfo TempThread in ThreadList)
				if (TempThread.ID == ThreadID.ToString())
					return true;
			
			return false;
		}

        private static ThreadInfo? GetThreadInfo(int ThreadID)
        {
            // checks if the thread info is already loaded
            foreach (ThreadInfo TempThread in ThreadList)
                if (TempThread.ID == ThreadID.ToString())
                    return TempThread;

            return null;
        }

		private static void AddNewThread(ProcessThread TempThread)
		{
            // loads a new thread
			ThreadInfo NewThread = new ThreadInfo();

			NewThread.ID = TempThread.Id.ToString();
			NewThread.OldCpuUsage = TempThread.TotalProcessorTime.Milliseconds;

			ThreadList[ThreadIndex++] = NewThread;
			CallNewThread(NewThread);
		}
	}

    // holds the threads info
	public struct ThreadInfo
	{
		public string ID;
		public string CpuUsage;
		public long OldCpuUsage;
	}
}
