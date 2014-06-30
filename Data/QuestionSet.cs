using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class QuestionSet
	{
		#region Domain Object Methods
		public Domain.QuestionSet GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.QuestionSet dto = new Domain.QuestionSet();
			dto.ID = this.ID;
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.Order = this.QuestionSetOrder;
			dto.TechnologyID = this.TechnologyID;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;
			dto.Locations = QuestionSetLocation.GetQuestionSetLocations(this.ID);

			foreach (Domain.QuestionSetLocation location in dto.Locations)
			{
				dto.LocationIDs.Add(location.LocationID);
			}

			if (getChildrenObject)
			{
				dto.Questions = QuestionSetQuestion.GetQuestionsInQuestionSet(this.ID);
			}

			if (getParent)
			{
				dto.Technology = DataAccess.TechnologyList.Find(i => i.ID == this.TechnologyID);
			}
			return dto;
		}
		#endregion

		#region CRUD
		public static int AddQuestionSet(Domain.QuestionSet dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				QuestionSet questionSet = new QuestionSet
				{
					Name = dto.Name,
					Description = dto.Description,
					TechnologyID = dto.TechnologyID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				// Set quest set order to max order + 1
				byte? maxOrder = db.QuestionSets.Where(i => i.TechnologyID == dto.TechnologyID).Max(i => (byte?)i.QuestionSetOrder);
				byte order = ++maxOrder ?? 1;
				questionSet.QuestionSetOrder = order;

				db.QuestionSets.InsertOnSubmit(questionSet);
				db.SubmitChanges();

				dto.ID = questionSet.ID;

				// Add question dependencies.
				QuestionSetQuestion.AddQuestionsToQuestionSet(dto);
				QuestionSetLocation.AddQuestionSetLocations(dto);
				return questionSet.ID;
			}
		}

		public static void UpdateQuestionSet(Domain.QuestionSet dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				QuestionSet questionSet = db.QuestionSets.Where(i => i.ID == dto.ID).SingleOrDefault();

				if (questionSet != null)
				{
					questionSet.Name = dto.Name;
					questionSet.Description = dto.Description;
					questionSet.DateModified = DateTime.Now;
					questionSet.TechnologyID = dto.TechnologyID;
					db.SubmitChanges();

					// Add questions. 
					QuestionSetQuestion.DeleteQuestionsForQuestionSet(dto.ID);
					QuestionSetQuestion.AddQuestionsToQuestionSet(dto);
					QuestionSetLocation.UpdateQuestionSetLocations(dto);
				}
			}
		}

		public static Domain.QuestionSet GetQuestionSet(int questionSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from questionSets in db.QuestionSets
					    where questionSets.ID == questionSetID
					    select questionSets).FirstOrDefault();

				return q.GetDomainObject(true, true);
			}
		}

		public static List<Domain.QuestionSet> GetQuestionSets()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.QuestionSet> questionSets = db.QuestionSets.OrderBy(i => i.Name).Select(i => i.GetDomainObject(false, true)).ToList();
				return questionSets;
			}
		}

		public static List<Domain.QuestionSet> GetQuestionSets(int technologyID, int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from questionSets in db.QuestionSets
					    join questionLocations in db.QuestionSetLocations on questionSets.ID equals questionLocations.QuestionSetID
					    where questionLocations.LocationID == locationID && questionSets.TechnologyID == technologyID
					    orderby questionLocations.QuestionSetOrder
					    select questionSets
					    );

				return q.Select(i => i.GetDomainObject(false, false)).ToList();
			}
		}

		public static List<Domain.QuestionSet> GetQuestionSetDetails(int technologyID, int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from questionSets in db.QuestionSets
					    join questionLocations in db.QuestionSetLocations on questionSets.ID equals questionLocations.QuestionSetID
					    where questionLocations.LocationID == locationID && questionSets.TechnologyID == technologyID
					    orderby questionLocations.QuestionSetOrder
					    select questionSets
					    ); 

				return q.Select(i => i.GetDomainObject(true, false)).ToList();
			}
		}

		public static List<Domain.QuestionSet> GetQuestionSetComplete(int technologyID, int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from questionSets in db.QuestionSets
					    join questionLocations in db.QuestionSetLocations on questionSets.ID equals questionLocations.QuestionSetID
					    where questionLocations.LocationID == locationID && questionSets.TechnologyID == technologyID
					    orderby questionLocations.QuestionSetOrder
					    select questionSets
					    );

				return q.Select(i => i.GetDomainObject(true, true)).ToList();
			}
		}

		public static void DeleteQuestionSet(int questionSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				// Delete question set / location
				QuestionSetLocation.DeleteQuestionSetLocationForQuestionSet(questionSetID);

				// Delete any question set relationship using this question set
				QuestionSetQuestion.DeleteQuestionsForQuestionSet(questionSetID);

				var rowsToDelete = from q in db.QuestionSets
							    where q.ID == questionSetID
							    select q;

				db.QuestionSets.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
		#endregion
	}
}
