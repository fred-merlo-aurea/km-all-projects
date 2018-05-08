#!perl
# edit files referencing old ECN access check methods
package match;
sub new {
    my( $class, $file, $namespace, $class, $permission ) = @_;
    return bless { _file => $file, _namespace => $namespace, _class => $class, _permission => $permission }, $class;
}
sub file:lvalue { shift->{_file}}
sub namespace:lvalue { shift->{_namespace}}
sub class:lvalue { shift->{_class}}
sub permission:lvalue { shift->{_permission}}
package main;
use strict;
use warnings;

my %searches = 
  (
   hasRole => {
	       #re => qr/([^\n;]*KM\.Platform\.User\.\w+\s*[^\)]*\))/,
	       re => qr/(KM\.Platform\.User\.\w+\s*[^\)]*\))/,
	       handler => sub {
		 my ($file, $namespace, $class, @list) = @_;
		 #return map match->new( $file, $namespace, $class, $_), split /\s*,\s*/, @list;
		 return map match->new( $file, $namespace, $class, $_), @list;
	       }
	      },
   hasPermission => {
		     handler => sub {
		       #warn join ', ', @_;
		       my ($file, $namespace, $class, @list) = @_;
		       return map match->new( $file, $namespace, $class, $_), @list;
		       #my $enum = q(ECN_Framework_Common.Objects.Accounts.Enums.UserPermission);
		       #my( @permissions ) = map { (/$enum\.(\w+)/),(/"([^"]*)"/) } @list;
		       #return map match->new( $file, $namespace, $class, $_), @permissions;
		     },
		     #re => qr/([^\n;]*ECN_Framework_BusinessLayer\.Accounts\.User\.HasPermission\s*\([^\n\)]*\))/
		     re => qr/(ECN_Framework_BusinessLayer\.Accounts\.User\.HasPermission\s*\([^\n\)]*\))/
		    }
  );

