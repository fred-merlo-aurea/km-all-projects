GRANT UPDATE
    ON SCHEMA::[dbo] TO [SubscriptionManager]
    AS [dbo];


GO
GRANT UPDATE
    ON SCHEMA::[dbo] TO [Developer]
    AS [dbo];


GO
GRANT SELECT
    ON SCHEMA::[dbo] TO [SubscriptionManager]
    AS [dbo];


GO
GRANT SELECT
    ON SCHEMA::[dbo] TO [QA]
    AS [dbo];


GO
GRANT SELECT
    ON SCHEMA::[dbo] TO [Developer]
    AS [dbo];


GO
GRANT REFERENCES
    ON SCHEMA::[dbo] TO [Developer]
    AS [dbo];


GO
GRANT INSERT
    ON SCHEMA::[dbo] TO [SubscriptionManager]
    AS [dbo];


GO
GRANT INSERT
    ON SCHEMA::[dbo] TO [Developer]
    AS [dbo];


GO
GRANT EXECUTE
    ON SCHEMA::[dbo] TO [SubscriptionManager]
    AS [dbo];


GO
GRANT EXECUTE
    ON SCHEMA::[dbo] TO [Developer]
    AS [dbo];


GO
GRANT ALTER
    ON SCHEMA::[dbo] TO [Developer]
    AS [dbo];

