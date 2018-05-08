#!perl
use strict;
use warnings;

my @header = grep length, map { s/^\s+|\s+$//g; $_ } split /\s*,\s*/, <DATA>;
my @groups = grep length, map { s/^\s+|\s+$//g; $_ } @header;
my %features;
my %groupFeatureMap;

open my$FOUT, '>', "INSERT-ECN-Menu-Feature-Security-Group-Map.dml.sql" or die qq(failed to create/update file; $!);
local $\ = "\n";
my $oldout = select $FOUT;
print qq(DBCC checkident ('MenuFeatureSecurityGroupMap', reseed, 0););

while(<DATA>) {
  next unless /\S/;
  chomp;
  my @f = map { s/^\s+|\s+$//g; $_ } split /\s*,\s*/;
  foreach(1..$#f) {
    next if $f[$_] =~ /no access/i;
    my $featureName = join ' ', $f[ 0 ], $f[ $_ ];
    my $groupName = $groups[$_-1];
    $features{ $featureName } ++;
    $groupFeatureMap{$featureName} ||= {};
    next if exists $groupFeatureMap{$featureName}->{$groupName};
    $groupFeatureMap{$featureName}->{$groupName} = 1;
    print <<"    END_OF_SQL";
    INSERT INTO MenuFeatureSecurityGroupMap(MenuFeatureID,SecurityGroupID,HasAccess,IsActive,CreatedByUserID,DateCreated)
      SELECT mf.MenuFeatureID,sg.SecurityGroupID,1,1,1,GETDATE()
       FROM MenuFeature mf
       JOIN SecurityGroup sg ON 1=1
      WHERE mf.FeatureName = '$featureName'
        AND sg.SecurityGroupName = '$groupName';
    END_OF_SQL
  }
}
#print for @groups;

select $oldout;
close $FOUT or die qq(error closing file; $!);
open my$GOUT, '>', "INSERT-ECN-Menu-Feature.dml.sql" or die qq(failed to create/update file; $!);
$oldout = select $GOUT;
local $\ = "\n";
print <<END_OF_SQL;
-- KM Platform User Email Marketing
-- DML script to insert ECN menu features
DELETE FROM MenuFeatureSecurityGroupMap;
DELETE FROM MenuFeature;
DBCC checkident ('MenuFeature', reseed, 1);
DECLARE \@dateCreated DATETIME = GETDATE();
INSERT INTO MenuFeature(FeatureName,IsActive,CreatedByUserID,DateCreated)
VALUES
END_OF_SQL
print join ",\n", map qq/('$_',1,1,\@dateCreated)/, sort keys %features;
select $oldout;
close $GOUT;

__DATA__
,System Administrator,Base-Channel Administrator,Customer Administator,Power User,Content Manager,Campaign Manager,List Manager,Reports Only,Reports With Masked Data Only
Manage Groups,Full Access,Full Access,Full Access,Full Access,View and Select,View and Select,Full Access,View and Select,View and Select
Group Filters,Full Access,Full Access,Full Access,Full Access,No Access,View and Select,Full Access,View and Select,View and Select
Group UDFs,Full Access,Full Access,Full Access,Full Access,View and Select,View and Select,Full Access,View and Select,View and Select
Group Smart Forms,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,No Access,No Access
Group Archive,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,No Access,No Access
Group Export,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,No Access,No Access
Add Emails,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,No Access,No Access
Import Data,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,No Access,No Access
Clean Emails,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,No Access,No Access
Master Suppression,Full Access,All but Delete,All but Delete,View,No Access,View,View,No Access,No Access
BaseChannel Suppression,Full Access,Full Access,No Access,View,No Access,View,View,No Access,No Access
Customer Suppression,Full Access,Full Access,Full Access,View,No Access,View,View,No Access,No Access
Channel Master Suppression,Full Access,All but Delete,No Access,View,No Access,View,View,No Access,No Access
Global Suppression,Full Access,No Access,No Access,No Access,No Access,View,View,No Access,No Access
No Threshold,Full Access,Full Access,Full Access,View,No Access,Full Access,Full Access,No Access,No Access
Group Config,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,No Access,No Access
Email Search,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,No Access,No Access
Update Email,Full Access,Full Access,No Access,No Access,No Access,No Access,No Access,No Access,No Access
,,,,,,,,,
Group Reports,,,,,,,,,
Audience Engagement Report,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,No Access,Masked
Blast Statistics Report,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,Full Access,Full Access
Click Report,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,Full Access,Full Access
Group Attribute Report,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,Full Access,Full Access
Group Statistics Report,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,Full Access,Full Access
List Size Over Time Report,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,Full Access,Full Access
Master Suppression Source Report,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,No Access,Masked
Subscriber On/Off Report,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,Full Access,Full Access
,,,,,,,,,
Content/Messages,,,,,,,,,
Manage Content,Full Access,Full Access,Full Access,Full Access,Full Access,View and Select,No Access,No Access,No Access
Lock Content,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
Link Alias,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
Content Archive,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
Manage Messages,Full Access,Full Access,Full Access,Full Access,Full Access,View and Select,No Access,No Access,No Access
ROI Calculation,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
Link Tracking,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
Message Archive,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
Create Content,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
Create Message,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
Dynamic Tags,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
Images/Storage,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
Link Source,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
Message Types,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
Message Type Priority,Full Access,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access
RSS Feeds,Full Access,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access
,,,,,,,,,
Blasts,,,,,,,,,
Create Regular Blast,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Create AB Blast,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Create Champion Blast,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Calendar View,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Saved Campaigns,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Scheduled Campaigns,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Sent Campaigns,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Reports Only,Reports Only
Active Campaigns,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Active Customer Blasts,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Blast Envelopes,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Campaign Item Templates,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Campaign Item Template Archive,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Manage Campaigns,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
View Blast Links,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
,,,,,,,,,
Sent Campaign Report - Hyperlinks,,,,,,,,,
Sends,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Opens,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Partial Access,Partial Access
Active Opens,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,Masked
All Opens,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,Masked
Opens by Time,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
Browser Stats,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
Unopened,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,Masked
Clicks,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Partial Access,Partial Access
Top Clicks,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
Top Visitors,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,Masked
Clicks by Time,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,Masked
Clicks HeatMap,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
No Clicks,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Bounces,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
By Domain,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Hard Bounces,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Unsubscribe Hard Bounces,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Soft Bounces,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Resend Soft Bounces,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Other Bounces,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Resends,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Forwards,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Unsubscribes,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Master Suppressed,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Abuse Complaints,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
ISP Feedback Loops,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Suppressed,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Social Media,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Subscriber Share,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Simple Share,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Simple Share Repost,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Conversion Tracking Report,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Reporting by ISP(s),Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Mailing Lists,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Filter,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Message,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
,,,,,,,,,
Blast Reports,,,,,,,,,
A/B Summary Report,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
Blast Comparison Report,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
Campaign Statistics Report,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
Champion Audit Report,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
Delivery Report,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
Emails Delivered By Percentage Report,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
Email Fatigue Report,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
Email Performance By Domain Report,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
Email Preview Usage Report,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access,No Access
Link Report,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,Masked
Performance By Day and Time,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,Full Access,Full Access
Top Evangelists,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,Full Access,No Access,Masked
Undeliverable Report,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,Full Access,No Access,Masked
Unsubscribe Reason Report,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,Full Access,No Access,Masked
Group Folders,Full Access,Full Access,Full Access,Full Access,No Access,View and Select,Full Access,View and Select,View and Select
Content Folders,Full Access,Full Access,Full Access,Full Access,Full Access,View and Select,No Access,View and Select,View and Select
Message Triggers,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,No Access,No Access,No Access
Scheduled Reports,,,,,,,,,
Advertiser Click,Full Access,Full Access,Full Access,Full Access,No Access,,No Access,,
Audience Engagement,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,Full Access,No Access,Masked
Blast Delivery,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,Full Access,Full Access,Full Access
Email Performance By Domain,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,Full Access,Full Access
Email Preview Usage,Full Access,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access,No Access
Group Statistics,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,Full Access,Full Access
Group Attribute,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,Full Access,Full Access,Full Access
Group Export to FTP,Full Access,Full Access,Full Access,Full Access,No Access,No Access,Full Access,No Access,No Access
Unsubscribe Reason,Full Access,Full Access,Full Access,Full Access,No Access,Full Access,Full Access,No Access,No Access
,,,,,,,,,
Admin,,,,,,,,,
BaseChannel,,,,,,,,,
Landing Pages,Full Access,Full Access,No Access,No Access,No Access,No Access,No Access,No Access,No Access
Subscription Mgmt,Full Access,Full Access,No Access,No Access,No Access,No Access,No Access,No Access,No Access
Omniture,Full Access,Full Access,No Access,No Access,No Access,No Access,No Access,No Access,No Access
Salesforce,Full Access,Full Access,No Access,No Access,No Access,No Access,No Access,No Access,No Access
Message Thresholds,Full Access,Full Access,No Access,No Access,No Access,No Access,No Access,No Access,No Access
Customer,,,,,,,,,
Landing Pages,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access,No Access,No Access
Omniture,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access,No Access,No Access
Gateway,Full Access,Full Access,Full Access,No Access,No Access,No Access,No Access,No Access,No Access
