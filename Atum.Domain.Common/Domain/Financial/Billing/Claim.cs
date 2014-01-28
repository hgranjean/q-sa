using System;

using Atum.Domain.Basis;

namespace Atum.Domain.Billing
{
	/// <summary;}}
	/// Summary description for Claim.
	/// {get{ return _summary;}}
	public class Claim : DomainObject
	{
		private long _claimId;

		public Claim()
		{
		}

		public long ClaimId{get{ return _claimId;}}
//		public object PersonId{get{ return _PersonId;}}
//		public object PracticeId{get{ return _PracticeId;}}
//		public object PatientOnsetDate{get{ return _PatientOnsetDate;}}
//		public object PreExistIllnessDate{get{ return _PreExistIllnessDate;}}
//		public object ReferringPhysicianId{get{ return _ReferringPhysicianId;}}
//		public object Diagnosis1{get{ return _Diagnosis1;}}
//		public object Diagnosis2{get{ return _Diagnosis2;}}
//		public object Diagnosis3{get{ return _Diagnosis3;}}
//		public object Diagnosis4{get{ return _Diagnosis4;}}
//		public object DiagnosisCodes{get{ return _DiagnosisCodes;}}
//		public object PatientNoWorkFromDate{get{ return _PatientNoWorkFromDate;}}
//		public object PatientNoWorkToDate{get{ return _PatientNoWorkToDate;}}
//		public object HospitalizedFromDate{get{ return _HospitalizedFromDate;}}
//		public object HospitalizedToDate{get{ return _HospitalizedToDate;}}
//		public object OutsideLab{get{ return _OutsideLab;}}
//		public object OutsideLabCharges{get{ return _OutsideLabCharges;}}
//		public object PriorAuthorizationNumber{get{ return _PriorAuthorizationNumber;}}
//		public object IsWorkersComp{get{ return _IsWorkersComp;}}
//		public object IsAutoAccident{get{ return _IsAutoAccident;}}
//		public object IsOther{get{ return _IsOther;}}
//		public object ClaimRemarks{get{ return _ClaimRemarks;}}
//		public object AccidentRemarks{get{ return _AccidentRemarks;}}
//		public object IsDeleted{get{ return _IsDeleted;}}
//		public object CoverageId{get{ return _CoverageId;}}
//		public object UserUpdatedById{get{ return _UserUpdatedById;}}
//		public object UserUpdateDate{get{ return _UserUpdateDate;}}
//		public object UserCreatedById{get{ return _UserCreatedById;}}
//		public object UserCreateDate{get{ return _UserCreateDate;}}
//		public object CoverageTypeId{get{ return _CoverageTypeId;}}
//		public object CoverageType{get{ return _CoverageType;}}
//		public object InsuranceId{get{ return _InsuranceId;}}
//		public object ClaimNumber{get{ return _var;}}
//		public object FacilityId{get{ return _var;}}
//		public object ReferringPhysicianName{get{ return _var;}}
//		public object MedicaidSubmissionCode{get{ return _var;}}
//		public object ConditionCorrelationToEmployment{get{ return _var;}}
//		public object ConditionCorrelationToAutoAccident{get{ return _var;}}
//		public object ConditionCorrelationToOtherAccident{get{ return _var;}}
//		public object ClaimRemarks_19{get{ return _var;}}
//		public object ClaimRemarks_10d{get{ return _var;}}
//		public object Problems{get{ return _var;}}

        protected override void setId(long id)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