my @files = grep not(/#/), map{chomp;s/^\.\///;$_} <DATA>;
my @mappings;
#my @files = q(./ecn.communicator/main/blasts/reports.aspx.cs);

local $\ = "\n"; # append newline to print statements
print join ',',qw[file namespace class OLD1 NEW1 OLD2 NEW2];
readfile($_) for @files

;  use Data::Dumper; print Dumper( \@mappings ); exit;

sub readfile {
  my( $file ) = @_; chomp $file;
  my( $original ) = do {
    open my $IN, '<', $file or die $file,':',$!;
    join '',<$IN>
  };
  my( $namespace ) = $1 if $original =~ /namespace\s+([^{\s]+)/msgxi;
  warn "ERROR: $file: NO NAMESPACE\n" and return unless $namespace;
  my( $class ) = $1 if $original =~ /class\s+(\w+)/;
  warn "ERROR: $file: NO CLASS\n" and return unless $class;

  foreach my$searchkey(keys %searches) {
    my $search = $searches{ $searchkey };
    my( @matches ) = ($original =~ m/$$search{re}/msgxi);
    push @mappings, $search->{handler}->( $file, $namespace, $class, @matches );
    #$searches{ $searchkey }->hander( $original =~ 
  }
}
sub readfile2 {
  my( $file ) = @_; chomp $file;
  my( $original ) = do {
    open my $IN, '<', $file or die $file,':',$!;
    join '',<$IN>
  };
  my( $namespace ) = $1 if $original =~ /namespace\s+([^{\s]+)/i;
  warn "ERROR: $file: NO NAMESPACE\n" and return unless $namespace;
  #print qq($file: $namespace\n);

  my( $class ) = $1 if $original =~ /class\s*(\w+)/;
  warn "ERROR: $file: NO CLASS\n" and return unless $class;
  #print qq($file: $namespace.$class\n);
  my $find = q(ECN_Framework_BusinessLayer.Accounts.User.HasPermission);
  my $enum = q(ECN_Framework_Common.Objects.Accounts.Enums.UserPermission);
  my( @list ) = $original =~ /^([^\n;]*ECN_Framework_BusinessLayer\.Accounts\.User\.HasPermission\s*\([^\n\)]*\))/msgi;
  my( @permissions ) = (map { (/$enum\.(\w+)/),(/"([^"]*)"/) } @list);
  print join ',', $file, $namespace, $class, map {$_,''} @permissions;
  #print "$file: $_: $list[$_]\n" for 0..$#list;
}

#readfile($_) for @files[ 10..12 ];

__DATA__
# ECN_Framework_BusinessLayer.Accounts.User.HasPermission
./ecn.CanonTradeshowWizard/SetupABCampaign.aspx.cs
./ecn.CanonTradeshowWizard/SetupCampaign.aspx.cs
./ecn.CanonTradeshowWizard/SetupChampionCampaign.aspx.cs
./ecn.communicator/main/blasts/active.aspx.cs
./ecn.communicator/main/blasts/bounces.aspx.cs
./ecn.communicator/main/blasts/clicks.aspx.cs
./ecn.communicator/main/blasts/clickslinks.aspx.cs
./ecn.communicator/main/blasts/forwards.aspx.cs
./ecn.communicator/main/blasts/NoClicks.aspx.cs
./ecn.communicator/main/blasts/NoOpens.aspx.cs
./ecn.communicator/main/blasts/reports.aspx.cs
./ecn.communicator/main/blasts/resends.aspx.cs
./ecn.communicator/main/blasts/Sends.aspx.cs
./ecn.communicator/main/blasts/Simple.aspx.cs
./ecn.communicator/main/blasts/Social.aspx.cs
./ecn.communicator/main/blasts/status.aspx.cs
./ecn.communicator/main/blasts/subscribes.aspx.cs
./ecn.communicator/main/blasts/suppressed.aspx.cs
./ecn.communicator/main/ECNWizard/Content/layoutEditorNew.ascx.cs
./ecn.communicator/main/ECNWizard/Group/groupExplorer.ascx.cs
./ecn.communicator/main/folders/folderseditor.aspx.cs
./ecn.communicator/main/lists/addressloader.aspx.cs
./ecn.communicator/main/lists/addressverifier.aspx.cs
./ecn.communicator/main/lists/ATHBimportDatafromFile.aspx.cs
./ecn.communicator/main/lists/emaileditor.aspx.cs
./ecn.communicator/main/lists/filters.aspx.cs
./ecn.communicator/main/lists/filtersOld.aspx.cs
./ecn.communicator/main/lists/filtersplusedit_old.aspx.cs
./ecn.communicator/main/lists/groupconfig.aspx.cs
./ecn.communicator/main/lists/groupeditor.aspx.cs
./ecn.communicator/main/lists/groupsubscribe.aspx.cs
./ecn.communicator/main/lists/groupsubscribePrePopSF.aspx.cs
./ecn.communicator/main/lists/importDatafromFile.aspx.cs
./ecn.communicator/main/lists/importmanager.aspx.cs
./ecn.communicator/main/lists/ReferralProgram.aspx.cs
./ecn.communicator/main/SMSWizard/default.aspx.cs
./ecn.communicator/main/SMSWizard/SetupCampaign.aspx.cs
./ecn.communicator/MasterPages/Communicator.Master.cs
./ECN_Framework_BusinessLayer/Activity/View/BlastActivity.cs
./ECN_Framework_BusinessLayer/Communicator/AccessCheck.cs
./ECN_Framework_BusinessLayer/Communicator/Blast/Base/Blast.cs
./ECN_Framework_BusinessLayer/Communicator/Blast/Object/BlastAB.cs
./ECN_Framework_BusinessLayer/Communicator/Blast/Object/BlastChampion.cs
./ECN_Framework_BusinessLayer/Communicator/Blast/Object/BlastLayout.cs
./ECN_Framework_BusinessLayer/Communicator/Blast/Object/BlastNoOpen.cs
./ECN_Framework_BusinessLayer/Communicator/Blast/Object/BlastRegular.cs
./ECN_Framework_BusinessLayer/Communicator/Blast/Object/BlastSMS.cs
./ECN_Framework_BusinessLayer/Communicator/BlastEnvelope.cs
./ECN_Framework_BusinessLayer/Communicator/BlastSingle.cs
./ECN_Framework_BusinessLayer/Communicator/Campaign.cs
./ECN_Framework_BusinessLayer/Communicator/CampaignItem.cs
./ECN_Framework_BusinessLayer/Communicator/CampaignItemBlast.cs
./ECN_Framework_BusinessLayer/Communicator/CampaignItemBlastRefBlast.cs
./ECN_Framework_BusinessLayer/Communicator/CampaignItemSuppression.cs
./ECN_Framework_BusinessLayer/Communicator/CampaignItemTestBlast.cs
./ECN_Framework_BusinessLayer/Communicator/Content.cs
./ECN_Framework_BusinessLayer/Communicator/ContentFilter.cs
./ECN_Framework_BusinessLayer/Communicator/EmailGroups.cs
./ECN_Framework_BusinessLayer/Communicator/Emails.cs
./ECN_Framework_BusinessLayer/Communicator/Filter.cs
./ECN_Framework_BusinessLayer/Communicator/FilterCondition.cs
./ECN_Framework_BusinessLayer/Communicator/FilterGroup.cs
./ECN_Framework_BusinessLayer/Communicator/Folder.cs
./ECN_Framework_BusinessLayer/Communicator/Group.cs
./ECN_Framework_BusinessLayer/Communicator/GroupDataFields.cs
./ECN_Framework_BusinessLayer/Communicator/Layout.cs
./ECN_Framework_BusinessLayer/Communicator/LayoutPlans.cs
./ECN_Framework_BusinessLayer/Communicator/Samples.cs
./ECN_Framework_BusinessLayer/Communicator/SmartFormsHistory.cs
./ECN_Framework_BusinessLayer/Communicator/SmartFormsPrePopFields.cs
./ECN_Framework_BusinessLayer/Communicator/TriggerPlans.cs
