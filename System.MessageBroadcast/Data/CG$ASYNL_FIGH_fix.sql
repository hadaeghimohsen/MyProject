-- =============================================================================
-- FIX CG$ASYNL_FIGH - Fighter sync trigger
-- -----------------------------------------------------------------------------
-- BUG FIXED: The old guard used UPDATE(LDMA_CODE) OR UPDATE(LDMA_STAT) OR UPDATE(LDMA_DATE)
-- which returns TRUE when the column merely APPEARS in the SET clause of the UPDATE
-- statement, even if its value did NOT change.
--
-- Consequence: any full-row application UPDATE that rewrites LDMA_* columns
-- (even with identical values) caused the trigger to bail out BEFORE marking the
-- record as LDMA_STAT='003', so modified records were never re-synced.
--
-- FIX: Compare the ACTUAL VALUES of the LDMA columns between inserted and deleted.
-- The trigger now bails only when an LDMA column's value REALLY changed
-- (i.e. the sync process itself wrote a new LDMA_STAT/LDMA_DATE/LDMA_CODE).
-- Business-data changes always set LDMA_STAT='003' and LDMA_DATE=NULL.
-- =============================================================================

IF OBJECT_ID(N'dbo.CG$ASYNL_FIGH', N'TR') IS NOT NULL
   DROP TRIGGER [dbo].[CG$ASYNL_FIGH];
GO

CREATE TRIGGER [dbo].[CG$ASYNL_FIGH]
ON [dbo].[Fighter]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    -- Bail ONLY if an LDMA column's VALUE actually changed (sync process write).
    -- Presence in the SET clause is NOT sufficient (that was the bug).
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN deleted d ON d.FILE_NO = i.FILE_NO
        WHERE ISNULL(i.LDMA_STAT, '') <> ISNULL(d.LDMA_STAT, '')
           OR ISNULL(i.LDMA_CODE, '') <> ISNULL(d.LDMA_CODE, '')
           OR ISNULL(CONVERT(varchar(23), i.LDMA_DATE, 121), '1900-01-01') <> ISNULL(CONVERT(varchar(23), d.LDMA_DATE, 121), '1900-01-01')
    )
    BEGIN
        RETURN;
    END

    -- Mark Fighter as updated (key is FILE_NO) - only when business data changed
    UPDATE [Fighter]
    SET LDMA_STAT = '003', LDMA_DATE = NULL
    FROM [Fighter] t
    INNER JOIN inserted i ON t.[FILE_NO] = i.[FILE_NO];

    -- CASCADE: mark parent Club if FGPB_TYPE_DNRM='003' AND ACTV_TAG_DNRM='101'
    IF EXISTS(SELECT 1 FROM inserted WHERE FGPB_TYPE_DNRM = '003')
    BEGIN
        UPDATE c
        SET LDMA_STAT = '003'
        FROM dbo.Club c
        INNER JOIN inserted i ON c.CODE = i.CLUB_CODE_DNRM
        WHERE i.FGPB_TYPE_DNRM = '003'
          AND c.LDMA_STAT <> '003'
          AND (c.LDMA_STAT IS NULL OR c.LDMA_STAT IN ('001', '002'));
    END
END
GO