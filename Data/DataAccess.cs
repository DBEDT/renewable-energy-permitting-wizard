using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using HawaiiDBEDT.Domain;

namespace HawaiiDBEDT.Data
{
	public class DataAccess
	{
		public static List<Domain.Permit> PermitList
		{
			get
			{
				return Permit.GetPermits();
			}
		}

		public static List<Domain.PermitSet> PermitSetList
		{
			get
			{
				return PermitSet.GetPermitSets();
			}
		}

		public static List<Domain.County> CountyList
		{
			get
			{
				return County.GetCounties();
			}
		}

		public static List<Domain.Location> LocationList
		{
			get
			{
				return Location.GetLocations();
			}
		}

		public static List<Domain.Department> DepartmentList
		{
			get
			{
				return Department.GetDepartments();
			}
		}

		public static List<Domain.Technology> TechnologyList
		{
			get
			{
				return Technology.GetTechnologies();
			}
		}

		public static List<Domain.PermitType> PermitTypeList
		{
			get
			{
				return PermitType.GetPermitTypes();
			}
		}

		public static List<Domain.QuestionType> QuestionTypeList
		{
			get
			{
				return QuestionType.GetQuestionTypes();
			}
		}

		public static List<Domain.PreEvaluationQuestion> PreEvaluationQuestionList
		{
			get
			{
				return PreEvaluationQuestion.GetPreEvaluationQuestions();
			}
		}

	}
}
