﻿using System.Web.UI.WebControls;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Blasts
{
    public partial class ReportsTest
    {
        private void VerifyLabelValueIfAuthorized()
        {
            _page.ShouldSatisfyAllConditions(
                () => Get<Label>(AolFeedbackUnsubscribePercentage).Text.ShouldBe("-175%"),
                () => Get<Label>(AolFeedbackUnsubscribeTotal).Text
                    .ShouldBe("<a href='subscribes.aspx?OnlyUnique=0&&code=FEEDBACK_UNSUB'>8</a>"),
                () => Get<Label>(AolFeedbackUnsubscribeTotalPercentage).Text.ShouldBe("-200%"),
                () => Get<Label>(AolFeedbackUnsubscribeUnique).Text
                    .ShouldBe("<a href='subscribes.aspx?OnlyUnique=1&&code=FEEDBACK_UNSUB'>7</a>"),
                () => Get<Label>(AbusePercentage).Text.ShouldBe("-200%"),
                () => Get<Label>(AbuseTotal).Text
                    .ShouldBe("<a href='subscribes.aspx?OnlyUnique=0&&code=ABUSERPT_UNSUB'>9</a>"),
                () => Get<Label>(AbuseTotalPercentage).Text.ShouldBe("-225%"),
                () => Get<Label>(AbuseUnique).Text
                    .ShouldBe("<a href='subscribes.aspx?OnlyUnique=1&&code=ABUSERPT_UNSUB'>8</a>"),
                () => Get<Label>(BouncesPercentage).Text.ShouldBe("500%"),
                () => Get<Label>(BouncesTotal).Text.ShouldBe("<a href='bounces.aspx?'>6</a>"),
                () => Get<Label>(BouncesTotalPercentage).Text.ShouldBe("100%"),
                () => Get<Label>(BouncesUnique).Text.ShouldBe("<a href='bounces.aspx?'>5</a>"),
                () => Get<Label>(ClickThrough).Text.ShouldBe("<a href='ClickThrough.aspx?'>3</a>"),
                () => Get<Label>(ClickThroughPercentage).Text.ShouldBe("100 %"),
                () => Get<Label>(ClicksPercentage).Text.ShouldBe("100 %"),
                () => Get<Label>(ClicksTotal).Text.ShouldBe("<a href='Clicks.aspx?'>3</a>"),
                () => Get<Label>(ClicksTotalPercentage).Text.ShouldBe("-75%"),
                () => Get<Label>(ClicksUnique).Text.ShouldBe("<a href='Clicks.aspx?'>2</a>"),
                () => Get<Label>(ForwardsPercentage).Text.ShouldBe("-300%"),
                () => Get<Label>(ForwardsTotal).Text.ShouldBe("<a href='forwards.aspx?'>13</a>"),
                () => Get<Label>(ForwardsTotalPercentage).Text.ShouldBe("-325%"),
                () => Get<Label>(ForwardsUnique).Text.ShouldBe("<a href='forwards.aspx?'>12</a>"),
                () => Get<Label>(MasterSuppressPercentage).Text.ShouldBe("-225%"),
                () => Get<Label>(MasterSuppressTotal).Text
                    .ShouldBe("<a href='subscribes.aspx?OnlyUnique=0&&code=MASTSUP_UNSUB'>10</a>"),
                () => Get<Label>(MasterSuppressTotalPercentage).Text.ShouldBe("-250%"),
                () => Get<Label>(MasterSuppressUnique).Text
                    .ShouldBe("<a href='subscribes.aspx?OnlyUnique=1&&code=MASTSUP_UNSUB'>9</a>"),
                () => Get<Label>(NoClickPercentage).Text.ShouldBe("-900%"),
                () => Get<Label>(NoClickTotal).Text.ShouldBe("<a href='noclicks.aspx?'>-9</a>"),
                () => Get<Label>(NoOpenPercentage).Text.ShouldBe("-300%"),
                () => Get<Label>(NoOpenTotal).Text.ShouldBe("<a href='noopens.aspx?'>-3</a>"),
                () => Get<Label>(OpensPercentage).Text.ShouldBe("-100%"),
                () => Get<Label>(OpensTotal).Text.ShouldBe("<a href='Opens.aspx?'>5</a>"),
                () => Get<Label>(OpensTotalPercentage).Text.ShouldBe("-125%"),
                () => Get<Label>(OpensUnique).Text.ShouldBe("<a href='Opens.aspx?'>4</a>"),
                () => Get<Label>(ResendsPercentage).Text.ShouldBe("-275%"),
                () => Get<Label>(ResendsTotal).Text.ShouldBe("<a href='resends.aspx?'>12</a>"),
                () => Get<Label>(ResendsTotalPercentage).Text.ShouldBe("-300%"),
                () => Get<Label>(ResendsUnique).Text.ShouldBe("<a href='resends.aspx?'>11</a>"),
                () => Get<Label>(SendsTotal).Text.ShouldBe("<a href='Sends.aspx?'>1</a>"),
                () => Get<Label>(SendsUnique).Text.ShouldBe("<a href='Sends.aspx?'>1</a>"),
                () => Get<Label>(SubscribesPercentage).Text.ShouldBe("-150%"),
                () => Get<Label>(SubscribesTotal).Text.ShouldBe("<a href='subscribes.aspx?OnlyUnique=0&'>7</a>"),
                () => Get<Label>(SubscribesTotalPercentage).Text.ShouldBe("-175%"),
                () => Get<Label>(SubscribesUnique).Text.ShouldBe("<a href='subscribes.aspx?OnlyUnique=1&'>6</a>"),
                () => Get<Label>(Successful).Text.ShouldBe("/1"),
                () => Get<Label>(SuccessfulPercentage).Text.ShouldBe("(-400%)"),
                () => Get<Label>(SuppressedPercentage).Text.ShouldBe("0"),
                () => Get<Label>(SuppressedTotal).Text.ShouldBe("<a href='suppressed.aspx?&value=ALL'>0</a>"),
                () => Get<Label>(SuppressedTotalPercentage).Text.ShouldBe("0"),
                () => Get<Label>(SuppressedUnique).Text.ShouldBe("<a href='suppressed.aspx?&value=ALL'>0</a>"));
        }

        private void VerifyLabelValuesWithInvalidDataTable()
        {
            _page.ShouldSatisfyAllConditions(
                () => Get<Label>(AolFeedbackUnsubscribePercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(AolFeedbackUnsubscribeTotalPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(AbusePercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(AbuseTotalPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(BouncesTotalPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(ClicksTotalPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(BouncesPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(ForwardsTotalPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(ClicksPercentage).Text.ShouldBe("0 %"),
                () => Get<Label>(ForwardsPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(AolFeedbackUnsubscribeTotal).Text.ShouldBe(Zero),
                () => Get<Label>(AolFeedbackUnsubscribeUnique).Text.ShouldBe("FEEDBACK_UNSUB"),
                () => Get<Label>(AbuseTotal).Text.ShouldBe(Zero),
                () => Get<Label>(AbuseUnique).Text.ShouldBe("ABUSERPT_UNSUB"),
                () => Get<Label>(BouncesTotal).Text.ShouldBe(Zero),
                () => Get<Label>(BouncesUnique).Text.ShouldBe("bounce"),
                () => Get<Label>(ClickThrough).Text.ShouldBe("clickthrough"),
                () => Get<Label>(ClickThroughPercentage).Text.ShouldBe("100 %"),
                () => Get<Label>(ClicksTotal).Text.ShouldBe(Zero),
                () => Get<Label>(ClicksUnique).Text.ShouldBe("click"),
                () => Get<Label>(ForwardsTotal).Text.ShouldBe(Zero),
                () => Get<Label>(ForwardsUnique).Text.ShouldBe("refer"),
                () => Get<Label>(MasterSuppressPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(MasterSuppressTotal).Text.ShouldBe(Zero),
                () => Get<Label>(MasterSuppressTotalPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(MasterSuppressUnique).Text.ShouldBe("MASTSUP_UNSUB"),
                () => Get<Label>(NoClickPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(NoClickTotal).Text.ShouldBe(Zero),
                () => Get<Label>(NoOpenPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(NoOpenTotal).Text.ShouldBe(Zero),
                () => Get<Label>(OpensPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(OpensTotal).Text.ShouldBe(Zero),
                () => Get<Label>(OpensTotalPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(OpensUnique).Text.ShouldBe("open"),
                () => Get<Label>(ResendsPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(ResendsTotal).Text.ShouldBe(Zero),
                () => Get<Label>(ResendsTotalPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(ResendsUnique).Text.ShouldBe("resend"),
                () => Get<Label>(SendsTotal).Text.ShouldBe(Zero),
                () => Get<Label>(SendsUnique).Text.ShouldBe(Zero),
                () => Get<Label>(SubscribesPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(SubscribesTotal).Text.ShouldBe(Zero),
                () => Get<Label>(SubscribesTotalPercentage).Text.ShouldBe(ZeroPercent),
                () => Get<Label>(SubscribesUnique).Text.ShouldBe("subscribe"),
                () => Get<Label>(Successful).Text.ShouldBe("/0"),
                () => Get<Label>(SuccessfulPercentage).Text.ShouldBe($"({ZeroPercent})"),
                () => Get<Label>(SuppressedPercentage).Text.ShouldBe(Zero),
                () => Get<Label>(SuppressedTotal).Text.ShouldBe(Zero),
                () => Get<Label>(SuppressedTotalPercentage).Text.ShouldBe(Zero),
                () => Get<Label>(SuppressedUnique).Text.ShouldBe(Zero));
        }

        private void VerifyLabelValuesIfNotAuthorized()
        {
            _page.ShouldSatisfyAllConditions(
                () => Get<Label>(AolFeedbackUnsubscribePercentage).Text.ShouldBe("-175%"),
                () => Get<Label>(AolFeedbackUnsubscribeTotalPercentage).Text.ShouldBe("-200%"),
                () => Get<Label>(AbusePercentage).Text.ShouldBe("-200%"),
                () => Get<Label>(AbuseTotalPercentage).Text.ShouldBe("-225%"),
                () => Get<Label>(BouncesTotalPercentage).Text.ShouldBe("100%"),
                () => Get<Label>(ClicksTotalPercentage).Text.ShouldBe("-75%"),
                () => Get<Label>(BouncesPercentage).Text.ShouldBe("500%"),
                () => Get<Label>(ForwardsTotalPercentage).Text.ShouldBe("-325%"),
                () => Get<Label>(ClicksPercentage).Text.ShouldBe("100 %"),
                () => Get<Label>(ForwardsPercentage).Text.ShouldBe("-300%"),
                () => Get<Label>(AolFeedbackUnsubscribeTotal).Text.ShouldBe("8"),
                () => Get<Label>(AolFeedbackUnsubscribeUnique).Text.ShouldBe("7"),
                () => Get<Label>(AbuseTotal).Text.ShouldBe("9"),
                () => Get<Label>(AbuseUnique).Text.ShouldBe("8"),
                () => Get<Label>(BouncesTotal).Text.ShouldBe("6"),
                () => Get<Label>(BouncesUnique).Text.ShouldBe("5"),
                () => Get<Label>(ClickThrough).Text.ShouldBe("3"),
                () => Get<Label>(ClickThroughPercentage).Text.ShouldBe("100 %"),
                () => Get<Label>(ClicksTotal).Text.ShouldBe("3"),
                () => Get<Label>(ClicksUnique).Text.ShouldBe("2"),
                () => Get<Label>(ForwardsTotal).Text.ShouldBe("13"),
                () => Get<Label>(ForwardsUnique).Text.ShouldBe("12"),
                () => Get<Label>(MasterSuppressPercentage).Text.ShouldBe("-225%"),
                () => Get<Label>(MasterSuppressTotal).Text.ShouldBe("10"),
                () => Get<Label>(MasterSuppressTotalPercentage).Text.ShouldBe("-250%"),
                () => Get<Label>(MasterSuppressUnique).Text.ShouldBe("9"),
                () => Get<Label>(NoClickPercentage).Text.ShouldBe("-900%"),
                () => Get<Label>(NoClickTotal).Text.ShouldBe("-9"),
                () => Get<Label>(NoOpenPercentage).Text.ShouldBe("-300%"),
                () => Get<Label>(NoOpenTotal).Text.ShouldBe("-3"),
                () => Get<Label>(OpensPercentage).Text.ShouldBe("-100%"),
                () => Get<Label>(OpensTotal).Text.ShouldBe("5"),
                () => Get<Label>(OpensTotalPercentage).Text.ShouldBe("-125%"),
                () => Get<Label>(OpensUnique).Text.ShouldBe("4"),
                () => Get<Label>(ResendsPercentage).Text.ShouldBe("-275%"),
                () => Get<Label>(ResendsTotal).Text.ShouldBe("12"),
                () => Get<Label>(ResendsTotalPercentage).Text.ShouldBe("-300%"),
                () => Get<Label>(ResendsUnique).Text.ShouldBe("11"),
                () => Get<Label>(SendsTotal).Text.ShouldBe("1"),
                () => Get<Label>(SendsUnique).Text.ShouldBe("1"),
                () => Get<Label>(SubscribesPercentage).Text.ShouldBe("-150%"),
                () => Get<Label>(SubscribesTotal).Text.ShouldBe("7"),
                () => Get<Label>(SubscribesTotalPercentage).Text.ShouldBe("-175%"),
                () => Get<Label>(SubscribesUnique).Text.ShouldBe("6"),
                () => Get<Label>(Successful).Text.ShouldBe("/1"),
                () => Get<Label>(SuccessfulPercentage).Text.ShouldBe("(-400%)"),
                () => Get<Label>(SuppressedPercentage).Text.ShouldBe(Zero),
                () => Get<Label>(SuppressedTotal).Text.ShouldBe("7"),
                () => Get<Label>(SuppressedTotalPercentage).Text.ShouldBe(Zero),
                () => Get<Label>(SuppressedUnique).Text.ShouldBe(Zero));
        }
    }
}