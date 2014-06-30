namespace HawaiiDBEDT.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;

    public class ApplicationManager
    {
        List<Domain.PermitDependency> permitDependencies = new List<Domain.PermitDependency>();
        public List<Domain.PermitDependency> PermitDependencies
        {
            get
            {
                if (permitDependencies.Count == 0)
                {
                    permitDependencies = Data.PermitDependency.GetPermitDependenciesAll(PermitList);
                }
                return permitDependencies;
            }
        }

        List<Domain.Permit> permitList = new List<Domain.Permit>();
        public List<Domain.Permit> PermitList
        {
            get
            {
                if (permitList.Count == 0)
                {
                    permitList = Data.Permit.GetPermits();
                }
                return permitList;
            }
        }

        public List<Domain.PermitDependency> GetChildPermitsNR(int permitID)
        {
            return PermitDependencies.Where(i => i.DependentPermitID == permitID).ToList();
        }

        public List<Domain.PermitDependency> GetParentPermitsNR(int permitID)
        {
            return PermitDependencies.Where(i => i.PermitID == permitID).ToList();
        }

        public List<Domain.Permit> GetDistinctPermitsForEvaluation(Domain.Evaluation evaluation)
        {
            List<Domain.Permit> permits = new List<Domain.Permit>();
            foreach (Domain.EvaluationResponse answer in evaluation.Answers)
            {
                permits.AddRange(Data.ResponsePermitSet.GetPermitsForResponse(answer.ResponseID, evaluation.Location.ID));
            }

            // Add permits for technology	
            permits.AddRange(Data.TechnologyPermitSet.GetPermitsInTechnology(evaluation.Technology.ID));

            // Add permits for location 
            permits.AddRange(Data.LocationPermitSet.GetPermitsInLocation(evaluation.Location.ID));

            // Add permits for capacity
            permits.AddRange(Data.PreEvaluationResponsePermitSet.GetPermitsForPreEvaluationResponse(evaluation.CapacityID, evaluation.Location.ID));

            // Add permits for capacity subquestions
            foreach (Domain.EvaluationResponse answer in evaluation.CapacityAnswers)
            {
                permits.AddRange(Data.ResponsePermitSet.GetPermitsForResponse(answer.ResponseID, evaluation.Location.ID));
            }

            var distinctPermits = permits.Distinct(new PermitComparer()).ToList();

            foreach (var permit in distinctPermits)
            {
                CalculatePermitDependencies(permit, distinctPermits, 0);
            }

            return distinctPermits;
        } 

        public static List<Domain.Permit> GetDistinctPermits(List<Domain.Permit> permits)
        {
            return permits.Distinct(new PermitComparer()).ToList();
        }

        private void CalculatePermitDependencies(Domain.Permit permit, List<Domain.Permit> distinctPermits, int j)
        {
            if (j > 6)
            {
                return;
            }

            j++;
            permit.Dependencies = Data.PermitDependency.GetPermitDependencies(permit.ID);
            if (permit.Dependencies.Count > 0)
            {
                foreach (Domain.PermitDependency dependency in permit.Dependencies)
                {
                    if (dependency.Permits.Count > 0)
                    {
                        IEnumerable<Domain.Permit> availablePermits = distinctPermits.Intersect(dependency.Permits, new PermitComparer());

                        if (availablePermits.ToList().Count > 0)
                        {
                            if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.FinishToStart)
                            {
                                int maxDelay = availablePermits.Max(i => i.EndDuration);

                                permit.StartDuration = maxDelay;
                                UpdateParentPermitDependency(permit, distinctPermits, j);
                                UpdateChildrenPermitDependency(permit, distinctPermits, j);
                            }
                            else if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.StartToStart)
                            {
                                int maxDelay = availablePermits.Max(i => i.StartDuration);
                                if (permit.StartDuration < maxDelay)
                                {
                                    permit.StartDuration = maxDelay;
                                    UpdateParentPermitDependency(permit, distinctPermits, j);
                                    UpdateChildrenPermitDependency(permit, distinctPermits, j);
                                }
                            }
                            else if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.FinishToFinish)
                            {
                                int maxEndDuration = availablePermits.Max(i => i.EndDuration);
                                if (permit.EndDuration < maxEndDuration)
                                {
                                    permit.StartDuration = maxEndDuration - permit.Duration;
                                    UpdateParentPermitDependency(permit, distinctPermits, j);
                                    UpdateChildrenPermitDependency(permit, distinctPermits, j);
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void UpdateChildrenPermitDependency(Domain.Permit permit, List<Domain.Permit> distinctPermits, int j)
        {
            List<Domain.PermitDependency> dependencies = GetChildPermitsNR(permit.ID);
            if (dependencies.Count > 0)
            {
                foreach (Domain.PermitDependency dependency in dependencies)
                {
                    // there should only be one permit
                    if (dependency.Permits.Count > 0)
                    {
                        int childPermitID = dependency.Permits[0].ID;
                        if (distinctPermits.Find(i => i.ID == childPermitID) != null)
                        {
                            Domain.Permit childPermit = distinctPermits.Find(i => i.ID == childPermitID);

                            if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.FinishToStart)
                            {
                                if (permit.EndDuration > childPermit.StartDuration)
                                {
                                    childPermit.StartDuration = permit.EndDuration;
                                    CalculatePermitDependencies(childPermit, distinctPermits, j);
                                }
                            }
                            else if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.StartToStart)
                            {
                                if (permit.StartDuration > childPermit.StartDuration)
                                {
                                    childPermit.StartDuration = permit.StartDuration;
                                    CalculatePermitDependencies(childPermit, distinctPermits, j);
                                }

                            }
                            else if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.FinishToFinish)
                            {
                                if (permit.EndDuration > childPermit.EndDuration)
                                {
                                    childPermit.StartDuration = permit.EndDuration - childPermit.Duration;
                                    CalculatePermitDependencies(childPermit, distinctPermits, j);
                                }
                            }

                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }


        private void UpdateParentPermitDependency(Domain.Permit permit, List<Domain.Permit> distinctPermits, int j)
        {
            List<Domain.PermitDependency> dependencies = GetParentPermitsNR(permit.ID);
            if (dependencies.Count > 0)
            {
                foreach (Domain.PermitDependency dependency in dependencies)
                {
                    int parentPermitID = dependency.DependentPermitID;
                    if (distinctPermits.Find(i => i.ID == parentPermitID) != null)
                    {
                        Domain.Permit parentPermit = distinctPermits.Find(i => i.ID == parentPermitID);

                        if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.FinishToStart)
                        {
                            if (parentPermit.EndDuration == permit.StartDuration)
                            {
                                break;
                            }

                            if (parentPermit.EndDuration > permit.StartDuration)
                            {
                                parentPermit.StartDuration = permit.StartDuration - parentPermit.Duration;
                                CalculatePermitDependencies(parentPermit, distinctPermits, j);
                            }
                        }
                        else if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.StartToStart)
                        {
                            if (permit.StartDuration > parentPermit.StartDuration)
                            {
                                parentPermit.StartDuration = permit.StartDuration;
                                CalculatePermitDependencies(parentPermit, distinctPermits, j);
                            }
                        }
                        else if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.FinishToFinish)
                        {
                            if (parentPermit.EndDuration > permit.EndDuration)
                            {
                                parentPermit.StartDuration = permit.EndDuration - parentPermit.Duration;
                                CalculatePermitDependencies(parentPermit, distinctPermits, j);
                            }
                        }

                    }
                }
            }
        }
    }
}
