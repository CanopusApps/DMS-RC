using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEPL.QMS.Common;
using TEPL.QMS.Workflow.DAL;
using TEPL.QMS.Workflow.Log;
using TEPL.QMS.Workflow.Models;
using Newtonsoft.Json;

namespace TEPL.QMS.Workflow.Business
{
    public class WorkflowActions
    {
        DALWorkflowActions objDALWF = new DALWorkflowActions();
        WFAdminDAL objAdminDALWF = new WFAdminDAL();
        public DataTable GetWorkflowStages(Guid WorkflowID)
        {
            return objDALWF.GetWorkflowStages(WorkflowID);
        }
        public List<WFStage> GetWFStages(Guid WorkflowID)
        {
            List<WFStage> list = new List<WFStage>();
            string strReturn = objAdminDALWF.GetWFStages(WorkflowID);
            list = BindModels.ConvertJSON<WFStage>(strReturn);
            return list;
        }

        public List<DocumentApprover> GetWorkflowApprover(Guid ProjectTypeID, Guid ProjectID, Guid NextStageID, string DocumentLevel, Guid SectionID)
        {
            List<DocumentApprover> objApprovers = null;
            DataTable dt = objDALWF.GetWorkflowApprover(ProjectTypeID, ProjectID, NextStageID, DocumentLevel, SectionID);
            objApprovers = BindModels.ConvertDataTable<DocumentApprover>(dt);
            return objApprovers;
        }
        public List<DocumentApprover> GetPrintWorkflowApprover(Guid RequestorID)
        {
            List<DocumentApprover> objApprovers = null;
            DataTable dt = objDALWF.GetPrintWorkflowApprover(RequestorID);
            objApprovers = BindModels.ConvertDataTable<DocumentApprover>(dt);
            return objApprovers;
        }
        public List<WFApprovers> GetWorkflowApprovers(Guid WorkflowID, Guid ProjectTypeID, Guid ProjectID, string DocumentLevel, Guid SectionID)
        {
            List<WFApprovers> objWFApprovers = null;
            string strReturn = objDALWF.GetWorkflowApprovers(WorkflowID, ProjectTypeID, ProjectID, DocumentLevel, SectionID);
            objWFApprovers = BindModels.ConvertJSON<WFApprovers>(strReturn);
            return objWFApprovers;
        }
        //public List<WFApprovers> GetWkflowApprovers(Guid WorkflowID, Guid ProjectTypeID, Guid ProjectID, string DocumentLevel, Guid SectionID, Guid LoggedInDepartmentID)
        //{
        //    List<WFApprovers> objWFApprovers = null;
        //    string strReturn = objDALWF.GetWkflowApprovers(WorkflowID, ProjectTypeID, ProjectID, DocumentLevel, SectionID, LoggedInDepartmentID);
        //    objWFApprovers = BindModels.ConvertJSON<WFApprovers>(strReturn);
        //    return objWFApprovers;
        //}
        public List<WFApprovers> GetWkflowApprovers(Guid WorkflowID, Guid ProjectTypeID, Guid ProjectID, string DocumentLevel, Guid SectionID, Guid LoggedInDepartmentID)
        {

            List<WFApprovers> objApprovers = null;            
            DataTable dt = objDALWF.GetWkflowApprovers(WorkflowID, ProjectTypeID, ProjectID, DocumentLevel, SectionID, LoggedInDepartmentID);                      
            objApprovers = PrintConvertDataTable<WFApprovers>(dt);
            return objApprovers;
        }
        public static List<T> PrintConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);

                if (item is WFApprovers approver)
                {
                    var conditionJson = row["Condition"].ToString();
                    var wfUsersJson = row["WFUsers"].ToString();

                    if (!string.IsNullOrEmpty(conditionJson))
                    {
                        approver.Condition = JsonConvert.DeserializeObject<List<WFCondition>>(conditionJson);
                    }
                    else
                    {
                        approver.Condition = new List<WFCondition>();
                    }

                    if (!string.IsNullOrEmpty(wfUsersJson))
                    {
                        approver.WFUsers = JsonConvert.DeserializeObject<List<WFUser>>(wfUsersJson);
                    }
                    else
                    {
                        approver.WFUsers = new List<WFUser>();
                    }
                }

                data.Add(item);
            }

            return data;
        }

        public static T GetItem<T>(DataRow row)
        {
            var obj = Activator.CreateInstance<T>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                if (row.Table.Columns.Contains(prop.Name))
                {
                    if (prop.PropertyType.IsGenericType &&
                        prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        continue;
                    }

                    prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                }
            }
            return obj;
        }


        public Stage GetWorkflowStage(Guid WorkflowID, Guid CurrentStageID)
        {
            DataTable dt = objDALWF.GetWorkflowStage(WorkflowID, CurrentStageID);
            List<Stage> objSt = new List<Stage>();
            objSt = BindModels.ConvertDataTable<Stage>(dt);

            return objSt[0];
        }

        public Guid WorkflowInitate(Guid WorkflowID, Guid DocumentID, Guid WFStageID, Guid ActionedID, Guid CreatedID)
        {
            return objDALWF.WorkflowInitate(WorkflowID, DocumentID, WFStageID, ActionedID, CreatedID);
        }
        public string ApproveAction(Guid WorkflowID, Guid ExecutionID, Guid WorkflowStageID, Guid NextWorkflowStageID, Guid ActionedID, string NextActionedID, string WorkflowAction, string ActionComments, string MulitpleApprovers, Guid CreatedID)
        {
            ExecuteAction(ExecutionID, WorkflowStageID, ActionedID, WorkflowAction, ActionComments, CreatedID, false);
            Stage objSt = GetWorkflowStage(WorkflowID, WorkflowStageID);
            CreateAction(ExecutionID, NextWorkflowStageID, NextActionedID, MulitpleApprovers, CreatedID);
            return "";
        }
        public string CreateAction(Guid ExecutionID, Guid WorkflowStageID, string ActionedID, string MultipleApprovers, Guid CreatedID)
        {
            return objDALWF.CreateAction(ExecutionID, WorkflowStageID, ActionedID, MultipleApprovers, CreatedID);
        }

        public string CreateActionForPrintRequest(Guid PrintRequestID, Guid ExecutionID, Guid WorkflowStageID, string ActionedID, Guid CreatedID)
        {
            return objDALWF.CreateActionForPrintRequest(PrintRequestID, ExecutionID, WorkflowStageID, ActionedID, CreatedID);
        }
        public string ExecuteAction(Guid ExecutionID, Guid WorkflowStageID, Guid ActionedID, string WorkflowAction, string ActionComments, Guid CreatedID, bool isDocumentUploaded)
        {
            return objDALWF.ExecuteAction(ExecutionID, WorkflowStageID, ActionedID, WorkflowAction, ActionComments, CreatedID, isDocumentUploaded);
        }

        public string ExecutePrintRequestAction(Guid ExecutionID, Guid WorkflowStageID, Guid ActionedID, string WorkflowAction, string ActionComments, Guid CreatedID)
        {
            return objDALWF.ExecutePrintRequestAction(ExecutionID, WorkflowStageID, ActionedID, WorkflowAction, ActionComments, CreatedID);
        }
        public DataTable GetExecutionDetails(Guid ExecutionID)
        {
            return objDALWF.GetExecutionDetails(ExecutionID);
        }
        public DataTable GetPendingActions(Guid ActionedID)
        {
            return objDALWF.GetPendingActions(ActionedID);
        }
    }
}
