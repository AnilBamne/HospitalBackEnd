--create a stored procedure that can handle the complete order processing workflow, including creating an order, updating inventory, calculating total price, and generating an invoice.
Create Procedure spSampleWorkFlow
@ProductName varchar(100),
@ProductPrice float,
@Quantity int
As
Begin
	Begin try
		Begin TRAN
		--creating order
			Insert into OrderTable(ProductName,Price,Quantity)Values(@ProductName,@ProductPrice,@Quantity);

			declare @TotalPrice float;
			 Set @TotalPrice=Select price,quantity from OrderTable Where ProductName=@ProductName;
			
		Commit TRAN
	end try
	Begin catch
	end catch
End