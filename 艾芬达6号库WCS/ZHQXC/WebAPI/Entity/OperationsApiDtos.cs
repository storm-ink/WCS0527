namespace ZHQXC
{
    public class ResumeTaskRequest
    {
        public string CurrentUserCode { get; set; }
    }

    public class OperationsDeviceDto
    {
        public string Name { get; set; }

        public string DeviceType { get; set; }

        public bool IsConnected { get; set; }

        public bool IsLocked { get; set; }

        public string LockerUser { get; set; }

        public string LockerIp { get; set; }

        public bool IsTaskable { get; set; }
    }
}
