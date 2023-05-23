Create Procedure spGetAllPatients
As
Begin
	Begin Try
		Begin Transaction
			Select * from PatientTable;
			Commit Transaction
	END TRY
	BEGIN CATCH
		-- Transaction uncommittable
		IF (XACT_STATE()) = -1
		ROLLBACK TRANSACTION
 
		-- Transaction committable
		IF (XACT_STATE()) = 1
		COMMIT TRANSACTION
	END CATCH
END;


