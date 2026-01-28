CREATE PROCEDURE sp_GetOrders
AS
BEGIN
    SET NOCOUNT ON;
    SELECT OrderId, OrderDate, CustomerName, TotalAmount
    FROM Orders
    ORDER BY OrderDate DESC;
END
GO


CREATE PROCEDURE sp_GetOrderDetails
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT OrderDetailId, OrderId, ProductId, ProductName, Quantity, UnitPrice
    FROM OrderDetails
    WHERE OrderId = @OrderId;
END
GO


CREATE PROCEDURE sp_CreateOrder
    @OrderDate DATETIME,
    @CustomerName NVARCHAR(100),
    @TotalAmount DECIMAL(18,2),
    @OrderDetailsJson NVARCHAR(MAX),
    @NewOrderId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Tạo đơn hàng mới
        INSERT INTO Orders (OrderDate, CustomerName, TotalAmount)
        VALUES (@OrderDate, @CustomerName, @TotalAmount);
        
        SET @NewOrderId = SCOPE_IDENTITY();
        
        -- Thêm chi tiết đơn hàng từ JSON
        INSERT INTO OrderDetails (OrderId, ProductId, ProductName, Quantity, UnitPrice)
        SELECT 
            @NewOrderId,
            ProductId,
            ProductName,
            Quantity,
            UnitPrice
        FROM OPENJSON(@OrderDetailsJson)
        WITH (
            ProductId INT,
            ProductName NVARCHAR(100),
            Quantity INT,
            UnitPrice DECIMAL(18,2)
        );
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO


CREATE PROCEDURE sp_DeleteOrder
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Xóa chi tiết đơn hàng trước
        DELETE FROM OrderDetails WHERE OrderId = @OrderId;
        
        -- Xóa đơn hàng
        DELETE FROM Orders WHERE OrderId = @OrderId;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO




CREATE PROCEDURE sp_GetOrderById
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT OrderId, OrderDate, CustomerName, TotalAmount
    FROM Orders
    WHERE OrderId = @OrderId;
END
GO
