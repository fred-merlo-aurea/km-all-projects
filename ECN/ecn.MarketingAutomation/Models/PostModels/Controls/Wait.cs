using System;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
using Newtonsoft.Json;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Wait : VisualControlBase
    {
        private decimal _WaitTime;
        private int? _Days;
        private int? _Hours;
        private int? _Minutes;
        private bool? _TimeChanged;
        private int? _OriginalDays;
        private int? _OriginalHours;
        private int? _OriginalMinutes;
        private int? _HeatMapStats;

        [JsonProperty(PropertyName = ControlConsts.WaitTimeProperty)]
        public decimal WaitTime
        {
            get
            {
                return _WaitTime;
            }
            set
            {
                _WaitTime = value;
            }
        }

        [JsonProperty(
            PropertyName = ControlConsts.DaysProperty,
            DefaultValueHandling = DefaultValueHandling.Populate,
            NullValueHandling = NullValueHandling.Ignore)]
        public int? Days
        {
            get
            {
                return _Days;
            }
            set
            {
                _Days = value;
            }
        }

        [JsonProperty(
            PropertyName = ControlConsts.HoursProperty,
            DefaultValueHandling = DefaultValueHandling.Populate,
            NullValueHandling = NullValueHandling.Ignore)]
        public int? Hours
        {
            get
            {
                return _Hours;
            }
            set
            {
                _Hours = value;
            }
        }

        [JsonProperty(
            PropertyName = ControlConsts.MinutesProperty,
            DefaultValueHandling = DefaultValueHandling.Populate,
            NullValueHandling = NullValueHandling.Ignore)]
        public int? Minutes
        {
            get
            {
                return _Minutes;
            }
            set
            {
                _Minutes = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.TimeChangedProperty)]
        public bool? TimeChanged
        {
            get
            {
                return _TimeChanged;
            }
            set
            {
                _TimeChanged = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.OriginalDaysProperty)]
        public int? OriginalDays
        {
            get
            {
                return _OriginalDays;
            }
            set
            {
                _OriginalDays = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.OriginalHoursProperty)]
        public int? OriginalHours
        {
            get
            {
                return _OriginalHours;
            }
            set
            {
                _OriginalHours = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.OriginalMinutesProperty)]
        public int? OriginalMinutes
        {
            get
            {
                return _OriginalMinutes;
            }
            set
            {
                _OriginalMinutes = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.HeatMapStatsProperty)]
        public int? HeatMapStats
        {
            get
            {
                return _HeatMapStats;
            }
            set
            {
                _HeatMapStats = value;
            }
        }

        public Wait()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Wait;
            _Text = ControlConsts.ControlTextWait;
            editable = new shapeEditable();
        }

        public void Validate(
            ControlBase parent,
            CommunicatorEntities.MarketingAutomation marketingAutomation,
            User currentUser)
        {
            var ecnMaster = new ECNException(new List<ECNError>());
            
            ValidateText(ecnMaster);
            var waitTime = ValidateTimeSpan(ecnMaster);

            if (IsDirty)
            {
                ValdateSendTime(ecnMaster, parent, marketingAutomation, waitTime);
            }

            if (ecnMaster.ErrorList.Any())
            {
                throw ecnMaster;
            }
        }

        private TimeSpan ValidateTimeSpan(ECNException ecnMaster)
        {
            var waitTime = new TimeSpan(0);

            if (Days.HasValue || Hours.HasValue || Minutes.HasValue)
            {
                if (Days == null)
                {
                    Days = 0;
                }
                else if (Days.Value < 0)
                {
                    ecnMaster.ErrorList.Add(GetCampaignItemError(ControlConsts.InvalidDaysErrorMessage));
                }

                if (Hours == null)
                {
                    Hours = 0;
                }
                else if (Hours.Value < 0)
                {
                    ecnMaster.ErrorList.Add(GetCampaignItemError(ControlConsts.InvalidHoursErrorMessage));
                }

                if (Minutes == null)
                {
                    Minutes = 0;
                }
                else if (Minutes.Value < 0)
                {
                    ecnMaster.ErrorList.Add(GetCampaignItemError(ControlConsts.InvalidMinutesErrorMessage));
                }

                waitTime = new TimeSpan(Days.Value, Hours.Value, Minutes.Value, 0);
            }
            else
            {
                var errorMessage = string.Format(ControlConsts.InvalidWaitTimeErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }

            return waitTime;
        }

        private void ValdateSendTime(
            ECNException ecnMaster,
            ControlBase parent,
            CommunicatorEntities.MarketingAutomation marketingAutomation,
            TimeSpan waitTime)
        {
            var parentCampaignItem = parent as CampaignItem;
            if (parentCampaignItem != null && parentCampaignItem.SendTime.HasValue)
            {
                var estSendTime = parentCampaignItem.SendTime.Value.AddDays(waitTime.TotalDays);

                ValidateSendTime(
                    ecnMaster,
                    marketingAutomation.StartDate.Value.Date,
                    marketingAutomation.EndDate.Value.Date,
                    estSendTime);
            }

            var parentEstimatedSendTime = parent as IEstimatedSendTime;
            if (parentEstimatedSendTime != null && parentEstimatedSendTime.EstSendTime.HasValue)
            {
                var estSendTime = parentEstimatedSendTime.EstSendTime.Value.AddDays(waitTime.TotalDays);

                ValidateSendTime(ecnMaster, marketingAutomation.EndDate.Value.Date, estSendTime);
            }

            if (parent is Direct_Click ||
                parent is Direct_Open ||
                parent is Group)
            {
                if (DateTime.Now.AddDays(waitTime.TotalDays).Date > marketingAutomation.EndDate.Value.Date)
                {
                    ecnMaster.ErrorList.Add(GetCampaignItemError(ControlConsts.WaitTimeOutsideOfRangeErrorMessage));
                }
            }
        }

        private void ValidateSendTime(ECNException ecnMaster, DateTime minDate, DateTime maxDate, DateTime estSendTime)
        {
            if (estSendTime.Date > maxDate || estSendTime.Date < minDate)
            {
                ecnMaster.ErrorList.Add(GetCampaignItemError(ControlConsts.WaitTimeOutsideOfRangeErrorMessage));
            }
            else if (estSendTime < DateTime.Now)
            {
                ecnMaster.ErrorList.Add(GetCampaignItemError(ControlConsts.WaitTimeInThePastErrorMessage));
            }
        }

        private void ValidateSendTime(ECNException ecnMaster, DateTime maxDate, DateTime estSendTime)
        {
            if (estSendTime.Date > maxDate)
            {
                ecnMaster.ErrorList.Add(GetCampaignItemError(ControlConsts.WaitTimeOutsideOfRangeErrorMessage));
            }
            else if (estSendTime < DateTime.Now)
            {
                ecnMaster.ErrorList.Add(GetCampaignItemError(ControlConsts.WaitTimeInThePastErrorMessage));
            }
        }
    }
}