﻿
CREATE TABLE [dbo].[PubSubscriptionsPurged](
	[PubSubscriptionsPurgedID] [int] IDENTITY(1,1) NOT NULL,
	[PurgedDate] [datetime] NOT NULL,
	[PubSubscriptionID] [int] NOT NULL,
	[SubscriptionID] [int] NOT NULL,
	[PubID] [int] NOT NULL,
	[demo7] [varchar](1) NULL,
	[Qualificationdate] [date] NULL,
	[PubQSourceID] [int] NULL,
	[PubCategoryID] [int] NULL,
	[PubTransactionID] [int] NULL,
	[EmailStatusID] [int] NULL,
	[StatusUpdatedDate] [datetime] NULL,
	[StatusUpdatedReason] [varchar](200) NULL,
	[Email] [varchar](100) NULL,
	[DateCreated] [datetime] NULL,
	[DateUpdated] [datetime] NULL,
	[CreatedByUserID] [int] NULL,
	[UpdatedByUserID] [int] NULL,
	[IsComp] [bit] NULL,
	[SubscriptionStatusID] [int] NULL,
	[AccountNumber] [varchar](50) NULL,
	[AddRemoveID] [int] NULL,
	[Copies] [int] NULL,
	[GraceIssues] [int] NULL,
	[IMBSEQ] [varchar](256) NULL,
	[IsActive] [bit] NULL,
	[IsPaid] [bit] NULL,
	[IsSubscribed] [bit] NULL,
	[MemberGroup] [varchar](256) NULL,
	[OnBehalfOf] [varchar](256) NULL,
	[OrigsSrc] [varchar](256) NULL,
	[Par3CID] [int] NULL,
	[SequenceID] [int] NULL,
	[Status] [varchar](50) NULL,
	[SubscriberSourceCode] [varchar](100) NULL,
	[SubSrcID] [int] NULL,
	[Verify] [varchar](100) NULL,
	[ExternalKeyID] [int] NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Company] [varchar](100) NULL,
	[Title] [varchar](255) NULL,
	[Occupation] [varchar](50) NULL,
	[AddressTypeID] [int] NULL,
	[Address1] [varchar](256) NULL,
	[Address2] [varchar](256) NULL,
	[Address3] [varchar](256) NULL,
	[City] [varchar](50) NULL,
	[RegionCode] [varchar](50) NULL,
	[RegionID] [int] NULL,
	[ZipCode] [varchar](50) NULL,
	[Plus4] [varchar](10) NULL,
	[CarrierRoute] [varchar](10) NULL,
	[County] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[CountryID] [int] NULL,
	[Latitude] [decimal](18, 15) NULL,
	[Longitude] [decimal](18, 15) NULL,
	[IsAddressValidated] [bit] NULL,
	[AddressValidationDate] [datetime] NULL,
	[AddressValidationSource] [varchar](50) NULL,
	[AddressValidationMessage] [varchar](max) NULL,
	[Phone] [varchar](100) NULL,
	[Fax] [varchar](100) NULL,
	[Mobile] [varchar](100) NULL,
	[Website] [varchar](255) NULL,
	[Birthdate] [date] NULL,
	[Age] [int] NULL,
	[Income] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[tmpSubscriptionID] [int] NULL,
	[IsLocked] [bit] NULL,
	[LockedByUserID] [int] NULL,
	[LockDate] [datetime] NULL,
	[LockDateRelease] [datetime] NULL,
	[PhoneExt] [varchar](25) NULL,
	[IsInActiveWaveMailing] [bit] NULL,
	[AddressTypeCodeId] [int] NULL,
	[AddressLastUpdatedDate] [datetime] NULL,
	[AddressUpdatedSourceTypeCodeId] [int] NULL,
	[WaveMailingID] [int] NULL,
	[IGrp_No] [uniqueidentifier] NULL,
	[SFRecordIdentifier] [uniqueidentifier] NULL,
	[ReqFlag] [int] NULL,
	[PubTransactionDate] [datetime] NULL,
	[EmailID] [int] NULL,
	[SubGenSubscriberID] [int] NULL,
	[MailPermission] [bit] NULL,
	[FaxPermission] [bit] NULL,
	[PhonePermission] [bit] NULL,
	[OtherProductsPermission] [bit] NULL,
	[ThirdPartyPermission] [bit] NULL,
	[EmailRenewPermission] [bit] NULL,
	[TextPermission] [bit] NULL,
	[SubGenSubscriptionID] [int] NULL,
	[SubGenPublicationID] [int] NULL,
	[SubGenMailingAddressId] [int] NULL,
	[SubGenBillingAddressId] [int] NULL,
	[IssuesLeft] [int] NULL,
	[UnearnedReveue] [money] NULL,
	[SubGenIsLead] [bit] NULL,
	[SubGenRenewalCode] [varchar](50) NULL,
	[SubGenSubscriptionRenewDate] [date] NULL,
	[SubGenSubscriptionExpireDate] [date] NULL,
	[SubGenSubscriptionLastQualifiedDate] [date] NULL,
 CONSTRAINT [PK_PubSubscriptionsPurged] PRIMARY KEY CLUSTERED 
(
	[PubSubscriptionsPurgedID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[PubSubscriptionsPurged] ADD  CONSTRAINT [DF_PubSubscriptionsPurged_PurgedDate]  DEFAULT (getdate()) FOR [PurgedDate]