using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HawaiiDBEDT.Domain.Enumerations;

namespace HawaiiDBEDT.Domain
{
	public class Evaluation : BaseClass
	{
		#region Properties

		// Pre-evaluation questions
		public Technology Technology { get; set; }
		public int TechnologyID { get; set; }
		public User User { get; set; }
		public int UserID { get; set; }
		public Location Location { get; set; }
		public int LocationID { get; set; }
		public int CapacityID { get; set; }

		public List<EvaluationResponse> CapacityAnswers { get; set; }

		public int FederalID { get; set; }

		public List<EvaluationResponse> FederalAnswers { get; set; }

		public List<QuestionSet> QuestionSets { get; set; }
		public List<PermitSet> PermitSets { get; set; }
		public List<Permit> Permits { get; set; }
        public List<Permit> DistinctPermits { get; set; } 

		public List<Question> UniqueQuestions { get; set; }
		public List<EvaluationResponse> Answers { get; set; }

		public int NumPages { get; set; }
		public int CurrentPage { get; set; }
		public bool? Active { get; set; }
		public int StatusID { get; set; }
		public int PageIndex
		{
			get
			{
				return CurrentPage - 1;
			}
		}

        public int? LastStepFilled { get; set; }
		#endregion


		#region Constructors
		public Evaluation()
		{
			Active = false;
			Name = "";
			Description = "";
			User = new User();
			CurrentPage = 0;
			Answers = new List<EvaluationResponse>();
			CapacityAnswers = new List<EvaluationResponse>();
			QuestionSets = new List<QuestionSet>();
			CapacityID = 0;
			StatusID = (int)EvaluationStatusEnum.InProgress; 
			PermitSets = new List<PermitSet>();
		}
		#endregion
	}
}
