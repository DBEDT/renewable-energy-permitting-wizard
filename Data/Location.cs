using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class Location
	{ 		
		#region Domain Object Methods
		public Domain.Location GetDomainObject(bool getChildren, bool getParent)
		{
			Domain.Location dto = new Domain.Location();
			dto.ID = this.ID;
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.CountyID = this.CountyID;
			dto.Order = this.TableDisplayOrder;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildren)
			{
				dto.PermitSets = LocationPermitSet.GetPermitSetsInLocation(this.ID);
			}

			if (getParent)
			{
				dto.County = County.GetCounty(this.CountyID);
			}
			return dto;
		}

		public Domain.BaseClass GetBaseDomainObject()
		{
			Domain.Location dto = new Domain.Location();
			dto.ID = this.ID;
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.Order = this.TableDisplayOrder;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;
			return dto;
		}
		#endregion

		#region CRUD

		public static Domain.Location GetLocation(int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from locations in db.Locations
					    where locations.ID == locationID
					    select locations).FirstOrDefault();

				return q.GetDomainObject(true, false);
			}
		}

		public static List<Domain.BaseClass> GetLocationLookups()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.BaseClass> locations = db.Locations.Select(i => i.GetBaseDomainObject()).ToList();
				return locations;
			}
		}

		public static List<Domain.Location> GetLocations()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.Location> locations = db.Locations.Select(i => i.GetDomainObject(false, false)).ToList();
				return locations;
			}
		}
		#endregion
	}
}
