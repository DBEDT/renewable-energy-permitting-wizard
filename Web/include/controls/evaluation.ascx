<%@ Register TagPrefix="HawaiiDBEDT" TagName="ProgressBar" Src="progress_bar.ascx" %>
<%@ Register TagPrefix="HawaiiDBEDT" TagName="PreEvaluation" Src="pre_evaluation.ascx" %>
<%@ Register TagPrefix="HawaiiDBEDT" TagName="QuestionSet" Src="question_set.ascx" %>
<%@ Register TagPrefix="HawaiiDBEDT" TagName="Results" Src="results.ascx" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="evaluation.ascx.cs" Inherits="HawaiiDBEDT.Web.Controls.EvaluationControl" %>
<!-- BEGIN EVAL PROGRESS BAR -->
<HawaiiDBEDT:ProgressBar ID="ProgressBar1" runat="server" />
<!-- END EVAL PROGRESS BAR-->
<HawaiiDBEDT:PreEvaluation ID="PreEvalControl1" runat="server" />
<HawaiiDBEDT:QuestionSet ID="QuestionSetControl1" runat="server" Visible="false" />
<HawaiiDBEDT:Results ID="ResultsControl1" runat="server" Visible="false" />