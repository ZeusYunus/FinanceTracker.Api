CREATE OR ALTER PROCEDURE dbo.GetMonthlySummary
    @UserId NVARCHAR(450),
    @Year INT,
    @Month INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StartDate DATE =
        DATEFROMPARTS(@Year, @Month, 1);

    DECLARE @EndDate DATE =
        DATEADD(MONTH, 1, @StartDate);

    SELECT
        COALESCE(
            SUM(CASE
                WHEN [Type] = 'Income' THEN Amount
                ELSE 0
            END), 0) AS Income,

        COALESCE(
            SUM(CASE
                WHEN [Type] = 'Expense' THEN Amount
                ELSE 0
            END), 0) AS Expenses,

        COALESCE(
            SUM(CASE
                WHEN [Type] = 'Income' THEN Amount
                WHEN [Type] = 'Expense' THEN -Amount
                ELSE 0
            END), 0) AS Balance

    FROM dbo.Transactions
    WHERE UserId = @UserId
      AND [Date] >= @StartDate
      AND [Date] < @EndDate;
END;
GO