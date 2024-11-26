using System;
using System.Collections.Generic;

namespace TEPL.QMS.Common.Models
{
    public class PrintRequest
    {
        public int SeqNo { get; set; }
        public Guid ID { get; set; }
        public Guid DocumentID { get; set; }
        public Guid DocumentPublishID { get; set; }
        public string DocumentNo { get; set; }
        public int RevisionNo { get; set; }
        public string EditableDocumentName { get; set; }
        public string EditableFilePath { get; set; }
        public byte[] EditableByteArray { get; set; }
        public string ReadableDocumentName { get; set; }
        public string ReadableFilePath { get; set; }
        public byte[] ReadableByteArray { get; set; }
        public string DocumentDescription { get; set; }
        public string RevisionReason { get; set; }
        public bool IsHistory { get; set; }
        public bool IsLatest { get; set; }
        public Guid ProjectTypeID { get; set; }
        public string ProjectTypeCode { get; set; }
        public string ProjectType { get; set; }
        public Guid ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public Guid DepartmentID { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public Guid SectionID { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public Guid DocumentCategoryID { get; set; }
        public string DocumentCategoryCode { get; set; }
        public string DocumentCategoryName { get; set; }
        public string DocumentLevel { get; set; }
        public Guid FunctionID { get; set; }
        public string FunctionName { get; set; }

        public Guid WorkflowID { get; set; }
        public Guid ExecutionID { get; set; }
        public string Status { get; set; }
        public string CurrentStage { get; set; }
        public Guid CurrentStageID { get; set; }
        public Guid PrintLocationID { get; set; }
        public string PrintLocationName { get; set; }
        public string PrintReason { get; set; }
        public Guid RequestorID { get; set; }
        public string RequestorName { get; set; }
        public DateTime RequestedDate { get; set; }
        public Guid ActionID { get; set; }
        public string ActionUserName { get; set; }
        public string Action { get; set; }
        public string ActionComments { get; set; }

        public string PrintPageOption { get; set; }
        public string PrintPage { get; set; }
        public string PrintCopies { get; set; }
        public string PaperSize { get; set; }




        //public Guid SectionID { get; set; }
        //public string SectionCode { get; set; }
        //public string SectionName { get; set; }
        //public Guid ProjectID { get; set; }
        //public string ProjectCode { get; set; }
        //public string ProjectName { get; set; }
        //public Guid ProjectTypeID { get; set; }
        //public string ProjectTypeCode { get; set; }
        //public string ProjectTypeName { get; set; }
        //public Guid DocumentCategoryID { get; set; }
        //public string DocumentCategoryCode { get; set; }
        //public string DocumentCategoryName { get; set; }
        //public Guid FunctionID { get; set; }
        //public string FunctionCode { get; set; }
        //public string FunctionName { get; set; }
        //public string EditableDocumentName { get; set; }
        //public string EditableFilePath { get; set; }
        //public byte[] EditableByteArray { get; set; }
        //public string ReadableDocumentName { get; set; }
        //public string ReadableFilePath { get; set; }
        //public byte[] ReadableByteArray { get; set; }
        //public decimal DraftVersion { get; set; }
        //public decimal OriginalVersion { get; set; }
        //public decimal EditVersion { get; set; }
        //public int Version { get; set; }
        //public string Comments { get; set; }
        //public bool IsMultipleApprovers { get; set; }
        //public MultipleApprovers MultipleApprovers { get; set; }
        //public MultipleApproversDisplay MultipleApproversDisplay { get; set; }
        //public Guid WorkflowID { get; set; }
        //public Guid WFExecutionID { get; set; }
        //public Guid ActionedID { get; set; }
        //public string ActionByName { get; set; }
        //public string Action { get; set; }
        //public string ActionComments { get; set; }
        //public Guid UploadedUserID { get; set; }
        //public string UploadedUserName { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public string PublishedOn { get; set; }
        //public Guid CurrentStageID { get; set; }
        //public string CurrentStage { get; set; }
        //public string WFStatus { get; set; }
        //public int PendingDays { get; set; }

        //public bool IsArchived { get; set; }
        //public Guid ArchivedUserID { get; set; }
        //public string ArchivedUserName { get; set; }
        //public string ArchivedDate { get; set; }

        public List<PrintWorkflowStages> PrintWorkflowStages;
    }

    public class PrintWorkflowStages
    {
        public int RNo { get; set; }
        public Guid ID { get; set; }
        public Guid PrintRequestID { get; set; }
        public Guid WFExecutionID { get; set; }
        public Guid WFStageID { get; set; }        
        public string StageName { get; set; }
        public string WFStatus { get; set; }
        public Guid WFActionUserID { get; set; }
        public string ActionUserName { get; set; }
        public string WFAction { get; set; }
        public string WFActionComments { get; set; }
        public DateTime? WFActionDate { get; set; }
        public string Status { get; set; }
    }
}
