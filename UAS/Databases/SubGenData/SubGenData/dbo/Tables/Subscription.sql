CREATE TABLE [dbo].[Subscription] (
    [subscription_id]         INT          NOT NULL,
    [account_id]              INT          NOT NULL,
    [publication_id]          INT          NULL,
    [mailing_address_id]      INT          NULL,
    [billing_address_id]      INT          NULL,
    [issues]                  INT          NULL,
    [copies]                  INT          NULL,
    [paid_issues_left]        INT          NULL,
    [unearned_revenue]        FLOAT (53)   NULL,
    [type]                    VARCHAR (50) NULL,
    [date_created]            DATETIME     NULL,
    [date_expired]            DATETIME     NULL,
    [date_last_renewed]       DATETIME     NULL,
    [last_purchase_bundle_id] INT          NULL,
    [renew_campaign_active]   BIT          NULL,
    [audit_classification]    VARCHAR (50) NULL,
    [audit_request_type]      VARCHAR (50) NULL,
    CONSTRAINT [PK_Subscription] PRIMARY KEY CLUSTERED ([subscription_id] ASC) WITH (FILLFACTOR = 90)
);

