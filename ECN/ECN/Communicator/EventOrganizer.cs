using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes;
using ecn.communicator.classes.Event;

namespace ecn.communicator.classes
{

    /// EventOrganizer allows you to add plans and process a partictular event

    public class EventOrganizer
    {
        int _customer_id;


        /// Event organizers have a customer ID

        /// <param name="new_id"> New Customer ID</param>
        public void CustomerID(int new_id)
        {
            _customer_id = new_id;
        }


        /// This verson of event processes a particular Email Activity Event

        /// <param name="log_event"> The Email Activity Log that just happened.</param>
        public void Event(EmailActivityLog log_event)
        {
            // Get the relivant data
            Layouts layout_to_check = log_event.Layout(); // Either find the layout we want or get the "0" layout and check that
            Groups group = log_event.Group();
            string type = log_event.EventType();

            // my_plan.GetLayoutPlan();  // Make aware of 0 layout id types.
            ArrayList groupLayoutPlans = LayoutPlans.GetGroupLayoutPlans(_customer_id, group.ID(), type, log_event.SmartFormID);
            ArrayList campaignLayoutPlans = LayoutPlans.GetCampaignLayoutPlans(_customer_id, layout_to_check.ID(), type);
            // TODO: Need to see if we allow default event for all groups or campaigns. - Yi

            foreach (LayoutPlans campaignLayoutPlan in campaignLayoutPlans)
            {
                FireEvent(campaignLayoutPlan, log_event);
            }

            foreach (LayoutPlans groupLayoutPlan in groupLayoutPlans)
            {
                FireEvent(groupLayoutPlan, log_event);
            }
        }

