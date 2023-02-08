--1
CREATE procedure spAvgFreightOrders@cust_id nvarchar(10),@avgFreight real outputAs begin	Select @avgFreight = AVG(freight) from orders group by CustomerID having CustomerID = @cust_idendCREATE procedure spInsertOrderDetails@customer_id nvarchar(10),@employee_id smallint,@freight real,@ship_name nvarchar(40),@product_id smallint,@quantity smallint,@discount realasbegin	declare @OrderID smallint, @unit_price real	begin transaction		set @OrderID = (select max(OrderID) from orders) + 1		set @unit_price = (select UnitPrice from Products where ProductID = @product_id)		begin try			INSERT INTO [Order Details] (OrderID, ProductID, UnitPrice, Quantity,discount)
        VALUES (@OrderID, @product_id, @unit_price, @Quantity,@discount )			commit transaction		end try		begin catch			Raiserror('Transaction canceled', 16, 1)			rollback transaction		end catchenddeclare @avgFreight real, @avgFreight outputspAvgFreightOrders 'ALFKI', print @avgFreight




-----------------------------------------------------------------------------------------------------------


--2

create procedure sp_salesbycountry
@Beginning_Date DateTime, @Ending_Date DateTime AS
SELECT Employees.Country, Employees.LastName, Employees.FirstName, Orders.ShippedDate, Orders.OrderID
FROM Employees  JOIN 
	Orders 
	ON Employees.EmployeeID = Orders.EmployeeID
WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date
GO

sp_salesbycountry   @Beginning_Date="1996-07-15 00:00:00.000", @Ending_Date ="1996-10-10 00:00:00.000"



-----------------------------------------------------------------------------



---3
create procedure sp_salesbyyear
@Beginning_Date DateTime, @Ending_Date DateTime
as
begin
select Orders.ShippedDate, Orders.OrderID, DATENAME(yy,ShippedDate) AS Year from Orders
WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date
end

sp_salesbyyear @Beginning_Date="1996-07-15 00:00:00.000", @Ending_Date ="1996-10-10 00:00:00.000"


----------------------------------------------------------------------------------------------=
--4

alter procedure sp_salesbycategory
@CategoryName nvarchar(15), @OrdYear nvarchar(4) = '1998'
as 
IF @OrdYear != '1996' AND @OrdYear != '1997' AND @OrdYear != '1998' 
BEGIN
	SELECT @OrdYear = '1998'
END


select  p.ProductName, Totalprice=round(sum(CONVERT(decimal(14,2),od.UnitPrice * (1 - od.Discount) *od.Quantity)),0) 
  from [order details] as od,Categories as c,Products as p ,Orders as o
   where c.CategoryID =p.CategoryID
	and od.ProductID=p.ProductID
	and OD.OrderID = O.OrderID 
	AND C.CategoryName = @CategoryName
	
	AND SUBSTRING(CONVERT(nvarchar(22), O.OrderDate, 111), 1, 4) = @OrdYear
	GROUP BY ProductName
ORDER BY ProductName


sp_salesbycategory @CategoryName = "Beverages", @OrdYear= "1996"

-----------------------------------------------------------------------------------------------------------------------
--5

alter procedure sp_tenmostexpensive

as
begin
 select top 10 ProductName, UnitPrice from Products
 order by UnitPrice  desc
end

sp_tenmostexpensive


-----------------------------------------------------------------------------------------------------------------------
--6

alter PROCEDURE InsertCustomerOrderDetails (@OrderID INT, @ProductID INT, @UnitPrice DECIMAL(10,2), @Quantity INT,@discount float)
AS
BEGIN
    INSERT INTO [Order Details] (OrderID, ProductID, UnitPrice, Quantity,Discount)
    VALUES (@OrderID, @ProductID, @UnitPrice, @Quantity,@discount)
END

InsertCustomerOrderDetails @OrderID =11077, @ProductID =16, @UnitPrice=8.76, @Quantity=2,@discount=0

select * from [Order Details]



---temp
CREATE PROCEDURE InsertCustomerOrderDetails (@OrderID INT, @ProductID INT, @UnitPrice DECIMAL(10,2), @Quantity INT,@discount float)
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM [Order Details] WHERE OrderID = @OrderID AND ProductID = @ProductID)
    BEGIN
         INSERT INTO [Order Details] (OrderID, ProductID, UnitPrice, Quantity,Discount)
			VALUES (@OrderID, @ProductID, @UnitPrice, @Quantity,@discount)
    END
    ELSE
    BEGIN
        RAISERROR('The combination of OrderID and ProductID already exists', 16, 1)
    END
END


-----------------------------------------------------------------------------------------
--6
CREATE PROCEDURE InsertCustomerOrderDetails (@OrderID INT, @ProductID INT, @UnitPrice DECIMAL(10,2), @Quantity INT,@discount float)
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM Orders WHERE OrderID = @OrderID)
    BEGIN
        RAISERROR('The OrderID does not exist', 16, 1)
    END
    ELSE IF NOT EXISTS (SELECT * FROM Products WHERE ProductID = @ProductID)
    BEGIN
        RAISERROR('The ProductID does not exist', 16, 1)
    END
    ELSE IF NOT EXISTS (SELECT * FROM [Order Details] WHERE OrderID = @OrderID AND ProductID = @ProductID)
    BEGIN
        INSERT INTO [Order Details] (OrderID, ProductID, UnitPrice, Quantity,discount)
        VALUES (@OrderID, @ProductID, @UnitPrice, @Quantity,@discount )
    END
    ELSE
    BEGIN
        RAISERROR('The combination of OrderID and ProductID already exists', 16, 1)
    END
END


InsertCustomerOrderDetails @OrderID =11077, @ProductID =76, @UnitPrice=8.76, @Quantity=2,@discount=0
select * from  [Order Details]



------------------------------------------------------------------------------------------------------------------------------/
--7
CREATE PROCEDURE UpdateCustomerOrderDetails (@OrderID INT, @ProductID INT, @UnitPrice DECIMAL(10,2), @Quantity INT,@discount float)
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM Orders WHERE OrderID = @OrderID)
    BEGIN
        RAISERROR('The OrderID does not exist', 16, 1)
    END
    ELSE IF NOT EXISTS (SELECT * FROM Products WHERE ProductID = @ProductID)
    BEGIN
        RAISERROR('The ProductID does not exist', 16, 1)
    END
    ELSE IF NOT EXISTS (SELECT * FROM [Order Details] WHERE OrderID = @OrderID AND ProductID = @ProductID)
    BEGIN
        RAISERROR('The combination of OrderID and ProductID does not exist', 16, 1)
    END
    ELSE
    BEGIN
        UPDATE [Order Details]
        SET UnitPrice = @UnitPrice, Quantity = @Quantity,Discount=@discount
        WHERE OrderID = @OrderID AND ProductID = @ProductID
    END
END


UpdateCustomerOrderDetails @OrderID=10248, @ProductID=11, @UnitPrice=12, @Quantity=10,@discount=0.01
select * from  [Order Details]