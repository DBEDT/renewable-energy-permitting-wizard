using System;
using System.Collections.Generic;

namespace HawaiiDBEDT.Data.Pdf
{
    using System.Configuration;
    using System.Data.SqlClient;
    using Domain.Pdf;

    public class ResponseGrid
    {
        private const string ResponseGridSelect =
            "select qs.QuestionSetID, qs.Name as QuestionSet, q.QuestionID, q.QuestionText, r.ResponseID, r.Name as Response, rps.PermitSetID, ps.Name as PermitSetName, min(p.URL) as URL, min(er.PageNo) as PageNo, count(p.PermitID) NumberOfPermits, er.Note"
            + " from dbo.EvaluationResponse er"
            + " join dbo.Evaluation e on er.EvaluationID = e.EvaluationID"
            + " join dbo.Question q on er.QuestionID = q.QuestionID"
            + " join dbo.QuestionSetQuestions qsq on qsq.QuestionID = q.QuestionID"
            + " join dbo.QuestionSet qs on qs.QuestionSetID = qsq.QuestionSetID"
            + " join dbo.QuestionSetLocations qsl on qsl.QuestionSetID = qsq.QuestionSetID"
            + " join dbo.Response r on er.ResponseID = r.ResponseID"
            + " left outer join dbo.ResponsePermitSet rps on rps.ResponseID = er.ResponseID"
            + " left outer join dbo.PermitSet ps on ps.PermitSetID = rps.PermitSetID"
            + " left outer join dbo.PermitSetPermits psp on psp.PermitSetID = ps.PermitSetID"
            + " left outer join dbo.Permit p on p.PermitID = psp.PermitID"
            + " where er.EvaluationID = @EvaluationId"
            + " and (rps.LocationID = e.LocationID or rps.PermitSetID is null)"
            + " and qsl.LocationID = e.LocationID and qs.TechnologyID = e.TechnologyID"
            + " group by qs.QuestionSetID, qs.Name, q.QuestionID, q.QuestionText, r.ResponseID, r.Name, rps.PermitSetID, ps.Name, er.Note"
            + " order by PageNo, QuestionText";

        public static List<Domain.Pdf.ResponseGrid> GetItems(int evaluationId)
        {
            var responses = new List<Domain.Pdf.ResponseGrid>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(ResponseGridSelect, connection))
                {
                    command.Parameters.Add(new SqlParameter("@EvaluationId", evaluationId));
                    using (var reader = command.ExecuteReader())
                    {
                        var helperList = new List<string>();
                        var helperDict = new Dictionary<string, List<int>>();
                        while (reader.Read())
                        {
                            var questionSetId = reader["QuestionSetID"].ToString();
                            var questionSet = reader["QuestionSet"].ToString(); 
                            var questionId = reader["QuestionID"].ToString();
                            var responseId = reader["ResponseID"].ToString();
                            var question = reader["QuestionText"].ToString();
                            var response = reader["Response"].ToString();
                            var pageNo = reader["PageNo"].ToString();
                            var note = reader["Note"].ToString();
                            int? permitSetId = null;
                            int permitCountValue;
                            string permitSetName = string.Empty, url = string.Empty, permitCount = string.Empty;
                            if (!string.IsNullOrEmpty(reader["PermitSetID"].ToString()))
                            {
                                permitSetId = Int32.Parse(reader["PermitSetID"].ToString());
                                permitSetName = reader["PermitSetName"].ToString();
                                permitCount = reader["NumberOfPermits"].ToString();
                                if (int.TryParse(permitCount, out permitCountValue))
                                {
                                    if (permitCountValue == 1)
                                    {
                                        url = reader["URL"].ToString();
                                    }
                                    else
                                    {
                                        url = string.Format("{0}evaluate/permit_set.aspx?id={1}", ConfigurationManager.AppSettings["ApplicationUrl"], permitSetId);
                                    }
                                }
                            }
                            var key = string.Format("{0}|{1}|{2}|{3}", questionSetId, questionId, responseId, pageNo);
                            if (!helperList.Contains(key))
                            {
                                var responseGridItem = new Domain.Pdf.ResponseGrid()
                                                           {
                                                               QuestionSet = questionSet,
                                                               Question = question,
                                                               Response = response,
                                                               PageNo = Int32.Parse(pageNo),
                                                               Note = note,
                                                               PermitSetInfo = new List<PermitSetInfo>()
                                                           };
                                if (permitSetId.HasValue)
                                {
                                    responseGridItem.PermitSetInfo.Add(new PermitSetInfo()
                                        {
                                            Name = permitSetName,
                                            Urls = new List<string> {url},
                                            PermitCount = permitCount
                                        });
                                    helperDict.Add(key, new List<int> { permitSetId.Value });
                                }
                                else
                                {
                                    helperDict.Add(key, new List<int> ());
                                }            
                                helperList.Add(key);
                                responses.Add(responseGridItem);
                            }
                            else if (permitSetId.HasValue)
                            {
                                var position = helperList.IndexOf(key);
                                if (!helperDict[key].Contains(permitSetId.Value))
                                {
                                    responses[position].PermitSetInfo.Add(new PermitSetInfo
                                                                              {
                                                                                  Name = permitSetName,
                                                                                  Urls = new List<string> { url },
                                                                                  PermitCount = permitCount
                                                                              });
                                }
                                else
                                {
                                    responses[position].PermitSetInfo[helperDict[key].IndexOf(permitSetId.Value)].Urls.Add(url);
                                }
                            }
                        }
                    }
                }
            }

            return responses;
        }
    }
}