        private void FireEvent(LayoutPlans my_plan, EmailActivityLog log_event)
        {
            if (my_plan.ID() != 0)
            {
                if (my_plan.Status.ToString().ToUpper().Equals("Y"))
                {
                    Blasts to_blast = my_plan.Blast();
                    string check_against = my_plan.Criteria();

                    // Don't allow sends on unsubscribe events.
                    if (!(my_plan.EventType().Equals("subscribe") && log_event.ActionValue().Equals("U")))
                    {
                        // Ensure they either have a null check or they meet the criteria

                        if (check_against == "" || check_against == log_event.ActionValue())
                        {
                            // Ensure they are a member of this group.
                            if (my_plan.Group().ID() != 0 && !my_plan.EventType().Equals("subscribe"))
                            {
                                Emails in_group_email = my_plan.Group().WhatEmail(log_event.Email().EmailAddress());
                                if (null != in_group_email)
                                {
                                    // Should have an object for this.. no time right now.

                                    if (Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "select count(blastSingleID) from blastsingles where blastID = " + to_blast.ID() + " and EmailID = " + log_event.Email().ID() + " and LayoutPlanID = " + my_plan.ID())) == 0)
                                    {
                                        DataFunctions.ExecuteScalar("communicator", "INSERT INTO BlastSingles (BlastID, EmailID, SendTime, LayoutPlanID, refBlastID) VALUES ( "
                                            + to_blast.ID() + " , "
                                            + log_event.Email().ID()
                                            + " , '" + GetSendDateTime(my_plan.Period()).ToString() + "', " + my_plan.ID() + "," + log_event.Blast().ID() + " ); SELECT @@IDENTITY");
                                    }
                                }
                            }
                            else
                            {
                                // no group,  or a subscribe request just send.
                                // Should have an object for this.. no time right now.

                                if (Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "select count(blastSingleID) from blastsingles where blastID = " + to_blast.ID() + " and EmailID = " + log_event.Email().ID() + " and LayoutPlanID = " + my_plan.ID())) == 0)
                                {
                                    string blastSingleInsertSQL = "INSERT INTO BlastSingles (BlastID, EmailID, SendTime, LayoutPlanID, refBlastID) VALUES ( "
                                        + to_blast.ID() + " , "
                                        + log_event.Email().ID()
                                        + " , '" + GetSendDateTime(my_plan.Period()).ToString() + "', " + my_plan.ID() + "," + log_event.Blast().ID() + " ); SELECT @@IDENTITY";

                                    DataFunctions.ExecuteScalar("communicator", blastSingleInsertSQL);

                                    // Check to see if there's a NO-OPEN Campaign set for this blast in the BlastSingles. IF YES, get the properties of that Plan
                                    // and Insert a record for the NO-OPEN trigger
                                    //sql -> SELECT tp.TriggerPlanID, tp.RefTriggerID, tp.BlastID, tp.Period FROM Blasts b JOIN TriggerPlans tp ON b.BlastID = tp.RefTriggerID WHERE tp.RefTriggerID = 85233 AND tp.EventType = 'noOpen'

                                    string noOpenSelectSQL = " SELECT tp.TriggerPlanID, tp.RefTriggerID, tp.BlastID, tp.Period " +
                                        " FROM [Blast] b JOIN TriggerPlans tp ON b.BlastID = tp.RefTriggerID " +
                                        " WHERE Isnull(Status,'Y') = 'Y' and tp.RefTriggerID = " + to_blast.ID() + " AND tp.EventType = 'noOpen'";
                                    DataTable dt = DataFunctions.GetDataTable(noOpenSelectSQL, System.Configuration.ConfigurationManager.AppSettings["com"].ToString());
                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            string blastIDToSend = dr["BlastID"].ToString();
                                            double followupPeriod = Convert.ToDouble(dr["Period"].ToString()) + Convert.ToDouble(my_plan.Period().ToString());
                                            string triggerPlanID = dr["TriggerPlanID"].ToString();
                                            blastSingleInsertSQL = "INSERT INTO BlastSingles (BlastID, EmailID, SendTime, LayoutPlanID, refBlastID) VALUES ( "
                                                + blastIDToSend + ", "
                                                + log_event.Email().ID() + ", ' " + GetSendDateTime(followupPeriod).ToString() + " ', " + triggerPlanID + "," + log_event.Blast().ID() + " ); SELECT @@IDENTITY";
                                            DataFunctions.ExecuteScalar("communicator", blastSingleInsertSQL);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private DateTime GetSendDateTime(double period)
        {
            return DateTime.Now.AddDays(period);
            //			if (period < 1) {
            //				return DateTime.Now.AddDays(period);
            //			}
            //
            //			EventTimer timer = new EventTimer(_customer_id);
            //			timer.Load();
            //			return timer.GetSendDateTime(	DateTime.Now.AddDays(period) );		
        }


        /// Process a blast event. It takes in the type (start or end) and the blast that just happened.

        /// <param name="blast">Recent Blast</param>
        /// <param name="type">Start or End event</param>
        //public void Event(Blasts blast, string type)
        //{
        //    BlastPlans my_plan = new BlastPlans();
        //    my_plan.Blast(blast);
        //    my_plan.EventType(type);
        //    my_plan.GetBlastPlan();
        //    // End plans based on a blast ID means a clone blast
        //    if (my_plan.ID() != 0)
        //    {
        //        CloneBlast(blast, my_plan);
        //    }

        //    // Ensure the default group gets the notify version. 
        //    my_plan.Blast(new Blasts(0));
        //    my_plan.GetBlastPlan();
        //    if (my_plan.ID() != 0)
        //    {
        //        BlastFinalEvent(blast, type, my_plan);
        //    }

        //}


        /// Coppies a blast into the future based on a plan. 

        /// <param name="blast">The blast we clone</param>
        /// <param name="plan">The plan for which date in the future</param>
        //private void CloneBlast(Blasts blast, BlastPlans plan)
        //{
        //    string blastType = "";
        //    try
        //    {
        //        blastType = DataFunctions.ExecuteScalar("communicator", "SELECT BlastType FROM Blasts WHERE BlastID = " + blast.ID()).ToString();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    if (blastType.Equals("sample", StringComparison.OrdinalIgnoreCase))
        //    {
        //        CloneSampleBlast(blast, blast.ID(), plan);
        //    }
        //    else
        //    {
        //        double period = plan.Period();
        //        int day = plan.BlastDay();
        //        string type = plan.PlanType();
        //        int oldBlastID = blast.ID();

        //        // Get all the blast info in case we just got a blast_ID
        //        blast.ResetFromDb();
        //        // Get the original send time to prevent time shifts
        //        DateTime my_time = blast.SendTime();
        //        if ("period".Equals(type, StringComparison.OrdinalIgnoreCase))
        //        {
        //            blast.SendTime(my_time.AddDays(period).ToString());
        //        }
        //        else
        //        { // Month type
        //            blast.SendTime(my_time.AddMonths(1).ToString());
        //        }
        //        blast.CreateBlast();
        //        plan.Blast(blast);
        //        plan.Update();

        //        ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
        //        if (sc.hasProductFeature("ecn.communicator", "Users Departments", blast.CustomerID().ToString()))
        //        {
        //            string deptID = DataFunctions.ExecuteScalar("communicator", "SELECT DepartmentID FROM DeptItemReferences WHERE ItemID = " + oldBlastID + " AND Item = 'BLST' ").ToString();
        //            string sqlquery = " INSERT INTO DeptItemReferences (DepartmentID, Item, ItemID) VALUES (" + deptID + ", 'BLST', " + blast.ID().ToString() + ") ";
        //            DataFunctions.Execute("communicator", sqlquery).ToString();
        //        }
        //    }
        //}

        //private void CloneSampleBlast(Blasts blast, int currentBlastID, BlastPlans plan)
        //{
        //    //if this is the first of the a/b blasts, if it's not - do nothing
        //    // need to create the blast with blasttype of mastersample
        //    // need to to create the samples record
        //    // need to create the new blast records
        //    // need to create both new sampleblasts records
        //    // update blastplans records

        //    string statusCode = DataFunctions.ExecuteScalar("communicator", "select StatusCode from Blasts where BlastID = (select blastID from sampleblasts where blastID <> " + currentBlastID.ToString() + " and sampleID in (select sampleID from sampleblasts where blastID = " + currentBlastID.ToString() + "))").ToString();
        //    if (statusCode.Equals("pending", StringComparison.OrdinalIgnoreCase))
        //    {
        //        string sampleName = "";

        //        DataTable origSampleBlastsDT = DataFunctions.GetDataTable("Select * from SampleBlasts where BlastID=" + currentBlastID.ToString());
        //        DataTable origSampleBlasts2DT = DataFunctions.GetDataTable("select * from SampleBlasts where SampleID = " + origSampleBlastsDT.Rows[0]["SampleID"].ToString() + " and BlastID <> " + currentBlastID.ToString());
        //        DataTable origBlastPlansDT = DataFunctions.GetDataTable("Select * from BlastPlans where BlastID=" + origSampleBlastsDT.Rows[0]["BlastID"].ToString());
        //        DataTable origBlastsDT = DataFunctions.GetDataTable("Select * from Blasts where BlastID=" + origSampleBlastsDT.Rows[0]["BlastID"].ToString());
        //        DataTable origBlasts2DT = DataFunctions.GetDataTable("Select * from Blasts where BlastID=" + origSampleBlasts2DT.Rows[0]["BlastID"].ToString());
        //        sampleName = DataFunctions.ExecuteScalar("communicator", "SELECT SampleName FROM Samples WHERE SampleID=" + origSampleBlastsDT.Rows[0]["SampleID"].ToString()).ToString();

        //        double period = plan.Period();
        //        int day = plan.BlastDay();
        //        string type = plan.PlanType();
        //        int oldBlastID = blast.ID();

        //        // Get all the blast info in case we just got a blast_ID
        //        blast.ResetFromDb();


        //        // Get the original send time to prevent time shifts
        //        DateTime my_time = blast.SendTime();
        //        DateTime new_time = blast.SendTime();
        //        if ("period".Equals(type, StringComparison.OrdinalIgnoreCase))
        //        {
        //            new_time = my_time.AddDays(period);
        //        }
        //        else
        //        { // Month type
        //            new_time = my_time.AddMonths(1);
        //        }
        //        //sunil wants to remove the addition of time to the sample name
        //        //if (sampleName.IndexOf("]") > 0 && sampleName.IndexOf("]") < sampleName.Length)
        //        //{
        //        //    sampleName = sampleName.Substring(0, 1 + sampleName.IndexOf("]"));
        //        //}
        //        //else
        //        //{
        //        //    sampleName = "[" + sampleName + "]";
        //        //}
        //        //sampleName += " " + new_time.ToString();
        //        //if (sampleName.Length > 255)
        //        //{
        //        //    sampleName = sampleName.Substring(0, 255);
        //        //}

        //        Samples my_sample = new Samples();
        //        my_sample.Name(sampleName);
        //        my_sample.CustomerID(Convert.ToInt32(origBlastsDT.Rows[0]["CustomerID"].ToString()));
        //        my_sample.UserID(Convert.ToInt32(origBlastsDT.Rows[0]["UserID"].ToString()));
        //        my_sample.Subject(sampleName);
        //        my_sample.EmailFrom(origBlastsDT.Rows[0]["EmailFrom"].ToString());
        //        my_sample.EmailFromName(origBlastsDT.Rows[0]["EmailFromName"].ToString());
        //        my_sample.ReplyTo(origBlastsDT.Rows[0]["ReplyTo"].ToString());
        //        my_sample.Layout(new Layouts(0));
        //        my_sample.FilterID(Convert.ToInt32(origBlastsDT.Rows[0]["FilterID"].ToString()));
        //        my_sample.Group(new Groups(Convert.ToInt32(origBlastsDT.Rows[0]["GroupID"].ToString())));
        //        my_sample.BlastCodeID(origBlastsDT.Rows[0]["CodeID"].ToString());
        //        my_sample.BlastFrequency("RECURRING");
        //        my_sample.SendTime(new_time.ToString());
        //        my_sample.BlastSuppressionlist = origBlastsDT.Rows[0]["BlastSuppression"].ToString();
        //        my_sample.OptoutPreference = Convert.ToBoolean(origBlastsDT.Rows[0]["AddOptOuts_to_MS"].ToString());

        //        my_sample.Create();
        //        my_sample.Subject(origBlastsDT.Rows[0]["EmailSubject"].ToString());
        //        Blasts my_blast = new Blasts();
        //        my_blast = my_sample.AttachLayoutByCount(new Layouts(Convert.ToInt32(origBlastsDT.Rows[0]["LayoutID"].ToString())), Convert.ToInt32(origSampleBlastsDT.Rows[0]["Amount"].ToString()));

        //        my_sample.Subject(origBlasts2DT.Rows[0]["EmailSubject"].ToString());
        //        Blasts my_b_blast = new Blasts();
        //        my_b_blast = my_sample.AttachLayoutByCount(new Layouts(Convert.ToInt32(origBlasts2DT.Rows[0]["LayoutID"].ToString())), Convert.ToInt32(origSampleBlasts2DT.Rows[0]["Amount"].ToString()));

        //        //add blast plans
        //        BlastPlans my_plan = new BlastPlans();
        //        my_plan.Blast(my_blast);
        //        my_plan.CustomerID(Convert.ToInt32(origBlastsDT.Rows[0]["CustomerID"].ToString()));
        //        my_plan.EventType("end");
        //        my_plan.Period(Convert.ToDouble(origBlastPlansDT.Rows[0]["Period"].ToString()));
        //        my_plan.Create();

        //        my_plan = new BlastPlans();
        //        my_plan.Blast(my_b_blast);
        //        my_plan.CustomerID(Convert.ToInt32(origBlastsDT.Rows[0]["CustomerID"].ToString()));
        //        my_plan.EventType("end");
        //        my_plan.Period(Convert.ToDouble(origBlastPlansDT.Rows[0]["Period"].ToString()));
        //        my_plan.Create();
        //    }
        //}


        /// If we have a plan for getting a message at the start and end of blasts, this method will perform that send

        /// <param name="blast">The blast we just sent</param>
        /// <param name="type">start or end</param>
        /// <param name="blast_plan">To know which group to send to</param>
        private void BlastFinalEvent(Blasts blast, string type, BlastPlans blast_plan)
        {
            // Setup the Channel
            ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
            cc.CustomerID(blast.CustomerID());

            // Get the blast we send this system message out under
            int sendblastid = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "SELECT BlastID from Blasts where CustomerID=" + _customer_id
                + " AND BlastType='blastevent" + type + "'"));

            ECN_Framework_Entities.Communicator.Blast event_blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(blast.ID(), false);
            ECN_Framework_Entities.Communicator.Blast master_blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(sendblastid, false);

            // Get the group. loop through it and send blasts to all subscribers
            DataTable dt = DataFunctions.GetDataTable("Select EmailID from EmailGroups Where GroupID = " + blast_plan.Group().ID() + " AND SubscribeTypeCode='S'", System.Configuration.ConfigurationManager.AppSettings["com"].ToString());
            EmailFunctions emailFunctions = new EmailFunctions();

            foreach (DataRow dr in dt.Rows)
            {

                emailFunctions.SendSystem(event_blast, master_blast, (int)dr["EmailID"], System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"], cc.getHostName(), cc.getBounceDomain());
            }
        }


        /// Adds a No Open Trigger plan for a particular Blast Trigger.

        /// <param name="refTriggerBlast">Which layout to listen to</param>
        /// <param name="eventType"> NO OPEN</param>
        /// <param name="blast"> Which Blast to send in this event</param>
        /// <param name="group"> Which group in this event - not used</param>
        /// <param name="name"> What we call this</param>
        /// <param name="period"> How many days after to send</param>
        /// <param name="type"> open click forward subscribe</param>
        /// <param name="criteria">criteria to trigger on</param>
        private int AddTriggerPlan(int refTriggerBlastID, string eventType, int blastID, int groupID, double period, string criteria, string actionName)
        {
            TriggerPlans my_plan = new TriggerPlans();
            my_plan.RefTriggerID = refTriggerBlastID;
            my_plan.EventType = eventType;
            my_plan.BlastID = blastID;
            my_plan.Period = period;
            my_plan.Criteria = criteria;
            my_plan.CustomerID = _customer_id;
            my_plan.ActionName = actionName;
            my_plan.GroupID = groupID;

            return my_plan.createTriggerPlan();
        }


        /// Adds a layout plan for a particular layout type.

        /// <param name="layout">Which layout to listen to</param>
        /// <param name="blast"> what blast to send</param>
        /// <param name="group"> Which group to send to</param>
        /// <param name="name"> What we call this</param>
        /// <param name="period"> How many days after to send</param>
        /// <param name="type"> open click forward subscribe</param>
        /// <param name="criteria">criteria to trigger on</param>
        private int AddLayoutPlan(Layouts layout, Blasts blast, Groups group, string name, double period, string type, string criteria)
        {
            LayoutPlans my_plan = new LayoutPlans();
            my_plan.Layout(layout);
            my_plan.EventType(type);
            my_plan.Blast(blast);
            my_plan.Period(period);
            my_plan.Criteria(criteria);
            my_plan.ActionName(name);
            my_plan.CustomerID(_customer_id);
            my_plan.Group(group);
            my_plan.Status = "Y";
            return my_plan.Create();
        }


        /// Creates Open Plan

        /// <param name="layout">Which layout to listen to</param>
        /// <param name="blast"> what blast to send</param>
        /// <param name="group"> Which group to send to</param>
        /// <param name="name"> What we call this</param>
        /// <param name="period"> How many days after to send</param>
        /// <param name="criteria">criteria to trigger on</param>
        public int AddOpenPlan(Layouts layout, Blasts blast, Groups group, string name, double period, string criteria)
        {
            return AddLayoutPlan(layout, blast, group, name, period, "open", criteria);
        }


        /// Creates Open No Click Plan

        /// <param name="layout">Which layout to listen to</param>
        /// <param name="blast"> what blast to send</param>
        /// <param name="group"> Which group to send to</param>
        /// <param name="name"> What we call this</param>
        /// <param name="period"> How many days after to send</param>
        /// <param name="criteria">criteria to trigger on</param>
        public int AddNoClickPlan(Layouts layout, Blasts blast, Groups group, string name, double period, string criteria)
        {
            return AddLayoutPlan(layout, blast, group, name, period, "noclick", criteria);
        }


        /// Creates Click Plan

        /// <param name="layout">Which layout to listen to</param>
        /// <param name="blast"> what blast to send</param>
        /// <param name="group"> Which group to send to</param>
        /// <param name="name"> What we call this</param>
        /// <param name="period"> How many days after to send</param>
        /// <param name="criteria">criteria to trigger on</param>
        public int AddClickPlan(Layouts layout, Blasts blast, Groups group, string name, double period, string criteria)
        {
            return AddLayoutPlan(layout, blast, group, name, period, "click", criteria);
        }

        /// Creates Forward Plan

        /// <param name="layout">Which layout to listen to</param>
        /// <param name="blast"> what blast to send</param>
        /// <param name="group"> Which group to send to</param>
        /// <param name="name"> What we call this</param>
        /// <param name="period"> How many days after to send</param>
        /// <param name="criteria">criteria to trigger on</param>
        public int AddForwardPlan(Layouts layout, Blasts blast, Groups group, string name, double period, string criteria)
        {
            return AddLayoutPlan(layout, blast, group, name, period, "refer", criteria);
        }

        /// Creates Subscribe Plan

        /// <param name="layout">Which layout to listen to</param>
        /// <param name="blast"> what blast to send</param>
        /// <param name="group"> Which group to send to</param>
        /// <param name="name"> What we call this</param>
        /// <param name="period"> How many days after to send</param>
        /// <param name="criteria">criteria to trigger on</param>
        public int AddSubscribePlan(Layouts layout, Blasts blast, Groups group, string name, double period, string criteria)
        {
            return AddLayoutPlan(layout, blast, group, name, period, "subscribe", criteria);
        }


        /// Adds a No Open Trigger plan for a particular Blast Trigger.

        /// <param name="refTriggerBlast">Which layout to listen to</param>
        /// <param name="eventType"> NO OPEN</param>
        /// <param name="blast"> Which Blast to send in this event</param>
        /// <param name="group"> Which group in this event - not used</param>
        /// <param name="name"> What we call this</param>
        /// <param name="period"> How many days after to send</param>
        /// <param name="type"> open click forward subscribe</param>
        /// <param name="criteria">criteria to trigger on</param>
        public int AddNoOpenTriggerPlan(int refTriggerBlastID, string eventType, int blastID, int groupID, double period, string criteria, string actionName)
        {
            return AddTriggerPlan(refTriggerBlastID, eventType, blastID, groupID, period, criteria, actionName);
        }


        /// Adds a blast plan at the end of the email

        /// <param name="blast"> Which blast to care about</param>
        /// <param name="period"> How many days after</param>
        public void AddBlastEndPlan(Blasts blast, double period)
        {
            BlastPlans my_plan = new BlastPlans();
            my_plan.Blast(blast);
            my_plan.CustomerID(_customer_id);
            my_plan.EventType("end");
            my_plan.Period(period);
            my_plan.GetBlastPlan();

            if (my_plan.ID() != 0)
            {
                my_plan.Update();
            }
            else
            {
                my_plan.Create();
            }
        }


        /// Create a Monthly Blast Plan

        /// <param name="blast">Which blast we care about</param>
        /// <param name="day"> How many days after</param>
        public void AddMonthBlastEndPlan(Blasts blast, int day)
        {
            BlastPlans my_plan = new BlastPlans();
            my_plan.Blast(blast);
            my_plan.CustomerID(_customer_id);
            my_plan.EventType("end");
            my_plan.BlastDay(day);
            my_plan.GetBlastPlan();

            if (my_plan.ID() != 0)
            {
                my_plan.Update();
            }
            else
            {
                my_plan.Create();
            }
        }

        public void AddBlastEndPlan(Groups send_here, Blasts blast)
        {
            AddBlastPlan(send_here, blast, "end");

        }

        public void AddBlastStartPlan(Groups send_here, Blasts blast)
        {
            AddBlastPlan(send_here, blast, "start");
        }

        public void AddBlastEndPlan(Groups send_here)
        {
            AddBlastPlan(send_here, new Blasts(0), "end");

        }

        public void AddBlastStartPlan(Groups send_here)
        {
            AddBlastPlan(send_here, new Blasts(0), "start");
        }


        /// Creates a blast start or endplan.

        /// <param name="send_here">Which group you want it sent to</param>
        /// <param name="blast">Which Blast ID you care about</param>
        /// <param name="type">Start or End</param>
        private void AddBlastPlan(Groups send_here, Blasts blast, string type)
        {
            BlastPlans my_plan = new BlastPlans();
            my_plan.CustomerID(_customer_id);
            my_plan.Blast(blast);
            my_plan.Group(send_here);
            my_plan.EventType(type);
            my_plan.GetBlastPlan();
            if (my_plan.ID() != 0)
            {
                my_plan.Update();
            }
            else
            {
                my_plan.Create();
            }
        }
    }
}
