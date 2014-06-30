using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data.Pdf
{
    public class PermitGrid
    {
        //public static List<Domain.Pdf.PermitGrid> GetItems(int responseID, int locationID)
        //{
        //    using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
        //    {
        //        var q = (from permitSets in db.PermitSets
        //                 join responsePermitSets in db.PreEvaluationResponsePermitSets on permitSets.ID equals responsePermitSets.PermitSetID
        //                 where responsePermitSets.PreEvaluationResponseID == responseID && responsePermitSets.LocationID == locationID
        //                 orderby permitSets.Name
        //                 select new Domain.Pdf.PermitGrid()
        //                            {
        //                                Name = permitSets.,
        //                                StartMonth = permitSets.
        //                            }).Distinct();

        //        return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
        //    }
        //}
    }
}
