using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoForksRackLocations.asdf
{
    public class CreateRackLocationSettings
    {
        List<UserLocationRelationship> _userLocationRelationships = new List<UserLocationRelationship>();

        public Int32 LeftRackNo { get; set; }
        public Int32 RightRackNo { get; set; }
        public Int32 Left2RackNo { get; set; }
        public Int32 Right2RackNo { get; set; }
        public String LanewayName { get; set; }
        public UserLocationRelationship[] UserLocationRelationships
        {
            get
            {
                return _userLocationRelationships.ToArray();
            }
        }

        public void AddUserLocationRelationship(UserLocationRelationship releationship)
        {
            _userLocationRelationships.Add(releationship);
        }
    }

}
