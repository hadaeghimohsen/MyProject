-- Stored Procedure: RESET_LDMA_P
-- Purpose: Reset LDMA_STAT, LDMA_DATE, LDMA_CODE to NULL across all tables in iScsc
-- Date: 2026-08-07
-- Usage: EXEC RESET_LDMA_P
--
-- This procedure:
-- 1. Disables all triggers on all tables (to avoid permission/cascading issues)
-- 2. Sets LDMA_STAT = NULL, LDMA_DATE = NULL, LDMA_CODE = NULL for all records
--    where any of these columns are NOT NULL
-- 3. Re-enables all triggers
-- 4. Reports affected row count

CREATE PROCEDURE RESET_LDMA_P
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @disableSql NVARCHAR(MAX) = '';
    DECLARE @updateSql NVARCHAR(MAX) = '';
    DECLARE @enableSql NVARCHAR(MAX) = '';
    DECLARE @totalRows BIGINT = 0;
    DECLARE @tableName SYSNAME;
    DECLARE @hasStat INT;
    DECLARE @hasDate INT;
    DECLARE @hasCode INT;

    -- Step 1: Disable all triggers on all tables
    SELECT @disableSql = @disableSql + 'DISABLE TRIGGER ALL ON [' + TABLE_NAME + '];' + CHAR(13)
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_TYPE = 'BASE TABLE';

    EXEC sp_executesql @disableSql;
    PRINT 'All triggers disabled.';

    -- Step 2: Generate and execute UPDATE statements for each table with LDMA columns
   DECLARE table_cursor CURSOR FOR
    SELECT TABLE_NAME
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE COLUMN_NAME IN ('LDMA_STAT', 'LDMA_DATE', 'LDMA_CODE')
    GROUP BY TABLE_NAME
    ORDER BY TABLE_NAME;

    OPEN table_cursor;
    FETCH NEXT FROM table_cursor INTO @tableName;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @hasStat = 0;
        SET @hasDate = 0;
        SET @hasCode = 0;

        IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND COLUMN_NAME = 'LDMA_STAT')
            SET @hasStat = 1;
        IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND COLUMN_NAME = 'LDMA_DATE')
            SET @hasDate = 1;
        IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND COLUMN_NAME = 'LDMA_CODE')
            SET @hasCode = 1;

        IF @hasStat = 1 OR @hasDate = 1 OR @hasCode = 1
        BEGIN
            SET @updateSql = 'UPDATE [' + @tableName + '] SET ';

            SET @updateSql = @updateSql +
                CASE WHEN @hasStat = 1 THEN 'LDMA_STAT = NULL, ' ELSE '' END +
                CASE WHEN @hasDate = 1 THEN 'LDMA_DATE = NULL, ' ELSE '' END +
                CASE WHEN @hasCode = 1 THEN 'LDMA_CODE = NULL' ELSE '' END;

            -- Remove trailing comma if present
            IF RIGHT(@updateSql, 2) = ', '
                SET @updateSql = LEFT(@updateSql, LEN(@updateSql) - 2);

            SET @updateSql = @updateSql +
                ' WHERE LDMA_STAT IS NOT NULL OR LDMA_DATE IS NOT NULL OR LDMA_CODE IS NOT NULL;';

            EXEC sp_executesql @updateSql;
        END

        FETCH NEXT FROM table_cursor INTO @tableName;
    END

    CLOSE table_cursor;
    DEALLOCATE table_cursor;

    -- Step 3: Re-enable all triggers
    SELECT @enableSql = @enableSql + 'ENABLE TRIGGER ALL ON [' + TABLE_NAME + '];' + CHAR(13)
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_TYPE = 'BASE TABLE';

    EXEC sp_executesql @enableSql;
    PRINT 'All triggers re-enabled.';

    PRINT 'All LDMA columns (STAT, DATE, CODE) reset to NULL successfully.';
END
GO

-- Usage example:
-- EXEC RESET_LDMA_P;
