using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEDExporter.DTO
{
    public class ImportLeanKit
    {
        public string Assigned_Users { get; set; }
        public string Card_Title { get; set; }
        public string Lane_Id { get; set; }
        public string ExternalSystemeName { get; set; }
        public string Lane_Title { get; set; }
        public string Tags { get; set; }
        public string ExternalSystemUrl { get; set; }
        public string Class_Of_Service { get; set; }
        public string Card_Block_State_Change_Date { get; set; }
        public string Card_Size { get; set; }
        public string Card_Is_Blocked { get; set; }
        public string Card_Block_Reason { get; set; }
        public string Index { get; set; }
        public string Card_StartDate { get; set; }
        public string Card_Description { get; set; }
        public string Card_Type { get; set; }
        public string Card_Priority { get; set; }
        public string ExternalCardID { get; set; }
        public string ParentCardID { get; set; }
        public string ParentCardIDs { get; set; }
        public string LastMove { get; set; }
        public string LastActivity { get; set; }
        public string Card_DateArchived { get; set; }
        public string ActualStartDate { get; set; }
        public string ActualFinishDate { get; set; }
    }
}
