using System;
using System.Data;
using ecn.common.classes;

namespace ecn.communicator.classes {
    
    /// A Blast Plan can happen at the beginning or the end of a blast. Eventually we should capture any exceptions and create an
    /// exception blast event that the users can set a plan for. For now, only start and end events fire.
    
    public class BlastPlans: DatabaseAccessor {
        Blasts _blast;
        Groups _group;
        int _customer_id;
        int _blast_day;
        double _period;
        string _type;

		
		/// Int Consturctor
		
		/// <param name="input_id">BlastPlanID</param>
        public BlastPlans(int input_id):base(input_id) {
            DefaultSetup();
        }
		
		/// String COnstructor
		
		/// <param name="input_id">BlastPlanID</param>
        public BlastPlans(string input_id):base(input_id) {
            DefaultSetup();
        }
		
		/// NullaryConstructor
		
        public BlastPlans():base() { 
            DefaultSetup();
        }

		
		/// helper constructor to set up the defaults.
		
        private void DefaultSetup() {
            _blast = new Blasts();
            _group = new Groups();
            _period = 0.0;
            _blast_day = 0;
        }

        public void Blast(Blasts blast) {
            _blast=blast;
        }
        public void Group(Groups group) {
            _group=group;
        }
        public void CustomerID(int cid) {
            _customer_id = cid;
        }
        public void Period(double per) {
            _period = per;
        }
        public void BlastDay(int bday) {
            _blast_day = bday;
        }
        public double Period() {

            if(ID() != 0) {
                try {
                    _period = Convert.ToDouble(DataFunctions.ExecuteScalar("Select Period from BlastPlans where BlastPlanID=" + ID() + " and IsDeleted = 0 "));
                } catch {

                }
            }
            return _period;
        }
        public int BlastDay() {

            if(ID() != 0) {
                try {
                    _blast_day = Convert.ToInt32(DataFunctions.ExecuteScalar("Select BlastDay from BlastPlans where BlastPlanID=" + ID() + " and IsDeleted = 0 "));
                } catch {

                }
            }
            return _blast_day;
        }

        public string PlanType() {

            if(ID() != 0) {
                try {
                    return DataFunctions.ExecuteScalar("Select PlanType from BlastPlans where BlastPlanID=" + ID() + " and IsDeleted = 0 ").ToString();
                } catch {

                }
            }
            return "period";
        }

		
		/// Group we will send this blast plan to.
		
		/// <returns> Group Object</returns>
        public Groups Group() {

            if(ID() != 0) {
                try {
                    _group.ID(Convert.ToInt32(DataFunctions.ExecuteScalar("Select GroupID from BlastPlans where BlastPlanID=" + ID() + " and IsDeleted = 0 ")));
                } catch {

                }
            }
            return _group;
        }

		
		/// Sets the event type for filtering purposes
		
		/// <param name="type">start and end</param>
        public void EventType(string type) {
            _type= type;
        }

		
		/// Sets the blastid, groupID, and period of a particular blast plan.
		
        public void Update() {
          
            DataFunctions.ExecuteScalar("communicator", "Update BlastPlans set Period = "+_period + ", BlastID="+_blast.ID()
                + " , GroupID = " + _group.ID() +
                " WHERE BlastPlanID = " + ID());
        }

		
		/// Creates a new blast plan based on if we have a day, period, or system if not.
		
        public void Create() {
            if(_customer_id == 0) {
                throw new Exception("CustID bad");
            }
            string plan_type = "";
            if(_blast_day > 0) {
                plan_type = "day";
            } else if( _period > 0.0) {
                plan_type = "period";
            } else {
                plan_type = "system";
            }
            ID(Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "INSERT INTO BlastPlans (BlastID, CustomerID, Period, BlastDay, GroupID, EventType, PlanType) VALUES (" +_blast.ID() 
                + " , " + _customer_id + " , " + _period + "," + _blast_day
                + " , "+ _group.ID() +" , '"+_type+"' , '"+ plan_type + "' ); SELECT @@IDENTITY")));
        }
    
		
		/// Finds if this blast has a plan associated with the EventType already set.
		
        public void GetBlastPlan() {
            try {
                ID(Convert.ToInt32(DataFunctions.ExecuteScalar("communicator","SELECT BlastPlanID FROM BlastPlans where BlastID = "+ _blast.ID()
                    + " AND EventType='" + _type + "'" + " and IsDeleted = 0 ")));
            } catch {
                ID(0);
            }
        }
		
		/// Gets all of our blast plans for display by the blast plan editor
		
		/// <returns>every blastplan we hve</returns>
        public DataTable GetBlastsPlansGrid() {
            string sqlquery=
                " SELECT * from BlastPlans where CustomerID = " + _customer_id + " and IsDeleted = 0 ";
            DataTable dt = DataFunctions.GetDataTable(sqlquery);
            return dt;
        }
    }
}
