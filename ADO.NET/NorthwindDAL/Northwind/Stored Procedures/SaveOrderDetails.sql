CREATE PROCEDURE [dbo].[SaveOrderDetails]
(
	@id INT,
	@xmlData NVARCHAR(MAX) = NULL
)
AS
BEGIN
	DECLARE @details XML = CONVERT(XML, @xmlData)
	INSERT INTO dbo.[Order Details]
	SELECT
		@id,
		det.value('@ProductId', 'INT') as productId,
		det.value('@Price', 'MONEY') as Price,
		det.value('@Quantity', 'SMALLINT') as Quantity,
		det.value('@Discount', 'REAL') as Discount
	FROM @details.nodes('/Details/Detail') col(det)
END
GO